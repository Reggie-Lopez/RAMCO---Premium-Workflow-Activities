// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.NullableValueTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class NullableValueTransformer : IExpressionTransformer<MemberExpression>
  {
    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[1]
    {
      ExpressionType.MemberAccess
    };

    public Expression Transform(MemberExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (expression), expression);
      if (expression.Member.Name == "Value" && this.IsDeclaredByNullableType(expression.Member))
        return (Expression) Expression.Convert(expression.Expression, expression.Type);
      return expression.Member.Name == "HasValue" && this.IsDeclaredByNullableType(expression.Member) ? (Expression) Expression.NotEqual(expression.Expression, (Expression) Expression.Constant((object) null, expression.Member.DeclaringType)) : (Expression) expression;
    }

    private bool IsDeclaredByNullableType(MemberInfo memberInfo) => memberInfo.DeclaringType.GetTypeInfo().IsGenericType && memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof (Nullable<>);
  }
}
