// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.IQueryModelVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public interface IQueryModelVisitor
  {
    void VisitQueryModel(QueryModel queryModel);

    void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel);

    void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index);

    void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index);

    void VisitJoinClause(
      JoinClause joinClause,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause);

    void VisitGroupJoinClause(GroupJoinClause joinClause, QueryModel queryModel, int index);

    void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index);

    void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index);

    void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index);

    void VisitSelectClause(SelectClause selectClause, QueryModel queryModel);

    void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index);
  }
}
