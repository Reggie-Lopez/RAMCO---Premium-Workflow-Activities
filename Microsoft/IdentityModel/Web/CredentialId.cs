// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.CredentialId
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public static class CredentialId
  {
    public static string CreateFriendlyPPID(string rawPPID)
    {
      if (string.IsNullOrEmpty(rawPPID))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (rawPPID));
      byte[] buffer;
      try
      {
        buffer = Convert.FromBase64String(rawPPID);
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3126", (object) rawPPID), (Exception) ex));
      }
      StringBuilder stringBuilder = new StringBuilder();
      byte[] hash;
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha1())
        hash = hashAlgorithm.ComputeHash(buffer);
      char[] charArray = "QL23456789ABCDEFGHJKMNPRSTUVWXYZ".ToCharArray();
      int length = "QL23456789ABCDEFGHJKMNPRSTUVWXYZ".Length;
      for (int index = 0; index < 10; ++index)
      {
        if (index == 3 || index == 7)
          stringBuilder.Append('-');
        stringBuilder.Append(charArray[(int) hash[index] % length]);
      }
      return stringBuilder.ToString();
    }
  }
}
