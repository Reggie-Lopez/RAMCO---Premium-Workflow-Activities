// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ExpressionResolver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class ExpressionResolver
  {
    private readonly IExpressionNode _currentNode;

    public ExpressionResolver(IExpressionNode currentNode)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (currentNode), currentNode);
      this._currentNode = currentNode;
    }

    public IExpressionNode CurrentNode => this._currentNode;

    public Expression GetResolvedExpression(
      Expression unresolvedExpression,
      ParameterExpression parameterToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (unresolvedExpression), unresolvedExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (parameterToBeResolved), parameterToBeResolved);
      return TransparentIdentifierRemovingExpressionVisitor.ReplaceTransparentIdentifiers(this._currentNode.Source.Resolve(parameterToBeResolved, unresolvedExpression, clauseGenerationContext));
    }
  }
}
