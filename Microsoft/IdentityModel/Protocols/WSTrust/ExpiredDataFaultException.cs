// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.ExpiredDataFaultException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  [Serializable]
  public class ExpiredDataFaultException : RequestFaultException
  {
    public ExpiredDataFaultException(string soapNamespace, string trustNamespace)
      : this(soapNamespace, trustNamespace, new FaultReason(Microsoft.IdentityModel.SR.GetString("ID3246")))
    {
    }

    public ExpiredDataFaultException(
      string soapNamespace,
      string trustNamespace,
      FaultReason reason)
      : base(reason, new FaultCode("Sender", soapNamespace, new FaultCode(ExpiredDataFaultException.GetFaultSubCodeName(trustNamespace), trustNamespace)))
    {
    }

    private static string GetFaultSubCodeName(string trustNamespace) => (WSTrustConstantsAdapter.GetConstantsAdapter(trustNamespace) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (trustNamespace), Microsoft.IdentityModel.SR.GetString("ID3228"))).FaultCodes.ExpiredData;
  }
}
