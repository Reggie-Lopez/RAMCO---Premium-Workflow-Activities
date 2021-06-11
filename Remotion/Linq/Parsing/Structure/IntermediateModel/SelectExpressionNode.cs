// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SelectExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

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
  public sealed class SelectExpressionNode : MethodCallExpressionNodeBase
  {
    private const int c_indexSelectorParameterPosition = 1;
    private readonly ResolvedExpressionCache<Expression> _cachedSelector;
    private readonly LambdaExpression _selector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Select").WithoutIndexSelector(1);

    public SelectExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression selector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (selector), selector);
      if (selector.Parameters.Count != 1)
        throw new ArgumentException("Selector must have exactly one parameter.", nameof (selector));
      this._selector = selector;
      this._cachedSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression Selector => this._selector;

    public Expression GetResolvedSelector(ClauseGenerationContext clauseGenerationContext) => this._cachedSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.Selector.Body, this.Selector.Parameters[0], clauseGenerationContext)));

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      Expression resolvedSelector = this.GetResolvedSelector(clauseGenerationContext);
      return ReplacingExpressionVisitor.Replace((Expression) inputParameter, resolvedSelector, expressionToBeResolved);
    }

    protected override void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.SelectClause.Selector = this.GetResolvedSelector(clauseGenerationContext);
    }
  }
}
