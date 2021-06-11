// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.ProtectedDataCookieTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public sealed class ProtectedDataCookieTransform : CookieTransform
  {
    public override byte[] Decode(byte[] encoded)
    {
      if (encoded == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encoded));
      if (encoded.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (encoded), Microsoft.IdentityModel.SR.GetString("ID6045"));
      try
      {
        return ProtectedData.Unprotect(encoded, (byte[]) null, DataProtectionScope.CurrentUser);
      }
      catch (CryptographicException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1073"), (Exception) ex));
      }
    }

    public override byte[] Encode(byte[] value)
    {
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (value.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID6044"));
      try
      {
        return ProtectedData.Protect(value, (byte[]) null, DataProtectionScope.CurrentUser);
      }
      catch (CryptographicException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1074"), (Exception) ex));
      }
    }
  }
}
