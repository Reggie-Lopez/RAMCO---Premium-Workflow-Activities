// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.SelectClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class SelectClause : IClause
  {
    private Expression _selector;

    public SelectClause(Expression selector)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (selector), selector);
      this._selector = selector;
    }

    public Expression Selector
    {
      get => this._selector;
      set => this._selector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitSelectClause(this, queryModel);
    }

    public SelectClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new SelectClause(this.Selector);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Selector = transformation(this.Selector);
    }

    public override string ToString() => "select " + this.Selector.BuildString();

    public StreamedSequenceInfo GetOutputDataInfo() => new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.Selector.Type), this.Selector);
  }
}
