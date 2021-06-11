// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.TupleExpressionBuilder
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing
{
  [ComVisible(true)]
  public static class TupleExpressionBuilder
  {
    public static Expression AggregateExpressionsIntoTuple(
      IEnumerable<Expression> expressions)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<Expression>>(nameof (expressions), expressions);
      return expressions.Reverse<Expression>().Aggregate<Expression>((Func<Expression, Expression, Expression>) ((current, expression) => TupleExpressionBuilder.CreateTupleExpression(expression, current)));
    }

    public static IEnumerable<Expression> GetExpressionsFromTuple(
      Expression tupleExpression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (tupleExpression), tupleExpression);
      for (; tupleExpression.Type.GetTypeInfo().IsGenericType && tupleExpression.Type.GetGenericTypeDefinition() == typeof (KeyValuePair<,>); tupleExpression = (Expression) Expression.MakeMemberAccess(tupleExpression, (MemberInfo) tupleExpression.Type.GetRuntimeProperty("Value")))
        yield return (Expression) Expression.MakeMemberAccess(tupleExpression, (MemberInfo) tupleExpression.Type.GetRuntimeProperty("Key"));
      yield return tupleExpression;
    }

    private static Expression CreateTupleExpression(Expression left, Expression right)
    {
      Type type = typeof (KeyValuePair<,>).MakeGenericType(left.Type, right.Type);
      return (Expression) Expression.New(type.GetTypeInfo().DeclaredConstructors.Single<ConstructorInfo>(), (IEnumerable<Expression>) new Expression[2]
      {
        left,
        right
      }, (MemberInfo) type.GetRuntimeProperty("Key").GetGetMethod(true), (MemberInfo) type.GetRuntimeProperty("Value").GetGetMethod(true));
    }
  }
}
