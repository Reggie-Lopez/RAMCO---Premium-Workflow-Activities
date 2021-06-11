// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.OfTypeResultOperator
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
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class OfTypeResultOperator : SequenceFromSequenceResultOperatorBase
  {
    private static readonly MethodInfo s_enumerableOfTypeMethod = typeof (Enumerable).GetRuntimeMethodChecked("OfType", new Type[1]
    {
      typeof (IEnumerable)
    });
    private Type _searchedItemType;

    public OfTypeResultOperator(Type searchedItemType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (searchedItemType), searchedItemType);
      this.SearchedItemType = searchedItemType;
    }

    public Type SearchedItemType
    {
      get => this._searchedItemType;
      set
      {
        ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
        this._searchedItemType = value;
      }
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new OfTypeResultOperator(this.SearchedItemType);

    public override StreamedSequence ExecuteInMemory<TInput>(StreamedSequence input)
    {
      IEnumerable<TInput> typedSequence = input.GetTypedSequence<TInput>();
      return new StreamedSequence((IEnumerable) ((IEnumerable) this.InvokeExecuteMethod(OfTypeResultOperator.s_enumerableOfTypeMethod.MakeGenericMethod(this.SearchedItemType), (object) typedSequence)).AsQueryable(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      StreamedSequenceInfo streamedSequenceInfo = ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.SearchedItemType), (Expression) this.GetNewItemExpression(streamedSequenceInfo.ItemExpression));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "OfType<" + (object) this.SearchedItemType + ">()";

    private UnaryExpression GetNewItemExpression(Expression inputItemExpression) => Expression.Convert(inputItemExpression, this.SearchedItemType);
  }
}
