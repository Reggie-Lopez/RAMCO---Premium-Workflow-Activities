// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.MultiReplacingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class MultiReplacingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly IDictionary<Expression, Expression> _expressionMapping;

    public static Expression Replace(
      IDictionary<Expression, Expression> expressionMapping,
      Expression sourceTree)
    {
      ArgumentUtility.CheckNotNull<IDictionary<Expression, Expression>>(nameof (expressionMapping), expressionMapping);
      ArgumentUtility.CheckNotNull<Expression>(nameof (sourceTree), sourceTree);
      return new MultiReplacingExpressionVisitor(expressionMapping).Visit(sourceTree);
    }

    private MultiReplacingExpressionVisitor(
      IDictionary<Expression, Expression> expressionMapping)
    {
      ArgumentUtility.CheckNotNull<IDictionary<Expression, Expression>>(nameof (expressionMapping), expressionMapping);
      this._expressionMapping = expressionMapping;
    }

    public override Expression Visit(Expression expression)
    {
      Expression expression1;
      return expression != null && this._expressionMapping.TryGetValue(expression, out expression1) ? expression1 : base.Visit(expression);
    }

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionVisitor) this).Visit));
      return (Expression) expression;
    }
  }
}
