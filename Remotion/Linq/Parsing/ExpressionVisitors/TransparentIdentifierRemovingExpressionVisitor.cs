// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TransparentIdentifierRemovingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class TransparentIdentifierRemovingExpressionVisitor : RelinqExpressionVisitor
  {
    public static Expression ReplaceTransparentIdentifiers(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      Expression node = expression;
      Expression expression1;
      do
      {
        expression1 = node;
        node = new TransparentIdentifierRemovingExpressionVisitor().Visit(node);
      }
      while (node != expression1);
      return node;
    }

    private TransparentIdentifierRemovingExpressionVisitor()
    {
    }

    protected override Expression VisitMember(MemberExpression memberExpression)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (memberExpression), memberExpression);
      IEnumerable<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding> createdByExpression = this.GetMemberBindingsCreatedByExpression(memberExpression.Expression);
      if (createdByExpression == null)
        return base.VisitMember(memberExpression);
      Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding memberBinding = createdByExpression.Where<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding>((Func<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding, bool>) (binding => binding.MatchesReadAccess(memberExpression.Member))).LastOrDefault<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding>();
      return memberBinding == null ? base.VisitMember(memberExpression) : memberBinding.AssociatedExpression;
    }

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(TransparentIdentifierRemovingExpressionVisitor.ReplaceTransparentIdentifiers));
      return (Expression) expression;
    }

    private IEnumerable<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding> GetMemberBindingsCreatedByExpression(
      Expression expression)
    {
      switch (expression)
      {
        case MemberInitExpression memberInitExpression:
          return memberInitExpression.Bindings.Where<System.Linq.Expressions.MemberBinding>((Func<System.Linq.Expressions.MemberBinding, bool>) (binding => binding is MemberAssignment)).Select<System.Linq.Expressions.MemberBinding, Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding>((Func<System.Linq.Expressions.MemberBinding, Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding>) (assignment => Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding.Bind(assignment.Member, ((MemberAssignment) assignment).Expression)));
        case NewExpression newExpression:
          if (newExpression.Members != null)
            return this.GetMemberBindingsForNewExpression(newExpression);
          break;
      }
      return (IEnumerable<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding>) null;
    }

    private IEnumerable<Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding> GetMemberBindingsForNewExpression(
      NewExpression newExpression)
    {
      for (int i = 0; i < newExpression.Members.Count; ++i)
        yield return Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding.Bind(newExpression.Members[i], newExpression.Arguments[i]);
    }
  }
}
