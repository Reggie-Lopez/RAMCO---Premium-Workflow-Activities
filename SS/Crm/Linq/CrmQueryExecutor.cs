// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.CrmQueryExecutor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using SS.Crm.Linq.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace SS.Crm.Linq
{
  internal class CrmQueryExecutor : IQueryExecutor
  {
    private IOrganizationService _service;
    private string _entityLogicalName;
    private ColumnSet _columns;
    private bool _retrieveAllRecords;
    private List<Type> _supportedAggregates = new List<Type>()
    {
      typeof (SumResultOperator),
      typeof (AverageResultOperator),
      typeof (MinResultOperator),
      typeof (MaxResultOperator),
      typeof (CountResultOperator)
    };

    public CrmQueryExecutor(
      IOrganizationService service,
      string entityLogicalName = null,
      ColumnSet columns = null,
      bool retrieveAllRecords = false)
    {
      this._service = service;
      this._entityLogicalName = entityLogicalName;
      this._columns = columns;
      this._retrieveAllRecords = retrieveAllRecords;
    }

    public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel) => this.ExecuteQuery<T>(queryModel);

    public T ExecuteScalar<T>(QueryModel queryModel) => this.ExecuteQuery<T>(queryModel).FirstOrDefault<T>();

    public IEnumerable<T> ExecuteQuery<T>(QueryModel queryModel)
    {
      CrmQueryModelVisitor modelVisitor = new CrmQueryModelVisitor(this._service, this._entityLogicalName, this._columns);
      modelVisitor.VisitQueryModel(queryModel);
      if (queryModel.ResultOperators.Where<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (resultOperator => resultOperator is SkipResultOperator || resultOperator is TakeResultOperator)).Count<ResultOperatorBase>() > 0)
      {
        if (!(queryModel.ResultOperators.Where<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (resultOperator => resultOperator is SkipResultOperator)).FirstOrDefault<ResultOperatorBase>() is SkipResultOperator skipResultOperator))
          skipResultOperator = new SkipResultOperator((Expression) Expression.Constant((object) 0));
        if (!(queryModel.ResultOperators.Where<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (resultOperator => resultOperator is TakeResultOperator)).FirstOrDefault<ResultOperatorBase>() is TakeResultOperator takeResultOperator))
          takeResultOperator = new TakeResultOperator((Expression) Expression.Constant((object) 0));
        queryModel.ResultOperators.Remove((ResultOperatorBase) skipResultOperator);
        queryModel.ResultOperators.Remove((ResultOperatorBase) takeResultOperator);
        if (!(takeResultOperator.Count is ConstantExpression) || !(skipResultOperator.Count is ConstantExpression))
          throw new NotSupportedException("The 'Skip' and 'Take' values must be a constant expression.");
        int num1 = (int) ((ConstantExpression) takeResultOperator.Count).Value;
        int num2 = (int) ((ConstantExpression) skipResultOperator.Count).Value;
        if (num2 != 0 && num1 > num2)
          throw new NotSupportedException("The 'Take' count must be <= the 'Skip' count.");
        if (num2 != 0 && (double) num2 / (double) num1 - Math.Truncate((double) num2 / (double) num1) != 0.0)
          throw new NotSupportedException("The 'Skip' count must be a multiple of the 'Take' count.");
        QueryExpression query = modelVisitor.Query;
        PagingInfo pagingInfo = new PagingInfo();
        pagingInfo.set_PageNumber(Math.Max(num2 / num1 + 1, 1));
        pagingInfo.set_Count(num1);
        query.set_PageInfo(pagingInfo);
      }
      if (queryModel.ResultOperators.Count == 1 && this._supportedAggregates.Contains(queryModel.ResultOperators.FirstOrDefault<ResultOperatorBase>().GetType()) || queryModel.ResultOperators.Count == 2 && queryModel.ResultOperators[0] is DistinctResultOperator && this._supportedAggregates.Contains(queryModel.ResultOperators.LastOrDefault<ResultOperatorBase>().GetType()))
      {
        int num = queryModel.ResultOperators.Count > 1 ? 1 : 0;
        ResultOperatorBase aggregate = queryModel.ResultOperators.LastOrDefault<ResultOperatorBase>();
        XDocument xdocument = XDocument.Parse(modelVisitor.Query.ToFetchXML());
        string aggregateOperatorString = aggregate.GetAggregateOperatorString();
        xdocument.Descendants((XName) "fetch").FirstOrDefault<XElement>().Add((object) new XAttribute((XName) "aggregate", (object) "true"));
        XElement xelement = xdocument.Descendants((XName) "attribute").FirstOrDefault<XElement>();
        string str = string.Format("{0}.{1}", (object) xelement.Attribute((XName) "name").Value, (object) aggregateOperatorString);
        xelement.Add((object) new XAttribute((XName) "aggregate", (object) aggregateOperatorString));
        xelement.Add((object) new XAttribute((XName) "alias", (object) str));
        if (num != 0)
          xelement.Add((object) new XAttribute((XName) "distinct", (object) "true"));
        ExecuteFetchRequest executeFetchRequest = new ExecuteFetchRequest();
        executeFetchRequest.set_FetchXml(xdocument.ToString());
        return (IEnumerable<T>) new List<T>()
        {
          (T) Convert.ChangeType((object) XDocument.Parse((this._service.Execute((OrganizationRequest) executeFetchRequest) as ExecuteFetchResponse).get_FetchXmlResult()).Descendants((XName) str).FirstOrDefault<XElement>().Value, typeof (T))
        };
      }
      if (modelVisitor.GroupByOperators != null && modelVisitor.GroupByOperators.Count > 0)
      {
        GroupResultOperator groupByOperator = modelVisitor.GroupByOperators[0] as GroupResultOperator;
        string groupByAttribute = groupByOperator.KeySelector.GetAttributeLogicalName(this._service, modelVisitor.EntityTypeAliases);
        string aggregateAttribute = groupByOperator.ElementSelector.GetAttributeLogicalName(this._service, modelVisitor.EntityTypeAliases);
        List<string> stringList = new List<string>();
        for (int index = 1; index < modelVisitor.ReturnBindings.Count; ++index)
        {
          ResultOperatorBase resultOperator = (modelVisitor.ReturnBindings[index].SourceExpression as SubQueryExpression).QueryModel.ResultOperators[0];
          stringList.Add(resultOperator.GetAggregateOperatorString());
        }
        XDocument xdocument = XDocument.Parse(modelVisitor.Query.ToFetchXML());
        xdocument.Descendants((XName) "fetch").FirstOrDefault<XElement>().Add((object) new XAttribute((XName) "aggregate", (object) "true"));
        XElement xelement1 = xdocument.Descendants((XName) "attribute").Where<XElement>((Func<XElement, bool>) (element => element.Attribute((XName) "name").Value == groupByAttribute)).FirstOrDefault<XElement>();
        xelement1.Add((object) new XAttribute((XName) "groupby", (object) "true"));
        xelement1.Add((object) new XAttribute((XName) "alias", (object) groupByAttribute));
        XElement xelement2 = xdocument.Descendants((XName) "attribute").Where<XElement>((Func<XElement, bool>) (element => element.Attribute((XName) "name").Value == aggregateAttribute)).FirstOrDefault<XElement>();
        string str1 = string.Format("{0}.{1}", (object) xelement2.Attribute((XName) "name").Value, (object) stringList[0]);
        xelement2.Add((object) new XAttribute((XName) "aggregate", (object) stringList[0]));
        xelement2.Add((object) new XAttribute((XName) "alias", (object) str1));
        for (int index = 1; index < stringList.Count; ++index)
        {
          XElement xelement3 = new XElement((XName) "attribute");
          string str2 = string.Format("{0}.{1}", (object) xelement2.Attribute((XName) "name").Value, (object) stringList[index]);
          xelement3.Add((object) new XAttribute((XName) "name", (object) aggregateAttribute));
          xelement3.Add((object) new XAttribute((XName) "aggregate", (object) stringList[index]));
          xelement3.Add((object) new XAttribute((XName) "alias", (object) str2));
          xelement2.AddAfterSelf((object) xelement3);
          xelement2 = xelement3;
        }
        ExecuteFetchRequest executeFetchRequest = new ExecuteFetchRequest();
        executeFetchRequest.set_FetchXml(xdocument.ToString());
        List<XElement> list = XDocument.Parse((this._service.Execute((OrganizationRequest) executeFetchRequest) as ExecuteFetchResponse).get_FetchXmlResult()).Descendants((XName) "result").ToList<XElement>();
        List<Entity> entities = new List<Entity>();
        foreach (XElement xelement3 in list)
        {
          Entity entity = new Entity();
          ((DataCollection<string, object>) entity.get_Attributes()).set_Item(groupByAttribute, Convert.ChangeType((object) xelement3.Element((XName) groupByAttribute).Value, groupByOperator.KeySelector.Type));
          for (int index = 0; index < stringList.Count; ++index)
          {
            string str2 = string.Format("{0}.{1}", (object) xelement2.Attribute((XName) "name").Value, (object) stringList[index]);
            ((DataCollection<string, object>) entity.get_Attributes()).set_Item(str2, Convert.ChangeType((object) xelement3.Element((XName) str2).Value, groupByOperator.ElementSelector.Type));
          }
          entities.Add(entity);
        }
        return this.ParseEntities<T>(modelVisitor, entities);
      }
      EntityCollection entityCollection = RetrieveHelper.RetrieveEntities(this._service, modelVisitor.Query, this._retrieveAllRecords);
      return this.ParseEntities<T>(modelVisitor, ((IEnumerable<Entity>) entityCollection.get_Entities()).ToList<Entity>());
    }

    private IEnumerable<T> ParseEntities<T>(
      CrmQueryModelVisitor modelVisitor,
      List<Entity> entities)
    {
      Type returnType = modelVisitor.ReturnType;
      if (returnType == typeof (Entity))
        return ((IEnumerable) entities).Cast<T>().AsEnumerable<T>();
      ConstructorInfo constructor = (ConstructorInfo) null;
      try
      {
        constructor = returnType.GetConstructor(new Type[1]
        {
          typeof (Entity)
        });
      }
      catch (Exception ex)
      {
      }
      if (constructor != (ConstructorInfo) null)
        return ((IEnumerable<Entity>) entities).Select<Entity, T>((Func<Entity, T>) (entity => (T) constructor.Invoke(new object[1]
        {
          (object) entity
        })));
      if (typeof (Entity).IsAssignableFrom(returnType))
      {
        constructor = returnType.GetConstructor(new Type[0]);
        if (!(constructor != (ConstructorInfo) null))
          throw new Exception("The type does not have a constructor that has not arguments");
        List<Entity> entityList = new List<Entity>();
        using (List<Entity>.Enumerator enumerator = entities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Entity current = enumerator.Current;
            Entity target = (Entity) constructor.Invoke(new object[0]);
            current.CopyTo(target);
            entityList.Add(target);
          }
        }
        return ((IEnumerable) entityList).Cast<T>();
      }
      if (CrmQueryExecutor.CheckIfAnonymousType(returnType))
      {
        constructor = ((IEnumerable<ConstructorInfo>) returnType.GetConstructors()).FirstOrDefault<ConstructorInfo>();
        ParameterInfo[] parameters = constructor.GetParameters();
        List<T> objList = new List<T>();
        using (List<Entity>.Enumerator enumerator = entities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Entity current = enumerator.Current;
            List<object> objectList = new List<object>();
            if (modelVisitor.GroupByOperators != null)
            {
              string attributeLogicalName = ((GroupResultOperator) modelVisitor.GroupByOperators[0]).KeySelector.GetAttributeLogicalName(this._service, modelVisitor.EntityTypeAliases);
              objectList.Add(current.get_Item(attributeLogicalName));
              for (int index = 1; index < modelVisitor.ReturnBindings.Count; ++index)
              {
                string aggregateOperatorString = ((SubQueryExpression) modelVisitor.ReturnBindings[index].SourceExpression).QueryModel.ResultOperators[0].GetAggregateOperatorString();
                string str = string.Format("{0}.{1}", (object) ((GroupResultOperator) modelVisitor.GroupByOperators[0]).ElementSelector.GetAttributeLogicalName(this._service, modelVisitor.EntityTypeAliases), (object) aggregateOperatorString);
                if (current.Contains(str))
                  objectList.Add(current.get_Item(str));
                else
                  objectList.Add((object) null);
              }
            }
            else
            {
              Dictionary<string, ConstructorInfo> entityConstructors = new Dictionary<string, ConstructorInfo>();
              foreach (ParameterInfo parameterInfo in parameters)
              {
                ParameterInfo parameter = parameterInfo;
                AnonymousBinding anonymousBinding = modelVisitor.ReturnBindings.Where<AnonymousBinding>((Func<AnonymousBinding, bool>) (binding => binding.TargetMember.Name == parameter.Name)).FirstOrDefault<AnonymousBinding>();
                if (anonymousBinding != null)
                  objectList.Add(this.GetExpressionValue(modelVisitor, anonymousBinding.SourceExpression, current, entityConstructors));
                else
                  objectList.Add((object) null);
              }
            }
            object obj = constructor.Invoke(objectList.ToArray());
            objList.Add((T) obj);
          }
        }
        return (IEnumerable<T>) objList;
      }
      constructor = typeof (T).GetConstructor(new Type[0]);
      if (!(constructor != (ConstructorInfo) null))
        throw new Exception(string.Format("Unable to convert to the type: {0}", (object) returnType.Name));
      List<T> objList1 = new List<T>();
      PropertyInfo[] properties = typeof (T).GetProperties();
      Dictionary<string, ConstructorInfo> entityConstructors1 = new Dictionary<string, ConstructorInfo>();
      using (List<Entity>.Enumerator enumerator = entities.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Entity current = enumerator.Current;
          T obj = (T) constructor.Invoke(new object[0]);
          foreach (PropertyInfo propertyInfo1 in properties)
          {
            PropertyInfo propertyInfo = propertyInfo1;
            AnonymousBinding anonymousBinding = modelVisitor.ReturnBindings.Where<AnonymousBinding>((Func<AnonymousBinding, bool>) (binding => binding.TargetMember.Name == propertyInfo.Name)).FirstOrDefault<AnonymousBinding>();
            if (anonymousBinding != null)
              propertyInfo.SetValue((object) obj, this.GetExpressionValue(modelVisitor, anonymousBinding.SourceExpression, current, entityConstructors1));
          }
          objList1.Add(obj);
        }
      }
      return (IEnumerable<T>) objList1;
    }

    private object GetExpressionValue(
      CrmQueryModelVisitor modelVisitor,
      Expression expression,
      Entity entity,
      Dictionary<string, ConstructorInfo> entityConstructors)
    {
      switch (expression)
      {
        case MemberExpression _:
          object obj = (object) null;
          MemberExpression expression1 = expression as MemberExpression;
          if (((IEnumerable<object>) expression1.Member.GetCustomAttributes(typeof (AttributeLogicalNameAttribute), false)).FirstOrDefault<object>() is AttributeLogicalNameAttribute logicalNameAttribute)
          {
            string aliasedEntityName = expression1.GetAliasedEntityName();
            string str = logicalNameAttribute.get_LogicalName();
            if (aliasedEntityName != modelVisitor.MainEntityTypeAlias)
              str = string.Format("{0}.{1}", (object) aliasedEntityName, (object) str);
            obj = str.Contains(".") ? (entity.Contains(str) ? ((AliasedValue) entity.get_Item(str)).get_Value() : (object) null) : (entity.Contains(str) ? entity.get_Item(str) : (object) null);
          }
          else if (expression1.Member.Name == "Id")
            return (object) entity.get_Id();
          return obj;
        case ConstantExpression _:
          return ((ConstantExpression) expression).Value;
        case UnaryExpression _:
          UnaryExpression expression2 = expression as UnaryExpression;
          string aliasedEntityName1 = expression2.GetAliasedEntityName();
          string str1 = expression2.Operand.GetAttributeLogicalName(this._service, modelVisitor.EntityTypeAliases);
          if (aliasedEntityName1 != modelVisitor.MainEntityTypeAlias)
            str1 = string.Format("{0}.{1}", (object) aliasedEntityName1, (object) str1);
          return !str1.Contains(".") ? (!entity.Contains(str1) ? (object) null : entity.get_Item(str1)) : (!entity.Contains(str1) ? (object) null : ((AliasedValue) entity.get_Item(str1)).get_Value());
        case MethodCallExpression _:
          MethodCallExpression methodCallExpression = expression as MethodCallExpression;
          if (methodCallExpression.Object == null)
            return methodCallExpression.Method.Invoke((object) methodCallExpression.Object, new object[0]);
          object expressionValue = this.GetExpressionValue(modelVisitor, methodCallExpression.Object, entity, entityConstructors);
          return methodCallExpression.Method.Invoke(expressionValue, new object[0]);
        case QuerySourceReferenceExpression _:
          QuerySourceReferenceExpression referenceExpression = expression as QuerySourceReferenceExpression;
          if (referenceExpression.ReferencedQuerySource is MainFromClause)
          {
            MainFromClause referencedQuerySource = referenceExpression.ReferencedQuerySource as MainFromClause;
            if (!entityConstructors.ContainsKey(referencedQuerySource.ItemName))
              entityConstructors[referencedQuerySource.ItemName] = referencedQuerySource.ItemType.GetConstructor(new Type[0]);
            Entity entity1 = entityConstructors[referencedQuerySource.ItemName].Invoke(new object[0]) as Entity;
            using (IEnumerator<KeyValuePair<string, object>> enumerator = ((DataCollection<string, object>) entity.get_Attributes()).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<string, object> current = enumerator.Current;
                if (!current.Key.Contains("."))
                  entity1.set_Item(current.Key, current.Value);
              }
            }
            return (object) entity1;
          }
          JoinClause joinClause = referenceExpression.ReferencedQuerySource is JoinClause ? referenceExpression.ReferencedQuerySource as JoinClause : throw new NotSupportedException(string.Format("The expression type {0} is not supported.", (object) expression.GetType().Name));
          if (!entityConstructors.ContainsKey(joinClause.ItemName))
            entityConstructors[joinClause.ItemName] = joinClause.ItemType.GetConstructor(new Type[0]);
          Entity entity2 = entityConstructors[joinClause.ItemName].Invoke(new object[0]) as Entity;
          using (IEnumerator<KeyValuePair<string, object>> enumerator = ((DataCollection<string, object>) entity.get_Attributes()).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, object> current = enumerator.Current;
              if (current.Key.Contains(string.Format("{0}.", (object) joinClause.ItemName)) && current.Value is AliasedValue)
                entity2.set_Item(((AliasedValue) current.Value).get_AttributeLogicalName(), ((AliasedValue) current.Value).get_Value());
            }
          }
          return (object) entity2;
        default:
          throw new NotSupportedException(string.Format("The expression type {0} is not supported.", (object) expression.GetType().Name));
      }
    }

    public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty) => this.ExecuteCollection<T>(queryModel).FirstOrDefault<T>();

    private static bool CheckIfAnonymousType(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      return Attribute.IsDefined((MemberInfo) type, typeof (CompilerGeneratedAttribute), false) && type.IsGenericType && type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")) && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
    }
  }
}
