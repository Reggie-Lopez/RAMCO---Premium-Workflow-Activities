// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.GroupResultOperator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class GroupResultOperator : SequenceFromSequenceResultOperatorBase, IQuerySource
  {
    private static readonly MethodInfo s_executeMethod = typeof (GroupResultOperator).GetRuntimeMethodChecked("ExecuteGroupingInMemory", new Type[1]
    {
      typeof (StreamedSequence)
    });
    private string _itemName;
    private Expression _keySelector;
    private Expression _elementSelector;

    public GroupResultOperator(string itemName, Expression keySelector, Expression elementSelector)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Expression>(nameof (keySelector), keySelector);
      ArgumentUtility.CheckNotNull<Expression>(nameof (elementSelector), elementSelector);
      this._itemName = itemName;
      this._elementSelector = elementSelector;
      this._keySelector = keySelector;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType => typeof (IGrouping<,>).MakeGenericType(this.KeySelector.Type, this.ElementSelector.Type);

    public Expression KeySelector
    {
      get => this._keySelector;
      set => this._keySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public Expression ElementSelector
    {
      get => this._elementSelector;
      set => this._elementSelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return (ResultOperatorBase) new GroupResultOperator(this.ItemName, this.KeySelector, this.ElementSelector);
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.KeySelector = transformation(this.KeySelector);
      this.ElementSelector = transformation(this.ElementSelector);
    }

    public override StreamedSequence ExecuteInMemory<TInput>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return (StreamedSequence) this.InvokeExecuteMethod(GroupResultOperator.s_executeMethod.MakeGenericMethod(typeof (TInput), this.KeySelector.Type, this.ElementSelector.Type), (object) input);
    }

    public StreamedSequence ExecuteGroupingInMemory<TSource, TKey, TElement>(
      StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<TSource>().GroupBy<TSource, TKey, TElement>((Func<TSource, TKey>) ReverseResolvingExpressionVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.KeySelector).Compile(), (Func<TSource, TElement>) ReverseResolvingExpressionVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.ElementSelector).Compile()).AsQueryable<IGrouping<TKey, TElement>>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.ItemType), (Expression) new QuerySourceReferenceExpression((IQuerySource) this));
    }

    public override string ToString() => string.Format("GroupBy({0}, {1})", (object) this.KeySelector.BuildString(), (object) this.ElementSelector.BuildString());
  }
}
