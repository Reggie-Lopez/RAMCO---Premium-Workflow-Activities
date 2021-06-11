// Decompiled with JetBrains decompiler
// Type: Remotion.Utilities.ArgumentUtility
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Remotion.Utilities
{
  internal static class ArgumentUtility
  {
    [AssertionMethod]
    public static T CheckNotNull<T>([InvokerParameterName] string argumentName, [NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T actualValue) => (object) actualValue != null ? actualValue : throw new ArgumentNullException(argumentName);

    [Conditional("DEBUG")]
    [AssertionMethod]
    public static void DebugCheckNotNull<T>([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL), NoEnumeration] T actualValue) => ArgumentUtility.CheckNotNull<T>(argumentName, actualValue);

    [AssertionMethod]
    public static string CheckNotNullOrEmpty([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string actualValue)
    {
      ArgumentUtility.CheckNotNull<string>(argumentName, actualValue);
      return actualValue.Length != 0 ? actualValue : throw ArgumentUtility.CreateArgumentEmptyException(argumentName);
    }

    [Conditional("DEBUG")]
    [AssertionMethod]
    public static void DebugCheckNotNullOrEmpty([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string actualValue) => ArgumentUtility.CheckNotNullOrEmpty(argumentName, actualValue);

    [AssertionMethod]
    public static T CheckNotNullOrEmpty<T>([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T enumerable) where T : IEnumerable
    {
      ArgumentUtility.CheckNotNull<T>(argumentName, enumerable);
      ArgumentUtility.CheckNotEmpty<T>(argumentName, enumerable);
      return enumerable;
    }

    [AssertionMethod]
    [Conditional("DEBUG")]
    public static void DebugCheckNotNullOrEmpty<T>([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T enumerable) where T : IEnumerable => ArgumentUtility.CheckNotNullOrEmpty<T>(argumentName, enumerable);

    [AssertionMethod]
    public static T CheckNotNullOrItemsNull<T>([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T enumerable) where T : IEnumerable
    {
      ArgumentUtility.CheckNotNull<T>(argumentName, enumerable);
      int index = 0;
      IEnumerator enumerator = enumerable.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current == null)
            throw ArgumentUtility.CreateArgumentItemNullException(argumentName, index);
          ++index;
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return enumerable;
    }

    [AssertionMethod]
    public static T CheckNotNullOrEmptyOrItemsNull<T>([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T enumerable) where T : IEnumerable
    {
      ArgumentUtility.CheckNotNullOrItemsNull<T>(argumentName, enumerable);
      ArgumentUtility.CheckNotEmpty<T>(argumentName, enumerable);
      return enumerable;
    }

    [AssertionMethod]
    public static string CheckNotEmpty([InvokerParameterName] string argumentName, string actualValue)
    {
      switch (actualValue)
      {
        case "":
          throw ArgumentUtility.CreateArgumentEmptyException(argumentName);
        default:
          return actualValue;
      }
    }

    [AssertionMethod]
    public static T CheckNotEmpty<T>([InvokerParameterName] string argumentName, T enumerable) where T : IEnumerable
    {
      if ((object) enumerable != null)
      {
        if (enumerable is ICollection collection)
        {
          if (collection.Count == 0)
            throw ArgumentUtility.CreateArgumentEmptyException(argumentName);
          return enumerable;
        }
        IEnumerator enumerator = enumerable.GetEnumerator();
        using (enumerator as IDisposable)
        {
          if (!enumerator.MoveNext())
            throw ArgumentUtility.CreateArgumentEmptyException(argumentName);
        }
      }
      return enumerable;
    }

    [AssertionMethod]
    public static Guid CheckNotEmpty([InvokerParameterName] string argumentName, Guid actualValue) => !(actualValue == Guid.Empty) ? actualValue : throw ArgumentUtility.CreateArgumentEmptyException(argumentName);

    public static object CheckNotNullAndType(
      [InvokerParameterName] string argumentName,
      [NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object actualValue,
      Type expectedType)
    {
      if (actualValue == null)
        throw new ArgumentNullException(argumentName);
      return expectedType.GetTypeInfo().IsAssignableFrom(actualValue.GetType().GetTypeInfo()) ? actualValue : throw ArgumentUtility.CreateArgumentTypeException(argumentName, actualValue.GetType(), expectedType);
    }

    public static TExpected CheckNotNullAndType<TExpected>([InvokerParameterName] string argumentName, [NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object actualValue)
    {
      if (actualValue == null)
        throw new ArgumentNullException(argumentName);
      return actualValue is TExpected expected ? expected : throw ArgumentUtility.CreateArgumentTypeException(argumentName, actualValue.GetType(), typeof (TExpected));
    }

    [Conditional("DEBUG")]
    [AssertionMethod]
    public static void DebugCheckNotNullAndType(
      [InvokerParameterName] string argumentName,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL), NoEnumeration] object actualValue,
      Type expectedType)
    {
      ArgumentUtility.CheckNotNullAndType(argumentName, actualValue, expectedType);
    }

    public static object CheckType([InvokerParameterName] string argumentName, [NoEnumeration] object actualValue, Type expectedType)
    {
      if (actualValue == null)
      {
        if (NullableTypeUtility.IsNullableType_NoArgumentCheck(expectedType))
          return (object) null;
        throw ArgumentUtility.CreateArgumentTypeException(argumentName, (Type) null, expectedType);
      }
      return expectedType.GetTypeInfo().IsAssignableFrom(actualValue.GetType().GetTypeInfo()) ? actualValue : throw ArgumentUtility.CreateArgumentTypeException(argumentName, actualValue.GetType(), expectedType);
    }

    public static TExpected CheckType<TExpected>([InvokerParameterName] string argumentName, [NoEnumeration] object actualValue)
    {
      if (actualValue == null)
      {
        try
        {
          return (TExpected) actualValue;
        }
        catch (NullReferenceException ex)
        {
          throw new ArgumentNullException(argumentName);
        }
      }
      else
        return actualValue is TExpected expected ? expected : throw ArgumentUtility.CreateArgumentTypeException(argumentName, actualValue.GetType(), typeof (TExpected));
    }

    public static Type CheckNotNullAndTypeIsAssignableFrom(
      [InvokerParameterName] string argumentName,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type actualType,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type expectedType)
    {
      if (actualType == (Type) null)
        throw new ArgumentNullException(argumentName);
      return ArgumentUtility.CheckTypeIsAssignableFrom(argumentName, actualType, expectedType);
    }

    public static Type CheckTypeIsAssignableFrom(
      [InvokerParameterName] string argumentName,
      Type actualType,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type expectedType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (expectedType), expectedType);
      return !(actualType != (Type) null) || expectedType.GetTypeInfo().IsAssignableFrom(actualType.GetTypeInfo()) ? actualType : throw new ArgumentException(string.Format("Parameter '{0}' is a '{2}', which cannot be assigned to type '{1}'.", (object) argumentName, (object) expectedType, (object) actualType), argumentName);
    }

    [AssertionMethod]
    [Conditional("DEBUG")]
    public static void DebugCheckTypeIsAssignableFrom(
      [InvokerParameterName] string argumentName,
      Type actualType,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type expectedType)
    {
      ArgumentUtility.CheckTypeIsAssignableFrom(argumentName, actualType, expectedType);
    }

    public static T CheckItemsType<T>([InvokerParameterName] string argumentName, T collection, Type itemType) where T : ICollection
    {
      if ((object) collection != null)
      {
        int index = 0;
        IEnumerator enumerator = collection.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            if (current != null && !itemType.GetTypeInfo().IsAssignableFrom(current.GetType().GetTypeInfo()))
              throw ArgumentUtility.CreateArgumentItemTypeException(argumentName, index, itemType, current.GetType());
            ++index;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      return collection;
    }

    public static T CheckItemsNotNullAndType<T>([InvokerParameterName] string argumentName, T collection, Type itemType) where T : ICollection
    {
      if ((object) collection != null)
      {
        int index = 0;
        IEnumerator enumerator = collection.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            if (current == null)
              throw ArgumentUtility.CreateArgumentItemNullException(argumentName, index);
            if (!itemType.GetTypeInfo().IsAssignableFrom(current.GetType().GetTypeInfo()))
              throw ArgumentUtility.CreateArgumentItemTypeException(argumentName, index, itemType, current.GetType());
            ++index;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      return collection;
    }

    public static ArgumentException CreateArgumentEmptyException(
      [InvokerParameterName] string argumentName)
    {
      return new ArgumentException(string.Format("Parameter '{0}' cannot be empty.", (object) argumentName), argumentName);
    }

    public static ArgumentException CreateArgumentTypeException(
      [InvokerParameterName] string argumentName,
      Type actualType,
      Type expectedType)
    {
      string str = actualType != (Type) null ? actualType.ToString() : "<null>";
      return expectedType == (Type) null ? new ArgumentException(string.Format("Parameter '{0}' has unexpected type '{1}'.", (object) argumentName, (object) str), argumentName) : new ArgumentException(string.Format("Parameter '{0}' has type '{2}' when type '{1}' was expected.", (object) argumentName, (object) expectedType, (object) str), argumentName);
    }

    public static ArgumentException CreateArgumentItemTypeException(
      [InvokerParameterName] string argumentName,
      int index,
      Type expectedType,
      Type actualType)
    {
      return new ArgumentException(string.Format("Item {0} of parameter '{1}' has the type '{2}' instead of '{3}'.", (object) index, (object) argumentName, (object) actualType, (object) expectedType), argumentName);
    }

    public static ArgumentNullException CreateArgumentItemNullException(
      [InvokerParameterName] string argumentName,
      int index)
    {
      return new ArgumentNullException(argumentName, string.Format("Item {0} of parameter '{1}' is null.", (object) index, (object) argumentName));
    }
  }
}
