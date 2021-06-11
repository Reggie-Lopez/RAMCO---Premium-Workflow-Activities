// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.VBCompareStringExpressionTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class VBCompareStringExpressionTransformer : IExpressionTransformer<BinaryExpression>
  {
    private const string c_vbOperatorsClassName = "Microsoft.VisualBasic.CompilerServices.Operators";
    private const string c_vbCompareStringOperatorMethodName = "CompareString";
    private static readonly MethodInfo s_stringCompareToMethod = typeof (string).GetRuntimeMethodChecked("CompareTo", new Type[1]
    {
      typeof (string)
    });

    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[6]
    {
      ExpressionType.Equal,
      ExpressionType.NotEqual,
      ExpressionType.LessThan,
      ExpressionType.GreaterThan,
      ExpressionType.LessThanOrEqual,
      ExpressionType.GreaterThanOrEqual
    };

    public Expression Transform(BinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<BinaryExpression>(nameof (expression), expression);
      if (!(expression.Left is MethodCallExpression left) || !this.IsVBOperator(left.Method, "CompareString"))
        return (Expression) expression;
      Expression right = expression.Right;
      ConstantExpression leftSideArgument2AsConstantExpression = left.Arguments[2] as ConstantExpression;
      return this.GetExpressionForNodeType(expression, left, leftSideArgument2AsConstantExpression);
    }

    private Expression GetExpressionForNodeType(
      BinaryExpression expression,
      MethodCallExpression leftSideAsMethodCallExpression,
      ConstantExpression leftSideArgument2AsConstantExpression)
    {
      switch (expression.NodeType)
      {
        case ExpressionType.Equal:
          return (Expression) new VBStringComparisonExpression((Expression) Expression.Equal(leftSideAsMethodCallExpression.Arguments[0], leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
        case ExpressionType.NotEqual:
          return (Expression) new VBStringComparisonExpression((Expression) Expression.NotEqual(leftSideAsMethodCallExpression.Arguments[0], leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
        default:
          VBStringComparisonExpression comparisonExpression = new VBStringComparisonExpression((Expression) Expression.Call(leftSideAsMethodCallExpression.Arguments[0], VBCompareStringExpressionTransformer.s_stringCompareToMethod, leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
          if (expression.NodeType == ExpressionType.GreaterThan)
            return (Expression) Expression.GreaterThan((Expression) comparisonExpression, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.GreaterThanOrEqual)
            return (Expression) Expression.GreaterThanOrEqual((Expression) comparisonExpression, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.LessThan)
            return (Expression) Expression.LessThan((Expression) comparisonExpression, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.LessThanOrEqual)
            return (Expression) Expression.LessThanOrEqual((Expression) comparisonExpression, (Expression) Expression.Constant((object) 0));
          throw new NotSupportedException(string.Format("Binary expression with node type '{0}' is not supported in a VB string comparison.", (object) expression.NodeType));
      }
    }

    private bool IsVBOperator(MethodInfo operatorMethod, string operatorName) => operatorMethod.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators" && operatorMethod.Name == operatorName;
  }
}
