// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.SignOutRequestMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class SignOutRequestMessage : WSFederationMessage
  {
    public SignOutRequestMessage(Uri baseUrl)
      : base(baseUrl, "wsignout1.0")
    {
    }

    public SignOutRequestMessage(Uri baseUrl, string reply)
      : base(baseUrl, "wsignout1.0")
      => this.SetUriParameter("wreply", reply);

    protected override void Validate()
    {
      base.Validate();
      string parameter = this.GetParameter("wa");
      if (string.IsNullOrEmpty(parameter) || !parameter.Equals("wsignout1.0"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3000", (object) parameter)));
    }

    public override void Write(TextWriter writer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      this.Validate();
      writer.Write(this.WriteQueryString());
    }

    public string Reply
    {
      get => this.GetParameter("wreply");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wreply");
        else
          this.SetUriParameter("wreply", value);
      }
    }
  }
}
