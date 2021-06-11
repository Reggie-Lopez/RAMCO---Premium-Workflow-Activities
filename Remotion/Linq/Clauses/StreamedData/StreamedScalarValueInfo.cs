// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedScalarValueInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.StreamedData
{
  [ComVisible(true)]
  public sealed class StreamedScalarValueInfo : StreamedValueInfo
  {
    private static readonly MethodInfo s_executeMethod = typeof (StreamedScalarValueInfo).GetRuntimeMethodChecked("ExecuteScalarQueryModel", new Type[2]
    {
      typeof (QueryModel),
      typeof (IQueryExecutor)
    });

    public StreamedScalarValueInfo(Type dataType)
      : base(dataType)
    {
    }

    public override IStreamedData ExecuteQueryModel(
      QueryModel queryModel,
      IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (IStreamedData) new StreamedValue(((Func<QueryModel, IQueryExecutor, object>) StreamedScalarValueInfo.s_executeMethod.MakeGenericMethod(this.DataType).CreateDelegate(typeof (Func<QueryModel, IQueryExecutor, object>), (object) this))(queryModel, executor), (StreamedValueInfo) this);
    }

    protected override StreamedValueInfo CloneWithNewDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      return (StreamedValueInfo) new StreamedScalarValueInfo(dataType);
    }

    public object ExecuteScalarQueryModel<T>(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (object) executor.ExecuteScalar<T>(queryModel);
    }
  }
}
