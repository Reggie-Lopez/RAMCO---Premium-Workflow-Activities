// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryProviderBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public abstract class QueryProviderBase : IQueryProvider
  {
    private static readonly MethodInfo s_genericCreateQueryMethod = typeof (QueryProviderBase).GetRuntimeMethods().Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "CreateQuery" && m.IsGenericMethod));
    private readonly IQueryParser _queryParser;
    private readonly IQueryExecutor _executor;

    protected QueryProviderBase(IQueryParser queryParser, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      this._queryParser = queryParser;
      this._executor = executor;
    }

    public IQueryParser QueryParser => this._queryParser;

    public IQueryExecutor Executor => this._executor;

    [Obsolete("This property has been replaced by the QueryParser property. Use QueryParser instead. (1.13.92)", true)]
    public ExpressionTreeParser ExpressionTreeParser => throw new NotImplementedException();

    public IQueryable CreateQuery(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      Type genericIenumerable = ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(expression.Type, nameof (expression));
      return (IQueryable) QueryProviderBase.s_genericCreateQueryMethod.MakeGenericMethod(genericIenumerable).Invoke((object) this, new object[1]
      {
        (object) expression
      });
    }

    public abstract IQueryable<T> CreateQuery<T>(Expression expression);

    public virtual IStreamedData Execute(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this.GenerateQueryModel(expression).Execute(this.Executor);
    }

    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return (TResult) this.Execute(expression).Value;
    }

    object IQueryProvider.Execute(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this.Execute(expression).Value;
    }

    public virtual QueryModel GenerateQueryModel(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this._queryParser.GetParsedQuery(expression);
    }
  }
}
