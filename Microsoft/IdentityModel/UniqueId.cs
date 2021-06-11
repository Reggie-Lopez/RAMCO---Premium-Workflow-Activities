// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.UniqueId
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public static class UniqueId
  {
    private const int RandomSaltSize = 16;
    private const string NcNamePrefix = "_";
    private const string UuidUriPrefix = "urn:uuid:";
    private static readonly string _reusableUuid = UniqueId.GetRandomUuid();
    private static readonly string _optimizedNcNamePrefix = "_" + UniqueId._reusableUuid + "-";

    public static string CreateUniqueId() => UniqueId._optimizedNcNamePrefix + UniqueId.GetNextId();

    public static string CreateUniqueId(string prefix)
    {
      if (string.IsNullOrEmpty(prefix))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (prefix));
      return prefix + UniqueId._reusableUuid + "-" + UniqueId.GetNextId();
    }

    public static string CreateRandomId() => "_" + UniqueId.GetRandomUuid();

    public static string CreateRandomId(string prefix)
    {
      if (string.IsNullOrEmpty(prefix))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (prefix));
      return prefix + UniqueId.GetRandomUuid();
    }

    public static string CreateRandomUri() => "urn:uuid:" + UniqueId.GetRandomUuid();

    private static string GetNextId()
    {
      RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
      byte[] data = new byte[16];
      randomNumberGenerator.GetBytes(data);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < data.Length; ++index)
        stringBuilder.AppendFormat("{0:X2}", (object) data[index]);
      return stringBuilder.ToString();
    }

    private static string GetRandomUuid() => Guid.NewGuid().ToString("D", (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
