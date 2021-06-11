// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.AdditionalFromClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class AdditionalFromClause : FromClauseBase, IBodyClause, IClause
  {
    public AdditionalFromClause(string itemName, Type itemType, Expression fromExpression)
      : base(ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName), ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType), ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression))
    {
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitAdditionalFromClause(this, queryModel, index);
    }

    public AdditionalFromClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      AdditionalFromClause additionalFromClause = new AdditionalFromClause(this.ItemName, this.ItemType, this.FromExpression);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) additionalFromClause));
      return additionalFromClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext) => (IBodyClause) this.Clone(cloneContext);
  }
}
