// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ChoiceResultOperatorBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Utilities;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public abstract class ChoiceResultOperatorBase : ValueFromSequenceResultOperatorBase
  {
    protected ChoiceResultOperatorBase(bool returnDefaultWhenEmpty) => this.ReturnDefaultWhenEmpty = returnDefaultWhenEmpty;

    public bool ReturnDefaultWhenEmpty { get; set; }

    public override sealed IStreamedDataInfo GetOutputDataInfo(
      IStreamedDataInfo inputInfo)
    {
      return (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));
    }

    protected StreamedValueInfo GetOutputDataInfo(
      [NotNull] StreamedSequenceInfo inputSequenceInfo)
    {
      ArgumentUtility.CheckNotNull<StreamedSequenceInfo>(nameof (inputSequenceInfo), inputSequenceInfo);
      return (StreamedValueInfo) new StreamedSingleValueInfo(inputSequenceInfo.ResultItemType, this.ReturnDefaultWhenEmpty);
    }
  }
}
