// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AggregateFromSeedResultOperator
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
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ResultOperators
{
  [ComVisible(true)]
  public sealed class AggregateFromSeedResultOperator : ValueFromSequenceResultOperatorBase
  {
    private static readonly MethodInfo s_executeMethod = typeof (AggregateFromSeedResultOperator).GetRuntimeMethodChecked("ExecuteAggregateInMemory", new Type[1]
    {
      typeof (StreamedSequence)
    });
    private Expression _seed;
    private LambdaExpression _func;
    private LambdaExpression _resultSelector;

    public AggregateFromSeedResultOperator(
      Expression seed,
      LambdaExpression func,
      LambdaExpression optionalResultSelector)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (seed), seed);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Type.GetTypeInfo().IsGenericTypeDefinition)
        throw new ArgumentException("Open generic delegates are not supported with AggregateFromSeedResultOperator", nameof (func));
      this.Seed = seed;
      this.Func = func;
      this.OptionalResultSelector = optionalResultSelector;
    }

    public LambdaExpression Func
    {
      get => this._func;
      set
      {
        ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (value), value);
        if (value.Type.GetTypeInfo().IsGenericTypeDefinition)
          throw new ArgumentException("Open generic delegates are not supported with AggregateFromSeedResultOperator", nameof (value));
        this._func = this.DescribesValidFuncType(value) ? value : throw new ArgumentException(string.Format("The aggregating function must be a LambdaExpression that describes an instantiation of 'Func<TAccumulate,TAccumulate>', but it is '{0}'.", (object) value.Type), nameof (value));
      }
    }

    public Expression Seed
    {
      get => this._seed;
      set => this._seed = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public LambdaExpression OptionalResultSelector
    {
      get => this._resultSelector;
      set
      {
        if (value != null && value.Type.GetTypeInfo().IsGenericTypeDefinition)
          throw new ArgumentException("Open generic delegates are not supported with AggregateFromSeedResultOperator", nameof (value));
        this._resultSelector = this.DescribesValidResultSelectorType(value) ? value : throw new ArgumentException(string.Format("The result selector must be a LambdaExpression that describes an instantiation of 'Func<TAccumulate,TResult>', but it is '{0}'.", (object) value.Type), nameof (value));
      }
    }

    public T GetConstantSeed<T>() => this.GetConstantValueFromExpression<T>("seed", this.Seed);

    public override StreamedValue ExecuteInMemory<TInput>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return (StreamedValue) this.InvokeExecuteMethod(AggregateFromSeedResultOperator.s_executeMethod.MakeGenericMethod(typeof (TInput), this.Seed.Type, this.GetResultType()), (object) input);
    }

    public StreamedValue ExecuteAggregateInMemory<TInput, TAggregate, TResult>(
      StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      TAggregate aggregate = input.GetTypedSequence<TInput>().Aggregate<TInput, TAggregate>(this.GetConstantSeed<TAggregate>(), (System.Func<TAggregate, TInput, TAggregate>) ReverseResolvingExpressionVisitor.ReverseResolveLambda(input.DataInfo.ItemExpression, this.Func, 1).Compile());
      StreamedValueInfo outputDataInfo = this.GetOutputDataInfo(input.DataInfo);
      return this.OptionalResultSelector == null ? new StreamedValue((object) aggregate, outputDataInfo) : new StreamedValue((object) ((System.Func<TAggregate, TResult>) this.OptionalResultSelector.Compile())(aggregate), outputDataInfo);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new AggregateFromSeedResultOperator(this.Seed, this.Func, this.OptionalResultSelector);

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo sequenceInfo)
    {
      Type genericTypeArgument = this.Func.Type.GetTypeInfo().GenericTypeArguments[0];
      if (!genericTypeArgument.GetTypeInfo().IsAssignableFrom(this.Seed.Type.GetTypeInfo()))
        throw new InvalidOperationException(string.Format("The seed expression and the aggregating function don't have matching types. The seed is of type '{0}', but the function aggregates '{1}'.", (object) this.Seed.Type, (object) genericTypeArgument));
      Type type = this.OptionalResultSelector != null ? this.OptionalResultSelector.Type.GetTypeInfo().GenericTypeArguments[0] : (Type) null;
      if (type != (Type) null && genericTypeArgument != type)
        throw new InvalidOperationException(string.Format("The aggregating function and the result selector don't have matching types. The function aggregates type '{0}', but the result selector takes '{1}'.", (object) genericTypeArgument, (object) type));
      return (StreamedValueInfo) new StreamedScalarValueInfo(this.GetResultType());
    }

    public override void TransformExpressions(System.Func<Expression, Expression> transformation)
    {
      this.Seed = transformation(this.Seed);
      this.Func = (LambdaExpression) transformation((Expression) this.Func);
      if (this.OptionalResultSelector == null)
        return;
      this.OptionalResultSelector = (LambdaExpression) transformation((Expression) this.OptionalResultSelector);
    }

    public override string ToString() => this.OptionalResultSelector != null ? string.Format("Aggregate({0}, {1}, {2})", (object) this.Seed.BuildString(), (object) this.Func.BuildString(), (object) this.OptionalResultSelector.BuildString()) : string.Format("Aggregate({0}, {1})", (object) this.Seed.BuildString(), (object) this.Func.BuildString());

    private Type GetResultType() => this.OptionalResultSelector == null ? this.Func.Body.Type : this.OptionalResultSelector.Body.Type;

    private bool DescribesValidFuncType(LambdaExpression value)
    {
      Type type = value.Type;
      if (!type.GetTypeInfo().IsGenericType || type.GetGenericTypeDefinition() != typeof (System.Func<,>))
        return false;
      Type[] genericTypeArguments = type.GetTypeInfo().GenericTypeArguments;
      return genericTypeArguments[0] == genericTypeArguments[1];
    }

    private bool DescribesValidResultSelectorType(LambdaExpression value)
    {
      if (value == null)
        return true;
      return value.Type.GetTypeInfo().IsGenericType && value.Type.GetGenericTypeDefinition() == typeof (System.Func<,>);
    }
  }
}
