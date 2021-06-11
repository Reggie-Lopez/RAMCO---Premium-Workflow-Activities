// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SessionSecurityTokenReceivedEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class SessionSecurityTokenReceivedEventArgs : CancelEventArgs
  {
    private SessionSecurityToken _sessionToken;
    private bool _reissueCookie;

    public SessionSecurityTokenReceivedEventArgs(SessionSecurityToken sessionToken) => this._sessionToken = sessionToken != null ? sessionToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionToken));

    public SessionSecurityToken SessionToken
    {
      get => this._sessionToken;
      set => this._sessionToken = value;
    }

    public bool ReissueCookie
    {
      get => this._reissueCookie;
      set => this._reissueCookie = value;
    }
  }
}
