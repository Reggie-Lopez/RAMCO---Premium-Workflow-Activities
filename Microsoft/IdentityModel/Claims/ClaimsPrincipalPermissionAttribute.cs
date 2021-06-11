// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsPrincipalPermissionAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Claims
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
  [ComVisible(true)]
  public class ClaimsPrincipalPermissionAttribute : CodeAccessSecurityAttribute
  {
    private string _resource;
    private string _operation;

    public ClaimsPrincipalPermissionAttribute(SecurityAction action)
      : base(action)
    {
    }

    public string Operation
    {
      get => this._operation;
      set => this._operation = value;
    }

    public string Resource
    {
      get => this._resource;
      set => this._resource = value;
    }

    public override IPermission CreatePermission() => (IPermission) new ClaimsPrincipalPermission(this._resource, this._operation);
  }
}
