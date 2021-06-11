// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.ExpressionTreeVisitors.ExpressionTreeVisitor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace SS.Crm.Linq.ExpressionTreeVisitors
{
  internal class ExpressionTreeVisitor : ThrowingExpressionVisitor
  {
    private object _value;
    private string _attributeLogicalName = string.Empty;
    private ConditionExpression _condition;
    private EntityTypeAlias _entityTypeAlias;
    private FilterExpression _filter;
    private Dictionary<string, string> _entityTypeAliases;
    private IOrganizationService _service;
    private static List<ExpressionType> _validEvaluateExpressionTypes = new List<ExpressionType>()
    {
      ExpressionType.Equal,
      ExpressionType.NotEqual,
      ExpressionType.GreaterThan,
      ExpressionType.GreaterThanOrEqual,
      ExpressionType.LessThan,
      ExpressionType.LessThanOrEqual,
      ExpressionType.Not
    };
    private static List<ExpressionType> _validPostponeExpressionTypes = new List<ExpressionType>()
    {
      ExpressionType.Add,
      ExpressionType.Subtract,
      ExpressionType.Multiply,
      ExpressionType.Divide
    };
    private List<string> _validMethodNames = new List<string>()
    {
      "Contains",
      "StartsWith",
      "EndsWith",
      "Equals",
      "IsNullOrEmpty"
    };

    public object Value => this._value;

    public string AttributeLogicalName => this._attributeLogicalName;

    public EntityTypeAlias EntityTypeAlias => this._entityTypeAlias;

    public ConditionExpression Condition => this._condition;

    public FilterExpression Filter => this._filter;

    public ExpressionTreeVisitor(
      IOrganizationService service,
      Dictionary<string, string> entityTypeAliases)
    {
      this._service = service;
      this._entityTypeAliases = entityTypeAliases;
    }

    protected override Expression VisitConstant(ConstantExpression expression)
    {
      this._value = expression.Value;
      return (Expression) expression;
    }

    protected override Expression VisitMember(MemberExpression expression)
    {
      this._entityTypeAlias = new EntityTypeAlias()
      {
        Type = expression.Member.DeclaringType,
        Alias = expression.GetAliasedEntityName()
      };
      this._attributeLogicalName = expression.GetAttributeLogicalName(this._service, this._entityTypeAliases);
      return (Expression) expression;
    }

    protected override Expression VisitBinary(BinaryExpression expression)
    {
      ExpressionTreeVisitor wherePartVisitor1 = new ExpressionTreeVisitor(this._service, this._entityTypeAliases);
      wherePartVisitor1.Visit(expression.Left);
      ExpressionTreeVisitor wherePartVisitor2 = new ExpressionTreeVisitor(this._service, this._entityTypeAliases);
      wherePartVisitor2.Visit(expression.Right);
      if (expression.NodeType == ExpressionType.AndAlso || expression.NodeType == ExpressionType.OrElse)
      {
        this._filter = new FilterExpression(expression.NodeType == ExpressionType.AndAlso ? (LogicalOperator) 0 : (LogicalOperator) 1);
        if (wherePartVisitor1.Condition != null)
        {
          this._entityTypeAlias = wherePartVisitor1.EntityTypeAlias;
          this._filter.AddCondition(wherePartVisitor1.Condition);
        }
        else if (wherePartVisitor1.Filter != null)
        {
          this._entityTypeAlias = wherePartVisitor1.EntityTypeAlias;
          ((Collection<FilterExpression>) this._filter.get_Filters()).Add(wherePartVisitor1.Filter);
        }
        if (wherePartVisitor2.Condition != null)
        {
          this._entityTypeAlias = wherePartVisitor2.EntityTypeAlias;
          this._filter.AddCondition(wherePartVisitor2.Condition);
        }
        else if (wherePartVisitor2.Filter != null)
        {
          this._entityTypeAlias = wherePartVisitor2.EntityTypeAlias;
          ((Collection<FilterExpression>) this._filter.get_Filters()).Add(wherePartVisitor2.Filter);
        }
      }
      else
      {
        if (!ExpressionTreeVisitor._validEvaluateExpressionTypes.Contains(expression.NodeType))
          throw new NotImplementedException(string.Format("The expression operator {0} is not supported.", (object) expression.NodeType.ToString()));
        this._condition = new ConditionExpression();
        this.ParsePartExpression(wherePartVisitor1, this._condition);
        this.ParsePartExpression(wherePartVisitor2, this._condition);
        switch (expression.NodeType)
        {
          case ExpressionType.Equal:
            if (((IEnumerable<object>) this._condition.get_Values()).Count<object>() == 0 || ((Collection<object>) this._condition.get_Values())[0] == null)
            {
              this._condition.set_Operator((ConditionOperator) 12);
              ((Collection<object>) this._condition.get_Values()).Clear();
              break;
            }
            this._condition.set_Operator((ConditionOperator) 0);
            break;
          case ExpressionType.GreaterThan:
            this._condition.set_Operator((ConditionOperator) 2);
            break;
          case ExpressionType.GreaterThanOrEqual:
            this._condition.set_Operator((ConditionOperator) 4);
            break;
          case ExpressionType.LessThan:
            this._condition.set_Operator((ConditionOperator) 3);
            break;
          case ExpressionType.LessThanOrEqual:
            this._condition.set_Operator((ConditionOperator) 5);
            break;
          case ExpressionType.Not:
            this._condition.set_Operator((ConditionOperator) 13);
            ((Collection<object>) this._condition.get_Values()).Clear();
            break;
          case ExpressionType.NotEqual:
            if (((IEnumerable<object>) this._condition.get_Values()).Count<object>() == 0 || ((Collection<object>) this._condition.get_Values())[0] == null)
            {
              this._condition.set_Operator((ConditionOperator) 13);
              ((Collection<object>) this._condition.get_Values()).Clear();
              break;
            }
            this._condition.set_Operator((ConditionOperator) 1);
            break;
        }
      }
      return (Expression) expression;
    }

    private void ParsePartExpression(
      ExpressionTreeVisitor wherePartVisitor,
      ConditionExpression condition)
    {
      if (!string.IsNullOrEmpty(wherePartVisitor.AttributeLogicalName))
      {
        condition.set_AttributeName(wherePartVisitor.AttributeLogicalName);
        this._entityTypeAlias = wherePartVisitor.EntityTypeAlias;
      }
      else
      {
        if (wherePartVisitor.Value == null)
          return;
        ((Collection<object>) condition.get_Values()).Add(wherePartVisitor.Value);
      }
    }

    protected internal override Expression VisitSubQuery(SubQueryExpression expression)
    {
      SubQueryModelVisitor queryModelVisitor = new SubQueryModelVisitor(this._service, this._entityTypeAliases);
      queryModelVisitor.VisitQueryModel(expression.QueryModel);
      if (queryModelVisitor.Condition != null)
      {
        this._condition = queryModelVisitor.Condition;
        this._entityTypeAlias = queryModelVisitor.EntityTypeAlias;
      }
      return (Expression) expression;
    }

    protected override Expression VisitMethodCall(MethodCallExpression expression)
    {
      if (this._validMethodNames.Contains(expression.Method.Name))
      {
        if (expression.Arguments[0] is ConstantExpression)
        {
          if (expression.Arguments[0] is ConstantExpression constantExpression)
          {
            object obj = constantExpression.Value;
            string str = string.Empty;
            if (expression.Object is MemberExpression)
            {
              MemberExpression member = expression.Object as MemberExpression;
              str = member.Member.GetAttributeLogicalName(this._service, member.GetEntityLogicalName(this._entityTypeAliases));
              this._entityTypeAlias = new EntityTypeAlias()
              {
                Type = ((MemberExpression) expression.Object).Member.DeclaringType,
                Alias = expression.GetAliasedEntityName()
              };
            }
            else if (expression.Object is UnaryExpression)
            {
              str = ((UnaryExpression) expression.Object).Operand.GetAttributeLogicalName(this._service, this._entityTypeAliases);
              this._entityTypeAlias = new EntityTypeAlias()
              {
                Type = ((UnaryExpression) expression.Object).Operand.Type,
                Alias = expression.GetAliasedEntityName()
              };
            }
            if (obj == null)
              throw new NotImplementedException(string.Format("The method call argument must be a constant expression."));
            string name = expression.Method.Name;
            if (!(name == "Contains"))
            {
              if (!(name == "StartsWith"))
              {
                if (!(name == "EndsWith"))
                {
                  if (!(name == "Equals"))
                  {
                    if (name == "IsNullOrEmpty")
                    {
                      this._filter = new FilterExpression((LogicalOperator) 0);
                      this._filter.AddCondition(str, (ConditionOperator) 13, new object[0]);
                      this._filter.AddCondition(str, (ConditionOperator) 1, new object[1]
                      {
                        (object) string.Empty
                      });
                    }
                  }
                  else
                    this._condition = new ConditionExpression(str, (ConditionOperator) 0, obj);
                }
                else
                  this._condition = new ConditionExpression(str, (ConditionOperator) 6, (object) string.Format("%{0}", obj));
              }
              else
                this._condition = new ConditionExpression(str, (ConditionOperator) 6, (object) string.Format("{0}%", obj));
            }
            else
              this._condition = new ConditionExpression(str, (ConditionOperator) 6, (object) string.Format("%{0}%", obj));
          }
        }
        else if (expression.Arguments[0] is MemberExpression)
        {
          MemberExpression expression1 = expression.Arguments[0] as MemberExpression;
          string attributeLogicalName = expression1.GetAttributeLogicalName(this._service, this._entityTypeAliases);
          this._entityTypeAlias = new EntityTypeAlias()
          {
            Type = expression1.Member.DeclaringType,
            Alias = expression.GetAliasedEntityName()
          };
          if (expression.Method.Name == "IsNullOrEmpty")
          {
            this._filter = new FilterExpression((LogicalOperator) 1);
            this._filter.AddCondition(attributeLogicalName, (ConditionOperator) 12, new object[0]);
            this._filter.AddCondition(attributeLogicalName, (ConditionOperator) 0, new object[1]
            {
              (object) string.Empty
            });
          }
        }
      }
      else
      {
        if (expression == null || expression.Object == null || !(expression.Object.Type == typeof (AttributeCollection)))
          throw new NotImplementedException(string.Format("The method call is not implemented: {0}", (object) expression.Method.Name));
        this._attributeLogicalName = (string) ((ConstantExpression) expression.Arguments[0]).Value;
        this._entityTypeAlias = new EntityTypeAlias()
        {
          Alias = expression.GetAliasedEntityName()
        };
      }
      return (Expression) expression;
    }

    protected override Expression VisitUnary(UnaryExpression expression)
    {
      if (expression.Operand is MethodCallExpression && ((MethodCallExpression) expression.Operand).Method.Name == "IsNullOrEmpty")
      {
        MethodCallExpression operand = expression.Operand as MethodCallExpression;
        MemberExpression expression1 = operand.Arguments[0] as MemberExpression;
        string attributeLogicalName = expression1.GetAttributeLogicalName(this._service, this._entityTypeAliases);
        this._entityTypeAlias = new EntityTypeAlias()
        {
          Type = expression1.Member.DeclaringType,
          Alias = expression.GetAliasedEntityName()
        };
        if (operand.Method.Name == "IsNullOrEmpty")
        {
          this._filter = new FilterExpression((LogicalOperator) 0);
          this._filter.AddCondition(attributeLogicalName, (ConditionOperator) 13, new object[0]);
          this._filter.AddCondition(attributeLogicalName, (ConditionOperator) 1, new object[1]
          {
            (object) string.Empty
          });
        }
      }
      else if (expression.Operand is MemberExpression)
      {
        this._attributeLogicalName = expression.Operand.GetAttributeLogicalName(this._service, this._entityTypeAliases);
        this._entityTypeAlias = new EntityTypeAlias()
        {
          Alias = expression.Operand.GetAliasedEntityName()
        };
      }
      else
      {
        if (!(expression.Operand is MethodCallExpression) || ((MethodCallExpression) expression.Operand).Object == null || !(((MethodCallExpression) expression.Operand).Object.Type == typeof (AttributeCollection)))
          throw new NotSupportedException(string.Format("The following method is not supported: {0}", (object) expression.Operand.ToString()));
        this._attributeLogicalName = (string) ((ConstantExpression) (expression.Operand as MethodCallExpression).Arguments[0]).Value;
        this._entityTypeAlias = new EntityTypeAlias()
        {
          Alias = expression.Operand.GetAliasedEntityName()
        };
      }
      return (Expression) expression;
    }

    protected override Exception CreateUnhandledItemException<T>(
      T unhandledItem,
      string visitMethod)
    {
      throw new NotImplementedException(string.Format("The CRM linq query provider has not implemented the {0} type method.", (object) visitMethod));
    }
  }
}
