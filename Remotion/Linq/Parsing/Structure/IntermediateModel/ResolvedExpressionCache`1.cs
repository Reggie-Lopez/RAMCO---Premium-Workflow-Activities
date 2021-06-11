// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ResolvedExpressionCache`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public sealed class ResolvedExpressionCache<T> where T : Expression
  {
    private readonly ExpressionResolver _resolver;
    private T _cachedExpression;

    public ResolvedExpressionCache(IExpressionNode currentNode)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (currentNode), currentNode);
      this._resolver = new ExpressionResolver(currentNode);
      this._cachedExpression = default (T);
    }

    public T GetOrCreate(Func<ExpressionResolver, T> generator)
    {
      if ((object) this._cachedExpression == null)
        this._cachedExpression = generator(this._resolver);
      return this._cachedExpression;
    }
  }
}
