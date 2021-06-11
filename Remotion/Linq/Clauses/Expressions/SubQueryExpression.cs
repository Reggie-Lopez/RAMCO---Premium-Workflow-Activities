// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.SubQueryExpression
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
  public sealed class SubQueryExpression : Expression
  {
    private readonly Type _type;

    public SubQueryExpression(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this._type = queryModel.GetOutputDataInfo().DataType;
      this.QueryModel = queryModel;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;

    public override Type Type => this._type;

    public QueryModel QueryModel { get; private set; }

    public override string ToString() => "{" + (object) this.QueryModel + "}";

    protected override Expression Accept(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      return !(visitor is RelinqExpressionVisitor expressionVisitor) ? base.Accept(visitor) : expressionVisitor.VisitSubQuery(this);
    }
  }
}
