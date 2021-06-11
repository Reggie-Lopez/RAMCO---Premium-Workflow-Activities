// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors.TransformingExpressionTreeProcessor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors
{
  [ComVisible(true)]
  public sealed class TransformingExpressionTreeProcessor : IExpressionTreeProcessor
  {
    private readonly IExpressionTranformationProvider _provider;

    public TransformingExpressionTreeProcessor(IExpressionTranformationProvider provider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (provider), provider);
      this._provider = provider;
    }

    public IExpressionTranformationProvider Provider => this._provider;

    public Expression Process(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return TransformingExpressionVisitor.Transform(expressionTree, this._provider);
    }
  }
}
