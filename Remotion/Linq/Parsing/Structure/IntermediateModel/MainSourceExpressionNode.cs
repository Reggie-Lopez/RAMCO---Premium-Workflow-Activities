// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MainSourceExpressionNode
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class MainSourceExpressionNode : IQuerySourceExpressionNode, IExpressionNode
  {
    private readonly Type _querySourceElementType;
    private readonly Expression _parsedExpression;
    private readonly Type _querySourceType;
    private readonly string _associatedIdentifier;

    public MainSourceExpressionNode(string associatedIdentifier, Expression expression)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (associatedIdentifier), associatedIdentifier);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckTypeIsAssignableFrom("expression.Type", expression.Type, typeof (IEnumerable));
      this._querySourceType = expression.Type;
      if (!ItemTypeReflectionUtility.TryGetItemTypeOfClosedGenericIEnumerable(expression.Type, out this._querySourceElementType))
        this._querySourceElementType = typeof (object);
      this._associatedIdentifier = associatedIdentifier;
      this._parsedExpression = expression;
    }

    public Type QuerySourceElementType => this._querySourceElementType;

    public Type QuerySourceType => this._querySourceType;

    public Expression ParsedExpression => this._parsedExpression;

    public string AssociatedIdentifier => this._associatedIdentifier;

    public IExpressionNode Source => (IExpressionNode) null;

    public Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    public QueryModel Apply(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      if (queryModel != null)
        throw new ArgumentException("QueryModel has to be null because MainSourceExpressionNode marks the start of a query.", nameof (queryModel));
      MainFromClause mainFromClause = this.CreateMainFromClause(clauseGenerationContext);
      SelectClause selectClause = new SelectClause((Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return new QueryModel(mainFromClause, selectClause)
      {
        ResultTypeOverride = this._querySourceType
      };
    }

    private MainFromClause CreateMainFromClause(
      ClauseGenerationContext clauseGenerationContext)
    {
      MainFromClause mainFromClause = new MainFromClause(this._associatedIdentifier, this._querySourceElementType, this._parsedExpression);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) mainFromClause);
      return mainFromClause;
    }
  }
}
