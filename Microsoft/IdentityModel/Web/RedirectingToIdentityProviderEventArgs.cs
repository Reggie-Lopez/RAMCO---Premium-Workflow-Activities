// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.RedirectingToIdentityProviderEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSFederation;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class RedirectingToIdentityProviderEventArgs : CancelEventArgs
  {
    private SignInRequestMessage _signInRequestMessage;

    public RedirectingToIdentityProviderEventArgs(SignInRequestMessage signInRequestMessage) => this._signInRequestMessage = signInRequestMessage != null ? signInRequestMessage : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signInRequestMessage));

    public SignInRequestMessage SignInRequestMessage
    {
      get => this._signInRequestMessage;
      set => this._signInRequestMessage = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
