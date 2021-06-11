// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenUnavailableException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  [Serializable]
  public class SecurityTokenUnavailableException : Exception
  {
    public SecurityTokenUnavailableException()
      : base(Microsoft.IdentityModel.SR.GetString("ID3255"))
    {
    }

    public SecurityTokenUnavailableException(string message)
      : base(message)
    {
    }

    public SecurityTokenUnavailableException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected SecurityTokenUnavailableException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
