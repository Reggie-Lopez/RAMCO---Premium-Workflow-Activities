// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.PublicExtensionMethods
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Linq;

namespace SS.Crm.Linq
{
  public static class PublicExtensionMethods
  {
    public static CrmQueryable<Entity> Queryable(
      this IOrganizationService service,
      string entityLogicalName,
      params string[] columns)
    {
      return ((IEnumerable<string>) columns).Any<string>() ? service.Queryable(entityLogicalName, new ColumnSet(columns)) : service.Queryable(entityLogicalName, (ColumnSet) null);
    }

    public static CrmQueryable<T> Queryable<T>(
      this IOrganizationService service,
      params string[] columns)
    {
      return ((IEnumerable<string>) columns).Any<string>() ? service.Queryable<T>(new ColumnSet(columns)) : service.Queryable<T>((ColumnSet) null);
    }

    public static CrmQueryable<Entity> Queryable(
      this IOrganizationService service,
      string entityLogicalName,
      ColumnSet columns)
    {
      return CrmQueryFactory.Queryable(service, entityLogicalName, columns);
    }

    public static CrmQueryable<T> Queryable<T>(
      this IOrganizationService service,
      ColumnSet columns)
    {
      return CrmQueryFactory.Queryable<T>(service, columns);
    }
  }
}
