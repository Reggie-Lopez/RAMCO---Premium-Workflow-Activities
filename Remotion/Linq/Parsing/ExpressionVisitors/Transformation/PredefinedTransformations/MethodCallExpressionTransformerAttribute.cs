// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.MethodCallExpressionTransformerAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  [ComVisible(true)]
  public class MethodCallExpressionTransformerAttribute : 
    Attribute,
    AttributeEvaluatingExpressionTransformer.IMethodCallExpressionTransformerAttribute
  {
    private readonly Type _transformerType;

    public MethodCallExpressionTransformerAttribute(Type transformerType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (transformerType), transformerType);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (transformerType), transformerType, typeof (IExpressionTransformer<MethodCallExpression>));
      this._transformerType = transformerType;
    }

    public Type TransformerType => this._transformerType;

    public IExpressionTransformer<MethodCallExpression> GetExpressionTransformer(
      MethodCallExpression expression)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expression), expression);
      try
      {
        return (IExpressionTransformer<MethodCallExpression>) Activator.CreateInstance(this._transformerType);
      }
      catch (MissingMemberException ex)
      {
        throw new InvalidOperationException(string.Format("The method call transformer '{0}' has no public default constructor and therefore cannot be used with the MethodCallExpressionTransformerAttribute.", (object) this._transformerType), (Exception) ex);
      }
    }
  }
}
