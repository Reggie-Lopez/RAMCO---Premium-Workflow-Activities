// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.PartialEvaluatingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class PartialEvaluatingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly PartialEvaluationInfo _partialEvaluationInfo;
    private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;

    public static Expression EvaluateIndependentSubtrees(
      Expression expressionTree,
      IEvaluatableExpressionFilter evaluatableExpressionFilter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      ArgumentUtility.CheckNotNull<IEvaluatableExpressionFilter>(nameof (evaluatableExpressionFilter), evaluatableExpressionFilter);
      return new PartialEvaluatingExpressionVisitor(EvaluatableTreeFindingExpressionVisitor.Analyze(expressionTree, evaluatableExpressionFilter), evaluatableExpressionFilter).Visit(expressionTree);
    }

    private PartialEvaluatingExpressionVisitor(
      PartialEvaluationInfo partialEvaluationInfo,
      IEvaluatableExpressionFilter evaluatableExpressionFilter)
    {
      ArgumentUtility.CheckNotNull<PartialEvaluationInfo>(nameof (partialEvaluationInfo), partialEvaluationInfo);
      ArgumentUtility.CheckNotNull<IEvaluatableExpressionFilter>(nameof (evaluatableExpressionFilter), evaluatableExpressionFilter);
      this._partialEvaluationInfo = partialEvaluationInfo;
      this._evaluatableExpressionFilter = evaluatableExpressionFilter;
    }

    public override Expression Visit(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      if (expression.NodeType != ExpressionType.Lambda)
      {
        if (this._partialEvaluationInfo.IsEvaluatableExpression(expression))
        {
          Expression subtree;
          try
          {
            subtree = this.EvaluateSubtree(expression);
          }
          catch (Exception ex)
          {
            return (Expression) new PartialEvaluationExceptionExpression(ex, base.Visit(expression));
          }
          return subtree != expression ? PartialEvaluatingExpressionVisitor.EvaluateIndependentSubtrees(subtree, this._evaluatableExpressionFilter) : subtree;
        }
      }
      return base.Visit(expression);
    }

    private Expression EvaluateSubtree(Expression subtree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (subtree), subtree);
      if (subtree.NodeType != ExpressionType.Constant)
        return (Expression) Expression.Constant(Expression.Lambda<Func<object>>((Expression) Expression.Convert(subtree, typeof (object))).Compile()(), subtree.Type);
      ConstantExpression constantExpression = (ConstantExpression) subtree;
      return constantExpression.Value is IQueryable queryable && queryable.Expression != constantExpression ? queryable.Expression : (Expression) constantExpression;
    }
  }
}
