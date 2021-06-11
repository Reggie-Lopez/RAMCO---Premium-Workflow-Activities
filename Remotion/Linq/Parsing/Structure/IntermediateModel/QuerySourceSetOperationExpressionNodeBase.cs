// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.QuerySourceSetOperationExpressionNodeBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public abstract class QuerySourceSetOperationExpressionNodeBase : 
    ResultOperatorExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    private readonly Expression _source2;
    private readonly Type _itemType;

    protected QuerySourceSetOperationExpressionNodeBase(
      MethodCallExpressionParseInfo parseInfo,
      Expression source2)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this._source2 = source2;
      this._itemType = ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(parseInfo.ParsedExpression.Type, "expression");
    }

    public Expression Source2 => this._source2;

    public Type ItemType => this._itemType;

    public override sealed Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected abstract ResultOperatorBase CreateSpecificResultOperator();

    protected override sealed ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      ResultOperatorBase specificResultOperator = this.CreateSpecificResultOperator();
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) specificResultOperator);
      return specificResultOperator;
    }
  }
}
