// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.AttributeEvaluatingExpressionTransformer
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

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class AttributeEvaluatingExpressionTransformer : IExpressionTransformer<Expression>
  {
    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[2]
    {
      ExpressionType.Call,
      ExpressionType.MemberAccess
    };

    public Expression Transform(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (expression is MemberExpression memberExpression && (object) (memberExpression.Member as PropertyInfo) != null)
      {
        PropertyInfo member = (PropertyInfo) memberExpression.Member;
        MethodInfo getMethod = member.GetGetMethod(true);
        Assertion.IsNotNull<MethodInfo>(getMethod, "No get-method was found for property '{0}' declared on type '{1}'.", (object) member.Name, (object) member.DeclaringType);
        AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute transformerProvider = AttributeEvaluatingExpressionTransformer.GetTransformerProvider(getMethod);
        if (transformerProvider != null)
          return AttributeEvaluatingExpressionTransformer.ApplyTransformer(transformerProvider, Expression.Call(memberExpression.Expression, getMethod));
      }
      if (expression is MethodCallExpression methodCallExpression)
      {
        AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute transformerProvider = AttributeEvaluatingExpressionTransformer.GetTransformerProvider(methodCallExpression.Method);
        if (transformerProvider != null)
          return AttributeEvaluatingExpressionTransformer.ApplyTransformer(transformerProvider, methodCallExpression);
      }
      return expression;
    }

    private static AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute GetTransformerProvider(
      MethodInfo methodInfo)
    {
      AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute[] array = methodInfo.GetCustomAttributes(true).OfType<AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute>().ToArray<AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute>();
      return array.Length <= 1 ? ((IEnumerable<AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute>) array).SingleOrDefault<AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute>() : throw new InvalidOperationException(string.Format("There is more than one attribute providing transformers declared for method '{0}.{1}'.", (object) methodInfo.DeclaringType, (object) methodInfo.Name));
    }

    private static Expression ApplyTransformer(
      AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute provider,
      MethodCallExpression methodCallExpression)
    {
      return (provider.GetExpressionTransformer(methodCallExpression) ?? throw new InvalidOperationException(string.Format("The '{0}' on method '{1}.{2}' returned 'null' instead of a transformer.", (object) provider.GetType().Name, (object) methodCallExpression.Method.DeclaringType, (object) methodCallExpression.Method.Name))).Transform(methodCallExpression);
    }

    public interface IMethodCallExpressionTransformerAttribute
    {
      IExpressionTransformer<MethodCallExpression> GetExpressionTransformer(
        MethodCallExpression expression);
    }
  }
}
