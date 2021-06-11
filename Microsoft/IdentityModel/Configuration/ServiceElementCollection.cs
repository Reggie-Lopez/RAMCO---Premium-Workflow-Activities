// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ServiceElementCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ConfigurationCollection(typeof (ServiceElement), AddItemName = "service", CollectionType = ConfigurationElementCollectionType.BasicMap)]
  [ComVisible(true)]
  public class ServiceElementCollection : ConfigurationElementCollection
  {
    protected override bool ThrowOnDuplicate => false;

    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new ServiceElement();

    protected override object GetElementKey(ConfigurationElement element)
    {
      if (element == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      return element is ServiceElement serviceElement ? (object) serviceElement.Name : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7013"));
    }

    public ServiceElement GetElement(string name)
    {
      ServiceElement serviceElement = name != null ? this.BaseGet((object) name) as ServiceElement : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));
      if (!StringComparer.Ordinal.Equals(name, "") && serviceElement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7012", (object) name));
      return serviceElement;
    }

    public bool IsConfigured => this.Count > 0;
  }
}
