// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionVisitors.ReverseResolvingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class ReverseResolvingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly Expression _itemExpression;
    private readonly ParameterExpression _lambdaParameter;

    public static LambdaExpression ReverseResolve(
      Expression itemExpression,
      Expression resolvedExpression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (resolvedExpression), resolvedExpression);
      ParameterExpression lambdaParameter = Expression.Parameter(itemExpression.Type, "input");
      return Expression.Lambda(new ReverseResolvingExpressionVisitor(itemExpression, lambdaParameter).Visit(resolvedExpression), lambdaParameter);
    }

    public static LambdaExpression ReverseResolveLambda(
      Expression itemExpression,
      LambdaExpression resolvedExpression,
      int parameterInsertionPosition)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (resolvedExpression), resolvedExpression);
      if (parameterInsertionPosition < 0 || parameterInsertionPosition > resolvedExpression.Parameters.Count)
        throw new ArgumentOutOfRangeException(nameof (parameterInsertionPosition));
      ParameterExpression lambdaParameter = Expression.Parameter(itemExpression.Type, "input");
      Expression body = new ReverseResolvingExpressionVisitor(itemExpression, lambdaParameter).Visit(resolvedExpression.Body);
      List<ParameterExpression> parameterExpressionList = new List<ParameterExpression>((IEnumerable<ParameterExpression>) resolvedExpression.Parameters);
      parameterExpressionList.Insert(parameterInsertionPosition, lambdaParameter);
      return Expression.Lambda(body, parameterExpressionList.ToArray());
    }

    private ReverseResolvingExpressionVisitor(
      Expression itemExpression,
      ParameterExpression lambdaParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (lambdaParameter), lambdaParameter);
      this._itemExpression = itemExpression;
      this._lambdaParameter = lambdaParameter;
    }

    protected internal override Expression VisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      try
      {
        return AccessorFindingExpressionVisitor.FindAccessorLambda((Expression) expression, this._itemExpression, this._lambdaParameter).Body;
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot create a LambdaExpression that retrieves the value of '{0}' from items with a structure of '{1}'. The item expression does not contain the value or it is too complex.", (object) expression.BuildString(), (object) this._itemExpression.BuildString()), (Exception) ex);
      }
    }
  }
}
