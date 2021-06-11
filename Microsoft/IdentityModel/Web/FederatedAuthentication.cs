// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.FederatedAuthentication
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Web.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public static class FederatedAuthentication
  {
    public const int DefaultMaxArrayLength = 2097152;
    public const int DefaultMaxStringContentLength = 2097152;
    internal static readonly string ModulesKey = typeof (FederatedAuthentication).AssemblyQualifiedName;
    [ThreadStatic]
    internal static IDictionary _currentItemsOverride;
    internal static ServiceConfiguration _serviceConfiguration;
    internal static object _serviceConfigurationLock = new object();

    public static ClaimsAuthorizationModule ClaimsAuthorizationModule => FederatedAuthentication.GetHttpModule<ClaimsAuthorizationModule>();

    public static ClaimsPrincipalHttpModule ClaimsPrincipalHttpModule => FederatedAuthentication.GetHttpModule<ClaimsPrincipalHttpModule>();

    public static ServiceConfiguration ServiceConfiguration
    {
      get
      {
        lock (FederatedAuthentication._serviceConfigurationLock)
        {
          if (FederatedAuthentication._serviceConfiguration == null)
          {
            FederatedAuthentication._serviceConfiguration = new ServiceConfiguration();
            ServiceConfigurationCreatedEventArgs e = new ServiceConfigurationCreatedEventArgs(FederatedAuthentication._serviceConfiguration);
            EventHandler<ServiceConfigurationCreatedEventArgs> configurationCreated = FederatedAuthentication.ServiceConfigurationCreated;
            if (configurationCreated != null)
              configurationCreated((object) null, e);
            FederatedAuthentication._serviceConfiguration = e.ServiceConfiguration;
            if (!FederatedAuthentication._serviceConfiguration.IsInitialized)
              FederatedAuthentication._serviceConfiguration.Initialize();
          }
          return FederatedAuthentication._serviceConfiguration;
        }
      }
    }

    public static SessionAuthenticationModule SessionAuthenticationModule => FederatedAuthentication.GetHttpModule<SessionAuthenticationModule>();

    public static WSFederationAuthenticationModule WSFederationAuthenticationModule => FederatedAuthentication.GetHttpModule<WSFederationAuthenticationModule>();

    public static event EventHandler<ServiceConfigurationCreatedEventArgs> ServiceConfigurationCreated;

    private static IDictionary GetCurrentContextItems()
    {
      if (FederatedAuthentication._currentItemsOverride != null)
        return FederatedAuthentication._currentItemsOverride;
      return HttpContext.Current != null ? HttpContext.Current.Items : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1061"));
    }

    private static T GetHttpModule<T>() where T : class, IHttpModule
    {
      Dictionary<Type, IHttpModule> contextModuleMap = FederatedAuthentication.GetHttpContextModuleMap();
      IHttpModule httpModule = (IHttpModule) null;
      if (!contextModuleMap.TryGetValue(typeof (T), out httpModule))
      {
        httpModule = (IHttpModule) FederatedAuthentication.GetHttpContextModule<T>();
        contextModuleMap.Add(typeof (T), httpModule);
      }
      return (T) httpModule;
    }

    private static Dictionary<Type, IHttpModule> GetHttpContextModuleMap()
    {
      IDictionary currentContextItems = FederatedAuthentication.GetCurrentContextItems();
      if (!(currentContextItems[(object) FederatedAuthentication.ModulesKey] is Dictionary<Type, IHttpModule> dictionary))
      {
        dictionary = new Dictionary<Type, IHttpModule>();
        currentContextItems[(object) FederatedAuthentication.ModulesKey] = (object) dictionary;
      }
      return dictionary;
    }

    private static T GetHttpContextModule<T>() where T : class, IHttpModule
    {
      obj = default (T);
      HttpModuleCollection modules = HttpContext.Current.ApplicationInstance.Modules;
      int index = 0;
      while (index < modules.Count && !(modules[index] is T obj))
        ++index;
      return obj;
    }
  }
}
