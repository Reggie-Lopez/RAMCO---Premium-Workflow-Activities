// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryableBase`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.Structure;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public abstract class QueryableBase<T> : 
    IOrderedQueryable<T>,
    IQueryable<T>,
    IEnumerable<T>,
    IOrderedQueryable,
    IQueryable,
    IEnumerable
  {
    private readonly IQueryProvider _queryProvider;

    protected QueryableBase(IQueryParser queryParser, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser);
      this._queryProvider = (IQueryProvider) new DefaultQueryProvider(this.GetType().GetGenericTypeDefinition(), queryParser, executor);
      this.Expression = (Expression) Expression.Constant((object) this);
    }

    protected QueryableBase(IQueryProvider provider)
    {
      ArgumentUtility.CheckNotNull<IQueryProvider>(nameof (provider), provider);
      this._queryProvider = provider;
      this.Expression = (Expression) Expression.Constant((object) this);
    }

    protected QueryableBase(IQueryProvider provider, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQueryProvider>(nameof (provider), provider);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (expression), expression.Type, typeof (IEnumerable<T>));
      this._queryProvider = provider;
      this.Expression = expression;
    }

    public Expression Expression { get; private set; }

    public IQueryProvider Provider => this._queryProvider;

    public Type ElementType => typeof (T);

    public IEnumerator<T> GetEnumerator() => this._queryProvider.Execute<IEnumerable<T>>(this.Expression).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this._queryProvider.Execute<IEnumerable>(this.Expression).GetEnumerator();
  }
}
