// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.PartialEvaluationExceptionExpression
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
  public sealed class PartialEvaluationExceptionExpression : Expression
  {
    private readonly Exception _exception;
    private readonly Expression _evaluatedExpression;

    public PartialEvaluationExceptionExpression(Exception exception, Expression evaluatedExpression)
    {
      ArgumentUtility.CheckNotNull<Exception>(nameof (exception), exception);
      this._exception = exception;
      this._evaluatedExpression = evaluatedExpression;
    }

    public override Type Type => this._evaluatedExpression.Type;

    public override ExpressionType NodeType => ExpressionType.Extension;

    public Exception Exception => this._exception;

    public Expression EvaluatedExpression => this._evaluatedExpression;

    public override bool CanReduce => true;

    public override Expression Reduce() => this._evaluatedExpression;

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      Expression evaluatedExpression = visitor.Visit(this._evaluatedExpression);
      return evaluatedExpression != this._evaluatedExpression ? (Expression) new PartialEvaluationExceptionExpression(this._exception, evaluatedExpression) : (Expression) this;
    }

    protected override Expression Accept(ExpressionVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionVisitor>(nameof (visitor), visitor);
      return visitor is IPartialEvaluationExceptionExpressionVisitor expressionVisitor ? expressionVisitor.VisitPartialEvaluationException(this) : base.Accept(visitor);
    }

    public override string ToString() => string.Format("PartialEvalException ({0} (\"{1}\"), {2})", (object) this._exception.GetType().Name, (object) this._exception.Message, (object) this._evaluatedExpression.BuildString());
  }
}
