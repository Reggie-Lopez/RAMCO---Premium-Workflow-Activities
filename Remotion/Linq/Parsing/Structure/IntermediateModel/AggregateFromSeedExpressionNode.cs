// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AggregateFromSeedExpressionNode
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
  public sealed class AggregateFromSeedExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly ResolvedExpressionCache<LambdaExpression> _cachedFunc;
    private readonly Expression _seed;
    private readonly LambdaExpression _func;
    private readonly LambdaExpression _optionalResultSelector;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Aggregate").WithSeedParameter();

    public AggregateFromSeedExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      Expression seed,
      LambdaExpression func,
      LambdaExpression optionalResultSelector)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (seed), seed);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Parameters.Count != 2)
        throw new ArgumentException("Func must have exactly two parameters.", nameof (func));
      if (optionalResultSelector != null && optionalResultSelector.Parameters.Count != 1)
        throw new ArgumentException("Result selector must have exactly one parameter.", nameof (optionalResultSelector));
      this._seed = seed;
      this._func = func;
      this._optionalResultSelector = optionalResultSelector;
      this._cachedFunc = new ResolvedExpressionCache<LambdaExpression>((IExpressionNode) this);
    }

    public Expression Seed => this._seed;

    public LambdaExpression Func => this._func;

    public LambdaExpression OptionalResultSelector => this._optionalResultSelector;

    public LambdaExpression GetResolvedFunc(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedFunc.GetOrCreate((System.Func<ExpressionResolver, LambdaExpression>) (r => Expression.Lambda(r.GetResolvedExpression(this._func.Body, this._func.Parameters[1], clauseGenerationContext), this._func.Parameters[0])));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new AggregateFromSeedResultOperator(this._seed, this.GetResolvedFunc(clauseGenerationContext), this._optionalResultSelector);
    }
  }
}
