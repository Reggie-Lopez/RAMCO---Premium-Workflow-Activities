// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AverageResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class AverageResultOperator : ValueFromSequenceResultOperatorBase
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new AverageResultOperator();

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      MethodInfo runtimeMethod = typeof (Enumerable).GetRuntimeMethod("Average", new Type[1]
      {
        typeof (IEnumerable<T>)
      });
      if (runtimeMethod == (MethodInfo) null)
        throw new NotSupportedException(string.Format("Cannot calculate the average of objects of type '{0}' in memory.", (object) typeof (T).FullName));
      return new StreamedValue(runtimeMethod.Invoke((object) null, (object[]) new IEnumerable<T>[1]
      {
        input.GetTypedSequence<T>()
      }), this.GetOutputDataInfo(input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo sequenceInfo) => (StreamedValueInfo) new StreamedScalarValueInfo(this.GetResultType(sequenceInfo.ResultItemType));

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Average()";

    private Type GetResultType(Type inputItemType)
    {
      if (inputItemType == typeof (int) || inputItemType == typeof (long))
        return typeof (double);
      return inputItemType == typeof (int?) || inputItemType == typeof (long?) ? typeof (double?) : inputItemType;
    }
  }
}
