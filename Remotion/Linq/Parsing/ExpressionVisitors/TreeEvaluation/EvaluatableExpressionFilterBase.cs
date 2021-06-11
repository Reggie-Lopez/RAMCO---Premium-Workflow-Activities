// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation.EvaluatableExpressionFilterBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation
{
  [ComVisible(true)]
  public abstract class EvaluatableExpressionFilterBase : IEvaluatableExpressionFilter
  {
    public virtual bool IsEvaluatableBinary(BinaryExpression node)
    {
      ArgumentUtility.CheckNotNull<BinaryExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableConditional(ConditionalExpression node)
    {
      ArgumentUtility.CheckNotNull<ConditionalExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableConstant(ConstantExpression node)
    {
      ArgumentUtility.CheckNotNull<ConstantExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableElementInit(ElementInit node)
    {
      ArgumentUtility.CheckNotNull<ElementInit>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableInvocation(InvocationExpression node)
    {
      ArgumentUtility.CheckNotNull<InvocationExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableLambda(LambdaExpression node)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableListInit(ListInitExpression node)
    {
      ArgumentUtility.CheckNotNull<ListInitExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMember(MemberExpression node)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMemberAssignment(MemberAssignment node)
    {
      ArgumentUtility.CheckNotNull<MemberAssignment>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMemberInit(MemberInitExpression node)
    {
      ArgumentUtility.CheckNotNull<MemberInitExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMemberListBinding(MemberListBinding node)
    {
      ArgumentUtility.CheckNotNull<MemberListBinding>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMemberMemberBinding(MemberMemberBinding node)
    {
      ArgumentUtility.CheckNotNull<MemberMemberBinding>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableMethodCall(MethodCallExpression node)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableNew(NewExpression node)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableNewArray(NewArrayExpression node)
    {
      ArgumentUtility.CheckNotNull<NewArrayExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableTypeBinary(TypeBinaryExpression node)
    {
      ArgumentUtility.CheckNotNull<TypeBinaryExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableUnary(UnaryExpression node)
    {
      ArgumentUtility.CheckNotNull<UnaryExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableBlock(BlockExpression node)
    {
      ArgumentUtility.CheckNotNull<BlockExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableCatchBlock(CatchBlock node)
    {
      ArgumentUtility.CheckNotNull<CatchBlock>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableDebugInfo(DebugInfoExpression node)
    {
      ArgumentUtility.CheckNotNull<DebugInfoExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableDefault(DefaultExpression node)
    {
      ArgumentUtility.CheckNotNull<DefaultExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableGoto(GotoExpression node)
    {
      ArgumentUtility.CheckNotNull<GotoExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableIndex(IndexExpression node)
    {
      ArgumentUtility.CheckNotNull<IndexExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableLabel(LabelExpression node)
    {
      ArgumentUtility.CheckNotNull<LabelExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableLabelTarget(LabelTarget node)
    {
      ArgumentUtility.CheckNotNull<LabelTarget>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableLoop(LoopExpression node)
    {
      ArgumentUtility.CheckNotNull<LoopExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableSwitch(SwitchExpression node)
    {
      ArgumentUtility.CheckNotNull<SwitchExpression>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableSwitchCase(SwitchCase node)
    {
      ArgumentUtility.CheckNotNull<SwitchCase>(nameof (node), node);
      return true;
    }

    public virtual bool IsEvaluatableTry(TryExpression node)
    {
      ArgumentUtility.CheckNotNull<TryExpression>(nameof (node), node);
      return true;
    }
  }
}
