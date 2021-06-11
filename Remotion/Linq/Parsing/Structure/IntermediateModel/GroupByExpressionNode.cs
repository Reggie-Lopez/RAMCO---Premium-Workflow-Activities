// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupByExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
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
  public sealed class GroupByExpressionNode : 
    ResultOperatorExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    private readonly ResolvedExpressionCache<Expression> _cachedKeySelector;
    private readonly ResolvedExpressionCache<Expression> _cachedElementSelector;
    private readonly LambdaExpression _keySelector;
    private readonly LambdaExpression _optionalElementSelector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("GroupBy").WithoutResultSelector().WithoutEqualityComparer();

    public GroupByExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector,
      LambdaExpression optionalElementSelector)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector);
      if (keySelector.Parameters.Count != 1)
        throw new ArgumentException("KeySelector must have exactly one parameter.", nameof (keySelector));
      if (optionalElementSelector != null && optionalElementSelector.Parameters.Count != 1)
        throw new ArgumentException("ElementSelector must have exactly one parameter.", nameof (optionalElementSelector));
      this._keySelector = keySelector;
      this._optionalElementSelector = optionalElementSelector;
      this._cachedKeySelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      if (optionalElementSelector == null)
        return;
      this._cachedElementSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression KeySelector => this._keySelector;

    public LambdaExpression OptionalElementSelector => this._optionalElementSelector;

    public Expression GetResolvedKeySelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedKeySelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this._keySelector.Body, this._keySelector.Parameters[0], clauseGenerationContext)));
    }

    public Expression GetResolvedOptionalElementSelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._optionalElementSelector == null ? (Expression) null : this._cachedElementSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this._optionalElementSelector.Body, this._optionalElementSelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      GroupResultOperator groupResultOperator = new GroupResultOperator(this.AssociatedIdentifier, this.GetResolvedKeySelector(clauseGenerationContext), this.GetResolvedOptionalElementSelector(clauseGenerationContext) ?? this.Source.Resolve(this._keySelector.Parameters[0], (Expression) this._keySelector.Parameters[0], clauseGenerationContext));
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) groupResultOperator);
      return (ResultOperatorBase) groupResultOperator;
    }
  }
}
