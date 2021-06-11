// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ValueFromSequenceResultOperatorBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public abstract class ValueFromSequenceResultOperatorBase : ResultOperatorBase
  {
    private static readonly MethodInfo s_executeMethod = typeof (ValueFromSequenceResultOperatorBase).GetRuntimeMethodChecked("ExecuteInMemory", new Type[1]
    {
      typeof (StreamedSequence)
    });

    public abstract StreamedValue ExecuteInMemory<T>(StreamedSequence sequence);

    public override sealed IStreamedData ExecuteInMemory(IStreamedData input)
    {
      StreamedSequence streamedSequence = ArgumentUtility.CheckNotNullAndType<StreamedSequence>(nameof (input), (object) input);
      return (IStreamedData) this.InvokeExecuteMethod(ValueFromSequenceResultOperatorBase.s_executeMethod.MakeGenericMethod(streamedSequence.DataInfo.ResultItemType), (object) streamedSequence);
    }
  }
}
