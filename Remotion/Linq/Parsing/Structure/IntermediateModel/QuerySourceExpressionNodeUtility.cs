// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.QuerySourceExpressionNodeUtility
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public static class QuerySourceExpressionNodeUtility
  {
    public static Expression ReplaceParameterWithReference(
      IQuerySourceExpressionNode referencedNode,
      ParameterExpression parameterToReplace,
      Expression expression,
      ClauseGenerationContext context)
    {
      ArgumentUtility.CheckNotNull<IQuerySourceExpressionNode>(nameof (referencedNode), referencedNode);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (parameterToReplace), parameterToReplace);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<ClauseGenerationContext>(nameof (context), context);
      QuerySourceReferenceExpression referenceExpression = new QuerySourceReferenceExpression(QuerySourceExpressionNodeUtility.GetQuerySourceForNode(referencedNode, context));
      return ReplacingExpressionVisitor.Replace((Expression) parameterToReplace, (Expression) referenceExpression, expression);
    }

    public static IQuerySource GetQuerySourceForNode(
      IQuerySourceExpressionNode node,
      ClauseGenerationContext context)
    {
      try
      {
        return (IQuerySource) context.GetContextInfo((IExpressionNode) node);
      }
      catch (KeyNotFoundException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve an IQuerySource for the given {0}. Be sure to call Apply before calling methods that require IQuerySources, and pass in the same QuerySourceClauseMapping to both.", (object) node.GetType().Name), (Exception) ex);
      }
    }
  }
}
