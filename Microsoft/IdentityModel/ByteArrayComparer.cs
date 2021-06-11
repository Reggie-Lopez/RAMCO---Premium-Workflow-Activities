// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.ByteArrayComparer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;

namespace Microsoft.IdentityModel
{
  internal class ByteArrayComparer : IEqualityComparer<byte[]>
  {
    private static ByteArrayComparer _instance = new ByteArrayComparer();

    public static ByteArrayComparer Instance => ByteArrayComparer._instance;

    private ByteArrayComparer()
    {
    }

    public bool Equals(byte[] x, byte[] y) => CryptoUtil.AreEqual(x, y);

    public int GetHashCode(byte[] obj)
    {
      int num = 0;
      for (int index = 0; index < obj.Length && index < 4; ++index)
        num = num << 8 | (int) obj[index];
      return num ^ obj.Length;
    }
  }
}
