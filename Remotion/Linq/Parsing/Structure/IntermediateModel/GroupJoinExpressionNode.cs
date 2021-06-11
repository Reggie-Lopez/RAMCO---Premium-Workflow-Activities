// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupJoinExpressionNode
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
  public sealed class GroupJoinExpressionNode : 
    MethodCallExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    private readonly ResolvedExpressionCache<Expression> _cachedResultSelector;
    private readonly JoinExpressionNode _joinExpressionNode;
    private readonly LambdaExpression _resultSelector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("GroupJoin").WithoutEqualityComparer();

    public GroupJoinExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      Expression innerSequence,
      LambdaExpression outerKeySelector,
      LambdaExpression innerKeySelector,
      LambdaExpression resultSelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerSequence), innerSequence);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (outerKeySelector), outerKeySelector);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (innerKeySelector), innerKeySelector);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (resultSelector), resultSelector);
      if (outerKeySelector.Parameters.Count != 1)
        throw new ArgumentException("Outer key selector must have exactly one parameter.", nameof (outerKeySelector));
      if (innerKeySelector.Parameters.Count != 1)
        throw new ArgumentException("Inner key selector must have exactly one parameter.", nameof (innerKeySelector));
      if (resultSelector.Parameters.Count != 2)
        throw new ArgumentException("Result selector must have exactly two parameters.", nameof (resultSelector));
      LambdaExpression resultSelector1 = Expression.Lambda((Expression) Expression.Constant((object) null), outerKeySelector.Parameters[0], innerKeySelector.Parameters[0]);
      this._joinExpressionNode = new JoinExpressionNode(parseInfo, innerSequence, outerKeySelector, innerKeySelector, resultSelector1);
      this._resultSelector = resultSelector;
      this._cachedResultSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public JoinExpressionNode JoinExpressionNode => this._joinExpressionNode;

    public LambdaExpression ResultSelector => this._resultSelector;

    public Expression GetResolvedResultSelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedResultSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, this._resultSelector.Parameters[1], this._resultSelector.Body, clauseGenerationContext), this._resultSelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      ArgumentUtility.CheckNotNull<ClauseGenerationContext>(nameof (clauseGenerationContext), clauseGenerationContext);
      Expression resolvedResultSelector = this.GetResolvedResultSelector(clauseGenerationContext);
      return ReplacingExpressionVisitor.Replace((Expression) inputParameter, resolvedResultSelector, expressionToBeResolved);
    }

    protected override void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      GroupJoinClause groupJoinClause = new GroupJoinClause(this._resultSelector.Parameters[1].Name, this._resultSelector.Parameters[1].Type, this._joinExpressionNode.CreateJoinClause(clauseGenerationContext));
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) groupJoinClause);
      queryModel.BodyClauses.Add((IBodyClause) groupJoinClause);
      queryModel.SelectClause.Selector = this.GetResolvedResultSelector(clauseGenerationContext);
    }
  }
}
