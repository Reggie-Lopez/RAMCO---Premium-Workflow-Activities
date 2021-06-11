// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionVisitors.CloningExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class CloningExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly QuerySourceMapping _querySourceMapping;

    public static Expression AdjustExpressionAfterCloning(
      Expression expression,
      QuerySourceMapping querySourceMapping)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      return new CloningExpressionVisitor(querySourceMapping).Visit(expression);
    }

    private CloningExpressionVisitor(QuerySourceMapping querySourceMapping) => this._querySourceMapping = querySourceMapping;

    protected internal override Expression VisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      return this._querySourceMapping.ContainsMapping(expression.ReferencedQuerySource) ? this._querySourceMapping.GetExpression(expression.ReferencedQuerySource) : (Expression) expression;
    }

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      return (Expression) new SubQueryExpression(expression.QueryModel.Clone(this._querySourceMapping));
    }
  }
}
