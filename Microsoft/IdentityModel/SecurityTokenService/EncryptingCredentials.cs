// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class EncryptingCredentials
  {
    private string _algorithm;
    private SecurityKey _key;
    private SecurityKeyIdentifier _keyIdentifier;

    public EncryptingCredentials()
    {
    }

    public EncryptingCredentials(
      SecurityKey key,
      SecurityKeyIdentifier keyIdentifier,
      string algorithm)
    {
      if (key == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (key));
      if (keyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifier));
      this._algorithm = !string.IsNullOrEmpty(algorithm) ? algorithm : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (algorithm));
      this._key = key;
      this._keyIdentifier = keyIdentifier;
    }

    public string Algorithm
    {
      get => this._algorithm;
      set => this._algorithm = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public SecurityKey SecurityKey
    {
      get => this._key;
      set => this._key = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public SecurityKeyIdentifier SecurityKeyIdentifier
    {
      get => this._keyIdentifier;
      set => this._keyIdentifier = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
