// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.RequestedProofToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.SecurityTokenService;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class RequestedProofToken
  {
    private string _computedKeyAlgorithm;
    private ProtectedKey _keys;

    public RequestedProofToken(string computedKeyAlgorithm)
    {
      if (string.IsNullOrEmpty(computedKeyAlgorithm))
        DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (computedKeyAlgorithm));
      this._computedKeyAlgorithm = computedKeyAlgorithm;
    }

    public RequestedProofToken(byte[] secret) => this._keys = new ProtectedKey(secret);

    public RequestedProofToken(byte[] secret, EncryptingCredentials wrappingCredentials) => this._keys = new ProtectedKey(secret, wrappingCredentials);

    public RequestedProofToken(ProtectedKey protectedKey) => this._keys = protectedKey != null ? protectedKey : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (protectedKey));

    public string ComputedKeyAlgorithm => this._computedKeyAlgorithm;

    public ProtectedKey ProtectedKey => this._keys;
  }
}
