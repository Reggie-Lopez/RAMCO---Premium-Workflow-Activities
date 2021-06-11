// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Ordering
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class Ordering
  {
    private Expression _expression;

    public Ordering(Expression expression, OrderingDirection direction)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      this._expression = expression;
      this.OrderingDirection = direction;
    }

    public Expression Expression
    {
      get => this._expression;
      set => this._expression = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public OrderingDirection OrderingDirection { get; set; }

    public void Accept(
      IQueryModelVisitor visitor,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      visitor.VisitOrdering(this, queryModel, orderByClause, index);
    }

    public Ordering Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new Ordering(this.Expression, this.OrderingDirection);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Expression = transformation(this.Expression);
    }

    public override string ToString() => this.Expression.BuildString() + (this.OrderingDirection == OrderingDirection.Asc ? " asc" : " desc");
  }
}
