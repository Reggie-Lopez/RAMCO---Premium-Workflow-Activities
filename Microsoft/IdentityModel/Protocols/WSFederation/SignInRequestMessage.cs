// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.SignInRequestMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class SignInRequestMessage : WSFederationMessage
  {
    public SignInRequestMessage(Uri baseUrl, string realm)
      : this(baseUrl, realm, (string) null)
    {
    }

    internal SignInRequestMessage(Uri baseUrl)
      : base(baseUrl, "wsignin1.0")
    {
    }

    public SignInRequestMessage(Uri baseUrl, string realm, string reply)
      : base(baseUrl, "wsignin1.0")
    {
      if (string.IsNullOrEmpty(realm) && string.IsNullOrEmpty(reply))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3204")));
      if (!string.IsNullOrEmpty(realm))
        this.SetParameter("wtrealm", realm);
      if (string.IsNullOrEmpty(reply))
        return;
      this.SetParameter("wreply", reply);
    }

    public string RequestUrl
    {
      get
      {
        StringBuilder sb = new StringBuilder(128);
        using (StringWriter stringWriter = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
        {
          this.Write((TextWriter) stringWriter);
          return sb.ToString();
        }
      }
    }

    protected override void Validate()
    {
      base.Validate();
      string parameter1 = this.GetParameter("wa");
      if (parameter1 != "wsignin1.0")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3000", (object) parameter1)));
      string parameter2 = this.GetParameter("wtrealm");
      string parameter3 = this.GetParameter("wreply");
      if (string.IsNullOrEmpty(parameter2) && string.IsNullOrEmpty(parameter3))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3204")));
      string parameter4 = this.GetParameter("wreq");
      string parameter5 = this.GetParameter("wreqptr");
      if (!string.IsNullOrEmpty(parameter4) && !string.IsNullOrEmpty(parameter5))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3142")));
    }

    public override void Write(TextWriter writer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      this.Validate();
      writer.Write(this.WriteQueryString());
    }

    public string Federation
    {
      get => this.GetParameter("wfed");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wfed");
        else
          this.SetParameter("wfed", value);
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

    public string CurrentTime
    {
      get => this.GetParameter("wct");
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this.RemoveParameter("wct");
        }
        else
        {
          if (!DateTime.TryParseExact(value, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime _))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0010", (object) value), nameof (value)));
          this.SetParameter("wct", value);
        }
      }
    }

    public string Freshness
    {
      get => this.GetParameter("wfresh");
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this.RemoveParameter("wfresh");
        }
        else
        {
          int result = -1;
          if (!int.TryParse(value, out result))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0018", (object) result.GetType()), nameof (value)));
          this.SetParameter("wfresh", value);
        }
      }
    }

    public string HomeRealm
    {
      get => this.GetParameter("whr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("whr");
        else
          this.SetParameter("whr", value);
      }
    }

    public string AuthenticationType
    {
      get => this.GetParameter("wauth");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wauth");
        else
          this.SetUriParameter("wauth", value);
      }
    }

    public string Policy
    {
      get => this.GetParameter("wp");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wp");
        else
          this.SetUriParameter("wp", value);
      }
    }

    public string Resource
    {
      get => this.GetParameter("wres");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wres");
        else
          this.SetUriParameter("wres", value);
      }
    }

    public string Realm
    {
      get => this.GetParameter("wtrealm");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wtrealm");
        else
          this.SetUriParameter("wtrealm", value);
      }
    }

    public string Request
    {
      get => this.GetParameter("wreq");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wreq");
        else
          this.SetParameter("wreq", value);
      }
    }

    public string RequestPtr
    {
      get => this.GetParameter("wreqptr");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wreqptr");
        else
          this.SetUriParameter("wreqptr", value);
      }
    }
  }
}
