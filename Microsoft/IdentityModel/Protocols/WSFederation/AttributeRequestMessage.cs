// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.AttributeRequestMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class AttributeRequestMessage : WSFederationMessage
  {
    public AttributeRequestMessage(Uri baseUrl)
      : base(baseUrl, "wattr1.0")
    {
    }

    public string Attribute
    {
      get => this.GetParameter("wattr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wattr");
        else
          this.SetParameter("wattr", value);
      }
    }

    public string AttributePtr
    {
      get => this.GetParameter("wattrptr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wattrptr");
        else
          this.SetUriParameter("wattrptr", value);
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
