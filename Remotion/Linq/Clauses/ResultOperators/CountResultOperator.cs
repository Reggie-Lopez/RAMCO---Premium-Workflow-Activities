// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.CountResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class CountResultOperator : ValueFromSequenceResultOperatorBase
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new CountResultOperator();

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input) => new StreamedValue((object) input.GetTypedSequence<T>().Count<T>(), this.GetOutputDataInfo(input.DataInfo));

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo sequenceInfo) => (StreamedValueInfo) new StreamedScalarValueInfo(typeof (int));

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Count()";
  }
}
