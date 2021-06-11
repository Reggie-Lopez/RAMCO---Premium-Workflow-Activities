// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AggregateExpressionNode
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
  public sealed class AggregateExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly ResolvedExpressionCache<LambdaExpression> _cachedFunc;
    private readonly LambdaExpression _func;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Aggregate").WithoutSeedParameter();

    public AggregateExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression func)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Parameters.Count != 2)
        throw new ArgumentException("Func must have exactly two parameters.", nameof (func));
      this._func = func;
      this._cachedFunc = new ResolvedExpressionCache<LambdaExpression>((IExpressionNode) this);
    }

    public LambdaExpression Func => this._func;

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
      return (ResultOperatorBase) new AggregateResultOperator(this.GetResolvedFunc(clauseGenerationContext));
    }
  }
}
