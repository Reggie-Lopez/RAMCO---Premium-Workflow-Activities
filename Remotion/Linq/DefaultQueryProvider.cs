// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.DefaultQueryProvider
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.Structure;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public sealed class DefaultQueryProvider : QueryProviderBase
  {
    private readonly Type _queryableType;

    public DefaultQueryProvider(
      Type queryableType,
      IQueryParser queryParser,
      IQueryExecutor executor)
      : base(ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser), ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor))
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (queryableType), queryableType);
      this.CheckQueryableType(queryableType);
      this._queryableType = queryableType;
    }

    private void CheckQueryableType(Type queryableType)
    {
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (queryableType), queryableType, typeof (IQueryable));
      TypeInfo typeInfo = queryableType.GetTypeInfo();
      if (!typeInfo.IsGenericTypeDefinition)
        throw new ArgumentException(string.Format("Expected the generic type definition of an implementation of IQueryable<T>, but was '{0}'.", (object) queryableType), nameof (queryableType));
      int length = typeInfo.GenericTypeParameters.Length;
      if (length != 1)
        throw new ArgumentException(string.Format("Expected the generic type definition of an implementation of IQueryable<T> with exactly one type argument, but found {0} arguments on '{1}.", (object) length, (object) queryableType), nameof (queryableType));
    }

    public Type QueryableType => this._queryableType;

    public override IQueryable<T> CreateQuery<T>(Expression expression) => (IQueryable<T>) Activator.CreateInstance(this.QueryableType.MakeGenericType(typeof (T)), (object) this, (object) expression);
  }
}
