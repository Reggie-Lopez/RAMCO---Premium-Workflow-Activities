// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.CompoundNodeTypeProvider
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  [ComVisible(true)]
  public sealed class CompoundNodeTypeProvider : INodeTypeProvider
  {
    private readonly List<INodeTypeProvider> _innerProviders;

    public CompoundNodeTypeProvider(IEnumerable<INodeTypeProvider> innerProviders)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<INodeTypeProvider>>(nameof (innerProviders), innerProviders);
      this._innerProviders = new List<INodeTypeProvider>(innerProviders);
    }

    public IList<INodeTypeProvider> InnerProviders => (IList<INodeTypeProvider>) this._innerProviders;

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this._innerProviders.Any<INodeTypeProvider>((Func<INodeTypeProvider, bool>) (p => p.IsRegistered(method)));
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this.InnerProviders.Select<INodeTypeProvider, Type>((Func<INodeTypeProvider, Type>) (p => p.GetNodeType(method))).FirstOrDefault<Type>((Func<Type, bool>) (t => t != (Type) null));
    }
  }
}
