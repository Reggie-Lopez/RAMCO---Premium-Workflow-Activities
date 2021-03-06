// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.FailedAuthenticationException
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
  public class FailedAuthenticationException : Exception
  {
    public FailedAuthenticationException()
      : base(Microsoft.IdentityModel.SR.GetString("ID3242"))
    {
    }

    public FailedAuthenticationException(string message)
      : base(message)
    {
    }

    public FailedAuthenticationException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected FailedAuthenticationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
