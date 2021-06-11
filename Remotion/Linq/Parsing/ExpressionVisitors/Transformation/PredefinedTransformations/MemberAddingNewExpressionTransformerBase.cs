// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.MemberAddingNewExpressionTransformerBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public abstract class MemberAddingNewExpressionTransformerBase : 
    IExpressionTransformer<NewExpression>
  {
    protected abstract bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments);

    protected abstract MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments);

    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[1]
    {
      ExpressionType.New
    };

    public Expression Transform(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      if (expression.Members != null || !this.CanAddMembers(expression.Type, expression.Arguments))
        return (Expression) expression;
      MemberInfo[] members = this.GetMembers(expression.Constructor, expression.Arguments);
      return (Expression) Expression.New(expression.Constructor, RelinqExpressionVisitor.AdjustArgumentsForNewExpression((IList<Expression>) expression.Arguments, (IList<MemberInfo>) members), members);
    }

    protected MemberInfo GetMemberForNewExpression(
      Type instantiatedType,
      string propertyName)
    {
      return (MemberInfo) instantiatedType.GetRuntimeProperty(propertyName).GetGetMethod(true);
    }
  }
}
