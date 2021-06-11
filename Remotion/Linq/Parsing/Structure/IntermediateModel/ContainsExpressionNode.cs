// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ContainsExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class ContainsExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly Expression _item;

    public static IEnumerable<MethodInfo> GetSupportedMethods() => ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Contains").WithoutEqualityComparer();

    public static IEnumerable<NameBasedRegistrationInfo> GetSupportedMethodNames()
    {
      yield return new NameBasedRegistrationInfo("Contains", (Func<MethodInfo, bool>) (mi =>
      {
        if (!(mi.DeclaringType != typeof (string)) || !typeof (IEnumerable).GetTypeInfo().IsAssignableFrom(mi.DeclaringType.GetTypeInfo()) || typeof (IDictionary).GetTypeInfo().IsAssignableFrom(mi.DeclaringType.GetTypeInfo()))
          return false;
        if (mi.IsStatic && mi.GetParameters().Length == 2)
          return true;
        return !mi.IsStatic && mi.GetParameters().Length == 1;
      }));
    }

    public ContainsExpressionNode(MethodCallExpressionParseInfo parseInfo, Expression item)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (item), item);
      this._item = item;
    }

    public Expression Item => this._item;

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new ContainsResultOperator(this._item);
    }
  }
}
