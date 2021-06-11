// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperatorBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using Remotion.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public abstract class ResultOperatorBase
  {
    public abstract IStreamedData ExecuteInMemory(IStreamedData input);

    public abstract IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo);

    public abstract ResultOperatorBase Clone(CloneContext cloneContext);

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitResultOperator(this, queryModel, index);
    }

    public abstract void TransformExpressions(Func<Expression, Expression> transformation);

    protected object InvokeExecuteMethod(MethodInfo method, object input)
    {
      if (!method.IsPublic)
        throw new ArgumentException("Method to invoke ('" + method.Name + "') must be a public method.", nameof (method));
      ResultOperatorBase resultOperatorBase = method.IsStatic ? (ResultOperatorBase) null : this;
      try
      {
        return method.Invoke((object) resultOperatorBase, new object[1]
        {
          input
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException(string.Format("Cannot call method '{0}' on input of type '{1}': {2}", (object) method.Name, (object) input.GetType(), (object) ex.Message), nameof (method));
      }
    }

    protected T GetConstantValueFromExpression<T>(string expressionName, Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (!typeof (T).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo()))
        throw new ArgumentException(string.Format("The value stored by the {0} expression ('{1}') is not of type '{2}', it is of type '{3}'.", (object) expressionName, (object) expression.BuildString(), (object) typeof (T), (object) expression.Type), nameof (expression));
      return expression is ConstantExpression constantExpression ? (T) constantExpression.Value : throw new ArgumentException(string.Format("The {0} expression ('{1}') is no ConstantExpression, it is a {2}.", (object) expressionName, (object) expression.BuildString(), (object) expression.GetType().Name), nameof (expression));
    }

    protected void CheckSequenceItemType(StreamedSequenceInfo inputInfo, Type expectedItemType)
    {
      if (!expectedItemType.GetTypeInfo().IsAssignableFrom(inputInfo.ResultItemType.GetTypeInfo()))
        throw new ArgumentException(string.Format("The input sequence must have items of type '{0}', but it has items of type '{1}'.", (object) expectedItemType, (object) inputInfo.ResultItemType), nameof (inputInfo));
    }
  }
}
