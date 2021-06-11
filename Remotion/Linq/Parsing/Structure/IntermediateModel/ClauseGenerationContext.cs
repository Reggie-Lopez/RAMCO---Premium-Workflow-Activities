// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ClauseGenerationContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public struct ClauseGenerationContext
  {
    private readonly Dictionary<IExpressionNode, object> _lookup;
    private readonly INodeTypeProvider _nodeTypeProvider;

    public ClauseGenerationContext(INodeTypeProvider nodeTypeProvider)
      : this()
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._lookup = new Dictionary<IExpressionNode, object>();
      this._nodeTypeProvider = nodeTypeProvider;
    }

    public INodeTypeProvider NodeTypeProvider => this._nodeTypeProvider;

    public int Count => this._lookup.Count;

    public void AddContextInfo(IExpressionNode node, object contextInfo)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (node), node);
      ArgumentUtility.CheckNotNull<object>(nameof (contextInfo), contextInfo);
      try
      {
        this._lookup.Add(node, contextInfo);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException("Node already has associated context info.");
      }
    }

    public object GetContextInfo(IExpressionNode node)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (node), node);
      object obj;
      if (!this._lookup.TryGetValue(node, out obj))
        throw new KeyNotFoundException("Node has no associated context info.");
      return obj;
    }
  }
}
