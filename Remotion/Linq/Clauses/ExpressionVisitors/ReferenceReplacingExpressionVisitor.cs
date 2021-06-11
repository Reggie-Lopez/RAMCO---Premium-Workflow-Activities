// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionVisitors.ReferenceReplacingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class ReferenceReplacingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly QuerySourceMapping _querySourceMapping;
    private readonly bool _throwOnUnmappedReferences;

    public static Expression ReplaceClauseReferences(
      Expression expression,
      QuerySourceMapping querySourceMapping,
      bool throwOnUnmappedReferences)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      return new ReferenceReplacingExpressionVisitor(querySourceMapping, throwOnUnmappedReferences).Visit(expression);
    }

    private ReferenceReplacingExpressionVisitor(
      QuerySourceMapping querySourceMapping,
      bool throwOnUnmappedReferences)
    {
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      this._querySourceMapping = querySourceMapping;
      this._throwOnUnmappedReferences = throwOnUnmappedReferences;
    }

    protected internal override Expression VisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      if (this._querySourceMapping.ContainsMapping(expression.ReferencedQuerySource))
        return this._querySourceMapping.GetExpression(expression.ReferencedQuerySource);
      if (this._throwOnUnmappedReferences)
        throw new InvalidOperationException("Cannot replace reference to clause '" + expression.ReferencedQuerySource.ItemName + "', there is no mapped expression.");
      return (Expression) expression;
    }

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      expression.QueryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionVisitor.ReplaceClauseReferences(ex, this._querySourceMapping, this._throwOnUnmappedReferences)));
      return (Expression) expression;
    }
  }
}
