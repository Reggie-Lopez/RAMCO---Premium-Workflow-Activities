// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.PseudonymRequestMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class PseudonymRequestMessage : WSFederationMessage
  {
    public PseudonymRequestMessage(Uri baseUrl)
      : base(baseUrl, "wpseudo1.0")
    {
    }

    public string Pseudonym
    {
      get => this.GetParameter("wpseudo");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wpseudo");
        else
          this.SetParameter("wpseudo", value);
      }
    }

    public string PseudonymPtr
    {
      get => this.GetParameter("wpseudoptr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wpseudoptr");
        else
          this.SetUriParameter("wpseudoptr", value);
      }
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

    protected override void Validate()
    {
    }

    public override void Write(TextWriter writer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      this.Validate();
      writer.Write(this.WriteQueryString());
    }
  }
}
