// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Entropy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.SecurityTokenService;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class Entropy : ProtectedKey
  {
    public Entropy(int entropySizeInBits)
      : this(CryptoUtil.GenerateRandomBytes(entropySizeInBits))
    {
    }

    public Entropy(byte[] secret)
      : base(secret)
    {
    }

    public Entropy(byte[] secret, EncryptingCredentials wrappingCredentials)
      : base(secret, wrappingCredentials)
    {
    }

    public Entropy(ProtectedKey protectedKey)
      : base(Entropy.GetKeyBytesFromProtectedKey(protectedKey), Entropy.GetWrappingCredentialsFromProtectedKey(protectedKey))
    {
    }

    private static byte[] GetKeyBytesFromProtectedKey(ProtectedKey protectedKey) => protectedKey != null ? protectedKey.GetKeyBytes() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (protectedKey));

    private static EncryptingCredentials GetWrappingCredentialsFromProtectedKey(
      ProtectedKey protectedKey)
    {
      return protectedKey != null ? protectedKey.WrappingCredentials : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (protectedKey));
    }
  }
}
