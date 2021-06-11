// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.CrmQueryFactory
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SS.Crm.Linq
{
  public class CrmQueryFactory
  {
    public static CrmQueryable<T> Queryable<T>(
      IOrganizationService service,
      ColumnSet columns = null,
      bool retrieveAllRecords = false)
    {
      return new CrmQueryable<T>(service, columns: columns, retrieveAllRecords: retrieveAllRecords);
    }

    public static CrmQueryable<Entity> Queryable(
      IOrganizationService service,
      string entityLogicalName,
      ColumnSet columns = null,
      bool retrieveAllRecords = false)
    {
      return new CrmQueryable<Entity>(service, entityLogicalName, columns, retrieveAllRecords);
    }
  }
}
