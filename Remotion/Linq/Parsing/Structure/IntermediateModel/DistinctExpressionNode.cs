// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.DistinctExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class DistinctExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Distinct").WithoutEqualityComparer();

    public DistinctExpressionNode(MethodCallExpressionParseInfo parseInfo)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
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

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new DistinctResultOperator();
    }
  }
}
