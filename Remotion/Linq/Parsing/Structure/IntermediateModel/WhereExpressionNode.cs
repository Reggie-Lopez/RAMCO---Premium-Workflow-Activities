// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.WhereExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
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
  public sealed class WhereExpressionNode : MethodCallExpressionNodeBase
  {
    private const int c_indexSelectorParameterPosition = 1;
    private readonly ResolvedExpressionCache<Expression> _cachedPredicate;
    private readonly LambdaExpression _predicate;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Where").WithoutIndexSelector(1);

    public WhereExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (predicate), predicate);
      if (predicate.Parameters.Count != 1)
        throw new ArgumentException("Predicate must have exactly one parameter.", nameof (predicate));
      this._predicate = predicate;
      this._cachedPredicate = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression Predicate => this._predicate;

    public Expression GetResolvedPredicate(ClauseGenerationContext clauseGenerationContext) => this._cachedPredicate.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this._predicate.Body, this._predicate.Parameters[0], clauseGenerationContext)));

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      WhereClause whereClause = new WhereClause(this.GetResolvedPredicate(clauseGenerationContext));
      queryModel.BodyClauses.Add((IBodyClause) whereClause);
    }
  }
}
