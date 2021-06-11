// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModel
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public sealed class QueryModel
  {
    private readonly UniqueIdentifierGenerator _uniqueIdentifierGenerator;
    private MainFromClause _mainFromClause;
    private SelectClause _selectClause;

    public QueryModel(MainFromClause mainFromClause, SelectClause selectClause)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (mainFromClause), mainFromClause);
      ArgumentUtility.CheckNotNull<SelectClause>(nameof (selectClause), selectClause);
      this._uniqueIdentifierGenerator = new UniqueIdentifierGenerator();
      this.MainFromClause = mainFromClause;
      this.SelectClause = selectClause;
      this.BodyClauses = new ObservableCollection<IBodyClause>();
      this.BodyClauses.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BodyClauses_CollectionChanged);
      this.ResultOperators = new ObservableCollection<ResultOperatorBase>();
      this.ResultOperators.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ResultOperators_CollectionChanged);
    }

    public Type ResultTypeOverride { get; set; }

    public Type GetResultType() => this.GetOutputDataInfo().DataType;

    public IStreamedDataInfo GetOutputDataInfo()
    {
      IStreamedDataInfo streamedDataInfo = this.ResultOperators.Aggregate<ResultOperatorBase, IStreamedDataInfo>((IStreamedDataInfo) this.SelectClause.GetOutputDataInfo(), (Func<IStreamedDataInfo, ResultOperatorBase, IStreamedDataInfo>) ((current, resultOperator) => resultOperator.GetOutputDataInfo(current)));
      if (this.ResultTypeOverride == (Type) null)
        return streamedDataInfo;
      try
      {
        return streamedDataInfo.AdjustDataType(this.ResultTypeOverride);
      }
      catch (Exception ex)
      {
        Type resultTypeOverride = this.ResultTypeOverride;
        this.ResultTypeOverride = (Type) null;
        throw new InvalidOperationException(string.Format("The query model's result type cannot be changed to '{0}'. The result type may only be overridden and set to values compatible with the ResultOperators' current data type ('{1}').", (object) resultTypeOverride, (object) streamedDataInfo.DataType), ex);
      }
    }

    public MainFromClause MainFromClause
    {
      get => this._mainFromClause;
      set
      {
        ArgumentUtility.CheckNotNull<MainFromClause>(nameof (value), value);
        this._mainFromClause = value;
        this._uniqueIdentifierGenerator.AddKnownIdentifier(value.ItemName);
      }
    }

    public SelectClause SelectClause
    {
      get => this._selectClause;
      set
      {
        ArgumentUtility.CheckNotNull<SelectClause>(nameof (value), value);
        this._selectClause = value;
      }
    }

    public ObservableCollection<IBodyClause> BodyClauses { get; private set; }

    public ObservableCollection<ResultOperatorBase> ResultOperators { get; private set; }

    public UniqueIdentifierGenerator GetUniqueIdentfierGenerator() => this._uniqueIdentifierGenerator;

    private void ResultOperators_CollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      ArgumentUtility.CheckNotNull<NotifyCollectionChangedEventArgs>(nameof (e), e);
      ArgumentUtility.CheckItemsNotNullAndType<IList>("e.NewItems", e.NewItems, typeof (ResultOperatorBase));
    }

    public void Accept(IQueryModelVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      visitor.VisitQueryModel(this);
    }

    public override string ToString()
    {
      string seed;
      if (this.IsIdentityQuery())
        seed = this.MainFromClause.FromExpression.BuildString();
      else
        seed = this.MainFromClause.ToString() + this.BodyClauses.Aggregate<IBodyClause, string>("", (Func<string, IBodyClause, string>) ((s, b) => s + " " + (object) b)) + " " + (object) this.SelectClause;
      return this.ResultOperators.Aggregate<ResultOperatorBase, string>(seed, (Func<string, ResultOperatorBase, string>) ((s, r) => s + " => " + (object) r));
    }

    public QueryModel Clone() => this.Clone(new QuerySourceMapping());

    public QueryModel Clone(QuerySourceMapping querySourceMapping)
    {
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      CloneContext cloneContext = new CloneContext(querySourceMapping);
      QueryModelBuilder queryModelBuilder = new QueryModelBuilder();
      queryModelBuilder.AddClause((IClause) this.MainFromClause.Clone(cloneContext));
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) this.BodyClauses)
        queryModelBuilder.AddClause((IClause) bodyClause.Clone(cloneContext));
      queryModelBuilder.AddClause((IClause) this.SelectClause.Clone(cloneContext));
      foreach (ResultOperatorBase resultOperator1 in (Collection<ResultOperatorBase>) this.ResultOperators)
      {
        ResultOperatorBase resultOperator2 = resultOperator1.Clone(cloneContext);
        queryModelBuilder.AddResultOperator(resultOperator2);
      }
      QueryModel queryModel = queryModelBuilder.Build();
      QueryModel.CloningExpressionVisitor expressionVisitor = new QueryModel.CloningExpressionVisitor(cloneContext.QuerySourceMapping);
      queryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionVisitor) expressionVisitor).Visit));
      queryModel.ResultTypeOverride = this.ResultTypeOverride;
      return queryModel;
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.MainFromClause.TransformExpressions(transformation);
      foreach (IClause bodyClause in (Collection<IBodyClause>) this.BodyClauses)
        bodyClause.TransformExpressions(transformation);
      this.SelectClause.TransformExpressions(transformation);
      foreach (ResultOperatorBase resultOperator in (Collection<ResultOperatorBase>) this.ResultOperators)
        resultOperator.TransformExpressions(transformation);
    }

    public string GetNewName(string prefix)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (prefix), prefix);
      return this._uniqueIdentifierGenerator.GetUniqueIdentifier(prefix);
    }

    private void BodyClauses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      ArgumentUtility.CheckNotNull<NotifyCollectionChangedEventArgs>(nameof (e), e);
      ArgumentUtility.CheckItemsNotNullAndType<IList>("e.NewItems", e.NewItems, typeof (IBodyClause));
      if (e.NewItems == null)
        return;
      foreach (IQuerySource querySource in e.NewItems.OfType<IFromClause>())
        this._uniqueIdentifierGenerator.AddKnownIdentifier(querySource.ItemName);
    }

    public IStreamedData Execute(IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return this.GetOutputDataInfo().ExecuteQueryModel(this, executor);
    }

    public bool IsIdentityQuery() => this.BodyClauses.Count == 0 && this.SelectClause.Selector is QuerySourceReferenceExpression && ((QuerySourceReferenceExpression) this.SelectClause.Selector).ReferencedQuerySource == this.MainFromClause;

    public QueryModel ConvertToSubQuery(string itemName)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      if (!(this.GetOutputDataInfo() is StreamedSequenceInfo outputDataInfo))
        throw new InvalidOperationException(string.Format("The query must return a sequence of items, but it selects a single object of type '{0}'.", (object) this.GetOutputDataInfo().DataType));
      MainFromClause mainFromClause = new MainFromClause(itemName, outputDataInfo.ResultItemType, (Expression) new SubQueryExpression(this));
      SelectClause selectClause = new SelectClause((Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return new QueryModel(mainFromClause, selectClause);
    }

    private sealed class CloningExpressionVisitor : RelinqExpressionVisitor
    {
      private readonly QuerySourceMapping _querySourceMapping;

      public CloningExpressionVisitor(QuerySourceMapping querySourceMapping) => this._querySourceMapping = querySourceMapping;

      protected internal override Expression VisitQuerySourceReference(
        QuerySourceReferenceExpression expression)
      {
        return this._querySourceMapping.ContainsMapping(expression.ReferencedQuerySource) ? this._querySourceMapping.GetExpression(expression.ReferencedQuerySource) : (Expression) expression;
      }

      protected internal override Expression VisitSubQuery(SubQueryExpression expression) => (Expression) new SubQueryExpression(expression.QueryModel.Clone(this._querySourceMapping));
    }
  }
}
