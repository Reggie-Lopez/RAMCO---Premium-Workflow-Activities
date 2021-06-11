// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.UnionExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class UnionExpressionNode : QuerySourceSetOperationExpressionNodeBase
  {
    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Union").WithoutEqualityComparer();

    public UnionExpressionNode(MethodCallExpressionParseInfo parseInfo, Expression source2)
      : base(parseInfo, source2)
    {
    }

    protected override ResultOperatorBase CreateSpecificResultOperator() => (ResultOperatorBase) new UnionResultOperator(this.AssociatedIdentifier, this.ItemType, this.Source2);
  }
}
