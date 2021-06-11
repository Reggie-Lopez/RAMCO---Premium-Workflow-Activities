// Decompiled with JetBrains decompiler
// Type: Remotion.Utilities.NullableTypeUtility
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Reflection;

namespace Remotion.Utilities
{
  internal static class NullableTypeUtility
  {
    public static bool IsNullableType(Type type) => !(type == (Type) null) ? NullableTypeUtility.IsNullableType_NoArgumentCheck(type) : throw new ArgumentNullException(nameof (type));

    internal static bool IsNullableType_NoArgumentCheck(Type expectedType) => !expectedType.GetTypeInfo().IsValueType || Nullable.GetUnderlyingType(expectedType) != (Type) null;

    public static Type GetNullableType(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (NullableTypeUtility.IsNullableType(type))
        return type;
      return typeof (Nullable<>).MakeGenericType(type);
    }

    public static Type GetBasicType(Type type)
    {
      Type type1 = !(type == (Type) null) ? Nullable.GetUnderlyingType(type) : throw new ArgumentNullException(nameof (type));
      return (object) type1 != null ? type1 : type;
    }
  }
}
