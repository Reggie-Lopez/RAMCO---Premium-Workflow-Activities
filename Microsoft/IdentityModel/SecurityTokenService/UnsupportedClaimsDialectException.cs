// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.UnsupportedClaimsDialectException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  [Serializable]
  public class UnsupportedClaimsDialectException : RequestException
  {
    public UnsupportedClaimsDialectException()
      : base(Microsoft.IdentityModel.SR.GetString("ID2096"))
    {
    }

    public UnsupportedClaimsDialectException(string message)
      : base(message)
    {
    }

    public UnsupportedClaimsDialectException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected UnsupportedClaimsDialectException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
