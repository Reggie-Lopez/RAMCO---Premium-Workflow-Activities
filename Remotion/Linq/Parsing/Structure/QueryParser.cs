// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.QueryParser
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure
{
  [ComVisible(true)]
  public sealed class QueryParser : IQueryParser
  {
    private readonly ExpressionTreeParser _expressionTreeParser;

    public static QueryParser CreateDefault()
    {
      ExpressionTransformerRegistry transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
      NullEvaluatableExpressionFilter expressionFilter = new NullEvaluatableExpressionFilter();
      return new QueryParser(new ExpressionTreeParser((INodeTypeProvider) ExpressionTreeParser.CreateDefaultNodeTypeProvider(), (IExpressionTreeProcessor) ExpressionTreeParser.CreateDefaultProcessor((IExpressionTranformationProvider) transformerRegistry, (IEvaluatableExpressionFilter) expressionFilter)));
    }

    public QueryParser(ExpressionTreeParser expressionTreeParser)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeParser>(nameof (expressionTreeParser), expressionTreeParser);
      this._expressionTreeParser = expressionTreeParser;
    }

    public ExpressionTreeParser ExpressionTreeParser => this._expressionTreeParser;

    public INodeTypeProvider NodeTypeProvider => this._expressionTreeParser.NodeTypeProvider;

    public IExpressionTreeProcessor Processor => this._expressionTreeParser.Processor;

    public QueryModel GetParsedQuery(Expression expressionTreeRoot)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTreeRoot), expressionTreeRoot);
      return this.ApplyAllNodes(this._expressionTreeParser.ParseTree(expressionTreeRoot), new ClauseGenerationContext(this._expressionTreeParser.NodeTypeProvider));
    }

    private QueryModel ApplyAllNodes(
      IExpressionNode node,
      ClauseGenerationContext clauseGenerationContext)
    {
      QueryModel queryModel = (QueryModel) null;
      if (node.Source != null)
        queryModel = this.ApplyAllNodes(node.Source, clauseGenerationContext);
      return node.Apply(queryModel, clauseGenerationContext);
    }
  }
}
