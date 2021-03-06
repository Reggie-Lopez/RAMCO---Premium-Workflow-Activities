// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Configuration.ChunkedCookieHandlerElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
  [ComVisible(true)]
  public class ChunkedCookieHandlerElement : ConfigurationElement
  {
    [ConfigurationProperty("chunkSize", DefaultValue = 2000, IsRequired = false)]
    public int ChunkSize
    {
      get => (int) this["chunkSize"];
      set => this["chunkSize"] = (object) value;
    }

    public bool IsConfigured => this.ChunkSize != 2000;
  }
}
