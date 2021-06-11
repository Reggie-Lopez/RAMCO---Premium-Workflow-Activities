// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.VBInformationIsNothingExpressionTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class VBInformationIsNothingExpressionTransformer : 
    IExpressionTransformer<MethodCallExpression>
  {
    private const string c_vbInformationClassName = "Microsoft.VisualBasic.Information";
    private const string c_vbIsNothingMethodName = "IsNothing";

    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[1]
    {
      ExpressionType.Call
    };

    public Expression Transform(MethodCallExpression expression)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expression), expression);
      return this.IsVBIsNothing(expression.Method) ? (Expression) Expression.Equal(expression.Arguments[0], (Expression) Expression.Constant((object) null)) : (Expression) expression;
    }

    private bool IsVBIsNothing(MethodInfo operatorMethod) => operatorMethod.DeclaringType.FullName == "Microsoft.VisualBasic.Information" && operatorMethod.Name == "IsNothing";
  }
}
