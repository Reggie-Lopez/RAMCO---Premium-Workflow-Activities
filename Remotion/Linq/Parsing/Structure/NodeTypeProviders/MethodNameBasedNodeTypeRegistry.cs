// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.MethodNameBasedNodeTypeRegistry
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Collections;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  [ComVisible(true)]
  public sealed class MethodNameBasedNodeTypeRegistry : INodeTypeProvider
  {
    private readonly IDictionary<string, ICollection<KeyValuePair<NameBasedRegistrationInfo, Type>>> _registeredNamedTypes = (IDictionary<string, ICollection<KeyValuePair<NameBasedRegistrationInfo, Type>>>) new Dictionary<string, ICollection<KeyValuePair<NameBasedRegistrationInfo, Type>>>();

    public static MethodNameBasedNodeTypeRegistry CreateFromRelinqAssembly()
    {
      MethodNameBasedNodeTypeRegistry nodeTypeRegistry = new MethodNameBasedNodeTypeRegistry();
      nodeTypeRegistry.Register(ContainsExpressionNode.GetSupportedMethodNames(), typeof (ContainsExpressionNode));
      return nodeTypeRegistry;
    }

    public int RegisteredNamesCount => this._registeredNamedTypes.CountValues<string, KeyValuePair<NameBasedRegistrationInfo, Type>>();

    public void Register(
      IEnumerable<NameBasedRegistrationInfo> registrationInfo,
      Type nodeType)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<NameBasedRegistrationInfo>>(nameof (registrationInfo), registrationInfo);
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      foreach (NameBasedRegistrationInfo key in registrationInfo)
        this._registeredNamedTypes.Add<string, KeyValuePair<NameBasedRegistrationInfo, Type>>(key.Name, new KeyValuePair<NameBasedRegistrationInfo, Type>(key, nodeType));
    }

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this.GetNodeType(method) != (Type) null;
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      ICollection<KeyValuePair<NameBasedRegistrationInfo, Type>> source;
      return !this._registeredNamedTypes.TryGetValue(method.Name, out source) ? (Type) null : source.Where<KeyValuePair<NameBasedRegistrationInfo, Type>>((Func<KeyValuePair<NameBasedRegistrationInfo, Type>, bool>) (info => info.Key.Filter(method))).Select<KeyValuePair<NameBasedRegistrationInfo, Type>, Type>((Func<KeyValuePair<NameBasedRegistrationInfo, Type>, Type>) (info => info.Value)).FirstOrDefault<Type>();
    }
  }
}
