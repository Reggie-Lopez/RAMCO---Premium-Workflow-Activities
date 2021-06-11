// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.DictionaryEntryNewExpressionTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class DictionaryEntryNewExpressionTransformer : MemberAddingNewExpressionTransformerBase
  {
    protected override MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments)
    {
      return new MemberInfo[2]
      {
        this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Key"),
        this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Value")
      };
    }

    protected override bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments)
    {
      return instantiatedType == typeof (DictionaryEntry) && arguments.Count == 2;
    }
  }
}
