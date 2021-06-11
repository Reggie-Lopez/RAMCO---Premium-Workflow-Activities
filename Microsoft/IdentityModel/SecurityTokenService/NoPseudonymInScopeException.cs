// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.NoPseudonymInScopeException
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
  public class NoPseudonymInScopeException : RequestException
  {
    public NoPseudonymInScopeException()
      : base(Microsoft.IdentityModel.SR.GetString("ID2091"))
    {
    }

    public NoPseudonymInScopeException(string message)
      : base(message)
    {
    }

    public NoPseudonymInScopeException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected NoPseudonymInScopeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
