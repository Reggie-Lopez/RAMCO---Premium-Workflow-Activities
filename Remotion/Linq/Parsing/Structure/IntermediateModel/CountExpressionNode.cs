// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.CountExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class CountExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static IEnumerable<MethodInfo> GetSupportedMethods()
    {
      foreach (MethodInfo whereNameMatch in ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Count"))
        yield return whereNameMatch;
      yield return typeof (List<>).GetRuntimeMethod("get_Count", new Type[0]);
      yield return typeof (ICollection<>).GetRuntimeMethod("get_Count", new Type[0]);
      yield return typeof (ICollection).GetRuntimeMethod("get_Count", new Type[0]);
      yield return typeof (Array).GetRuntimeMethod("get_Length", new Type[0]);
      Type arrayListType = Type.GetType("System.Collections.ArrayList", false);
      if (arrayListType != (Type) null)
      {
        MethodInfo methodInfo = arrayListType.GetRuntimeMethod("get_Count", new Type[0]);
        Assertion.IsNotNull<MethodInfo>(methodInfo, "Property 'Count' was not found on type 'System.Collections.ArrayList'.");
        yield return methodInfo;
      }
    }

    public CountExpressionNode(
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
      return (ResultOperatorBase) new CountResultOperator();
    }
  }
}
