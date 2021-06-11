// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionParseInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public struct MethodCallExpressionParseInfo
  {
    private readonly string _associatedIdentifier;
    private readonly IExpressionNode _source;
    private readonly MethodCallExpression _parsedExpression;

    public MethodCallExpressionParseInfo(
      string associatedIdentifier,
      IExpressionNode source,
      MethodCallExpression parsedExpression)
      : this()
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (associatedIdentifier), associatedIdentifier);
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (source), source);
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (parsedExpression), parsedExpression);
      this._associatedIdentifier = associatedIdentifier;
      this._source = source;
      this._parsedExpression = parsedExpression;
    }

    public string AssociatedIdentifier => this._associatedIdentifier;

    public IExpressionNode Source => this._source;

    public MethodCallExpression ParsedExpression => this._parsedExpression;
  }
}
