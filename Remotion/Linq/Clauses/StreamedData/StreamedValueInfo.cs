// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedValueInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.StreamedData
{
  [ComVisible(true)]
  public abstract class StreamedValueInfo : IStreamedDataInfo, IEquatable<IStreamedDataInfo>
  {
    internal StreamedValueInfo(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      this.DataType = dataType;
    }

    public Type DataType { get; private set; }

    public abstract IStreamedData ExecuteQueryModel(
      QueryModel queryModel,
      IQueryExecutor executor);

    protected abstract StreamedValueInfo CloneWithNewDataType(Type dataType);

    public virtual IStreamedDataInfo AdjustDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      return dataType.GetTypeInfo().IsAssignableFrom(this.DataType.GetTypeInfo()) ? (IStreamedDataInfo) this.CloneWithNewDataType(dataType) : throw new ArgumentException(string.Format("'{0}' cannot be used as the new data type for a value of type '{1}'.", (object) dataType, (object) this.DataType), nameof (dataType));
    }

    public override sealed bool Equals(object obj) => this.Equals(obj as IStreamedDataInfo);

    public virtual bool Equals(IStreamedDataInfo obj) => obj != null && !(this.GetType() != obj.GetType()) && this.DataType.Equals(((StreamedValueInfo) obj).DataType);

    public override int GetHashCode() => this.DataType.GetHashCode();
  }
}
