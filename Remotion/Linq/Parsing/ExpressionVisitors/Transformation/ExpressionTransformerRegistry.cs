// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.Transformation.ExpressionTransformerRegistry
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Collections;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.Transformation
{
  [ComVisible(true)]
  public class ExpressionTransformerRegistry : IExpressionTranformationProvider
  {
    private readonly IDictionary<ExpressionType, ICollection<ExpressionTransformation>> _transformations = (IDictionary<ExpressionType, ICollection<ExpressionTransformation>>) new Dictionary<ExpressionType, ICollection<ExpressionTransformation>>();
    private readonly List<ExpressionTransformation> _genericTransformations = new List<ExpressionTransformation>();

    public static ExpressionTransformerRegistry CreateDefault()
    {
      ExpressionTransformerRegistry transformerRegistry = new ExpressionTransformerRegistry();
      transformerRegistry.Register<BinaryExpression>((IExpressionTransformer<BinaryExpression>) new VBCompareStringExpressionTransformer());
      transformerRegistry.Register<MethodCallExpression>((IExpressionTransformer<MethodCallExpression>) new VBInformationIsNothingExpressionTransformer());
      transformerRegistry.Register<InvocationExpression>((IExpressionTransformer<InvocationExpression>) new InvocationOfLambdaExpressionTransformer());
      transformerRegistry.Register<MemberExpression>((IExpressionTransformer<MemberExpression>) new NullableValueTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new KeyValuePairNewExpressionTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new DictionaryEntryNewExpressionTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new TupleNewExpressionTransformer());
      transformerRegistry.Register<Expression>((IExpressionTransformer<Expression>) new AttributeEvaluatingExpressionTransformer());
      return transformerRegistry;
    }

    public int RegisteredTransformerCount => this._transformations.CountValues<ExpressionType, ExpressionTransformation>();

    public ExpressionTransformation[] GetAllTransformations(
      ExpressionType expressionType)
    {
      ICollection<ExpressionTransformation> source;
      return this._transformations.TryGetValue(expressionType, out source) ? source.ToArray<ExpressionTransformation>() : new ExpressionTransformation[0];
    }

    public IEnumerable<ExpressionTransformation> GetTransformations(
      Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ICollection<ExpressionTransformation> first;
      this._transformations.TryGetValue(expression.NodeType, out first);
      return first != null ? first.Concat<ExpressionTransformation>((IEnumerable<ExpressionTransformation>) this._genericTransformations) : (IEnumerable<ExpressionTransformation>) this._genericTransformations;
    }

    public void Register<T>(IExpressionTransformer<T> transformer) where T : Expression
    {
      ArgumentUtility.CheckNotNull<IExpressionTransformer<T>>(nameof (transformer), transformer);
      ExpressionTransformation expressionTransformation = (ExpressionTransformation) (expr => ExpressionTransformerRegistry.TransformExpression<T>(expr, transformer));
      if (transformer.SupportedExpressionTypes == null)
      {
        if (typeof (T) != typeof (Expression))
          throw new ArgumentException(string.Format("Cannot register an IExpressionTransformer<{0}> as a generic transformer. Generic transformers must implement IExpressionTransformer<Expression>.", (object) typeof (T).Name), nameof (transformer));
        this._genericTransformations.Add(expressionTransformation);
      }
      else
      {
        foreach (ExpressionType supportedExpressionType in transformer.SupportedExpressionTypes)
          this._transformations.Add<ExpressionType, ExpressionTransformation>(supportedExpressionType, expressionTransformation);
      }
    }

    private static Expression TransformExpression<T>(
      Expression expression,
      IExpressionTransformer<T> transformer)
      where T : Expression
    {
      T expression1;
      try
      {
        expression1 = (T) expression;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("A '{0}' with node type '{1}' cannot be handled by the IExpressionTransformer<{2}>. The transformer was probably registered for a wrong ExpressionType.", (object) expression.GetType().Name, (object) expression.NodeType, (object) typeof (T).Name), (Exception) ex);
      }
      return transformer.Transform(expression1);
    }
  }
}
