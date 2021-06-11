// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.UnionResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
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
  public sealed class UnionResultOperator : SequenceFromSequenceResultOperatorBase, IQuerySource
  {
    private string _itemName;
    private Type _itemType;
    private Expression _source2;

    public UnionResultOperator(string itemName, Type itemType, Expression source2)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this.ItemName = itemName;
      this.ItemType = itemType;
      this.Source2 = source2;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    public Expression Source2
    {
      get => this._source2;
      set => this._source2 = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public IEnumerable GetConstantSource2() => this.GetConstantValueFromExpression<IEnumerable>("source2", this.Source2);

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new UnionResultOperator(this._itemName, this._itemType, this._source2);

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input) => new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Union<T>((IEnumerable<T>) this.GetConstantSource2()).AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      this.CheckSequenceItemType(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo), this._itemType);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this._itemType), (Expression) new QuerySourceReferenceExpression((IQuerySource) this));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Source2 = transformation(this.Source2);
    }

    public override string ToString() => "Union(" + this.Source2.BuildString() + ")";
  }
}
