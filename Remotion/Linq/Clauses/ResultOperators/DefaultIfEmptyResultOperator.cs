// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.DefaultIfEmptyResultOperator
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
  public sealed class DefaultIfEmptyResultOperator : SequenceTypePreservingResultOperatorBase
  {
    public DefaultIfEmptyResultOperator(Expression optionalDefaultValue) => this.OptionalDefaultValue = optionalDefaultValue;

    public Expression OptionalDefaultValue { get; set; }

    public object GetConstantOptionalDefaultValue() => this.OptionalDefaultValue == null ? (object) null : this.GetConstantValueFromExpression<object>("default value", this.OptionalDefaultValue);

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new DefaultIfEmptyResultOperator(this.OptionalDefaultValue);

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      IEnumerable<T> typedSequence = input.GetTypedSequence<T>();
      return new StreamedSequence((IEnumerable) (this.OptionalDefaultValue != null ? typedSequence.DefaultIfEmpty<T>((T) this.GetConstantOptionalDefaultValue()) : typedSequence.DefaultIfEmpty<T>()).AsQueryable<T>(), this.GetOutputDataInfo(input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      if (this.OptionalDefaultValue == null)
        return;
      this.OptionalDefaultValue = transformation(this.OptionalDefaultValue);
    }

    public override string ToString() => this.OptionalDefaultValue == null ? "DefaultIfEmpty()" : "DefaultIfEmpty(" + this.OptionalDefaultValue.BuildString() + ")";
  }
}
