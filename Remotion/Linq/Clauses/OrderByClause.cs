// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.OrderByClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class OrderByClause : IBodyClause, IClause
  {
    public OrderByClause()
    {
      this.Orderings = new ObservableCollection<Ordering>();
      this.Orderings.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Orderings_CollectionChanged);
    }

    public ObservableCollection<Ordering> Orderings { get; private set; }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitOrderByClause(this, queryModel, index);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      foreach (Ordering ordering in (Collection<Ordering>) this.Orderings)
        ordering.TransformExpressions(transformation);
    }

    public OrderByClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      OrderByClause orderByClause = new OrderByClause();
      foreach (Ordering ordering1 in (Collection<Ordering>) this.Orderings)
      {
        Ordering ordering2 = ordering1.Clone(cloneContext);
        orderByClause.Orderings.Add(ordering2);
      }
      return orderByClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext) => (IBodyClause) this.Clone(cloneContext);

    public override string ToString() => "orderby " + StringUtility.Join<Ordering>(", ", (IEnumerable<Ordering>) this.Orderings);

    private void Orderings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      ArgumentUtility.CheckNotNull<NotifyCollectionChangedEventArgs>(nameof (e), e);
      ArgumentUtility.CheckItemsNotNullAndType<IList>("e.NewItems", e.NewItems, typeof (Ordering));
    }
  }
}
