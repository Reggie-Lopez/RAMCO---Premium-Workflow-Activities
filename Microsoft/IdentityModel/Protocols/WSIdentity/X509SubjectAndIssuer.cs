// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.X509SubjectAndIssuer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class X509SubjectAndIssuer
  {
    private EkuPolicy _ekuPolicy;
    private string _x509Issuer;
    private string _x509Subject;

    public X509SubjectAndIssuer(string x509Subject, string x509Issuer)
      : this(x509Subject, x509Issuer, (EkuPolicy) null)
    {
    }

    public X509SubjectAndIssuer(string x509Subject, string x509Issuer, EkuPolicy ekuPolicy)
    {
      if (string.IsNullOrEmpty(x509Subject))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (x509Subject));
      this._x509Issuer = !string.IsNullOrEmpty(x509Issuer) ? x509Issuer : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (x509Issuer));
      this._x509Subject = x509Subject;
      this._ekuPolicy = ekuPolicy;
    }

    public EkuPolicy EkuPolicy
    {
      get => this._ekuPolicy;
      set => this._ekuPolicy = value;
    }

    public string X509Issuer
    {
      get => this._x509Issuer;
      set => this._x509Issuer = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public string X509Subject
    {
      get => this._x509Subject;
      set => this._x509Subject = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }
  }
}
