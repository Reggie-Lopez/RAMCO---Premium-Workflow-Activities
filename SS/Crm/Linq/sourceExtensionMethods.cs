// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.sourceExtensionMethods
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SS.Crm.Linq
{
  internal static class sourceExtensionMethods
  {
    private static Dictionary<string, EntityMetadata> _entityMetadata = new Dictionary<string, EntityMetadata>();

    internal static void CopyTo(this Entity source, Entity target)
    {
      if (target == null || target == source)
        return;
      ((DataCollection<Relationship, EntityCollection>) target.get_RelatedEntities()).Clear();
      ((DataCollection<string, object>) target.get_Attributes()).Clear();
      ((DataCollection<string, string>) target.get_FormattedValues()).Clear();
      target.set_LogicalName(source.get_LogicalName());
      ((DataCollection<Relationship, EntityCollection>) target.get_RelatedEntities()).AddRange((IEnumerable<KeyValuePair<Relationship, EntityCollection>>) source.get_RelatedEntities());
      ((DataCollection<string, object>) target.get_Attributes()).AddRange((IEnumerable<KeyValuePair<string, object>>) source.get_Attributes());
      ((DataCollection<string, string>) target.get_FormattedValues()).AddRange((IEnumerable<KeyValuePair<string, string>>) source.get_FormattedValues());
      target.set_ExtensionData(source.get_ExtensionData());
      target.set_EntityState(source.get_EntityState());
      target.set_Id(source.get_Id());
    }

    internal static string GetAttributeLogicalName(
      this MemberInfo member,
      IOrganizationService service,
      string entityLogicalName)
    {
      if (((IEnumerable<object>) member.GetCustomAttributes(typeof (AttributeLogicalNameAttribute), false)).FirstOrDefault<object>() is AttributeLogicalNameAttribute logicalNameAttribute)
        return logicalNameAttribute.get_LogicalName();
      if (!typeof (Entity).IsAssignableFrom(member.DeclaringType) || !(member.Name == "Id") || string.IsNullOrEmpty(entityLogicalName))
        return (string) null;
      if (!sourceExtensionMethods._entityMetadata.ContainsKey(entityLogicalName))
      {
        RetrieveEntityRequest retrieveEntityRequest1 = new RetrieveEntityRequest();
        retrieveEntityRequest1.set_LogicalName(entityLogicalName);
        retrieveEntityRequest1.set_EntityFilters((EntityFilters) 1);
        retrieveEntityRequest1.set_RetrieveAsIfPublished(true);
        RetrieveEntityRequest retrieveEntityRequest2 = retrieveEntityRequest1;
        RetrieveEntityResponse retrieveEntityResponse = service.Execute((OrganizationRequest) retrieveEntityRequest2) as RetrieveEntityResponse;
        sourceExtensionMethods._entityMetadata.Add(entityLogicalName, retrieveEntityResponse.get_EntityMetadata());
      }
      return sourceExtensionMethods._entityMetadata.ContainsKey(entityLogicalName) ? sourceExtensionMethods._entityMetadata[entityLogicalName].get_PrimaryIdAttribute() : "Id";
    }

    internal static string GetAttributeLogicalName(
      this Expression expression,
      IOrganizationService service,
      Dictionary<string, string> entityTypeAliases)
    {
      string str = string.Empty;
      switch (expression)
      {
        case MemberExpression _:
          MemberExpression member = expression as MemberExpression;
          string entityLogicalName = member.GetEntityLogicalName(entityTypeAliases);
          str = member.Member.GetAttributeLogicalName(service, entityLogicalName);
          if (string.IsNullOrEmpty(str))
          {
            str = member.Expression.GetAttributeLogicalName(service, entityTypeAliases);
            break;
          }
          break;
        case UnaryExpression _:
          str = (expression as UnaryExpression).Operand.GetAttributeLogicalName(service, entityTypeAliases);
          break;
        case MethodCallExpression _:
          if (((MethodCallExpression) expression).Object != null && ((MethodCallExpression) expression).Object.Type == typeof (AttributeCollection))
          {
            str = (string) ((ConstantExpression) (expression as MethodCallExpression).Arguments[0]).Value;
            break;
          }
          break;
      }
      return str;
    }

    internal static string GetEntityLogicalName(this Type type) => ((IEnumerable<object>) type.GetCustomAttributes(typeof (EntityLogicalNameAttribute), false)).FirstOrDefault<object>() is EntityLogicalNameAttribute logicalNameAttribute ? logicalNameAttribute.get_LogicalName() : (string) null;

    internal static string GetEntityLogicalName(
      this MemberExpression member,
      Dictionary<string, string> entityTypeAliases)
    {
      string str = member.Member.DeclaringType.GetEntityLogicalName();
      if (string.IsNullOrEmpty(str))
      {
        if (member.Expression is MemberExpression && member.Expression.Type == typeof (EntityReference))
          str = ((MemberExpression) member.Expression).Member.DeclaringType.GetEntityLogicalName();
        else if (member.Expression is QuerySourceReferenceExpression)
        {
          QuerySourceReferenceExpression expression = member.Expression as QuerySourceReferenceExpression;
          if (entityTypeAliases.ContainsKey(expression.ReferencedQuerySource.ItemName))
            str = entityTypeAliases[expression.ReferencedQuerySource.ItemName];
        }
      }
      return str;
    }

    internal static Type GetEntityType(this MemberExpression member)
    {
      Type declaringType = member.Member.DeclaringType;
      if (string.IsNullOrEmpty(declaringType.GetEntityLogicalName()) && member.Expression is MemberExpression && (member.Expression.Type == typeof (EntityReference) && !string.IsNullOrEmpty(((MemberExpression) member.Expression).Member.DeclaringType.GetEntityLogicalName())))
        declaringType = ((MemberExpression) member.Expression).Member.DeclaringType;
      return declaringType;
    }

    internal static string GetAliasedEntityName(this Expression expression)
    {
      switch (expression)
      {
        case MethodCallExpression _:
          MethodCallExpression methodCallExpression = expression as MethodCallExpression;
          return methodCallExpression.Object != null ? methodCallExpression.Object.GetAliasedEntityName() : methodCallExpression.Arguments[0].GetAliasedEntityName();
        case MemberExpression _:
          return (expression as MemberExpression).Expression.GetAliasedEntityName();
        case QuerySourceReferenceExpression _:
          return (expression as QuerySourceReferenceExpression).ReferencedQuerySource.ItemName;
        case UnaryExpression _:
          return (expression as UnaryExpression).Operand.GetAliasedEntityName();
        case SubQueryExpression _:
          return (expression as SubQueryExpression).QueryModel.MainFromClause.ItemName;
        case ConstantExpression _:
          return string.Empty;
        case PartialEvaluationExceptionExpression _:
          return (expression as PartialEvaluationExceptionExpression).EvaluatedExpression.GetAliasedEntityName();
        default:
          throw new Exception(string.Format("Please update the '{0}' method to handle the expression type of '{1}'", (object) nameof (GetAliasedEntityName), (object) expression.GetType().ToString()));
      }
    }

    internal static string GetEntityTypeName(this Expression expression)
    {
      switch (expression)
      {
        case SubQueryExpression _:
          return (expression as SubQueryExpression).QueryModel.MainFromClause.FromExpression.GetEntityTypeName();
        case ConstantExpression _:
          ConstantExpression constantExpression = expression as ConstantExpression;
          IQueryable queryable = constantExpression.Value is IQueryable ? constantExpression.Value as IQueryable : throw new NotImplementedException(string.Format("Please update the 'GetEntityTypeName' method handle the expression type of '{0}'", (object) expression.GetType().ToString()));
          return queryable is ICrmQueryable ? ((ICrmQueryable) queryable).EntityLogicalName : queryable.ElementType.GetEntityLogicalName();
        default:
          throw new NotImplementedException(string.Format("Please update the 'GetEntityTypeName' method handle the expression type of '{0}'", (object) expression.GetType().ToString()));
      }
    }

    internal static string ToFetchXML(this QueryExpression query)
    {
      string str1 = "<fetch mapping=\"logical\">" + string.Format("<entity name=\"{0}\">", (object) query.get_EntityName()) + query.get_ColumnSet().ToFetchXMLPart();
      using (IEnumerator<OrderExpression> enumerator = ((Collection<OrderExpression>) query.get_Orders()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          OrderExpression current = enumerator.Current;
          str1 += current.ToFetchXMLPart();
        }
      }
      string str2 = str1 + query.get_Criteria().ToFetchXMLPart();
      using (IEnumerator<LinkEntity> enumerator = ((Collection<LinkEntity>) query.get_LinkEntities()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          LinkEntity current = enumerator.Current;
          str2 += current.ToFetchXMLPart();
        }
      }
      return str2 + "</entity>" + "</fetch>";
    }

    internal static string ToFetchXMLPart(this FilterExpression filter)
    {
      string str = string.Format("<filter type=\"{0}\">", (object) filter.get_FilterOperator().ToString().ToLowerInvariant());
      using (IEnumerator<ConditionExpression> enumerator = ((Collection<ConditionExpression>) filter.get_Conditions()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ConditionExpression current = enumerator.Current;
          str += current.ToFetchXMLPart();
        }
      }
      using (IEnumerator<FilterExpression> enumerator = ((Collection<FilterExpression>) filter.get_Filters()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          FilterExpression current = enumerator.Current;
          str += current.ToFetchXMLPart();
        }
      }
      return str + "</filter>";
    }

    internal static string ToFetchXMLPart(this ConditionExpression condition)
    {
      string operatorString = condition.get_Operator().GetOperatorString();
      string str1 = "";
      if (condition.get_Values() == null || ((Collection<object>) condition.get_Values()).Count == 0)
        str1 = string.Format("<condition attribute=\"{0}\" operator=\"{1}\" />", (object) condition.get_AttributeName(), (object) operatorString);
      else if (((Collection<object>) condition.get_Values()).Count > 1 || condition.get_Operator() == 8 || condition.get_Operator() == 9)
      {
        string str2 = string.Format("<condition attribute=\"{0}\" operator=\"{1}\">", (object) condition.get_AttributeName(), (object) operatorString);
        foreach (object obj in (Collection<object>) condition.get_Values())
          str2 = !(obj is EntityReference) ? str2 + string.Format("<value>{0}</value>", (object) obj.ToString()) : str2 + string.Format("<value uitype=\"{0}\">{1}</value>", (object) ((EntityReference) obj).get_LogicalName(), (object) ((EntityReference) obj).get_Id().ToString());
        str1 = str2 + "</condition>";
      }
      else if (((Collection<object>) condition.get_Values()).Count == 1)
        str1 = string.Format("<condition attribute=\"{0}\" operator=\"{1}\" value=\"{2}\" />", (object) condition.get_AttributeName(), (object) operatorString, (object) ((Collection<object>) condition.get_Values())[0].ToString());
      return str1;
    }

    internal static string ToFetchXMLPart(this ColumnSet columns)
    {
      string str = "";
      if (columns.get_AllColumns())
      {
        str = "<all-attributes />";
      }
      else
      {
        foreach (string column in (Collection<string>) columns.get_Columns())
          str += string.Format("<attribute name=\"{0}\" />", (object) column);
      }
      return str;
    }

    internal static string ToFetchXMLPart(this LinkEntity linkEntity) => string.Format("<link-entity name=\"{0}\" from=\"{1}\" to=\"{2}\" alias=\"{3}\">", (object) linkEntity.get_LinkToEntityName(), (object) linkEntity.get_LinkFromAttributeName(), (object) linkEntity.get_LinkToAttributeName(), (object) linkEntity.get_EntityAlias()) + linkEntity.get_Columns().ToFetchXMLPart() + linkEntity.get_LinkCriteria().ToFetchXMLPart() + "</link-entity>";

    internal static string ToFetchXMLPart(this OrderExpression order) => string.Format("<order attribute=\"{0}\" descending=\"{1}\" />", (object) order.get_AttributeName(), order.get_OrderType() == null ? (object) "false" : (object) "true");

    internal static string GetOperatorString(this ConditionOperator op)
    {
      switch ((int) op)
      {
        case 0:
          return "eq";
        case 1:
        case 52:
          return "ne";
        case 2:
          return "gt";
        case 3:
          return "lt";
        case 4:
          return "ge";
        case 5:
          return "le";
        case 6:
        case 49:
        case 54:
          return "like";
        case 7:
          return "not-like";
        case 8:
          return "in";
        case 9:
          return "not-in";
        case 10:
          return "between";
        case 11:
          return "not-between";
        case 12:
          return "null";
        case 13:
          return "not-null";
        case 14:
          return "yesterday";
        case 15:
          return "today";
        case 16:
          return "tomorrow";
        case 17:
          return "last-seven-days";
        case 18:
          return "next-seven-days";
        case 19:
          return "last-week";
        case 20:
          return "this-week";
        case 21:
          return "next-week";
        case 22:
          return "last-month";
        case 23:
          return "this-month";
        case 24:
          return "next-month";
        case 25:
          return "on";
        case 26:
          return "on-or-before";
        case 27:
          return "on-or-after";
        case 28:
          return "last-year";
        case 29:
          return "this-year";
        case 30:
          return "next-year";
        case 31:
          return "last-x-hours";
        case 32:
          return "next-x-hours";
        case 33:
          return "last-x-days";
        case 34:
          return "next-x-days";
        case 35:
          return "last-x-weeks";
        case 36:
          return "next-x-weeks";
        case 37:
          return "last-x-months";
        case 38:
          return "next-x-months";
        case 39:
          return "last-x-years";
        case 40:
          return "next-x-years";
        case 41:
          return "eq-userid";
        case 42:
          return "ne-userid";
        case 43:
          return "eq-businessid";
        case 44:
          return "ne-businessid";
        case 50:
        case 55:
        case 57:
          return "not-like";
        case 53:
          return "olderthan-x-months";
        case 58:
          return "this-fiscal-year";
        case 59:
          return "this-fiscal-period";
        case 60:
          return "next-fiscal-year";
        case 61:
          return "next-fiscal-period";
        case 62:
          return "last-fiscal-year";
        case 63:
          return "last-fiscal-period";
        case 64:
          return "last-x-fiscal-years";
        case 65:
          return "last-x-fiscal-periods";
        case 66:
          return "next-x-fiscal-years";
        case 67:
          return "next-x-fiscal-periods";
        case 68:
          return "in-fiscal-year";
        case 69:
          return "in-fiscal-period";
        case 70:
          return "in-fiscal-period-and-year";
        case 71:
          return "in-or-before-fiscal-period-and-year";
        case 72:
          return "in-or-after-fiscal-period-and-year";
        default:
          throw new NotSupportedException(string.Format("Please update the '{0}' method to handle the '{1}' operator.", (object) nameof (GetOperatorString), (object) op.ToString()));
      }
    }

    internal static string GetAggregateOperatorString(this ResultOperatorBase aggregate)
    {
      switch (aggregate)
      {
        case SumResultOperator _:
          return "sum";
        case AverageResultOperator _:
          return "avg";
        case MaxResultOperator _:
          return "max";
        case MinResultOperator _:
          return "min";
        case CountResultOperator _:
          return "countcolumn";
        default:
          throw new NotSupportedException(string.Format("The '{0}' aggregate type is not supported.", (object) aggregate.GetType().ToString()));
      }
    }
  }
}
