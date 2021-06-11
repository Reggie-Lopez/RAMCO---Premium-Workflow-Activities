// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.JoinClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class JoinClause : IBodyClause, IClause, IQuerySource
  {
    private Type _itemType;
    private string _itemName;
    private Expression _innerSequence;
    private Expression _outerKeySelector;
    private Expression _innerKeySelector;

    public JoinClause(
      string itemName,
      Type itemType,
      Expression innerSequence,
      Expression outerKeySelector,
      Expression innerKeySelector)
    {
      ArgumentUtility.CheckNotNull<string>(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerSequence), innerSequence);
      ArgumentUtility.CheckNotNull<Expression>(nameof (outerKeySelector), outerKeySelector);
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerKeySelector), innerKeySelector);
      this._itemName = itemName;
      this._itemType = itemType;
      this._innerSequence = innerSequence;
      this._outerKeySelector = outerKeySelector;
      this._innerKeySelector = innerKeySelector;
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Expression InnerSequence
    {
      get => this._innerSequence;
      set => this._innerSequence = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public Expression OuterKeySelector
    {
      get => this._outerKeySelector;
      set => this._outerKeySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public Expression InnerKeySelector
    {
      get => this._innerKeySelector;
      set => this._innerKeySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitJoinClause(this, queryModel, index);
    }

    public void Accept(
      IQueryModelVisitor visitor,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
      visitor.VisitJoinClause(this, queryModel, groupJoinClause);
    }

    public JoinClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      JoinClause joinClause = new JoinClause(this.ItemName, this.ItemType, this.InnerSequence, this.OuterKeySelector, this.InnerKeySelector);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) joinClause));
      return joinClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext) => (IBodyClause) this.Clone(cloneContext);

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.InnerSequence = transformation(this.InnerSequence);
      this.OuterKeySelector = transformation(this.OuterKeySelector);
      this.InnerKeySelector = transformation(this.InnerKeySelector);
    }

    public override string ToString() => string.Format("join {0} {1} in {2} on {3} equals {4}", (object) this.ItemType.Name, (object) this.ItemName, (object) this.InnerSequence.BuildString(), (object) this.OuterKeySelector.BuildString(), (object) this.InnerKeySelector.BuildString());
  }
}
