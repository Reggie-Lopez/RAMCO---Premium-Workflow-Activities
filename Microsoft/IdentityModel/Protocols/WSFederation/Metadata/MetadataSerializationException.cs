// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.MetadataSerializationException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  [Serializable]
  public class MetadataSerializationException : Exception
  {
    public MetadataSerializationException()
      : this(Microsoft.IdentityModel.SR.GetString("ID3198"))
    {
    }

    public MetadataSerializationException(string message)
      : base(message)
    {
    }

    public MetadataSerializationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected MetadataSerializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
