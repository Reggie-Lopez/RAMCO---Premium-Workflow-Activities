// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.FirstResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class FirstResultOperator : ChoiceResultOperatorBase
  {
    public FirstResultOperator(bool returnDefaultWhenEmpty)
      : base(returnDefaultWhenEmpty)
    {
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new FirstResultOperator(this.ReturnDefaultWhenEmpty);

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      IEnumerable<T> typedSequence = input.GetTypedSequence<T>();
      return new StreamedValue((object) (this.ReturnDefaultWhenEmpty ? typedSequence.FirstOrDefault<T>() : typedSequence.First<T>()), this.GetOutputDataInfo(input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => this.ReturnDefaultWhenEmpty ? "FirstOrDefault()" : "First()";
  }
}
