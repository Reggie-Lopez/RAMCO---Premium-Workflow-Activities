// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AllResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.ExpressionVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class AllResultOperator : ValueFromSequenceResultOperatorBase
  {
    private Expression _predicate;

    public AllResultOperator(Expression predicate)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (predicate), predicate);
      this.Predicate = predicate;
    }

    public Expression Predicate
    {
      get => this._predicate;
      set => this._predicate = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().All<T>((Func<T, bool>) ReverseResolvingExpressionVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.Predicate).Compile()), this.GetOutputDataInfo(input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new AllResultOperator(this.Predicate);

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Predicate = transformation(this.Predicate);
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo sequenceInfo) => (StreamedValueInfo) new StreamedScalarValueInfo(typeof (bool));

    public override string ToString() => "All(" + this.Predicate.BuildString() + ")";
  }
}
