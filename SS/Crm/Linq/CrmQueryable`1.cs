// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.CrmQueryable`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System.Linq;
using System.Linq.Expressions;

namespace SS.Crm.Linq
{
  public class CrmQueryable<T> : QueryableBase<T>, ICrmQueryable
  {
    private string _entityLogicalName;

    public CrmQueryable(
      IOrganizationService service,
      string entityLogicalName = null,
      ColumnSet columns = null,
      bool retrieveAllRecords = false)
      : base((IQueryParser) QueryParser.CreateDefault(), CrmQueryable<T>.CreateExecutor(service, entityLogicalName, columns, retrieveAllRecords))
    {
      this._entityLogicalName = entityLogicalName;
      if (!string.IsNullOrEmpty(this._entityLogicalName))
        return;
      this._entityLogicalName = typeof (T).GetEntityLogicalName();
    }

    public CrmQueryable(IQueryProvider provider, Expression expression)
      : base(provider, expression)
    {
    }

    private static IQueryExecutor CreateExecutor(
      IOrganizationService service,
      string entityLogicalName = null,
      ColumnSet columns = null,
      bool retrieveAllRecords = false)
    {
      return (IQueryExecutor) new CrmQueryExecutor(service, entityLogicalName, columns, retrieveAllRecords);
    }

    public string EntityLogicalName => this._entityLogicalName;
  }
}
