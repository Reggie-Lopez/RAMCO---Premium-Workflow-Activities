// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModelVisitorBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Collections;
using Remotion.Utilities;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public abstract class QueryModelVisitorBase : IQueryModelVisitor
  {
    public virtual void VisitQueryModel(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.MainFromClause.Accept((IQueryModelVisitor) this, queryModel);
      this.VisitBodyClauses(queryModel.BodyClauses, queryModel);
      queryModel.SelectClause.Accept((IQueryModelVisitor) this, queryModel);
      this.VisitResultOperators(queryModel.ResultOperators, queryModel);
    }

    public virtual void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<AdditionalFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<JoinClause>(nameof (joinClause), joinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitJoinClause(
      JoinClause joinClause,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause)
    {
      ArgumentUtility.CheckNotNull<JoinClause>(nameof (joinClause), joinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
    }

    public virtual void VisitGroupJoinClause(
      GroupJoinClause groupJoinClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      groupJoinClause.JoinClause.Accept((IQueryModelVisitor) this, queryModel, groupJoinClause);
    }

    public virtual void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<WhereClause>(nameof (whereClause), whereClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitOrderByClause(
      OrderByClause orderByClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this.VisitOrderings(orderByClause.Orderings, queryModel, orderByClause);
    }

    public virtual void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      ArgumentUtility.CheckNotNull<Ordering>(nameof (ordering), ordering);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
    }

    public virtual void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<SelectClause>(nameof (selectClause), selectClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>(nameof (resultOperator), resultOperator);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    protected virtual void VisitBodyClauses(
      ObservableCollection<IBodyClause> bodyClauses,
      QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<ObservableCollection<IBodyClause>>(nameof (bodyClauses), bodyClauses);
      foreach (IndexValuePair<IBodyClause> indexValuePair in bodyClauses.AsChangeResistantEnumerableWithIndex<IBodyClause>())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, indexValuePair.Index);
    }

    protected virtual void VisitOrderings(
      ObservableCollection<Ordering> orderings,
      QueryModel queryModel,
      OrderByClause orderByClause)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      ArgumentUtility.CheckNotNull<ObservableCollection<Ordering>>(nameof (orderings), orderings);
      foreach (IndexValuePair<Ordering> indexValuePair in orderings.AsChangeResistantEnumerableWithIndex<Ordering>())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, orderByClause, indexValuePair.Index);
    }

    protected virtual void VisitResultOperators(
      ObservableCollection<ResultOperatorBase> resultOperators,
      QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<ObservableCollection<ResultOperatorBase>>(nameof (resultOperators), resultOperators);
      foreach (IndexValuePair<ResultOperatorBase> indexValuePair in resultOperators.AsChangeResistantEnumerableWithIndex<ResultOperatorBase>())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, indexValuePair.Index);
    }
  }
}
