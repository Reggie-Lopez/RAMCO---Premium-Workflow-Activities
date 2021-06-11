// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.WhereClause
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
  public sealed class WhereClause : IBodyClause, IClause
  {
    private Expression _predicate;

    public WhereClause(Expression predicate)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (predicate), predicate);
      this._predicate = predicate;
    }

    public Expression Predicate
    {
      get => this._predicate;
      set => this._predicate = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitWhereClause(this, queryModel, index);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Predicate = transformation(this.Predicate);
    }

    public WhereClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new WhereClause(this.Predicate);
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext) => (IBodyClause) this.Clone(cloneContext);

    public override string ToString() => "where " + this.Predicate.BuildString();
  }
}
