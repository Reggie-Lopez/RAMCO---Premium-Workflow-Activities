// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation.EvaluatableTreeFindingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation
{
  [ComVisible(true)]
  public sealed class EvaluatableTreeFindingExpressionVisitor : 
    RelinqExpressionVisitor,
    IPartialEvaluationExceptionExpressionVisitor
  {
    private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;
    private readonly PartialEvaluationInfo _partialEvaluationInfo = new PartialEvaluationInfo();
    private bool _isCurrentSubtreeEvaluatable;

    public static PartialEvaluationInfo Analyze(
      [NotNull] Expression expressionTree,
      [NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      ArgumentUtility.CheckNotNull<IEvaluatableExpressionFilter>(nameof (evaluatableExpressionFilter), evaluatableExpressionFilter);
      EvaluatableTreeFindingExpressionVisitor expressionVisitor = new EvaluatableTreeFindingExpressionVisitor(evaluatableExpressionFilter);
      expressionVisitor.Visit(expressionTree);
      return expressionVisitor._partialEvaluationInfo;
    }

    private EvaluatableTreeFindingExpressionVisitor(
      IEvaluatableExpressionFilter evaluatableExpressionFilter)
    {
      this._evaluatableExpressionFilter = evaluatableExpressionFilter;
    }

    public override Expression Visit(Expression expression)
    {
      if (expression == null)
        return base.Visit((Expression) null);
      bool subtreeEvaluatable = this._isCurrentSubtreeEvaluatable;
      this._isCurrentSubtreeEvaluatable = this.IsCurrentExpressionEvaluatable(expression);
      Expression expression1 = base.Visit(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._partialEvaluationInfo.AddEvaluatableExpression(expression);
      this._isCurrentSubtreeEvaluatable &= subtreeEvaluatable;
      return expression1;
    }

    protected override Expression VisitBinary(BinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<BinaryExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitBinary(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableBinary(expression);
      return expression1;
    }

    protected override Expression VisitConditional(ConditionalExpression expression)
    {
      ArgumentUtility.CheckNotNull<ConditionalExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitConditional(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableConditional(expression);
      return expression1;
    }

    protected override Expression VisitConstant(ConstantExpression expression)
    {
      ArgumentUtility.CheckNotNull<ConstantExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitConstant(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableConstant(expression);
      return expression1;
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
      ArgumentUtility.CheckNotNull<ElementInit>(nameof (node), node);
      ElementInit elementInit = base.VisitElementInit(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableElementInit(node);
      return elementInit;
    }

    protected override Expression VisitInvocation(InvocationExpression expression)
    {
      ArgumentUtility.CheckNotNull<InvocationExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitInvocation(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableInvocation(expression);
      return expression1;
    }

    protected override Expression VisitLambda<T>(Expression<T> expression)
    {
      ArgumentUtility.CheckNotNull<Expression<T>>(nameof (expression), expression);
      Expression expression1 = base.VisitLambda<T>(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableLambda((LambdaExpression) expression);
      return expression1;
    }

    protected override Expression VisitMember(MemberExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (expression), expression);
      if (this.IsQueryableExpression(expression.Expression))
        this._isCurrentSubtreeEvaluatable = false;
      Expression expression1 = base.VisitMember(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMember(expression);
      return expression1;
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
      ArgumentUtility.CheckNotNull<MemberAssignment>(nameof (node), node);
      MemberAssignment memberAssignment = base.VisitMemberAssignment(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMemberAssignment(node);
      return memberAssignment;
    }

    protected override Expression VisitMemberInit(MemberInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberInitExpression>(nameof (expression), expression);
      ExpressionVisitor.Visit<MemberBinding>(expression.Bindings, new Func<MemberBinding, MemberBinding>(((ExpressionVisitor) this).VisitMemberBinding));
      if (!this._isCurrentSubtreeEvaluatable)
        return (Expression) expression;
      this.Visit((Expression) expression.NewExpression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMemberInit(expression);
      return (Expression) expression;
    }

    protected override MemberListBinding VisitMemberListBinding(
      MemberListBinding node)
    {
      ArgumentUtility.CheckNotNull<MemberListBinding>(nameof (node), node);
      MemberListBinding memberListBinding = base.VisitMemberListBinding(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMemberListBinding(node);
      return memberListBinding;
    }

    protected override Expression VisitMethodCall(MethodCallExpression expression)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expression), expression);
      if (this.IsQueryableExpression(expression.Object))
        this._isCurrentSubtreeEvaluatable = false;
      for (int index = 0; index < expression.Arguments.Count && this._isCurrentSubtreeEvaluatable; ++index)
      {
        if (this.IsQueryableExpression(expression.Arguments[index]))
          this._isCurrentSubtreeEvaluatable = false;
      }
      Expression expression1 = base.VisitMethodCall(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMethodCall(expression);
      return expression1;
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(
      MemberMemberBinding node)
    {
      ArgumentUtility.CheckNotNull<MemberMemberBinding>(nameof (node), node);
      MemberMemberBinding memberMemberBinding = base.VisitMemberMemberBinding(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableMemberMemberBinding(node);
      return memberMemberBinding;
    }

    protected override Expression VisitListInit(ListInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<ListInitExpression>(nameof (expression), expression);
      ExpressionVisitor.Visit<ElementInit>(expression.Initializers, new Func<ElementInit, ElementInit>(((ExpressionVisitor) this).VisitElementInit));
      if (!this._isCurrentSubtreeEvaluatable)
        return (Expression) expression;
      this.Visit((Expression) expression.NewExpression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableListInit(expression);
      return (Expression) expression;
    }

    protected override Expression VisitNew(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitNew(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableNew(expression);
      return expression1;
    }

    protected override Expression VisitParameter(ParameterExpression expression)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (expression), expression);
      this._isCurrentSubtreeEvaluatable = false;
      return base.VisitParameter(expression);
    }

    protected override Expression VisitNewArray(NewArrayExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewArrayExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitNewArray(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableNewArray(expression);
      return expression1;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<TypeBinaryExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitTypeBinary(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableTypeBinary(expression);
      return expression1;
    }

    protected override Expression VisitUnary(UnaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<UnaryExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitUnary(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableUnary(expression);
      return expression1;
    }

    protected override Expression VisitBlock(BlockExpression expression)
    {
      ArgumentUtility.CheckNotNull<BlockExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitBlock(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableBlock(expression);
      return expression1;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
      ArgumentUtility.CheckNotNull<CatchBlock>(nameof (node), node);
      CatchBlock catchBlock = base.VisitCatchBlock(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableCatchBlock(node);
      return catchBlock;
    }

    protected override Expression VisitDebugInfo(DebugInfoExpression expression)
    {
      ArgumentUtility.CheckNotNull<DebugInfoExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitDebugInfo(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableDebugInfo(expression);
      return expression1;
    }

    protected override Expression VisitDefault(DefaultExpression expression)
    {
      ArgumentUtility.CheckNotNull<DefaultExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitDefault(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableDefault(expression);
      return expression1;
    }

    protected override Expression VisitGoto(GotoExpression expression)
    {
      ArgumentUtility.CheckNotNull<GotoExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitGoto(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableGoto(expression);
      return expression1;
    }

    protected override Expression VisitIndex(IndexExpression expression)
    {
      ArgumentUtility.CheckNotNull<IndexExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitIndex(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableIndex(expression);
      return expression1;
    }

    protected override Expression VisitLabel(LabelExpression expression)
    {
      ArgumentUtility.CheckNotNull<LabelExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitLabel(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableLabel(expression);
      return expression1;
    }

    protected override LabelTarget VisitLabelTarget(LabelTarget node)
    {
      LabelTarget labelTarget = base.VisitLabelTarget(node);
      if (node == null || !this._isCurrentSubtreeEvaluatable)
        return labelTarget;
      this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableLabelTarget(node);
      return labelTarget;
    }

    protected override Expression VisitLoop(LoopExpression expression)
    {
      ArgumentUtility.CheckNotNull<LoopExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitLoop(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableLoop(expression);
      return expression1;
    }

    protected override Expression VisitSwitch(SwitchExpression expression)
    {
      ArgumentUtility.CheckNotNull<SwitchExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitSwitch(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableSwitch(expression);
      return expression1;
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
      ArgumentUtility.CheckNotNull<SwitchCase>(nameof (node), node);
      SwitchCase switchCase = base.VisitSwitchCase(node);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableSwitchCase(node);
      return switchCase;
    }

    protected override Expression VisitTry(TryExpression expression)
    {
      ArgumentUtility.CheckNotNull<TryExpression>(nameof (expression), expression);
      Expression expression1 = base.VisitTry(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._isCurrentSubtreeEvaluatable = this._evaluatableExpressionFilter.IsEvaluatableTry(expression);
      return expression1;
    }

    private bool IsCurrentExpressionEvaluatable(Expression expression)
    {
      if (expression.NodeType != ExpressionType.Extension)
        return Enum.IsDefined(typeof (ExpressionType), (object) expression.NodeType);
      return expression.CanReduce && this.IsCurrentExpressionEvaluatable(expression.ReduceAndCheck());
    }

    private bool IsQueryableExpression(Expression expression) => expression != null && typeof (IQueryable).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo());

    public Expression VisitPartialEvaluationException(
      PartialEvaluationExceptionExpression partialEvaluationExceptionExpression)
    {
      ArgumentUtility.CheckNotNull<PartialEvaluationExceptionExpression>(nameof (partialEvaluationExceptionExpression), partialEvaluationExceptionExpression);
      this._isCurrentSubtreeEvaluatable = false;
      return (Expression) partialEvaluationExceptionExpression;
    }
  }
}
