// Decompiled with JetBrains decompiler
// Type: Remotion.Utilities.Assertion
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace Remotion.Utilities
{
  internal static class Assertion
  {
    private const string c_msgIsTrue = "Assertion failed: Expression evaluates to true.";
    private const string c_msgIsFalse = "Assertion failed: Expression evaluates to false.";
    private const string c_msgIsNull = "Assertion failed: Expression evaluates to a null reference.";
    private const string c_msgIsNotNull = "Assertion failed: Expression does not evaluate to a null reference.";
    private static readonly object[] s_emptyArguments = new object[0];

    [Conditional("DEBUG")]
    [AssertionMethod]
    public static void DebugAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message) => Assertion.IsTrue(assertion, message);

    [Conditional("DEBUG")]
    [StringFormatMethod("message")]
    [AssertionMethod]
    public static void DebugAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message, params object[] arguments) => Assertion.IsTrue(assertion, message, arguments);

    [Conditional("DEBUG")]
    [AssertionMethod]
    public static void DebugAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion) => Assertion.IsTrue(assertion);

    [Conditional("TRACE")]
    [AssertionMethod]
    public static void TraceAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message) => Assertion.IsTrue(assertion, message);

    [AssertionMethod]
    [StringFormatMethod("message")]
    [Conditional("TRACE")]
    public static void TraceAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message, params object[] arguments) => Assertion.IsTrue(assertion, message, arguments);

    [Conditional("TRACE")]
    [AssertionMethod]
    public static void TraceAssert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion) => Assertion.IsTrue(assertion);

    [AssertionMethod]
    public static void IsTrue([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message) => Assertion.IsTrue(assertion, message, Assertion.s_emptyArguments);

    [StringFormatMethod("message")]
    [AssertionMethod]
    public static void IsTrue([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message, params object[] arguments)
    {
      if (!assertion)
        throw new InvalidOperationException(string.Format(message, arguments));
    }

    [AssertionMethod]
    public static void IsTrue([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion) => Assertion.IsTrue(assertion, "Assertion failed: Expression evaluates to false.");

    [AssertionMethod]
    public static void IsFalse([AssertionCondition(AssertionConditionType.IS_FALSE)] bool expression, string message) => Assertion.IsFalse(expression, message, Assertion.s_emptyArguments);

    [AssertionMethod]
    public static void IsFalse([AssertionCondition(AssertionConditionType.IS_FALSE)] bool expression) => Assertion.IsFalse(expression, "Assertion failed: Expression evaluates to true.");

    [StringFormatMethod("message")]
    [AssertionMethod]
    public static void IsFalse([AssertionCondition(AssertionConditionType.IS_FALSE)] bool expression, string message, params object[] arguments)
    {
      if (expression)
        throw new InvalidOperationException(string.Format(message, arguments));
    }

    [AssertionMethod]
    public static T IsNotNull<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T obj, string message) => Assertion.IsNotNull<T>(obj, message, Assertion.s_emptyArguments);

    [AssertionMethod]
    public static T IsNotNull<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T obj) => Assertion.IsNotNull<T>(obj, "Assertion failed: Expression evaluates to a null reference.");

    [AssertionMethod]
    [StringFormatMethod("message")]
    public static T IsNotNull<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T obj, string message, params object[] arguments) => (object) obj != null ? obj : throw new InvalidOperationException(string.Format(message, arguments));

    [AssertionMethod]
    public static void IsNull([AssertionCondition(AssertionConditionType.IS_NULL)] object obj, string message) => Assertion.IsNull(obj, message, Assertion.s_emptyArguments);

    [AssertionMethod]
    public static void IsNull([AssertionCondition(AssertionConditionType.IS_NULL)] object obj) => Assertion.IsNull(obj, "Assertion failed: Expression does not evaluate to a null reference.");

    [StringFormatMethod("message")]
    [AssertionMethod]
    public static void IsNull([AssertionCondition(AssertionConditionType.IS_NULL)] object obj, string message, params object[] arguments)
    {
      if (obj != null)
        throw new InvalidOperationException(string.Format(message, arguments));
    }
  }
}
