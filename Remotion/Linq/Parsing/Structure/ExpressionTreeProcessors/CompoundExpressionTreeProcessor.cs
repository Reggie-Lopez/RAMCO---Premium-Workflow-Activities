// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors.CompoundExpressionTreeProcessor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors
{
  [ComVisible(true)]
  public sealed class CompoundExpressionTreeProcessor : IExpressionTreeProcessor
  {
    private readonly List<IExpressionTreeProcessor> _innerProcessors;

    public CompoundExpressionTreeProcessor(
      IEnumerable<IExpressionTreeProcessor> innerProcessors)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<IExpressionTreeProcessor>>(nameof (innerProcessors), innerProcessors);
      this._innerProcessors = new List<IExpressionTreeProcessor>(innerProcessors);
    }

    public IList<IExpressionTreeProcessor> InnerProcessors => (IList<IExpressionTreeProcessor>) this._innerProcessors;

    public Expression Process(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return this._innerProcessors.Aggregate<IExpressionTreeProcessor, Expression>(expressionTree, (Func<Expression, IExpressionTreeProcessor, Expression>) ((expr, processor) => processor.Process(expr)));
    }
  }
}
