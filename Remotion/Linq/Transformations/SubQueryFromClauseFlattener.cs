// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Transformations.SubQueryFromClauseFlattener
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionVisitors;
using Remotion.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Transformations
{
  [ComVisible(true)]
  public class SubQueryFromClauseFlattener : QueryModelVisitorBase
  {
    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (fromClause.FromExpression is SubQueryExpression fromExpression)
        this.FlattenSubQuery(fromExpression, (IFromClause) fromClause, queryModel, 0);
      base.VisitMainFromClause(fromClause, queryModel);
    }

    public override void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<AdditionalFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (fromClause.FromExpression is SubQueryExpression fromExpression)
        this.FlattenSubQuery(fromExpression, (IFromClause) fromClause, queryModel, index + 1);
      base.VisitAdditionalFromClause(fromClause, queryModel, index);
    }

    protected virtual void FlattenSubQuery(
      SubQueryExpression subQueryExpression,
      IFromClause fromClause,
      QueryModel queryModel,
      int destinationIndex)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (subQueryExpression), subQueryExpression);
      ArgumentUtility.CheckNotNull<IFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this.CheckFlattenable(subQueryExpression.QueryModel);
      MainFromClause mainFromClause = subQueryExpression.QueryModel.MainFromClause;
      fromClause.CopyFromSource((IFromClause) mainFromClause);
      QuerySourceMapping innerSelectorMapping = new QuerySourceMapping();
      innerSelectorMapping.AddMapping((IQuerySource) fromClause, subQueryExpression.QueryModel.SelectClause.Selector);
      queryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionVisitor.ReplaceClauseReferences(ex, innerSelectorMapping, false)));
      this.InsertBodyClauses(subQueryExpression.QueryModel.BodyClauses, queryModel, destinationIndex);
      QuerySourceMapping innerBodyClauseMapping = new QuerySourceMapping();
      innerBodyClauseMapping.AddMapping((IQuerySource) mainFromClause, (Expression) new QuerySourceReferenceExpression((IQuerySource) fromClause));
      queryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionVisitor.ReplaceClauseReferences(ex, innerBodyClauseMapping, false)));
    }

    protected virtual void CheckFlattenable(QueryModel subQueryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (subQueryModel), subQueryModel);
      if (subQueryModel.ResultOperators.Count > 0)
        throw new NotSupportedException(string.Format("The subquery '{0}' cannot be flattened and pulled out of the from clause because it contains result operators.", (object) subQueryModel));
      if (subQueryModel.BodyClauses.Any<IBodyClause>((Func<IBodyClause, bool>) (bc => bc is OrderByClause)))
        throw new NotSupportedException(string.Format("The subquery '{0}' cannot be flattened and pulled out of the from clause because it contains an OrderByClause.", (object) subQueryModel));
    }

    protected void InsertBodyClauses(
      ObservableCollection<IBodyClause> bodyClauses,
      QueryModel destinationQueryModel,
      int destinationIndex)
    {
      ArgumentUtility.CheckNotNull<ObservableCollection<IBodyClause>>(nameof (bodyClauses), bodyClauses);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (destinationQueryModel), destinationQueryModel);
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) bodyClauses)
      {
        destinationQueryModel.BodyClauses.Insert(destinationIndex, bodyClause);
        ++destinationIndex;
      }
    }
  }
}
