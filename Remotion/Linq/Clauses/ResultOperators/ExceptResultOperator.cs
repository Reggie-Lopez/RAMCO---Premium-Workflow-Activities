// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ExceptResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class ExceptResultOperator : SequenceTypePreservingResultOperatorBase
  {
    private Expression _source2;

    public ExceptResultOperator(Expression source2)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this.Source2 = source2;
    }

    public Expression Source2
    {
      get => this._source2;
      set
      {
        ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
        ReflectionUtility.CheckTypeIsClosedGenericIEnumerable(value.Type, nameof (value));
        this._source2 = value;
      }
    }

    public IEnumerable<T> GetConstantSource2<T>() => this.GetConstantValueFromExpression<IEnumerable<T>>("source2", this.Source2);

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new ExceptResultOperator(this.Source2);

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Except<T>(this.GetConstantSource2<T>()).AsQueryable<T>(), this.GetOutputDataInfo(input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Source2 = transformation(this.Source2);
    }

    public override string ToString() => "Except(" + this.Source2.BuildString() + ")";
  }
}
