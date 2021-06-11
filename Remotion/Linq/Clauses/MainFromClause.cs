// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.MainFromClause
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
  public sealed class MainFromClause : FromClauseBase
  {
    public MainFromClause(string itemName, Type itemType, Expression fromExpression)
      : base(ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName), ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType), ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression))
    {
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitMainFromClause(this, queryModel);
    }

    public MainFromClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      MainFromClause mainFromClause = new MainFromClause(this.ItemName, this.ItemType, this.FromExpression);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return mainFromClause;
    }
  }
}
