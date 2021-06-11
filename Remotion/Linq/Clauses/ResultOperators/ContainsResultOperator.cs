// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ContainsResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class ContainsResultOperator : ValueFromSequenceResultOperatorBase
  {
    private Expression _item;

    public ContainsResultOperator(Expression item)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (item), item);
      this.Item = item;
    }

    public Expression Item
    {
      get => this._item;
      set => this._item = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public T GetConstantItem<T>() => this.GetConstantValueFromExpression<T>("item", this.Item);

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().Contains<T>(this.GetConstantItem<T>()), this.GetOutputDataInfo(input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new ContainsResultOperator(this.Item);

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo inputInfo)
    {
      if (!inputInfo.ResultItemType.GetTypeInfo().IsAssignableFrom(this.Item.Type.GetTypeInfo()))
        throw new ArgumentException(string.Format("The items of the input sequence of type '{0}' are not compatible with the item expression of type '{1}'.", (object) inputInfo.ResultItemType, (object) this.Item.Type), nameof (inputInfo));
      return (StreamedValueInfo) new StreamedScalarValueInfo(typeof (bool));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Item = transformation(this.Item);
    }

    public override string ToString() => "Contains(" + this.Item.BuildString() + ")";
  }
}
