// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedValue
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.StreamedData
{
  [ComVisible(true)]
  public sealed class StreamedValue : IStreamedData
  {
    public StreamedValue(object value, StreamedValueInfo streamedValueInfo)
    {
      ArgumentUtility.CheckNotNull<StreamedValueInfo>(nameof (streamedValueInfo), streamedValueInfo);
      ArgumentUtility.CheckType(nameof (value), value, streamedValueInfo.DataType);
      this.Value = value;
      this.DataInfo = streamedValueInfo;
    }

    public StreamedValueInfo DataInfo { get; private set; }

    public object Value { get; private set; }

    IStreamedDataInfo IStreamedData.DataInfo => (IStreamedDataInfo) this.DataInfo;

    public T GetTypedValue<T>()
    {
      try
      {
        return (T) this.Value;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as type '{0}' because it is of type '{1}'.", (object) typeof (T).FullName, (object) this.Value.GetType().FullName), (Exception) ex);
      }
      catch (NullReferenceException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as type '{0}' because it is null.", (object) typeof (T).FullName), (Exception) ex);
      }
    }
  }
}
