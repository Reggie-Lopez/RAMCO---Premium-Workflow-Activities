// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ThenByExpressionNode
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
  public sealed class ThenByExpressionNode : MethodCallExpressionNodeBase
  {
    private readonly ResolvedExpressionCache<Expression> _cachedSelector;
    private readonly LambdaExpression _keySelector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("ThenBy").WithoutComparer();

    public ThenByExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector);
      if (keySelector.Parameters.Count != 1)
        throw new ArgumentException("KeySelector must have exactly one parameter.", nameof (keySelector));
      this._keySelector = keySelector;
      this._cachedSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression KeySelector => this._keySelector;

    public Expression GetResolvedKeySelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this._keySelector.Body, this._keySelector.Parameters[0], clauseGenerationContext)));
    }

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
      (this.GetOrderByClause(queryModel) ?? throw new NotSupportedException("ThenByDescending expressions must follow OrderBy, OrderByDescending, ThenBy, or ThenByDescending expressions.")).Orderings.Add(new Ordering(this.GetResolvedKeySelector(clauseGenerationContext), OrderingDirection.Asc));
    }

    private OrderByClause GetOrderByClause(QueryModel queryModel) => queryModel.BodyClauses.Count == 0 ? (OrderByClause) null : queryModel.BodyClauses[queryModel.BodyClauses.Count - 1] as OrderByClause;
  }
}
