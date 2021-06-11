// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation.IEvaluatableExpressionFilter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation
{
  [ComVisible(true)]
  public interface IEvaluatableExpressionFilter
  {
    bool IsEvaluatableBinary([NotNull] BinaryExpression node);

    bool IsEvaluatableConditional([NotNull] ConditionalExpression node);

    bool IsEvaluatableConstant([NotNull] ConstantExpression node);

    bool IsEvaluatableElementInit([NotNull] ElementInit node);

    bool IsEvaluatableInvocation([NotNull] InvocationExpression node);

    bool IsEvaluatableLambda([NotNull] LambdaExpression node);

    bool IsEvaluatableListInit([NotNull] ListInitExpression node);

    bool IsEvaluatableMember([NotNull] MemberExpression node);

    bool IsEvaluatableMemberAssignment([NotNull] MemberAssignment node);

    bool IsEvaluatableMemberInit([NotNull] MemberInitExpression node);

    bool IsEvaluatableMemberListBinding([NotNull] MemberListBinding node);

    bool IsEvaluatableMemberMemberBinding([NotNull] MemberMemberBinding node);

    bool IsEvaluatableMethodCall([NotNull] MethodCallExpression node);

    bool IsEvaluatableNew([NotNull] NewExpression node);

    bool IsEvaluatableNewArray([NotNull] NewArrayExpression node);

    bool IsEvaluatableTypeBinary([NotNull] TypeBinaryExpression node);

    bool IsEvaluatableUnary([NotNull] UnaryExpression node);

    bool IsEvaluatableBlock([NotNull] BlockExpression node);

    bool IsEvaluatableCatchBlock([NotNull] CatchBlock node);

    bool IsEvaluatableDebugInfo([NotNull] DebugInfoExpression node);

    bool IsEvaluatableDefault([NotNull] DefaultExpression node);

    bool IsEvaluatableGoto([NotNull] GotoExpression node);

    bool IsEvaluatableIndex([NotNull] IndexExpression node);

    bool IsEvaluatableLabel([NotNull] LabelExpression node);

    bool IsEvaluatableLabelTarget([NotNull] LabelTarget node);

    bool IsEvaluatableLoop([NotNull] LoopExpression node);

    bool IsEvaluatableSwitch([NotNull] SwitchExpression node);

    bool IsEvaluatableSwitchCase([NotNull] SwitchCase node);

    bool IsEvaluatableTry([NotNull] TryExpression node);
  }
}
