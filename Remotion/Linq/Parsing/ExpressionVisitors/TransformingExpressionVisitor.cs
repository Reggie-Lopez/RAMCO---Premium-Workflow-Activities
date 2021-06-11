// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.TransformingExpressionVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors
{
  [ComVisible(true)]
  public sealed class TransformingExpressionVisitor : RelinqExpressionVisitor
  {
    private readonly IExpressionTranformationProvider _tranformationProvider;

    public static Expression Transform(
      Expression expression,
      IExpressionTranformationProvider tranformationProvider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (tranformationProvider), tranformationProvider);
      return new TransformingExpressionVisitor(tranformationProvider).Visit(expression);
    }

    private TransformingExpressionVisitor(
      IExpressionTranformationProvider tranformationProvider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (tranformationProvider), tranformationProvider);
      this._tranformationProvider = tranformationProvider;
    }

    public override Expression Visit(Expression expression)
    {
      Expression expression1 = base.Visit(expression);
      if (expression1 == null)
        return (Expression) null;
      foreach (ExpressionTransformation transformation in this._tranformationProvider.GetTransformations(expression1))
      {
        Expression node = transformation(expression1);
        Assertion.IsNotNull<Expression>(node);
        if (node != expression1)
          return this.Visit(node);
      }
      return expression1;
    }
  }
}
