// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionVisitors.AccessorFindingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class AccessorFindingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly Expression _searchedExpression;
    private readonly ParameterExpression _inputParameter;
    private readonly Stack<Expression> _accessorPathStack = new Stack<Expression>();

    public static LambdaExpression FindAccessorLambda(
      Expression searchedExpression,
      Expression fullExpression,
      ParameterExpression inputParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (searchedExpression), searchedExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (fullExpression), fullExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      if (inputParameter.Type != fullExpression.Type)
        throw new ArgumentException(string.Format("The inputParameter's type '{0}' must match the fullExpression's type '{1}'.", (object) inputParameter.Type, (object) fullExpression.Type), nameof (inputParameter));
      AccessorFindingExpressionVisitor expressionVisitor = new AccessorFindingExpressionVisitor(searchedExpression, inputParameter);
      expressionVisitor.Visit(fullExpression);
      return expressionVisitor.AccessorPath != null ? expressionVisitor.AccessorPath : throw new ArgumentException(string.Format("The given expression '{0}' does not contain the searched expression '{1}' in a nested NewExpression with member assignments or a MemberBindingExpression.", (object) fullExpression.BuildString(), (object) searchedExpression.BuildString()), nameof (fullExpression));
    }

    private AccessorFindingExpressionVisitor(
      Expression searchedExpression,
      ParameterExpression inputParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (searchedExpression), searchedExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      this._searchedExpression = searchedExpression;
      this._inputParameter = inputParameter;
      this._accessorPathStack.Push((Expression) this._inputParameter);
    }

    private LambdaExpression AccessorPath { get; set; }

    public override Expression Visit(Expression expression)
    {
      if (object.Equals((object) expression, (object) this._searchedExpression))
      {
        this.AccessorPath = Expression.Lambda(this._accessorPathStack.Peek(), this._inputParameter);
        return expression;
      }
      switch (expression)
      {
        case NewExpression _:
        case MemberInitExpression _:
        case UnaryExpression _:
          return base.Visit(expression);
        default:
          return expression;
      }
    }

    protected override Expression VisitNew(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      if (expression.Members != null && expression.Members.Count > 0)
      {
        for (int index = 0; index < expression.Members.Count; ++index)
          this.CheckAndVisitMemberAssignment(expression.Members[index], expression.Arguments[index]);
      }
      return (Expression) expression;
    }

    protected override Expression VisitUnary(UnaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<UnaryExpression>(nameof (expression), expression);
      if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
      {
        this._accessorPathStack.Push((Expression) Expression.Convert(this._accessorPathStack.Peek(), expression.Operand.Type));
        base.VisitUnary(expression);
        this._accessorPathStack.Pop();
      }
      return (Expression) expression;
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding memberBinding)
    {
      ArgumentUtility.CheckNotNull<MemberBinding>(nameof (memberBinding), memberBinding);
      return memberBinding is MemberAssignment ? base.VisitMemberBinding(memberBinding) : memberBinding;
    }

    protected override MemberAssignment VisitMemberAssignment(
      MemberAssignment memberAssigment)
    {
      ArgumentUtility.CheckNotNull<MemberAssignment>(nameof (memberAssigment), memberAssigment);
      this.CheckAndVisitMemberAssignment(memberAssigment.Member, memberAssigment.Expression);
      return memberAssigment;
    }

    private void CheckAndVisitMemberAssignment(MemberInfo member, Expression expression)
    {
      this._accessorPathStack.Push(this.GetMemberAccessExpression(this._accessorPathStack.Peek(), member));
      this.Visit(expression);
      this._accessorPathStack.Pop();
    }

    private Expression GetMemberAccessExpression(Expression input, MemberInfo member)
    {
      Expression expression = this.EnsureMemberIsAccessibleFromInput(input, member);
      MethodInfo method = member as MethodInfo;
      return method != (MethodInfo) null ? (Expression) Expression.Call(expression, method) : (Expression) Expression.MakeMemberAccess(expression, member);
    }

    private Expression EnsureMemberIsAccessibleFromInput(
      Expression input,
      MemberInfo member)
    {
      TypeInfo typeInfo1 = member.DeclaringType.GetTypeInfo();
      TypeInfo typeInfo2 = input.Type.GetTypeInfo();
      if (typeInfo1.IsAssignableFrom(typeInfo2))
        return input;
      Assertion.IsTrue((typeInfo2.IsAssignableFrom(typeInfo1) ? 1 : 0) != 0, "Input expression of type '{0}' cannot be converted to declaring type '{1}' of member '{2}'.", (object) input.Type.FullName, (object) member.DeclaringType.FullName, (object) member.Name);
      return (Expression) Expression.Convert(input, member.DeclaringType);
    }
  }
}
