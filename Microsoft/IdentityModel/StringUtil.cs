﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.StringUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel
{
  internal static class StringUtil
  {
    public static string OptimizeString(string value)
    {
      if (value != null)
      {
        string str = string.IsInterned(value);
        if (str != null)
          return str;
      }
      return value;
    }
  }
}
