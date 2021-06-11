// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.FederationException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols
{
  [ComVisible(true)]
  [Serializable]
  public class FederationException : Exception
  {
    public FederationException()
      : this(Microsoft.IdentityModel.SR.GetString("ID3205"))
    {
    }

    public FederationException(string message)
      : base(message)
    {
    }

    public FederationException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected FederationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
