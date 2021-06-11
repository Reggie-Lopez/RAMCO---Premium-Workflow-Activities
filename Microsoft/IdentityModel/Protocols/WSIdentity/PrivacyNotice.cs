// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.PrivacyNotice
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class PrivacyNotice
  {
    private long _version;
    private string _location;

    public PrivacyNotice(string privacyNoticeLocation)
      : this(privacyNoticeLocation, 1L)
    {
    }

    public PrivacyNotice(string privacyNoticeLocation, long version)
    {
      if (string.IsNullOrEmpty(privacyNoticeLocation))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (privacyNoticeLocation));
      if (version < 1L || version > (long) uint.MaxValue)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (version), SR.GetString("ID2028", (object) (long) uint.MaxValue));
      this._location = privacyNoticeLocation;
      this._version = version;
    }

    public string Location => this._location;

    public long Version => this._version;
  }
}
