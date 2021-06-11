// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.ItemTypeReflectionUtility
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Utilities
{
  [ComVisible(true)]
  public static class ItemTypeReflectionUtility
  {
    [ContractAnnotation("=>true, itemType:notnull; =>false, itemType:null")]
    public static bool TryGetItemTypeOfClosedGenericIEnumerable(
      [NotNull] Type possibleEnumerableType,
      out Type itemType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (possibleEnumerableType), possibleEnumerableType);
      TypeInfo typeInfo1 = possibleEnumerableType.GetTypeInfo();
      if (typeInfo1.IsArray)
      {
        itemType = typeInfo1.GetElementType();
        return true;
      }
      if (!ItemTypeReflectionUtility.IsIEnumerable(typeInfo1))
      {
        itemType = (Type) null;
        return false;
      }
      if (typeInfo1.IsGenericTypeDefinition)
      {
        itemType = (Type) null;
        return false;
      }
      if (typeInfo1.IsGenericType && possibleEnumerableType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
      {
        itemType = typeInfo1.GenericTypeArguments[0];
        return true;
      }
      TypeInfo typeInfo2 = typeInfo1.ImplementedInterfaces.Select<Type, TypeInfo>((Func<Type, TypeInfo>) (t => t.GetTypeInfo())).FirstOrDefault<TypeInfo>(new Func<TypeInfo, bool>(ItemTypeReflectionUtility.IsGenericIEnumerable));
      if ((Type) typeInfo2 == (Type) null)
      {
        itemType = (Type) null;
        return false;
      }
      itemType = typeInfo2.GenericTypeArguments[0];
      return true;
    }

    private static bool IsIEnumerable(TypeInfo type) => typeof (IEnumerable).GetTypeInfo().IsAssignableFrom(type);

    private static bool IsGenericIEnumerable(TypeInfo enumerableType) => ItemTypeReflectionUtility.IsIEnumerable(enumerableType) && enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition() == typeof (IEnumerable<>);
  }
}
