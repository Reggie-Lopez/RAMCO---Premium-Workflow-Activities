// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AggregateResultOperator
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
  public sealed class AggregateResultOperator : ValueFromSequenceResultOperatorBase
  {
    private LambdaExpression _func;

    public AggregateResultOperator(LambdaExpression func)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Type.GetTypeInfo().IsGenericTypeDefinition)
        throw new ArgumentException("Open generic delegates are not supported with AggregateResultOperator", nameof (func));
      this.Func = func;
    }

    public LambdaExpression Func
    {
      get => this._func;
      set
      {
        ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (value), value);
        if (value.Type.GetTypeInfo().IsGenericTypeDefinition)
          throw new ArgumentException("Open generic delegates are not supported with AggregateResultOperator", nameof (value));
        this._func = this.DescribesValidFuncType(value) ? value : throw new ArgumentException(string.Format("The aggregating function must be a LambdaExpression that describes an instantiation of 'Func<T,T>', but it is '{0}'.", (object) value.Type), nameof (value));
      }
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().Aggregate<T>((System.Func<T, T, T>) ReverseResolvingExpressionVisitor.ReverseResolveLambda(input.DataInfo.ItemExpression, this.Func, 1).Compile()), this.GetOutputDataInfo(input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext) => (ResultOperatorBase) new AggregateResultOperator(this.Func);

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => (IStreamedDataInfo) this.GetOutputDataInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo));

    private StreamedValueInfo GetOutputDataInfo([NotNull] StreamedSequenceInfo sequenceInfo)
    {
      Type expectedItemType = this.GetExpectedItemType();
      this.CheckSequenceItemType(sequenceInfo, expectedItemType);
      return (StreamedValueInfo) new StreamedScalarValueInfo(this.Func.Body.Type);
    }

    public override void TransformExpressions(System.Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<System.Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Func = (LambdaExpression) transformation((Expression) this.Func);
    }

    public override string ToString() => "Aggregate(" + this.Func.BuildString() + ")";

    private bool DescribesValidFuncType(LambdaExpression value)
    {
      Type type = value.Type;
      if (!type.GetTypeInfo().IsGenericType || type.GetGenericTypeDefinition() != typeof (System.Func<,>))
        return false;
      Type[] genericTypeArguments = type.GetTypeInfo().GenericTypeArguments;
      return genericTypeArguments[0].GetTypeInfo().IsAssignableFrom(genericTypeArguments[1].GetTypeInfo());
    }

    private Type GetExpectedItemType() => this.Func.Type.GetTypeInfo().GenericTypeArguments[0];
  }
}
