// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SelectManyExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class SelectManyExpressionNode : 
    MethodCallExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    private const int c_indexSelectorParameterPosition = 1;
    private readonly ResolvedExpressionCache<Expression> _cachedCollectionSelector;
    private readonly ResolvedExpressionCache<Expression> _cachedResultSelector;
    private readonly LambdaExpression _collectionSelector;
    private readonly LambdaExpression _resultSelector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("SelectMany").WithoutIndexSelector(1);

    public SelectManyExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression collectionSelector,
      LambdaExpression resultSelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (collectionSelector), collectionSelector);
      if (collectionSelector.Parameters.Count != 1)
        throw new ArgumentException("Collection selector must have exactly one parameter.", nameof (collectionSelector));
      this._collectionSelector = collectionSelector;
      if (resultSelector != null)
      {
        if (resultSelector.Parameters.Count != 2)
          throw new ArgumentException("Result selector must have exactly two parameters.", nameof (resultSelector));
        this._resultSelector = resultSelector;
      }
      else
      {
        ParameterExpression parameterExpression1 = Expression.Parameter(collectionSelector.Parameters[0].Type, collectionSelector.Parameters[0].Name);
        ParameterExpression parameterExpression2 = Expression.Parameter(ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(this.CollectionSelector.Body.Type, nameof (collectionSelector)), parseInfo.AssociatedIdentifier);
        this._resultSelector = Expression.Lambda((Expression) parameterExpression2, parameterExpression1, parameterExpression2);
      }
      this._cachedCollectionSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      this._cachedResultSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression CollectionSelector => this._collectionSelector;

    public LambdaExpression ResultSelector => this._resultSelector;

    public Expression GetResolvedCollectionSelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedCollectionSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this._collectionSelector.Body, this._collectionSelector.Parameters[0], clauseGenerationContext)));
    }

    public Expression GetResolvedResultSelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedResultSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, this.ResultSelector.Parameters[1], this.ResultSelector.Body, clauseGenerationContext), this._resultSelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      Expression resolvedResultSelector = this.GetResolvedResultSelector(clauseGenerationContext);
      return ReplacingExpressionVisitor.Replace((Expression) inputParameter, resolvedResultSelector, expressionToBeResolved);
    }

    protected override void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      AdditionalFromClause additionalFromClause = new AdditionalFromClause(this._resultSelector.Parameters[1].Name, this._resultSelector.Parameters[1].Type, this.GetResolvedCollectionSelector(clauseGenerationContext));
      queryModel.BodyClauses.Add((IBodyClause) additionalFromClause);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) additionalFromClause);
      queryModel.SelectClause.Selector = this.GetResolvedResultSelector(clauseGenerationContext);
    }
  }
}
