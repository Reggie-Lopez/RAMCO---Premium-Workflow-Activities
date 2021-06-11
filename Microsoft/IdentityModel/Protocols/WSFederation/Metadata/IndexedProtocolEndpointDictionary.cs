// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.IndexedProtocolEndpointDictionary
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class IndexedProtocolEndpointDictionary : SortedList<int, IndexedProtocolEndpoint>
  {
    public IndexedProtocolEndpoint Default
    {
      get
      {
        IndexedProtocolEndpoint protocolEndpoint = (IndexedProtocolEndpoint) null;
        foreach (KeyValuePair<int, IndexedProtocolEndpoint> keyValuePair in (SortedList<int, IndexedProtocolEndpoint>) this)
        {
          bool? isDefault = keyValuePair.Value.IsDefault;
          if ((!isDefault.GetValueOrDefault() ? 0 : (isDefault.HasValue ? 1 : 0)) != 0)
            return keyValuePair.Value;
          if (!keyValuePair.Value.IsDefault.HasValue && protocolEndpoint == null)
            protocolEndpoint = keyValuePair.Value;
        }
        if (protocolEndpoint != null)
          return protocolEndpoint;
        return this.Count > 0 ? this[this.Keys[0]] : (IndexedProtocolEndpoint) null;
      }
    }
  }
}
