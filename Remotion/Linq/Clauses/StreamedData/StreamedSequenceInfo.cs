// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedSequenceInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.StreamedData
{
  [ComVisible(true)]
  public sealed class StreamedSequenceInfo : IStreamedDataInfo, IEquatable<IStreamedDataInfo>
  {
    private static readonly MethodInfo s_executeMethod = typeof (StreamedSequenceInfo).GetRuntimeMethodChecked("ExecuteCollectionQueryModel", new Type[2]
    {
      typeof (QueryModel),
      typeof (IQueryExecutor)
    });

    public StreamedSequenceInfo(Type dataType, Expression itemExpression)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      this.ResultItemType = ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(dataType, nameof (dataType));
      if (!this.ResultItemType.GetTypeInfo().IsAssignableFrom(itemExpression.Type.GetTypeInfo()))
        throw new ArgumentException(string.Format("ItemExpression is of type '{0}', but should be '{1}' (or derived from it).", (object) itemExpression.Type, (object) this.ResultItemType), nameof (itemExpression));
      this.DataType = dataType;
      this.ItemExpression = itemExpression;
    }

    public Type ResultItemType { get; private set; }

    public Expression ItemExpression { get; private set; }

    public Type DataType { get; private set; }

    public IStreamedDataInfo AdjustDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      if (dataType.GetTypeInfo().IsGenericTypeDefinition)
      {
        try
        {
          dataType = dataType.MakeGenericType(this.ResultItemType);
        }
        catch (ArgumentException ex)
        {
          throw new ArgumentException(string.Format("The generic type definition '{0}' could not be closed over the type of the ResultItemType ('{1}'). {2}", (object) dataType, (object) this.ResultItemType, (object) ex.Message), nameof (dataType));
        }
      }
      Assertion.IsNotNull<Type>(dataType, "dateType cannot be null.");
      Assertion.IsNotNull<Expression>(this.ItemExpression, "ItemExpression cannot be null.");
      try
      {
        return (IStreamedDataInfo) new StreamedSequenceInfo(dataType, this.ItemExpression);
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException(string.Format("'{0}' cannot be used as the data type for a sequence with an ItemExpression of type '{1}'.", (object) dataType, (object) this.ResultItemType), nameof (dataType));
      }
    }

    public IStreamedData ExecuteQueryModel(
      QueryModel queryModel,
      IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      IQueryable queryable = ((Func<QueryModel, IQueryExecutor, IEnumerable>) StreamedSequenceInfo.s_executeMethod.MakeGenericMethod(this.ResultItemType).CreateDelegate(typeof (Func<QueryModel, IQueryExecutor, IEnumerable>), (object) this))(queryModel, executor).AsQueryable();
      return (IStreamedData) new StreamedSequence((IEnumerable) queryable, new StreamedSequenceInfo(queryable.GetType(), this.ItemExpression));
    }

    public IEnumerable ExecuteCollectionQueryModel<T>(
      QueryModel queryModel,
      IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (IEnumerable) executor.ExecuteCollection<T>(queryModel);
    }

    public override sealed bool Equals(object obj) => this.Equals(obj as IStreamedDataInfo);

    public bool Equals(IStreamedDataInfo obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      StreamedSequenceInfo streamedSequenceInfo = (StreamedSequenceInfo) obj;
      return this.DataType.Equals(streamedSequenceInfo.DataType) && this.ItemExpression.Equals((object) streamedSequenceInfo.ItemExpression);
    }

    public override int GetHashCode() => this.DataType.GetHashCode() ^ this.ItemExpression.GetHashCode();
  }
}
