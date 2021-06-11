// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.MethodInfoBasedNodeTypeRegistry
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

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
  public sealed class MethodInfoBasedNodeTypeRegistry : INodeTypeProvider
  {
    private static readonly Dictionary<MethodInfo, Lazy<MethodInfo[]>> s_genericMethodDefinitionCandidates = new Dictionary<MethodInfo, Lazy<MethodInfo[]>>();
    private readonly Dictionary<MethodInfo, Type> _registeredMethodInfoTypes = new Dictionary<MethodInfo, Type>();

    public static MethodInfoBasedNodeTypeRegistry CreateFromRelinqAssembly()
    {
      MethodInfoBasedNodeTypeRegistry nodeTypeRegistry = new MethodInfoBasedNodeTypeRegistry();
      nodeTypeRegistry.Register(AggregateExpressionNode.GetSupportedMethods(), typeof (AggregateExpressionNode));
      nodeTypeRegistry.Register(AggregateFromSeedExpressionNode.GetSupportedMethods(), typeof (AggregateFromSeedExpressionNode));
      nodeTypeRegistry.Register(AllExpressionNode.GetSupportedMethods(), typeof (AllExpressionNode));
      nodeTypeRegistry.Register(AnyExpressionNode.GetSupportedMethods(), typeof (AnyExpressionNode));
      nodeTypeRegistry.Register(AverageExpressionNode.GetSupportedMethods(), typeof (AverageExpressionNode));
      nodeTypeRegistry.Register(CastExpressionNode.GetSupportedMethods(), typeof (CastExpressionNode));
      nodeTypeRegistry.Register(ConcatExpressionNode.GetSupportedMethods(), typeof (ConcatExpressionNode));
      nodeTypeRegistry.Register(ContainsExpressionNode.GetSupportedMethods(), typeof (ContainsExpressionNode));
      nodeTypeRegistry.Register(CountExpressionNode.GetSupportedMethods(), typeof (CountExpressionNode));
      nodeTypeRegistry.Register(DefaultIfEmptyExpressionNode.GetSupportedMethods(), typeof (DefaultIfEmptyExpressionNode));
      nodeTypeRegistry.Register(DistinctExpressionNode.GetSupportedMethods(), typeof (DistinctExpressionNode));
      nodeTypeRegistry.Register(ExceptExpressionNode.GetSupportedMethods(), typeof (ExceptExpressionNode));
      nodeTypeRegistry.Register(FirstExpressionNode.GetSupportedMethods(), typeof (FirstExpressionNode));
      nodeTypeRegistry.Register(GroupByExpressionNode.GetSupportedMethods(), typeof (GroupByExpressionNode));
      nodeTypeRegistry.Register(GroupByWithResultSelectorExpressionNode.GetSupportedMethods(), typeof (GroupByWithResultSelectorExpressionNode));
      nodeTypeRegistry.Register(GroupJoinExpressionNode.GetSupportedMethods(), typeof (GroupJoinExpressionNode));
      nodeTypeRegistry.Register(IntersectExpressionNode.GetSupportedMethods(), typeof (IntersectExpressionNode));
      nodeTypeRegistry.Register(JoinExpressionNode.GetSupportedMethods(), typeof (JoinExpressionNode));
      nodeTypeRegistry.Register(LastExpressionNode.GetSupportedMethods(), typeof (LastExpressionNode));
      nodeTypeRegistry.Register(LongCountExpressionNode.GetSupportedMethods(), typeof (LongCountExpressionNode));
      nodeTypeRegistry.Register(MaxExpressionNode.GetSupportedMethods(), typeof (MaxExpressionNode));
      nodeTypeRegistry.Register(MinExpressionNode.GetSupportedMethods(), typeof (MinExpressionNode));
      nodeTypeRegistry.Register(OfTypeExpressionNode.GetSupportedMethods(), typeof (OfTypeExpressionNode));
      nodeTypeRegistry.Register(OrderByDescendingExpressionNode.GetSupportedMethods(), typeof (OrderByDescendingExpressionNode));
      nodeTypeRegistry.Register(OrderByExpressionNode.GetSupportedMethods(), typeof (OrderByExpressionNode));
      nodeTypeRegistry.Register(ReverseExpressionNode.GetSupportedMethods(), typeof (ReverseExpressionNode));
      nodeTypeRegistry.Register(SelectExpressionNode.GetSupportedMethods(), typeof (SelectExpressionNode));
      nodeTypeRegistry.Register(SelectManyExpressionNode.GetSupportedMethods(), typeof (SelectManyExpressionNode));
      nodeTypeRegistry.Register(SingleExpressionNode.GetSupportedMethods(), typeof (SingleExpressionNode));
      nodeTypeRegistry.Register(SkipExpressionNode.GetSupportedMethods(), typeof (SkipExpressionNode));
      nodeTypeRegistry.Register(SumExpressionNode.GetSupportedMethods(), typeof (SumExpressionNode));
      nodeTypeRegistry.Register(TakeExpressionNode.GetSupportedMethods(), typeof (TakeExpressionNode));
      nodeTypeRegistry.Register(ThenByDescendingExpressionNode.GetSupportedMethods(), typeof (ThenByDescendingExpressionNode));
      nodeTypeRegistry.Register(ThenByExpressionNode.GetSupportedMethods(), typeof (ThenByExpressionNode));
      nodeTypeRegistry.Register(UnionExpressionNode.GetSupportedMethods(), typeof (UnionExpressionNode));
      nodeTypeRegistry.Register(WhereExpressionNode.GetSupportedMethods(), typeof (WhereExpressionNode));
      return nodeTypeRegistry;
    }

    public static MethodInfo GetRegisterableMethodDefinition(
      MethodInfo method,
      bool throwOnAmbiguousMatch)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      MethodInfo genericMethodDefinition = method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
      if (!genericMethodDefinition.DeclaringType.GetTypeInfo().IsGenericType)
        return genericMethodDefinition;
      Lazy<MethodInfo[]> lazy;
      lock (MethodInfoBasedNodeTypeRegistry.s_genericMethodDefinitionCandidates)
      {
        if (!MethodInfoBasedNodeTypeRegistry.s_genericMethodDefinitionCandidates.TryGetValue(method, out lazy))
        {
          lazy = new Lazy<MethodInfo[]>((Func<MethodInfo[]>) (() => MethodInfoBasedNodeTypeRegistry.GetGenericMethodDefinitionCandidates(genericMethodDefinition)));
          MethodInfoBasedNodeTypeRegistry.s_genericMethodDefinitionCandidates.Add(method, lazy);
        }
      }
      if (lazy.Value.Length == 1)
        return ((IEnumerable<MethodInfo>) lazy.Value).Single<MethodInfo>();
      if (!throwOnAmbiguousMatch)
        return (MethodInfo) null;
      throw new NotSupportedException(string.Format("A generic method definition cannot be resolved for method '{0}' on type '{1}' because a distinct match is not possible. The method can still be registered using the following syntax:\r\n\r\npublic static readonly NameBasedRegistrationInfo[] SupportedMethodNames = \r\n    new[] {{\r\n        new NameBasedRegistrationInfo (\r\n            \"{2}\", \r\n            mi => /* match rule based on MethodInfo */\r\n        )\r\n    }};", (object) method, (object) genericMethodDefinition.DeclaringType.GetGenericTypeDefinition(), (object) method.Name));
    }

    private static MethodInfo[] GetGenericMethodDefinitionCandidates(
      MethodInfo referenceMethodDefinition)
    {
      Type genericTypeDefinition = referenceMethodDefinition.DeclaringType.GetGenericTypeDefinition();
      \u003C\u003Ef__AnonymousType0<string, Type>[] referenceMethodSignature = new \u003C\u003Ef__AnonymousType0<string, Type>[1]
      {
        new
        {
          Name = "returnValue",
          Type = referenceMethodDefinition.ReturnType
        }
      }.Concat(((IEnumerable<ParameterInfo>) referenceMethodDefinition.GetParameters()).Select(p => new
      {
        Name = p.Name,
        Type = p.ParameterType
      })).ToArray();
      \u003C\u003Ef__AnonymousType1<MethodInfo, string[], Type[]>[] array = genericTypeDefinition.GetRuntimeMethods().Select(m => new
      {
        Method = m,
        SignatureNames = ((IEnumerable<string>) new string[1]
        {
          "returnValue"
        }).Concat<string>(((IEnumerable<ParameterInfo>) m.GetParameters()).Select<ParameterInfo, string>((Func<ParameterInfo, string>) (p => p.Name))).ToArray<string>(),
        SignatureTypes = ((IEnumerable<Type>) new Type[1]
        {
          m.ReturnType
        }).Concat<Type>(((IEnumerable<ParameterInfo>) m.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType))).ToArray<Type>()
      }).Where(c => c.Method.Name == referenceMethodDefinition.Name && c.SignatureTypes.Length == referenceMethodSignature.Length).ToArray();
      for (int i = 0; i < referenceMethodSignature.Length; ++i)
        array = array.Where(c => c.SignatureNames[i] == referenceMethodSignature[i].Name).Where(c => c.SignatureTypes[i] == referenceMethodSignature[i].Type || c.SignatureTypes[i].GetTypeInfo().ContainsGenericParameters).ToArray();
      return array.Select(c => c.Method).ToArray<MethodInfo>();
    }

    public int RegisteredMethodInfoCount => this._registeredMethodInfoTypes.Count;

    public void Register(IEnumerable<MethodInfo> methods, Type nodeType)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (methods), methods);
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (nodeType), nodeType, typeof (IExpressionNode));
      foreach (MethodInfo method in methods)
      {
        if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
          throw new InvalidOperationException(string.Format("Cannot register closed generic method '{0}', try to register its generic method definition instead.", (object) method.Name));
        if (method.DeclaringType.GetTypeInfo().IsGenericType && !method.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
          throw new InvalidOperationException(string.Format("Cannot register method '{0}' in closed generic type '{1}', try to register its equivalent in the generic type definition instead.", (object) method.Name, (object) method.DeclaringType));
        this._registeredMethodInfoTypes[method] = nodeType;
      }
    }

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this.GetNodeType(method) != (Type) null;
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      MethodInfo methodDefinition = MethodInfoBasedNodeTypeRegistry.GetRegisterableMethodDefinition(method, false);
      if (methodDefinition == (MethodInfo) null)
        return (Type) null;
      Type type;
      this._registeredMethodInfoTypes.TryGetValue(methodDefinition, out type);
      return type;
    }
  }
}
