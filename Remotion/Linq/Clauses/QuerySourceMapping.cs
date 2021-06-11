// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.QuerySourceMapping
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class QuerySourceMapping
  {
    private readonly Dictionary<IQuerySource, Expression> _lookup = new Dictionary<IQuerySource, Expression>();

    public bool ContainsMapping(IQuerySource querySource)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      return this._lookup.ContainsKey(querySource);
    }

    public void AddMapping(IQuerySource querySource, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      try
      {
        this._lookup.Add(querySource, expression);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException(string.Format("Query source ({0}) has already been associated with an expression.", (object) querySource));
      }
    }

    public void ReplaceMapping(IQuerySource querySource, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (!this.ContainsMapping(querySource))
        throw new InvalidOperationException(string.Format("Query source ({0}) has not been associated with an expression, cannot replace its mapping.", (object) querySource));
      this._lookup[querySource] = expression;
    }

    public Expression GetExpression(IQuerySource querySource)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      Expression expression;
      if (!this._lookup.TryGetValue(querySource, out expression))
        throw new KeyNotFoundException(string.Format("Query source ({0}) has not been associated with an expression.", (object) querySource));
      return expression;
    }
  }
}
