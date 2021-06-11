// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.ExpressionTreeVisitors.SubQueryModelVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SS.Crm.Linq.ExpressionTreeVisitors
{
  internal class SubQueryModelVisitor : QueryModelVisitorBase
  {
    private ConditionExpression _condition;
    private List<object> _values;
    private EntityTypeAlias _entityTypeAlias;
    private IOrganizationService _service;
    private Dictionary<string, string> _entityTypeAliases;

    public EntityTypeAlias EntityTypeAlias => this._entityTypeAlias;

    public ConditionExpression Condition => this._condition;

    public SubQueryModelVisitor(
      IOrganizationService service,
      Dictionary<string, string> entityTypeAliases)
    {
      this._service = service;
      this._entityTypeAliases = entityTypeAliases;
    }

    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      if (fromClause.FromExpression is ConstantExpression)
      {
        ConstantExpression fromExpression = fromClause.FromExpression as ConstantExpression;
        if (fromExpression.Value is IEnumerable)
        {
          this._values = new List<object>();
          foreach (object obj in (IEnumerable) fromExpression.Value)
          {
            if (obj.GetType().IsEnum)
              this._values.Add((object) (int) obj);
            else
              this._values.Add(obj);
          }
        }
      }
      base.VisitMainFromClause(fromClause, queryModel);
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      base.VisitResultOperator(resultOperator, queryModel, index);
      if (resultOperator is ContainsResultOperator && this._values != null)
      {
        ContainsResultOperator containsResultOperator = resultOperator as ContainsResultOperator;
        if (containsResultOperator.Item != null && containsResultOperator.Item is MemberExpression)
        {
          MemberExpression expression = containsResultOperator.Item as MemberExpression;
          if (((IEnumerable<object>) expression.Member.GetCustomAttributes(typeof (AttributeLogicalNameAttribute), false)).FirstOrDefault<object>() is AttributeLogicalNameAttribute logicalNameAttribute)
          {
            ConditionExpression conditionExpression = new ConditionExpression();
            conditionExpression.set_AttributeName(logicalNameAttribute.get_LogicalName());
            this._condition = conditionExpression;
            this._condition.set_Operator((ConditionOperator) 8);
            this._condition.get_Values().AddRange(this._values.ToArray());
            this._entityTypeAlias = new EntityTypeAlias()
            {
              Alias = expression.GetAliasedEntityName()
            };
          }
          else if (expression.Expression.Type == typeof (EntityReference) && expression.Expression is MemberExpression)
          {
            AttributeLogicalNameAttribute logicalNameAttribute = ((IEnumerable<object>) ((MemberExpression) expression.Expression).Member.GetCustomAttributes(typeof (AttributeLogicalNameAttribute), false)).FirstOrDefault<object>() as AttributeLogicalNameAttribute;
            ConditionExpression conditionExpression = new ConditionExpression();
            conditionExpression.set_AttributeName(logicalNameAttribute.get_LogicalName());
            this._condition = conditionExpression;
            this._condition.set_Operator((ConditionOperator) 8);
            this._condition.get_Values().AddRange(this._values.ToArray());
            this._entityTypeAlias = new EntityTypeAlias()
            {
              Alias = expression.GetAliasedEntityName()
            };
          }
        }
        else if (containsResultOperator.Item != null && containsResultOperator.Item is UnaryExpression)
        {
          UnaryExpression expression = containsResultOperator.Item as UnaryExpression;
          string attributeLogicalName = expression.GetAttributeLogicalName(this._service, this._entityTypeAliases);
          ConditionExpression conditionExpression = new ConditionExpression();
          conditionExpression.set_AttributeName(attributeLogicalName);
          this._condition = conditionExpression;
          this._condition.set_Operator((ConditionOperator) 8);
          this._condition.get_Values().AddRange(this._values.ToArray());
          this._entityTypeAlias = new EntityTypeAlias()
          {
            Alias = expression.Operand.GetAliasedEntityName()
          };
        }
      }
      if (this._condition == null)
        throw new NotImplementedException(string.Format("The CRM Linq query provider cannot handle this expression: {0}", (object) resultOperator.ToString()));
    }
  }
}
