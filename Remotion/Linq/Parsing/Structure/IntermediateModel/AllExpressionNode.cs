// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AllExpressionNode
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
  public sealed class AllExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly ResolvedExpressionCache<Expression> _cachedPredicate;
    private readonly LambdaExpression _predicate;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("All");

    public AllExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (predicate), predicate);
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
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new AllResultOperator(this.GetResolvedPredicate(clauseGenerationContext));
    }
  }
}
