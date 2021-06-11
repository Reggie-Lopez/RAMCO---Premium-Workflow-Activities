// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.Helpers.RetrieveHelper
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace SS.Crm.Linq.Helpers
{
  internal class RetrieveHelper
  {
    internal static EntityCollection RetrieveEntities(
      IOrganizationService service,
      QueryExpression query,
      bool retrieveAllEntities = false)
    {
      PagingInfo pageInfo1 = query.get_PageInfo();
      if ((pageInfo1 != null ? ((uint) pageInfo1.get_Count() > 0U ? 1 : 0) : 1) != 0)
        retrieveAllEntities = false;
      EntityCollection entityCollection = service.RetrieveMultiple((QueryBase) query);
      while (entityCollection.get_MoreRecords() & retrieveAllEntities)
      {
        IList<Entity> entities = (IList<Entity>) entityCollection.get_Entities();
        query.get_PageInfo().set_PagingCookie(entityCollection.get_PagingCookie());
        if (query.get_PageInfo().get_PageNumber() == 0)
        {
          PagingInfo pageInfo2 = query.get_PageInfo();
          pageInfo2.set_PageNumber(pageInfo2.get_PageNumber() + 1);
        }
        PagingInfo pageInfo3 = query.get_PageInfo();
        pageInfo3.set_PageNumber(pageInfo3.get_PageNumber() + 1);
        entityCollection = service.RetrieveMultiple((QueryBase) query);
        entityCollection.get_Entities().AddRange((IEnumerable<Entity>) entities);
      }
      return entityCollection;
    }
  }
}
