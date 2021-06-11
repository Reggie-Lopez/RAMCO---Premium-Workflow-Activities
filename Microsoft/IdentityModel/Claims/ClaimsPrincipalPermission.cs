// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsPrincipalPermission
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public sealed class ClaimsPrincipalPermission : 
    IPermission,
    ISecurityEncodable,
    IUnrestrictedPermission
  {
    private List<ClaimsPrincipalPermission.ResourceAction> _resourceActions = new List<ClaimsPrincipalPermission.ResourceAction>();

    public static void CheckAccess(string resource, string action) => new ClaimsPrincipalPermission(resource, action).Demand();

    public ClaimsPrincipalPermission(string resource, string action) => this._resourceActions.Add(new ClaimsPrincipalPermission.ResourceAction(resource, action));

    private ClaimsPrincipalPermission(
      IEnumerable<ClaimsPrincipalPermission.ResourceAction> resourceActions)
    {
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in resourceActions)
        this._resourceActions.Add(new ClaimsPrincipalPermission.ResourceAction(resourceAction.Resource, resourceAction.Action));
    }

    private void ThrowSecurityException()
    {
      AssemblyName assemblyName = (AssemblyName) null;
      Evidence evidence = (Evidence) null;
      new PermissionSet(PermissionState.Unrestricted).Assert();
      try
      {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        assemblyName = callingAssembly.GetName();
        if ((object) callingAssembly != (object) Assembly.GetExecutingAssembly())
          evidence = callingAssembly.Evidence;
      }
      catch
      {
      }
      PermissionSet.RevertAssert();
      throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new SecurityException("ID4266", assemblyName, (PermissionSet) null, (PermissionSet) null, (MethodInfo) null, SecurityAction.Demand, (object) this, (IPermission) this, evidence), TraceEventType.Error);
    }

    public IPermission Copy() => (IPermission) new ClaimsPrincipalPermission((IEnumerable<ClaimsPrincipalPermission.ResourceAction>) this._resourceActions);

    public void Demand()
    {
      ClaimsAuthorizationManager authorizationManager = ServiceConfiguration.GetCurrent().ClaimsAuthorizationManager;
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in this._resourceActions)
      {
        if (!(Thread.CurrentPrincipal is IClaimsPrincipal currentPrincipal))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4284"));
        AuthorizationContext context = new AuthorizationContext(currentPrincipal, resourceAction.Resource, resourceAction.Action);
        if (!authorizationManager.CheckAccess(context))
          this.ThrowSecurityException();
      }
    }

    public IPermission Intersect(IPermission target)
    {
      if (target == null)
        return (IPermission) null;
      if (!(target is ClaimsPrincipalPermission principalPermission))
        return (IPermission) null;
      List<ClaimsPrincipalPermission.ResourceAction> resourceActionList = new List<ClaimsPrincipalPermission.ResourceAction>();
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in principalPermission._resourceActions)
      {
        if (this._resourceActions.Contains(resourceAction))
          resourceActionList.Add(resourceAction);
      }
      return (IPermission) new ClaimsPrincipalPermission((IEnumerable<ClaimsPrincipalPermission.ResourceAction>) resourceActionList);
    }

    public bool IsSubsetOf(IPermission target)
    {
      if (target == null || !(target is ClaimsPrincipalPermission principalPermission))
        return false;
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in this._resourceActions)
      {
        if (!principalPermission._resourceActions.Contains(resourceAction))
          return false;
      }
      return true;
    }

    public IPermission Union(IPermission target)
    {
      if (target == null)
        return (IPermission) null;
      if (!(target is ClaimsPrincipalPermission principalPermission))
        return (IPermission) null;
      List<ClaimsPrincipalPermission.ResourceAction> resourceActionList = new List<ClaimsPrincipalPermission.ResourceAction>();
      resourceActionList.AddRange((IEnumerable<ClaimsPrincipalPermission.ResourceAction>) principalPermission._resourceActions);
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in this._resourceActions)
      {
        if (!resourceActionList.Contains(resourceAction))
          resourceActionList.Add(resourceAction);
      }
      return (IPermission) new ClaimsPrincipalPermission((IEnumerable<ClaimsPrincipalPermission.ResourceAction>) resourceActionList);
    }

    public void FromXml(SecurityElement e) => throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new NotImplementedException(), TraceEventType.Error);

    public SecurityElement ToXml()
    {
      SecurityElement securityElement = new SecurityElement("IPermission");
      Type type = this.GetType();
      StringBuilder stringBuilder = new StringBuilder(type.Assembly.ToString());
      stringBuilder.Replace('"', '\'');
      securityElement.AddAttribute("class", type.FullName + ", " + (object) stringBuilder);
      securityElement.AddAttribute("version", "1");
      foreach (ClaimsPrincipalPermission.ResourceAction resourceAction in this._resourceActions)
      {
        SecurityElement child = new SecurityElement("ResourceAction");
        child.AddAttribute("resource", resourceAction.Resource);
        child.AddAttribute("action", resourceAction.Action);
        securityElement.AddChild(child);
      }
      return securityElement;
    }

    public bool IsUnrestricted() => true;

    private class ResourceAction
    {
      public string Action;
      public string Resource;

      public ResourceAction(string resource, string action)
      {
        this.Resource = !string.IsNullOrEmpty(resource) ? resource : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (resource));
        this.Action = action;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is ClaimsPrincipalPermission.ResourceAction resourceAction))
          return base.Equals(obj);
        return string.CompareOrdinal(resourceAction.Resource, this.Resource) == 0 && string.CompareOrdinal(resourceAction.Action, this.Action) == 0;
      }

      public override int GetHashCode() => this.Resource.GetHashCode() ^ this.Action.GetHashCode();
    }
  }
}
