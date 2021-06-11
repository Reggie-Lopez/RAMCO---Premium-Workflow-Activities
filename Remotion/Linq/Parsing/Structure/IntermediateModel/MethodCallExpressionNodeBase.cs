// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionNodeBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public abstract class MethodCallExpressionNodeBase : IExpressionNode
  {
    private IExpressionNode _source;
    private readonly Type _nodeResultType;
    private readonly string _associatedIdentifier;

    protected MethodCallExpressionNodeBase(MethodCallExpressionParseInfo parseInfo)
    {
      this._associatedIdentifier = parseInfo.AssociatedIdentifier != null ? parseInfo.AssociatedIdentifier : throw new ArgumentException("Unitialized struct.", nameof (parseInfo));
      this._source = parseInfo.Source;
      this._nodeResultType = parseInfo.ParsedExpression.Type;
    }

    public string AssociatedIdentifier => this._associatedIdentifier;

    public IExpressionNode Source => this._source;

    public Type NodeResultType => this._nodeResultType;

    public abstract Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext);

    protected abstract void ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext);

    public QueryModel Apply(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel = this.WrapQueryModelAfterEndOfQuery(queryModel, clauseGenerationContext);
      this.ApplyNodeSpecificSemantics(queryModel, clauseGenerationContext);
      this.SetResultTypeOverride(queryModel);
      return queryModel;
    }

    protected virtual QueryModel WrapQueryModelAfterEndOfQuery(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      return this._source is ResultOperatorExpressionNodeBase source ? this.WrapQueryModel(queryModel, source.AssociatedIdentifier, clauseGenerationContext) : queryModel;
    }

    protected virtual void SetResultTypeOverride(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.ResultTypeOverride = this.NodeResultType;
    }

    private QueryModel WrapQueryModel(
      QueryModel queryModel,
      string associatedIdentifier,
      ClauseGenerationContext clauseGenerationContext)
    {
      SubQueryExpression subQueryExpression = new SubQueryExpression(queryModel);
      MainSourceExpressionNode sourceExpressionNode = new MainSourceExpressionNode(associatedIdentifier, (Expression) subQueryExpression);
      this._source = (IExpressionNode) sourceExpressionNode;
      return sourceExpressionNode.Apply((QueryModel) null, clauseGenerationContext);
    }

    protected NotSupportedException CreateResolveNotSupportedException() => new NotSupportedException(this.GetType().Name + " does not support resolving of expressions, because it does not stream any data to the following node.");

    protected NotSupportedException CreateOutputParameterNotSupportedException() => new NotSupportedException(this.GetType().Name + " does not support creating a parameter for its output because it does not stream any data to the following node.");
  }
}
