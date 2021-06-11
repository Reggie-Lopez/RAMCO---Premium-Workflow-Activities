// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.SkipResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class SkipResultOperator : SequenceTypePreservingResultOperatorBase
  {
    private Expression _count;

    public SkipResultOperator(Expression count)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (count), count);
      this.Count = count;
    }

    public Expression Count
    {
      get => this._count;
      set
      {
        ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
        this._count = !(value.Type != typeof (int)) ? value : throw new ArgumentException(string.Format("The value expression returns '{0}', an expression returning 'System.Int32' was expected.", (object) value.Type), nameof (value));
      }
    }

    public int GetConstantCount() => this.GetConstantValueFromExpression<int>("count", this.Count);

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new SkipResultOperator(this.Count);

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input) => new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Skip<T>(this.GetConstantCount()).AsQueryable<T>(), this.GetOutputDataInfo(input.DataInfo));

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Count = transformation(this.Count);
    }

    public override string ToString() => "Skip(" + this.Count.BuildString() + ")";
  }
}
