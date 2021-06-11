// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupByWithResultSelectorExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class GroupByWithResultSelectorExpressionNode : 
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    private readonly SelectExpressionNode _selectExpressionNode;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("GroupBy").WithResultSelector().WithoutEqualityComparer();

    public GroupByWithResultSelectorExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector,
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (elementSelectorOrResultSelector), elementSelectorOrResultSelector);
      this._selectExpressionNode = new SelectExpressionNode(GroupByWithResultSelectorExpressionNode.CreateParseInfoWithGroupNode(parseInfo, keySelector, elementSelectorOrResultSelector, resultSelectorOrNull), GroupByWithResultSelectorExpressionNode.CreateSelectorForSelectNode(keySelector, elementSelectorOrResultSelector, resultSelectorOrNull));
    }

    private static MethodCallExpressionParseInfo CreateParseInfoWithGroupNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector,
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      LambdaExpression optionalElementSelector = GroupByWithResultSelectorExpressionNode.GetOptionalElementSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      Type genericIenumerable = ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(parseInfo.ParsedExpression.Arguments[0].Type, "parseInfo.ParsedExpression.Arguments[0].Type");
      MethodCallExpression parsedExpression;
      if (optionalElementSelector == null)
        parsedExpression = Expression.Call(typeof (Enumerable), "GroupBy", new Type[2]
        {
          genericIenumerable,
          keySelector.Body.Type
        }, parseInfo.ParsedExpression.Arguments[0], (Expression) keySelector);
      else
        parsedExpression = Expression.Call(typeof (Enumerable), "GroupBy", new Type[3]
        {
          genericIenumerable,
          keySelector.Body.Type,
          optionalElementSelector.Body.Type
        }, parseInfo.ParsedExpression.Arguments[0], (Expression) keySelector, (Expression) optionalElementSelector);
      GroupByExpressionNode byExpressionNode = new GroupByExpressionNode(new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, parseInfo.Source, parsedExpression), keySelector, optionalElementSelector);
      return new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, (IExpressionNode) byExpressionNode, parseInfo.ParsedExpression);
    }

    private static LambdaExpression CreateSelectorForSelectNode(
      LambdaExpression keySelector,
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      LambdaExpression resultSelector = GroupByWithResultSelectorExpressionNode.GetResultSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      LambdaExpression optionalElementSelector = GroupByWithResultSelectorExpressionNode.GetOptionalElementSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      Type type1 = optionalElementSelector != null ? optionalElementSelector.Body.Type : keySelector.Parameters[0].Type;
      Type type2 = typeof (IGrouping<,>).MakeGenericType(keySelector.Body.Type, type1);
      PropertyInfo runtimeProperty = type2.GetRuntimeProperty("Key");
      ParameterExpression parameterExpression = Expression.Parameter(type2, "group");
      MemberExpression memberExpression = Expression.MakeMemberAccess((Expression) parameterExpression, (MemberInfo) runtimeProperty);
      return Expression.Lambda(MultiReplacingExpressionVisitor.Replace((IDictionary<Expression, Expression>) new Dictionary<Expression, Expression>(2)
      {
        {
          (Expression) resultSelector.Parameters[1],
          (Expression) parameterExpression
        },
        {
          (Expression) resultSelector.Parameters[0],
          (Expression) memberExpression
        }
      }, resultSelector.Body), parameterExpression);
    }

    private static LambdaExpression GetOptionalElementSelector(
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      return resultSelectorOrNull != null ? elementSelectorOrResultSelector : (LambdaExpression) null;
    }

    private static LambdaExpression GetResultSelector(
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      if (resultSelectorOrNull != null)
      {
        if (resultSelectorOrNull.Parameters.Count != 2)
          throw new ArgumentException("ResultSelector must have exactly two parameters.", nameof (resultSelectorOrNull));
        return resultSelectorOrNull;
      }
      if (elementSelectorOrResultSelector.Parameters.Count != 2)
        throw new ArgumentException("ResultSelector must have exactly two parameters.", nameof (elementSelectorOrResultSelector));
      return elementSelectorOrResultSelector;
    }

    public IExpressionNode Source => this._selectExpressionNode.Source;

    public string AssociatedIdentifier => this._selectExpressionNode.AssociatedIdentifier;

    public Expression Selector => (Expression) this._selectExpressionNode.Selector;

    public Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return this._selectExpressionNode.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    public QueryModel Apply(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      return this._selectExpressionNode.Apply(queryModel, clauseGenerationContext);
    }
  }
}
