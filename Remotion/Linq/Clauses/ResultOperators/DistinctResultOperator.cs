// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.DistinctResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class DistinctResultOperator : SequenceTypePreservingResultOperatorBase
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new DistinctResultOperator();

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input) => new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Distinct<T>().AsQueryable<T>(), this.GetOutputDataInfo(input.DataInfo));

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Distinct()";
  }
}
