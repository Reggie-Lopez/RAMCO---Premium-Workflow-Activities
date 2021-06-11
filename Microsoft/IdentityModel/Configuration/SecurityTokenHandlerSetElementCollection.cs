// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.SecurityTokenHandlerSetElementCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ConfigurationCollection(typeof (SecurityTokenHandlerElementCollection), AddItemName = "securityTokenHandlers", CollectionType = ConfigurationElementCollectionType.BasicMap)]
  [ComVisible(true)]
  public class SecurityTokenHandlerSetElementCollection : ConfigurationElementCollection
  {
    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new SecurityTokenHandlerElementCollection();

    protected override object GetElementKey(ConfigurationElement element) => (object) ((SecurityTokenHandlerElementCollection) element).Name;

    public bool IsConfigured => this.Count > 0;
  }
}
