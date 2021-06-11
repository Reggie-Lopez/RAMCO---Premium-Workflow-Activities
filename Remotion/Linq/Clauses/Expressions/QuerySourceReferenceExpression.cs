// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.QuerySourceReferenceExpression
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.Expressions
{
  [ComVisible(true)]
  public sealed class QuerySourceReferenceExpression : Expression
  {
    private readonly Type _type;

    public QuerySourceReferenceExpression(IQuerySource querySource)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      this._type = querySource.ItemType;
      this.ReferencedQuerySource = querySource;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;

    public override Type Type => this._type;

    public IQuerySource ReferencedQuerySource { get; private set; }

    public override bool Equals(object obj) => obj is QuerySourceReferenceExpression referenceExpression && this.ReferencedQuerySource == referenceExpression.ReferencedQuerySource;

    public override int GetHashCode() => this.ReferencedQuerySource.GetHashCode();

    public override string ToString() => "[" + this.ReferencedQuerySource.ItemName + "]";

    protected override Expression Accept(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      return !(visitor is RelinqExpressionVisitor expressionVisitor) ? base.Accept(visitor) : expressionVisitor.VisitQuerySourceReference(this);
    }
  }
}
