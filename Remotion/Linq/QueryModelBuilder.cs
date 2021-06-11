// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModelBuilder
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public sealed class QueryModelBuilder
  {
    private readonly List<ResultOperatorBase> _resultOperators = new List<ResultOperatorBase>();
    private readonly List<IBodyClause> _bodyClauses = new List<IBodyClause>();

    public MainFromClause MainFromClause { get; private set; }

    public SelectClause SelectClause { get; private set; }

    public ReadOnlyCollection<IBodyClause> BodyClauses => new ReadOnlyCollection<IBodyClause>((IList<IBodyClause>) this._bodyClauses);

    public ReadOnlyCollection<ResultOperatorBase> ResultOperators => new ReadOnlyCollection<ResultOperatorBase>((IList<ResultOperatorBase>) this._resultOperators);

    public void AddClause(IClause clause)
    {
      ArgumentUtility.CheckNotNull<IClause>(nameof (clause), clause);
      switch (clause)
      {
        case MainFromClause mainFromClause:
          this.MainFromClause = this.MainFromClause == null ? mainFromClause : throw new InvalidOperationException("Builder already has a MainFromClause.");
          break;
        case SelectClause selectClause:
          this.SelectClause = this.SelectClause == null ? selectClause : throw new InvalidOperationException("Builder already has a SelectClause.");
          break;
        case IBodyClause bodyClause:
          this._bodyClauses.Add(bodyClause);
          break;
        default:
          throw new ArgumentException(string.Format("Cannot add clause of type '{0}' to a query model. Only instances of IBodyClause, MainFromClause, or ISelectGroupClause are supported.", (object) clause.GetType()), nameof (clause));
      }
    }

    public void AddResultOperator(ResultOperatorBase resultOperator)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>(nameof (resultOperator), resultOperator);
      this._resultOperators.Add(resultOperator);
    }

    public QueryModel Build()
    {
      if (this.MainFromClause == null)
        throw new InvalidOperationException("No MainFromClause was added to the builder.");
      QueryModel queryModel = this.SelectClause != null ? new QueryModel(this.MainFromClause, this.SelectClause) : throw new InvalidOperationException("No SelectOrGroupClause was added to the builder.");
      foreach (IBodyClause bodyClause in this.BodyClauses)
        queryModel.BodyClauses.Add(bodyClause);
      foreach (ResultOperatorBase resultOperator in this.ResultOperators)
        queryModel.ResultOperators.Add(resultOperator);
      return queryModel;
    }
  }
}
