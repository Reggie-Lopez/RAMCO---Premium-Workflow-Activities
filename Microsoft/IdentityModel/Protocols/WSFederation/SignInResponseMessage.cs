// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.SignInResponseMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class SignInResponseMessage : WSFederationMessage
  {
    public SignInResponseMessage(Uri baseUrl, string result)
      : base(baseUrl, "wsignin1.0")
    {
      if (string.IsNullOrEmpty(result))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (result)));
      this.SetParameter("wresult", result);
    }

    public SignInResponseMessage(Uri baseUrl, Uri resultPtr)
      : base(baseUrl, "wsignin1.0")
    {
      if (resultPtr == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (resultPtr));
      this.SetParameter("wresultptr", resultPtr.AbsoluteUri);
    }

    public SignInResponseMessage(
      Uri baseUrl,
      RequestSecurityTokenResponse response,
      WSFederationSerializer federationSerializer,
      WSTrustSerializationContext context)
      : base(baseUrl, "wsignin1.0")
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (federationSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (federationSerializer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this.SetParameter("wresult", federationSerializer.GetResponseAsString(response, context));
      this.Context = response.Context;
    }

    protected override void Validate()
    {
      base.Validate();
      string parameter = this.GetParameter("wa");
      if (parameter != "wsignin1.0")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3000", (object) parameter)));
      bool flag1 = !string.IsNullOrEmpty(this.GetParameter("wresult"));
      bool flag2 = !string.IsNullOrEmpty(this.GetParameter("wresultptr"));
      if (flag1 && flag2)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3016")));
      if (!flag1 && !flag2)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3001")));
    }

    public override void Write(TextWriter writer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      this.Validate();
      writer.Write(this.WriteFormPost());
    }

    public string Result
    {
      get => this.GetParameter("wresult");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wresult");
        else
          this.SetParameter("wresult", value);
      }
    }

    public string ResultPtr
    {
      get => this.GetParameter("wresultptr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wresultptr");
        else
          this.SetUriParameter("wresultptr", value);
      }
    }
  }
}
