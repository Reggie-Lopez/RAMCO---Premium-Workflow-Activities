// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SupportedMethodSpecifications
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  internal static class SupportedMethodSpecifications
  {
    public static IEnumerable<MethodInfo> WhereNameMatches(
      this IEnumerable<MethodInfo> input,
      string name)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (name), name);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.Name == name));
    }

    public static IEnumerable<MethodInfo> WithoutEqualityComparer(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => !SupportedMethodSpecifications.HasGenericDelegateOfType(mi, typeof (IEqualityComparer<>))));
    }

    public static IEnumerable<MethodInfo> WithoutComparer(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => !SupportedMethodSpecifications.HasGenericDelegateOfType(mi, typeof (IComparer<>))));
    }

    public static IEnumerable<MethodInfo> WithoutSeedParameter(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.GetParameters().Length == 2));
    }

    public static IEnumerable<MethodInfo> WithSeedParameter(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.GetParameters().Length == 3 || mi.GetParameters().Length == 4));
    }

    public static IEnumerable<MethodInfo> WithoutResultSelector(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => ((IEnumerable<ParameterInfo>) mi.GetParameters()).All<ParameterInfo>((Func<ParameterInfo, bool>) (p => p.Name != "resultSelector"))));
    }

    public static IEnumerable<MethodInfo> WithResultSelector(
      this IEnumerable<MethodInfo> input)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => ((IEnumerable<ParameterInfo>) mi.GetParameters()).Any<ParameterInfo>((Func<ParameterInfo, bool>) (p => p.Name == "resultSelector"))));
    }

    public static IEnumerable<MethodInfo> WithoutIndexSelector(
      this IEnumerable<MethodInfo> input,
      int parameterPosition)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (input), input);
      return input.Where<MethodInfo>((Func<MethodInfo, bool>) (mi => !SupportedMethodSpecifications.HasIndexSelectorParameter(mi, parameterPosition)));
    }

    private static bool HasGenericDelegateOfType(MethodInfo methodInfo, Type genericDelegateType) => ((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).Select<ParameterInfo, TypeInfo>((Func<ParameterInfo, TypeInfo>) (p => p.ParameterType.GetTypeInfo())).Any<TypeInfo>((Func<TypeInfo, bool>) (p => p.IsGenericType && genericDelegateType.GetTypeInfo().IsAssignableFrom(p.GetGenericTypeDefinition().GetTypeInfo())));

    private static bool HasIndexSelectorParameter(MethodInfo methodInfo, int parameterPosition)
    {
      ParameterInfo[] parameters = methodInfo.GetParameters();
      return parameters.Length > parameterPosition && parameters[parameterPosition].ParameterType.GetTypeInfo().UnwrapEnumerable().GenericTypeArguments[1] == typeof (int);
    }

    private static TypeInfo UnwrapEnumerable(this TypeInfo typeInfo)
    {
      TypeInfo typeInfo1 = typeInfo;
      while (typeInfo1.ContainsGenericParameters)
        typeInfo1 = typeInfo1.BaseType.GetTypeInfo();
      return typeof (Expression).GetTypeInfo().IsAssignableFrom(typeInfo1) ? typeInfo.GenericTypeArguments[0].GetTypeInfo() : typeInfo;
    }
  }
}
