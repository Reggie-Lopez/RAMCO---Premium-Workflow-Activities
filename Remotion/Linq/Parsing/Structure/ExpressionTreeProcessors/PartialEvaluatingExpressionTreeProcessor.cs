// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors.PartialEvaluatingExpressionTreeProcessor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors
{
  [ComVisible(true)]
  public sealed class PartialEvaluatingExpressionTreeProcessor : IExpressionTreeProcessor
  {
    private readonly IEvaluatableExpressionFilter _filter;

    public PartialEvaluatingExpressionTreeProcessor(IEvaluatableExpressionFilter filter)
    {
      ArgumentUtility.CheckNotNull<IEvaluatableExpressionFilter>(nameof (filter), filter);
      this._filter = filter;
    }

    public IEvaluatableExpressionFilter Filter => this._filter;

    public Expression Process(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return PartialEvaluatingExpressionVisitor.EvaluateIndependentSubtrees(expressionTree, this._filter);
    }
  }
}
