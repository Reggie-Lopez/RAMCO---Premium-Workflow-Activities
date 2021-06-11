// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.FromClauseBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public abstract class FromClauseBase : IFromClause, IClause, IQuerySource
  {
    private string _itemName;
    private Type _itemType;
    private Expression _fromExpression;

    internal FromClauseBase(string itemName, Type itemType, Expression fromExpression)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression);
      this._itemName = itemName;
      this._itemType = itemType;
      this._fromExpression = fromExpression;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    public Expression FromExpression
    {
      get => this._fromExpression;
      set => this._fromExpression = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public virtual void CopyFromSource(IFromClause source)
    {
      ArgumentUtility.CheckNotNull<IFromClause>(nameof (source), source);
      this.FromExpression = source.FromExpression;
      this.ItemName = source.ItemName;
      this.ItemType = source.ItemType;
    }

    public virtual void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.FromExpression = transformation(this.FromExpression);
    }

    public override string ToString() => string.Format("from {0} {1} in {2}", (object) this.ItemType.Name, (object) this.ItemName, (object) this.FromExpression.BuildString());
  }
}
