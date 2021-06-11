// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.RelinqExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing
{
  [ComVisible(true)]
  public abstract class RelinqExpressionVisitor : ExpressionVisitor
  {
    public static IEnumerable<Expression> AdjustArgumentsForNewExpression(
      IList<Expression> arguments,
      IList<MemberInfo> members)
    {
      ArgumentUtility.CheckNotNull<IList<Expression>>(nameof (arguments), arguments);
      ArgumentUtility.CheckNotNull<IList<MemberInfo>>(nameof (members), members);
      if (arguments.Count != members.Count)
        throw new ArgumentException("Incorrect number of arguments for the given members.");
      for (int i = 0; i < arguments.Count; ++i)
      {
        Type memberReturnType = ReflectionUtility.GetMemberReturnType(members[i]);
        if (arguments[i].Type == memberReturnType)
          yield return arguments[i];
        else
          yield return (Expression) Expression.Convert(arguments[i], memberReturnType);
      }
    }

    protected override Expression VisitNew(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      ReadOnlyCollection<Expression> readOnlyCollection = this.VisitAndConvert<Expression>(expression.Arguments, nameof (VisitNew));
      if (readOnlyCollection == expression.Arguments)
        return (Expression) expression;
      return expression.Members == null ? (Expression) Expression.New(expression.Constructor, (IEnumerable<Expression>) readOnlyCollection) : (Expression) Expression.New(expression.Constructor, RelinqExpressionVisitor.AdjustArgumentsForNewExpression((IList<Expression>) readOnlyCollection, (IList<MemberInfo>) expression.Members), (IEnumerable<MemberInfo>) expression.Members);
    }

    protected internal virtual Expression VisitSubQuery(SubQueryExpression expression) => (Expression) expression;

    protected internal virtual Expression VisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      return (Expression) expression;
    }
  }
}
