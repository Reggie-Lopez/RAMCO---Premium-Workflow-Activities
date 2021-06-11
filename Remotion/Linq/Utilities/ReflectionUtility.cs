// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.ReflectionUtility
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Remotion.Linq.Utilities
{
  internal static class ReflectionUtility
  {
    private static readonly ReadOnlyCollection<MethodInfo> s_enumerableAndQueryableMethods = new ReadOnlyCollection<MethodInfo>((IList<MethodInfo>) typeof (Enumerable).GetRuntimeMethods().Concat<MethodInfo>(typeof (Queryable).GetRuntimeMethods()).ToList<MethodInfo>());

    public static ReadOnlyCollection<MethodInfo> EnumerableAndQueryableMethods => ReflectionUtility.s_enumerableAndQueryableMethods;

    public static MethodInfo GetMethod<T>(Expression<Func<T>> wrappedCall)
    {
      ArgumentUtility.CheckNotNull<Expression<Func<T>>>(nameof (wrappedCall), wrappedCall);
      switch (wrappedCall.Body.NodeType)
      {
        case ExpressionType.Call:
          return ((MethodCallExpression) wrappedCall.Body).Method;
        case ExpressionType.MemberAccess:
          PropertyInfo member = ((MemberExpression) wrappedCall.Body).Member as PropertyInfo;
          MethodInfo methodInfo = member != (PropertyInfo) null ? member.GetGetMethod(true) : (MethodInfo) null;
          if (methodInfo != (MethodInfo) null)
            return methodInfo;
          break;
      }
      throw new ArgumentException(string.Format("Cannot extract a method from the given expression '{0}'.", (object) wrappedCall.Body), nameof (wrappedCall));
    }

    public static MethodInfo GetRuntimeMethodChecked(
      this Type type,
      string methodName,
      Type[] parameterTypes)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (type), type);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (methodName), methodName);
      ArgumentUtility.CheckNotNull<Type[]>(nameof (parameterTypes), parameterTypes);
      MethodInfo runtimeMethod = type.GetRuntimeMethod(methodName, parameterTypes);
      Assertion.IsNotNull<MethodInfo>(runtimeMethod, "Method '{0} ({1})' was not found on type '{2}'", (object) methodName, (object) StringUtility.Join<string>(", ", ((IEnumerable<Type>) parameterTypes).Select<Type, string>((Func<Type, string>) (t => t.Name))), (object) type.FullName);
      return runtimeMethod;
    }

    public static Type GetMemberReturnType(MemberInfo member)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (member), member);
      PropertyInfo propertyInfo = member as PropertyInfo;
      if (propertyInfo != (PropertyInfo) null)
        return propertyInfo.PropertyType;
      FieldInfo fieldInfo = member as FieldInfo;
      if (fieldInfo != (FieldInfo) null)
        return fieldInfo.FieldType;
      MethodInfo methodInfo = member as MethodInfo;
      return methodInfo != (MethodInfo) null ? methodInfo.ReturnType : throw new ArgumentException("Argument must be FieldInfo, PropertyInfo, or MethodInfo.", nameof (member));
    }

    public static void CheckTypeIsClosedGenericIEnumerable(Type enumerableType, string argumentName)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (enumerableType), enumerableType);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (argumentName), argumentName);
      if (!ItemTypeReflectionUtility.TryGetItemTypeOfClosedGenericIEnumerable(enumerableType, out Type _))
        throw new ArgumentException(string.Format("Expected a closed generic type implementing IEnumerable<T>, but found '{0}'.", (object) enumerableType), argumentName);
    }

    public static Type GetItemTypeOfClosedGenericIEnumerable(
      Type enumerableType,
      string argumentName)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (enumerableType), enumerableType);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (argumentName), argumentName);
      Type itemType;
      if (!ItemTypeReflectionUtility.TryGetItemTypeOfClosedGenericIEnumerable(enumerableType, out itemType))
        throw new ArgumentException(string.Format("Expected a closed generic type implementing IEnumerable<T>, but found '{0}'.", (object) enumerableType), argumentName);
      return itemType;
    }
  }
}
