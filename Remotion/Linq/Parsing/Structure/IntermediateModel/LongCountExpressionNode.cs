// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.LongCountExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class LongCountExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static IEnumerable<MethodInfo> GetSupportedMethods()
    {
      foreach (MethodInfo whereNameMatch in ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("LongCount"))
        yield return whereNameMatch;
      MethodInfo arrayLongLengthMethodInfo = typeof (Array).GetRuntimeMethod("get_LongLength", new Type[0]);
      if (arrayLongLengthMethodInfo != (MethodInfo) null)
        yield return arrayLongLengthMethodInfo;
    }

    public LongCountExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression optionalPredicate)
      : base(parseInfo, optionalPredicate, (LambdaExpression) null)
    {
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
      return (ResultOperatorBase) new LongCountResultOperator();
    }
  }
}
