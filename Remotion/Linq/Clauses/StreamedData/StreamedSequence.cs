// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedSequence
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.StreamedData
{
  [ComVisible(true)]
  public sealed class StreamedSequence : IStreamedData
  {
    public StreamedSequence([NotNull] IEnumerable sequence, [NotNull] StreamedSequenceInfo streamedSequenceInfo)
    {
      ArgumentUtility.CheckNotNull<StreamedSequenceInfo>(nameof (streamedSequenceInfo), streamedSequenceInfo);
      ArgumentUtility.CheckNotNullAndType(nameof (sequence), (object) sequence, streamedSequenceInfo.DataType);
      this.DataInfo = streamedSequenceInfo;
      this.Sequence = sequence;
    }

    [NotNull]
    public StreamedSequenceInfo DataInfo { get; private set; }

    object IStreamedData.Value => (object) this.Sequence;

    IStreamedDataInfo IStreamedData.DataInfo => (IStreamedDataInfo) this.DataInfo;

    [NotNull]
    public IEnumerable Sequence { get; private set; }

    [NotNull]
    public IEnumerable<T> GetTypedSequence<T>()
    {
      try
      {
        return (IEnumerable<T>) this.Sequence;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as a sequence with item type '{0}' because its items are of type '{1}'.", (object) typeof (T).FullName, (object) this.DataInfo.ResultItemType.FullName), (Exception) ex);
      }
    }
  }
}
