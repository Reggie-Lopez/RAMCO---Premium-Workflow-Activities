// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.TupleNewExpressionTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class TupleNewExpressionTransformer : MemberAddingNewExpressionTransformerBase
  {
    protected override bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments)
    {
      return instantiatedType.Namespace == "System" && instantiatedType.Name.StartsWith("Tuple`");
    }

    protected override MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments)
    {
      return arguments.Select<Expression, MemberInfo>((Func<Expression, int, MemberInfo>) ((expr, i) => this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Item" + (object) (i + 1)))).ToArray<MemberInfo>();
    }
  }
}
