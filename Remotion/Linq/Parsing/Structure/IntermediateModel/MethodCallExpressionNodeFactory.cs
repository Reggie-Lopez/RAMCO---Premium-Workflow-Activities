// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionNodeFactory
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  [ComVisible(true)]
  public static class MethodCallExpressionNodeFactory
  {
    public static IExpressionNode CreateExpressionNode(
      Type nodeType,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (nodeType), nodeType, typeof (IExpressionNode));
      ArgumentUtility.CheckNotNull<object[]>(nameof (additionalConstructorParameters), additionalConstructorParameters);
      ConstructorInfo[] array = nodeType.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsPublic)).ToArray<ConstructorInfo>();
      if (array.Length > 1)
        throw new ArgumentException(string.Format("Expression node type '{0}' contains too many constructors. It must only contain a single constructor, allowing null to be passed for any optional arguments.", (object) nodeType.FullName), nameof (nodeType));
      object[] parameterArray = MethodCallExpressionNodeFactory.GetParameterArray(array[0], parseInfo, additionalConstructorParameters);
      try
      {
        return (IExpressionNode) array[0].Invoke(parameterArray);
      }
      catch (ArgumentException ex)
      {
        throw new ExpressionNodeInstantiationException(MethodCallExpressionNodeFactory.GetArgumentMismatchMessage(ex));
      }
    }

    private static string GetArgumentMismatchMessage(ArgumentException ex) => ex.Message.Contains(typeof (LambdaExpression).Name) && ex.Message.Contains(typeof (ConstantExpression).Name) ? string.Format("{0} If you tried to pass a delegate instead of a LambdaExpression, this is not supported because delegates are not parsable expressions.", (object) ex.Message) : string.Format("The given arguments did not match the expected arguments: {0}", (object) ex.Message);

    private static object[] GetParameterArray(
      ConstructorInfo nodeTypeConstructor,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      ParameterInfo[] parameters = nodeTypeConstructor.GetParameters();
      if (additionalConstructorParameters.Length > parameters.Length - 1)
        throw new ExpressionNodeInstantiationException(string.Format("The constructor of expression node type '{0}' only takes {1} parameters, but you specified {2} (including the parse info parameter).", (object) nodeTypeConstructor.DeclaringType.FullName, (object) parameters.Length, (object) (additionalConstructorParameters.Length + 1)));
      object[] objArray = new object[parameters.Length];
      objArray[0] = (object) parseInfo;
      additionalConstructorParameters.CopyTo((Array) objArray, 1);
      return objArray;
    }
  }
}
