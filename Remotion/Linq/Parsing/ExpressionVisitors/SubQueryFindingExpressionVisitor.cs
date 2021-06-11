// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.SubQueryFindingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class SubQueryFindingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly INodeTypeProvider _nodeTypeProvider;
    private readonly ExpressionTreeParser _expressionTreeParser;
    private readonly QueryParser _queryParser;

    public static Expression Process(
      Expression expressionTree,
      INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      return new SubQueryFindingExpressionVisitor(nodeTypeProvider).Visit(expressionTree);
    }

    private SubQueryFindingExpressionVisitor(INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._nodeTypeProvider = nodeTypeProvider;
      this._expressionTreeParser = new ExpressionTreeParser(this._nodeTypeProvider, (IExpressionTreeProcessor) new NullExpressionTreeProcessor());
      this._queryParser = new QueryParser(this._expressionTreeParser);
    }

    public override Expression Visit(Expression expression)
    {
      MethodCallExpression operatorExpression = this._expressionTreeParser.GetQueryOperatorExpression(expression);
      return operatorExpression != null && this._nodeTypeProvider.IsRegistered(operatorExpression.Method) ? (Expression) this.CreateSubQueryNode(operatorExpression) : base.Visit(expression);
    }

    private SubQueryExpression CreateSubQueryNode(
      MethodCallExpression methodCallExpression)
    {
      return new SubQueryExpression(this._queryParser.GetParsedQuery((Expression) methodCallExpression));
    }
  }
}
