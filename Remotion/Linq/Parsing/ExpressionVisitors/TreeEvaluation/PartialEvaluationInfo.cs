// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation.PartialEvaluationInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation
{
  [ComVisible(true)]
  public class PartialEvaluationInfo
  {
    private readonly HashSet<Expression> _evaluatableExpressions = new HashSet<Expression>();

    public int Count => this._evaluatableExpressions.Count;

    public void AddEvaluatableExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      this._evaluatableExpressions.Add(expression);
    }

    public bool IsEvaluatableExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this._evaluatableExpressions.Contains(expression);
    }
  }
}
