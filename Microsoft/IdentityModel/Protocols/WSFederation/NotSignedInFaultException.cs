// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.NotSignedInFaultException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  [Serializable]
  public class NotSignedInFaultException : RequestFaultException
  {
    public NotSignedInFaultException(string soapNamespace)
      : this(soapNamespace, new FaultReason(Microsoft.IdentityModel.SR.GetString("ID3235")))
    {
    }

    public NotSignedInFaultException(string soapNamespace, FaultReason reason)
      : base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("NotSignedIn", "http://docs.oasis-open.org/wsfed/federation/200706")))
    {
    }
  }
}
