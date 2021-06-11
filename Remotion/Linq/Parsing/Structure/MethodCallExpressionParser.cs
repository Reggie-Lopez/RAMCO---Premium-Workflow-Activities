// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.MethodCallExpressionParser
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure
{
  [ComVisible(true)]
  public sealed class MethodCallExpressionParser
  {
    private readonly INodeTypeProvider _nodeTypeProvider;

    public MethodCallExpressionParser(INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._nodeTypeProvider = nodeTypeProvider;
    }

    public IExpressionNode Parse(
      string associatedIdentifier,
      IExpressionNode source,
      IEnumerable<Expression> arguments,
      MethodCallExpression expressionToParse)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (associatedIdentifier), associatedIdentifier);
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (source), source);
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expressionToParse), expressionToParse);
      ArgumentUtility.CheckNotNull<IEnumerable<Expression>>(nameof (arguments), arguments);
      Type nodeType = this.GetNodeType(expressionToParse);
      Expression[] array = arguments.Select<Expression, Expression>(new Func<Expression, Expression>(this.ProcessArgumentExpression)).ToArray<Expression>();
      MethodCallExpressionParseInfo parseInfo = new MethodCallExpressionParseInfo(associatedIdentifier, source, expressionToParse);
      return this.CreateExpressionNode(nodeType, parseInfo, (object[]) array);
    }

    private Type GetNodeType(MethodCallExpression expressionToParse)
    {
      Type nodeType = this._nodeTypeProvider.GetNodeType(expressionToParse.Method);
      return !(nodeType == (Type) null) ? nodeType : throw this.CreateParsingErrorException(expressionToParse, "This overload of the method '{0}.{1}' is currently not supported.", (object) expressionToParse.Method.DeclaringType.FullName, (object) expressionToParse.Method.Name);
    }

    private Expression ProcessArgumentExpression(Expression argumentExpression) => SubQueryFindingExpressionVisitor.Process(this.UnwrapArgumentExpression(argumentExpression), this._nodeTypeProvider);

    private Expression UnwrapArgumentExpression(Expression expression)
    {
      if (expression.NodeType == ExpressionType.Quote)
        return ((UnaryExpression) expression).Operand;
      return expression.NodeType == ExpressionType.Constant && ((ConstantExpression) expression).Value is LambdaExpression ? (Expression) ((ConstantExpression) expression).Value : expression;
    }

    private IExpressionNode CreateExpressionNode(
      Type nodeType,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      try
      {
        return MethodCallExpressionNodeFactory.CreateExpressionNode(nodeType, parseInfo, additionalConstructorParameters);
      }
      catch (ExpressionNodeInstantiationException ex)
      {
        throw this.CreateParsingErrorException(parseInfo.ParsedExpression, "{0}", (object) ex.Message);
      }
    }

    private NotSupportedException CreateParsingErrorException(
      MethodCallExpression expression,
      string message,
      params object[] args)
    {
      return new NotSupportedException(string.Format("Could not parse expression '{0}': ", (object) expression.BuildString()) + string.Format(message, args));
    }
  }
}
