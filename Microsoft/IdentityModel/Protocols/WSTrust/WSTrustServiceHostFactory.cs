// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustServiceHostFactory
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Compilation;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustServiceHostFactory : ServiceHostFactory
  {
    public override ServiceHostBase CreateServiceHost(
      string constructorString,
      Uri[] baseAddresses)
    {
      return (ServiceHostBase) new WSTrustServiceHost((!string.IsNullOrEmpty(constructorString) ? this.CreateSecurityTokenServiceConfiguration(constructorString) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (constructorString))) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3096", (object) constructorString))), baseAddresses);
    }

    protected virtual SecurityTokenServiceConfiguration CreateSecurityTokenServiceConfiguration(
      string constructorString)
    {
      Type type = !string.IsNullOrEmpty(constructorString) ? BuildManager.GetType(constructorString, true) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (constructorString));
      return type.IsSubclassOf(typeof (SecurityTokenServiceConfiguration)) ? Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null) as SecurityTokenServiceConfiguration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3087", (object) type)));
    }
  }
}
