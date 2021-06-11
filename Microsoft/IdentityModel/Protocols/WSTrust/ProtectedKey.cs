// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.ProtectedKey
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.SecurityTokenService;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class ProtectedKey
  {
    private byte[] _secret;
    private EncryptingCredentials _wrappingCredentials;

    public ProtectedKey(byte[] secret) => this._secret = secret;

    public ProtectedKey(byte[] secret, EncryptingCredentials wrappingCredentials)
    {
      this._secret = secret;
      this._wrappingCredentials = wrappingCredentials;
    }

    public byte[] GetKeyBytes() => this._secret;

    public EncryptingCredentials WrappingCredentials => this._wrappingCredentials;
  }
}
