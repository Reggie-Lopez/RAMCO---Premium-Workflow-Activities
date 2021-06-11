// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.MessageExpiredException
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
  public class MessageExpiredException : Exception
  {
    public MessageExpiredException()
      : base(Microsoft.IdentityModel.SR.GetString("ID3256"))
    {
    }

    public MessageExpiredException(string message)
      : base(message)
    {
    }

    public MessageExpiredException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected MessageExpiredException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
