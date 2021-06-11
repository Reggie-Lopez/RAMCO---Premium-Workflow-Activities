// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations.InvocationOfLambdaExpressionTransformer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
  [ComVisible(true)]
  public class InvocationOfLambdaExpressionTransformer : IExpressionTransformer<InvocationExpression>
  {
    public ExpressionType[] SupportedExpressionTypes => new ExpressionType[1]
    {
      ExpressionType.Invoke
    };

    public Expression Transform(InvocationExpression expression)
    {
      ArgumentUtility.CheckNotNull<InvocationExpression>(nameof (expression), expression);
      return this.StripTrivialConversions(expression.Expression) is LambdaExpression lambdaExpression ? this.InlineLambdaExpression(lambdaExpression, expression.Arguments) : (Expression) expression;
    }

    private Expression StripTrivialConversions(Expression invokedExpression)
    {
      while (invokedExpression.NodeType == ExpressionType.Convert && invokedExpression.Type == ((UnaryExpression) invokedExpression).Operand.Type && ((UnaryExpression) invokedExpression).Method == (MethodInfo) null)
        invokedExpression = ((UnaryExpression) invokedExpression).Operand;
      return invokedExpression;
    }

    private Expression InlineLambdaExpression(
      LambdaExpression lambdaExpression,
      ReadOnlyCollection<Expression> arguments)
    {
      Dictionary<Expression, Expression> dictionary = new Dictionary<Expression, Expression>(arguments.Count);
      Expression body = lambdaExpression.Body;
      for (int index = 0; index < lambdaExpression.Parameters.Count; ++index)
        dictionary.Add((Expression) lambdaExpression.Parameters[index], arguments[index]);
      return MultiReplacingExpressionVisitor.Replace((IDictionary<Expression, Expression>) dictionary, body);
    }
  }
}
