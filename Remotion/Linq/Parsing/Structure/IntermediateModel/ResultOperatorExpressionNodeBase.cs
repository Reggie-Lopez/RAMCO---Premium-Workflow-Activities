// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ResultOperatorExpressionNodeBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public abstract class ResultOperatorExpressionNodeBase : MethodCallExpressionNodeBase
  {
    private readonly MethodCallExpression _parsedExpression;

    protected ResultOperatorExpressionNodeBase(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression optionalPredicate,
      LambdaExpression optionalSelector)
      : base(ResultOperatorExpressionNodeBase.TransformParseInfo(parseInfo, optionalPredicate, optionalSelector))
    {
      if (optionalPredicate != null && optionalPredicate.Parameters.Count != 1)
        throw new ArgumentException("OptionalPredicate must have exactly one parameter.", nameof (optionalPredicate));
      if (optionalSelector != null && optionalSelector.Parameters.Count != 1)
        throw new ArgumentException("OptionalSelector must have exactly one parameter.", nameof (optionalSelector));
      this._parsedExpression = parseInfo.ParsedExpression;
    }

    protected abstract ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext);

    public MethodCallExpression ParsedExpression => this._parsedExpression;

    protected override void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ResultOperatorBase resultOperator = this.CreateResultOperator(clauseGenerationContext);
      queryModel.ResultOperators.Add(resultOperator);
    }

    protected override sealed QueryModel WrapQueryModelAfterEndOfQuery(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      return queryModel;
    }

    private static MethodCallExpressionParseInfo TransformParseInfo(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression optionalPredicate,
      LambdaExpression optionalSelector)
    {
      IExpressionNode source = parseInfo.Source;
      if (optionalPredicate != null)
        source = (IExpressionNode) new WhereExpressionNode(parseInfo, optionalPredicate);
      if (optionalSelector != null)
        source = (IExpressionNode) new SelectExpressionNode(new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, source, parseInfo.ParsedExpression), optionalSelector);
      return new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, source, parseInfo.ParsedExpression);
    }
  }
}
