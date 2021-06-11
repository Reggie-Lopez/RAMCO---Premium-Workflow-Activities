// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SecurityTokenReceivedEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class SecurityTokenReceivedEventArgs : CancelEventArgs
  {
    private SecurityToken _securityToken;
    private string _signInContext;

    public SecurityTokenReceivedEventArgs(SecurityToken securityToken)
      : this(securityToken, (string) null)
    {
    }

    public SecurityTokenReceivedEventArgs(SecurityToken securityToken, string signInContext)
      : base(false)
    {
      this._securityToken = securityToken != null ? securityToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityToken));
      this._signInContext = signInContext;
    }

    public SecurityToken SecurityToken
    {
      get => this._securityToken;
      set => this._securityToken = value;
    }

    public string SignInContext => this._signInContext;
  }
}
