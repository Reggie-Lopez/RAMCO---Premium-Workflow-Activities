// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.ReplacingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class ReplacingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly Expression _replacedExpression;
    private readonly Expression _replacementExpression;

    public static Expression Replace(
      Expression replacedExpression,
      Expression replacementExpression,
      Expression sourceTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (replacedExpression), replacedExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (replacementExpression), replacementExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (sourceTree), sourceTree);
      return new ReplacingExpressionVisitor(replacedExpression, replacementExpression).Visit(sourceTree);
    }

    private ReplacingExpressionVisitor(
      Expression replacedExpression,
      Expression replacementExpression)
    {
      this._replacedExpression = replacedExpression;
      this._replacementExpression = replacementExpression;
    }

    public override Expression Visit(Expression expression) => object.Equals((object) expression, (object) this._replacedExpression) ? this._replacementExpression : base.Visit(expression);

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionVisitor) this).Visit));
      return (Expression) expression;
    }
  }
}
