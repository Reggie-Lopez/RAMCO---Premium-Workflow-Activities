// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.VBStringComparisonExpression
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.Expressions
{
  [ComVisible(true)]
  public sealed class VBStringComparisonExpression : Expression
  {
    private readonly Expression _comparison;
    private readonly bool _textCompare;

    public VBStringComparisonExpression(Expression comparison, bool textCompare)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (comparison), comparison);
      this._comparison = comparison;
      this._textCompare = textCompare;
    }

    public override Type Type => this._comparison.Type;

    public override ExpressionType NodeType => ExpressionType.Extension;

    public Expression Comparison => this._comparison;

    public bool TextCompare => this._textCompare;

    public override bool CanReduce => true;

    public override Expression Reduce() => this._comparison;

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      Expression comparison = visitor.Visit(this._comparison);
      return comparison != this._comparison ? (Expression) new VBStringComparisonExpression(comparison, this._textCompare) : (Expression) this;
    }

    protected override Expression Accept(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      return visitor is IVBSpecificExpressionVisitor expressionVisitor ? expressionVisitor.VisitVBStringComparison(this) : base.Accept(visitor);
    }

    public override string ToString() => string.Format("VBCompareString({0}, {1})", (object) this.Comparison.BuildString(), (object) this.TextCompare);
  }
}
