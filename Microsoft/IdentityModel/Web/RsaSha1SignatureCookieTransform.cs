// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.RsaSha1SignatureCookieTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class RsaSha1SignatureCookieTransform : RsaSignatureCookieTransform
  {
    public RsaSha1SignatureCookieTransform(RSA key)
      : base(key)
      => this.HashName = "SHA1";

    public RsaSha1SignatureCookieTransform(X509Certificate2 certificate)
      : base(certificate)
      => this.HashName = "SHA1";
  }
}
