// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.SequenceTypePreservingResultOperatorBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Utilities;
using System.Linq;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public abstract class SequenceTypePreservingResultOperatorBase : 
    SequenceFromSequenceResultOperatorBase
  {
    public override sealed IStreamedDataInfo GetOutputDataInfo(
      IStreamedDataInfo inputInfo)
    {
      return (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));
    }

    protected StreamedSequenceInfo GetOutputDataInfo(
      StreamedSequenceInfo inputSequenceInfo)
    {
      ArgumentUtility.CheckNotNull<StreamedSequenceInfo>(nameof (inputSequenceInfo), inputSequenceInfo);
      return new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(inputSequenceInfo.ResultItemType), inputSequenceInfo.ItemExpression);
    }
  }
}
