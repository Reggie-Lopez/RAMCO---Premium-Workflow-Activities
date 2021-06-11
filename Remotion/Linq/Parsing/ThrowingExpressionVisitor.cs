// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ThrowingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing
{
  [ComVisible(true)]
  public abstract class ThrowingExpressionVisitor : RelinqExpressionVisitor
  {
    private static readonly Assembly s_systemLinqAssembly = typeof (Expression).GetTypeInfo().Assembly;

    protected abstract Exception CreateUnhandledItemException<T>(
      T unhandledItem,
      string visitMethod);

    public override Expression Visit(Expression expression)
    {
      if (expression == null)
        return base.Visit((Expression) null);
      if (!object.ReferenceEquals((object) ThrowingExpressionVisitor.s_systemLinqAssembly, (object) expression.GetType().GetTypeInfo().Assembly))
        return base.Visit(expression);
      return this.IsWellKnownStandardExpression(expression) ? base.Visit(expression) : this.VisitUnknownStandardExpression(expression, nameof (Visit) + (object) expression.NodeType, new Func<Expression, Expression>(((ExpressionVisitor) this).Visit));
    }

    protected virtual Expression VisitUnknownStandardExpression(
      Expression expression,
      string visitMethod,
      Func<Expression, Expression> baseBehavior)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      throw this.CreateUnhandledItemException<Expression>(expression, visitMethod);
    }

    private bool IsWellKnownStandardExpression(Expression expression)
    {
      switch (expression)
      {
        case UnaryExpression _:
          return true;
        case BinaryExpression _:
          return true;
        case TypeBinaryExpression _:
          return true;
        case ConstantExpression _:
          return true;
        case ConditionalExpression _:
          return true;
        case ParameterExpression _:
          return true;
        case LambdaExpression _:
          return true;
        case MethodCallExpression _:
          return true;
        case InvocationExpression _:
          return true;
        case MemberExpression _:
          return true;
        case NewExpression _:
          return true;
        case NewArrayExpression _:
          return true;
        case MemberInitExpression _:
          return true;
        case ListInitExpression _:
          return true;
        case BlockExpression _:
          return true;
        case DebugInfoExpression _:
          return true;
        case DefaultExpression _:
          return true;
        case GotoExpression _:
          return true;
        case IndexExpression _:
          return true;
        case LabelExpression _:
          return true;
        case LoopExpression _:
          return true;
        case RuntimeVariablesExpression _:
          return true;
        case SwitchExpression _:
          return true;
        case TryExpression _:
          return true;
        default:
          return false;
      }
    }

    protected virtual TResult VisitUnhandledItem<TItem, TResult>(
      TItem unhandledItem,
      string visitMethod,
      Func<TItem, TResult> baseBehavior)
      where TItem : TResult
    {
      ArgumentUtility.CheckNotNull<TItem>(nameof (unhandledItem), unhandledItem);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (visitMethod), visitMethod);
      ArgumentUtility.CheckNotNull<Func<TItem, TResult>>(nameof (baseBehavior), baseBehavior);
      throw this.CreateUnhandledItemException<TItem>(unhandledItem, visitMethod);
    }

    protected override Expression VisitExtension(Expression expression) => expression.CanReduce ? this.Visit(expression.ReduceAndCheck()) : this.VisitUnhandledItem<Expression, Expression>(expression, nameof (VisitExtension), new Func<Expression, Expression>(this.BaseVisitExtension));

    protected Expression BaseVisitExtension(Expression expression) => base.VisitExtension(expression);

    protected override Expression VisitUnary(UnaryExpression expression) => this.VisitUnhandledItem<UnaryExpression, Expression>(expression, nameof (VisitUnary), new Func<UnaryExpression, Expression>(this.BaseVisitUnary));

    protected Expression BaseVisitUnary(UnaryExpression expression) => base.VisitUnary(expression);

    protected override Expression VisitBinary(BinaryExpression expression) => this.VisitUnhandledItem<BinaryExpression, Expression>(expression, nameof (VisitBinary), new Func<BinaryExpression, Expression>(this.BaseVisitBinary));

    protected Expression BaseVisitBinary(BinaryExpression expression) => base.VisitBinary(expression);

    protected override Expression VisitTypeBinary(TypeBinaryExpression expression) => this.VisitUnhandledItem<TypeBinaryExpression, Expression>(expression, nameof (VisitTypeBinary), new Func<TypeBinaryExpression, Expression>(this.BaseVisitTypeBinary));

    protected Expression BaseVisitTypeBinary(TypeBinaryExpression expression) => base.VisitTypeBinary(expression);

    protected override Expression VisitConstant(ConstantExpression expression) => this.VisitUnhandledItem<ConstantExpression, Expression>(expression, nameof (VisitConstant), new Func<ConstantExpression, Expression>(this.BaseVisitConstant));

    protected Expression BaseVisitConstant(ConstantExpression expression) => base.VisitConstant(expression);

    protected override Expression VisitConditional(ConditionalExpression expression) => this.VisitUnhandledItem<ConditionalExpression, Expression>(expression, nameof (VisitConditional), new Func<ConditionalExpression, Expression>(this.BaseVisitConditional));

    protected Expression BaseVisitConditional(ConditionalExpression arg) => base.VisitConditional(arg);

    protected override Expression VisitParameter(ParameterExpression expression) => this.VisitUnhandledItem<ParameterExpression, Expression>(expression, nameof (VisitParameter), new Func<ParameterExpression, Expression>(this.BaseVisitParameter));

    protected Expression BaseVisitParameter(ParameterExpression expression) => base.VisitParameter(expression);

    protected override Expression VisitLambda<T>(Expression<T> expression) => this.VisitUnhandledItem<Expression<T>, Expression>(expression, nameof (VisitLambda), new Func<Expression<T>, Expression>(this.BaseVisitLambda<T>));

    protected Expression BaseVisitLambda<T>(Expression<T> expression) => base.VisitLambda<T>(expression);

    protected override Expression VisitMethodCall(MethodCallExpression expression) => this.VisitUnhandledItem<MethodCallExpression, Expression>(expression, nameof (VisitMethodCall), new Func<MethodCallExpression, Expression>(this.BaseVisitMethodCall));

    protected Expression BaseVisitMethodCall(MethodCallExpression expression) => base.VisitMethodCall(expression);

    protected override Expression VisitInvocation(InvocationExpression expression) => this.VisitUnhandledItem<InvocationExpression, Expression>(expression, nameof (VisitInvocation), new Func<InvocationExpression, Expression>(this.BaseVisitInvocation));

    protected Expression BaseVisitInvocation(InvocationExpression expression) => base.VisitInvocation(expression);

    protected override Expression VisitMember(MemberExpression expression) => this.VisitUnhandledItem<MemberExpression, Expression>(expression, nameof (VisitMember), new Func<MemberExpression, Expression>(this.BaseVisitMember));

    protected Expression BaseVisitMember(MemberExpression expression) => base.VisitMember(expression);

    protected override Expression VisitNew(NewExpression expression) => this.VisitUnhandledItem<NewExpression, Expression>(expression, nameof (VisitNew), new Func<NewExpression, Expression>(this.BaseVisitNew));

    protected Expression BaseVisitNew(NewExpression expression) => base.VisitNew(expression);

    protected override Expression VisitNewArray(NewArrayExpression expression) => this.VisitUnhandledItem<NewArrayExpression, Expression>(expression, nameof (VisitNewArray), new Func<NewArrayExpression, Expression>(this.BaseVisitNewArray));

    protected Expression BaseVisitNewArray(NewArrayExpression expression) => base.VisitNewArray(expression);

    protected override Expression VisitMemberInit(MemberInitExpression expression) => this.VisitUnhandledItem<MemberInitExpression, Expression>(expression, nameof (VisitMemberInit), new Func<MemberInitExpression, Expression>(this.BaseVisitMemberInit));

    protected Expression BaseVisitMemberInit(MemberInitExpression expression) => base.VisitMemberInit(expression);

    protected override Expression VisitListInit(ListInitExpression expression) => this.VisitUnhandledItem<ListInitExpression, Expression>(expression, nameof (VisitListInit), new Func<ListInitExpression, Expression>(this.BaseVisitListInit));

    protected Expression BaseVisitListInit(ListInitExpression expression) => base.VisitListInit(expression);

    protected override Expression VisitBlock(BlockExpression expression) => this.VisitUnhandledItem<BlockExpression, Expression>(expression, nameof (VisitBlock), new Func<BlockExpression, Expression>(this.BaseVisitBlock));

    protected Expression BaseVisitBlock(BlockExpression expression) => base.VisitBlock(expression);

    protected override Expression VisitDebugInfo(DebugInfoExpression expression) => this.VisitUnhandledItem<DebugInfoExpression, Expression>(expression, nameof (VisitDebugInfo), new Func<DebugInfoExpression, Expression>(this.BaseVisitDebugInfo));

    protected Expression BaseVisitDebugInfo(DebugInfoExpression expression) => base.VisitDebugInfo(expression);

    protected override Expression VisitDefault(DefaultExpression expression) => this.VisitUnhandledItem<DefaultExpression, Expression>(expression, nameof (VisitDefault), new Func<DefaultExpression, Expression>(this.BaseVisitDefault));

    protected Expression BaseVisitDefault(DefaultExpression expression) => base.VisitDefault(expression);

    protected override Expression VisitGoto(GotoExpression expression) => this.VisitUnhandledItem<GotoExpression, Expression>(expression, nameof (VisitGoto), new Func<GotoExpression, Expression>(this.BaseVisitGoto));

    protected Expression BaseVisitGoto(GotoExpression expression) => base.VisitGoto(expression);

    protected override Expression VisitIndex(IndexExpression expression) => this.VisitUnhandledItem<IndexExpression, Expression>(expression, nameof (VisitIndex), new Func<IndexExpression, Expression>(this.BaseVisitIndex));

    protected Expression BaseVisitIndex(IndexExpression expression) => base.VisitIndex(expression);

    protected override Expression VisitLabel(LabelExpression expression) => this.VisitUnhandledItem<LabelExpression, Expression>(expression, nameof (VisitLabel), new Func<LabelExpression, Expression>(this.BaseVisitLabel));

    protected Expression BaseVisitLabel(LabelExpression expression) => base.VisitLabel(expression);

    protected override Expression VisitLoop(LoopExpression expression) => this.VisitUnhandledItem<LoopExpression, Expression>(expression, nameof (VisitLoop), new Func<LoopExpression, Expression>(this.BaseVisitLoop));

    protected Expression BaseVisitLoop(LoopExpression expression) => base.VisitLoop(expression);

    protected override Expression VisitRuntimeVariables(
      RuntimeVariablesExpression expression)
    {
      return this.VisitUnhandledItem<RuntimeVariablesExpression, Expression>(expression, nameof (VisitRuntimeVariables), new Func<RuntimeVariablesExpression, Expression>(this.BaseVisitRuntimeVariables));
    }

    protected Expression BaseVisitRuntimeVariables(RuntimeVariablesExpression expression) => base.VisitRuntimeVariables(expression);

    protected override Expression VisitSwitch(SwitchExpression expression) => this.VisitUnhandledItem<SwitchExpression, Expression>(expression, nameof (VisitSwitch), new Func<SwitchExpression, Expression>(this.BaseVisitSwitch));

    protected Expression BaseVisitSwitch(SwitchExpression expression) => base.VisitSwitch(expression);

    protected override Expression VisitTry(TryExpression expression) => this.VisitUnhandledItem<TryExpression, Expression>(expression, nameof (VisitTry), new Func<TryExpression, Expression>(this.BaseVisitTry));

    protected Expression BaseVisitTry(TryExpression expression) => base.VisitTry(expression);

    protected override MemberBinding VisitMemberBinding(MemberBinding expression) => this.BaseVisitMemberBinding(expression);

    protected MemberBinding BaseVisitMemberBinding(MemberBinding expression) => base.VisitMemberBinding(expression);

    protected override ElementInit VisitElementInit(ElementInit elementInit) => this.VisitUnhandledItem<ElementInit, ElementInit>(elementInit, nameof (VisitElementInit), new Func<ElementInit, ElementInit>(this.BaseVisitElementInit));

    protected ElementInit BaseVisitElementInit(ElementInit elementInit) => base.VisitElementInit(elementInit);

    protected override MemberAssignment VisitMemberAssignment(
      MemberAssignment memberAssigment)
    {
      return this.VisitUnhandledItem<MemberAssignment, MemberAssignment>(memberAssigment, nameof (VisitMemberAssignment), new Func<MemberAssignment, MemberAssignment>(this.BaseVisitMemberAssignment));
    }

    protected MemberAssignment BaseVisitMemberAssignment(
      MemberAssignment memberAssigment)
    {
      return base.VisitMemberAssignment(memberAssigment);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(
      MemberMemberBinding binding)
    {
      return this.VisitUnhandledItem<MemberMemberBinding, MemberMemberBinding>(binding, nameof (VisitMemberMemberBinding), new Func<MemberMemberBinding, MemberMemberBinding>(this.BaseVisitMemberMemberBinding));
    }

    protected MemberMemberBinding BaseVisitMemberMemberBinding(
      MemberMemberBinding binding)
    {
      return base.VisitMemberMemberBinding(binding);
    }

    protected override MemberListBinding VisitMemberListBinding(
      MemberListBinding listBinding)
    {
      return this.VisitUnhandledItem<MemberListBinding, MemberListBinding>(listBinding, nameof (VisitMemberListBinding), new Func<MemberListBinding, MemberListBinding>(this.BaseVisitMemberListBinding));
    }

    protected MemberListBinding BaseVisitMemberListBinding(
      MemberListBinding listBinding)
    {
      return base.VisitMemberListBinding(listBinding);
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock expression) => this.VisitUnhandledItem<CatchBlock, CatchBlock>(expression, nameof (VisitCatchBlock), new Func<CatchBlock, CatchBlock>(this.BaseVisitCatchBlock));

    protected CatchBlock BaseVisitCatchBlock(CatchBlock expression) => base.VisitCatchBlock(expression);

    protected override LabelTarget VisitLabelTarget(LabelTarget expression) => this.VisitUnhandledItem<LabelTarget, LabelTarget>(expression, nameof (VisitLabelTarget), new Func<LabelTarget, LabelTarget>(this.BaseVisitLabelTarget));

    protected LabelTarget BaseVisitLabelTarget(LabelTarget expression) => base.VisitLabelTarget(expression);

    protected override SwitchCase VisitSwitchCase(SwitchCase expression) => this.VisitUnhandledItem<SwitchCase, SwitchCase>(expression, nameof (VisitSwitchCase), new Func<SwitchCase, SwitchCase>(this.BaseVisitSwitchCase));

    protected SwitchCase BaseVisitSwitchCase(SwitchCase expression) => base.VisitSwitchCase(expression);

    protected internal override Expression VisitSubQuery(SubQueryExpression expression) => this.VisitUnhandledItem<SubQueryExpression, Expression>(expression, nameof (VisitSubQuery), new Func<SubQueryExpression, Expression>(this.BaseVisitSubQuery));

    protected Expression BaseVisitSubQuery(SubQueryExpression expression) => base.VisitSubQuery(expression);

    protected internal override Expression VisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      return this.VisitUnhandledItem<QuerySourceReferenceExpression, Expression>(expression, nameof (VisitQuerySourceReference), new Func<QuerySourceReferenceExpression, Expression>(this.BaseVisitQuerySourceReference));
    }

    protected Expression BaseVisitQuerySourceReference(
      QuerySourceReferenceExpression expression)
    {
      return base.VisitQuerySourceReference(expression);
    }
  }
}
