// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustFeb2005ConstantsAdapter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  internal class WSTrustFeb2005ConstantsAdapter : WSTrustConstantsAdapter
  {
    private static WSTrustFeb2005ConstantsAdapter _instance;
    private static WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005Actions _trustFeb2005Actions;
    private static WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005ComputedKeyAlgorithm _trustFeb2005ComputedKeyAlgorithm;
    private static WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005KeyTypes _trustFeb2005KeyTypes;
    private static WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005RequestTypes _trustFeb2005RequestTypes;

    protected WSTrustFeb2005ConstantsAdapter()
    {
      this.NamespaceURI = "http://schemas.xmlsoap.org/ws/2005/02/trust";
      this.Prefix = "t";
    }

    internal static WSTrustFeb2005ConstantsAdapter Instance
    {
      get
      {
        if (WSTrustFeb2005ConstantsAdapter._instance == null)
          WSTrustFeb2005ConstantsAdapter._instance = new WSTrustFeb2005ConstantsAdapter();
        return WSTrustFeb2005ConstantsAdapter._instance;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustActions Actions
    {
      get
      {
        if (WSTrustFeb2005ConstantsAdapter._trustFeb2005Actions == null)
          WSTrustFeb2005ConstantsAdapter._trustFeb2005Actions = new WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005Actions();
        return (WSTrustConstantsAdapter.WSTrustActions) WSTrustFeb2005ConstantsAdapter._trustFeb2005Actions;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm
    {
      get
      {
        if (WSTrustFeb2005ConstantsAdapter._trustFeb2005ComputedKeyAlgorithm == null)
          WSTrustFeb2005ConstantsAdapter._trustFeb2005ComputedKeyAlgorithm = new WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005ComputedKeyAlgorithm();
        return (WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm) WSTrustFeb2005ConstantsAdapter._trustFeb2005ComputedKeyAlgorithm;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustKeyTypes KeyTypes
    {
      get
      {
        if (WSTrustFeb2005ConstantsAdapter._trustFeb2005KeyTypes == null)
          WSTrustFeb2005ConstantsAdapter._trustFeb2005KeyTypes = new WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005KeyTypes();
        return (WSTrustConstantsAdapter.WSTrustKeyTypes) WSTrustFeb2005ConstantsAdapter._trustFeb2005KeyTypes;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustRequestTypes RequestTypes
    {
      get
      {
        if (WSTrustFeb2005ConstantsAdapter._trustFeb2005RequestTypes == null)
          WSTrustFeb2005ConstantsAdapter._trustFeb2005RequestTypes = new WSTrustFeb2005ConstantsAdapter.WSTrustFeb2005RequestTypes();
        return (WSTrustConstantsAdapter.WSTrustRequestTypes) WSTrustFeb2005ConstantsAdapter._trustFeb2005RequestTypes;
      }
    }

    internal class WSTrustFeb2005Actions : WSTrustConstantsAdapter.WSTrustActions
    {
      internal WSTrustFeb2005Actions()
      {
        this.Cancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel";
        this.CancelResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel";
        this.Issue = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
        this.IssueResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue";
        this.Renew = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew";
        this.RenewResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew";
        this.RequestSecurityContextToken = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT";
        this.RequestSecurityContextTokenCancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT-Cancel";
        this.RequestSecurityContextTokenResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT";
        this.RequestSecurityContextTokenResponseCancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT-Cancel";
        this.Validate = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate";
        this.ValidateResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate";
      }
    }

    internal class WSTrustFeb2005ComputedKeyAlgorithm : 
      WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm
    {
      internal WSTrustFeb2005ComputedKeyAlgorithm() => this.Psha1 = "http://schemas.xmlsoap.org/ws/2005/02/trust/CK/PSHA1";
    }

    internal class WSTrustFeb2005KeyTypes : WSTrustConstantsAdapter.WSTrustKeyTypes
    {
      internal WSTrustFeb2005KeyTypes()
      {
        this.Asymmetric = "http://schemas.xmlsoap.org/ws/2005/02/trust/PublicKey";
        this.Bearer = "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey";
        this.Symmetric = "http://schemas.xmlsoap.org/ws/2005/02/trust/SymmetricKey";
      }
    }

    internal class WSTrustFeb2005RequestTypes : WSTrustConstantsAdapter.WSTrustRequestTypes
    {
      internal WSTrustFeb2005RequestTypes()
      {
        this.Cancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/Cancel";
        this.Issue = "http://schemas.xmlsoap.org/ws/2005/02/trust/Issue";
        this.Renew = "http://schemas.xmlsoap.org/ws/2005/02/trust/Renew";
        this.Validate = "http://schemas.xmlsoap.org/ws/2005/02/trust/Validate";
      }
    }
  }
}
