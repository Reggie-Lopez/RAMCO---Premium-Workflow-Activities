// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrust13ConstantsAdapter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  internal class WSTrust13ConstantsAdapter : WSTrustConstantsAdapter
  {
    private static WSTrust13ConstantsAdapter _instance;
    private static WSTrust13ConstantsAdapter.WSTrust13ElementNames _trust13ElementNames;
    private static WSTrust13ConstantsAdapter.WSTrust13Actions _trust13ActionNames;
    private static WSTrust13ConstantsAdapter.WSTrust13ComputedKeyAlgorithm _trust13ComputedKeyAlgorithm;
    private static WSTrust13ConstantsAdapter.WSTrust13KeyTypes _trust13KeyTypes;
    private static WSTrust13ConstantsAdapter.WSTrust13RequestTypes _trust13RequestTypes;

    protected WSTrust13ConstantsAdapter()
    {
      this.NamespaceURI = "http://docs.oasis-open.org/ws-sx/ws-trust/200512";
      this.Prefix = "trust";
    }

    internal static WSTrust13ConstantsAdapter Instance
    {
      get
      {
        if (WSTrust13ConstantsAdapter._instance == null)
          WSTrust13ConstantsAdapter._instance = new WSTrust13ConstantsAdapter();
        return WSTrust13ConstantsAdapter._instance;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustActions Actions
    {
      get
      {
        if (WSTrust13ConstantsAdapter._trust13ActionNames == null)
          WSTrust13ConstantsAdapter._trust13ActionNames = new WSTrust13ConstantsAdapter.WSTrust13Actions();
        return (WSTrustConstantsAdapter.WSTrustActions) WSTrust13ConstantsAdapter._trust13ActionNames;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm
    {
      get
      {
        if (WSTrust13ConstantsAdapter._trust13ComputedKeyAlgorithm == null)
          WSTrust13ConstantsAdapter._trust13ComputedKeyAlgorithm = new WSTrust13ConstantsAdapter.WSTrust13ComputedKeyAlgorithm();
        return (WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm) WSTrust13ConstantsAdapter._trust13ComputedKeyAlgorithm;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustElementNames Elements
    {
      get
      {
        if (WSTrust13ConstantsAdapter._trust13ElementNames == null)
          WSTrust13ConstantsAdapter._trust13ElementNames = new WSTrust13ConstantsAdapter.WSTrust13ElementNames();
        return (WSTrustConstantsAdapter.WSTrustElementNames) WSTrust13ConstantsAdapter._trust13ElementNames;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustKeyTypes KeyTypes
    {
      get
      {
        if (WSTrust13ConstantsAdapter._trust13KeyTypes == null)
          WSTrust13ConstantsAdapter._trust13KeyTypes = new WSTrust13ConstantsAdapter.WSTrust13KeyTypes();
        return (WSTrustConstantsAdapter.WSTrustKeyTypes) WSTrust13ConstantsAdapter._trust13KeyTypes;
      }
    }

    internal override WSTrustConstantsAdapter.WSTrustRequestTypes RequestTypes
    {
      get
      {
        if (WSTrust13ConstantsAdapter._trust13RequestTypes == null)
          WSTrust13ConstantsAdapter._trust13RequestTypes = new WSTrust13ConstantsAdapter.WSTrust13RequestTypes();
        return (WSTrustConstantsAdapter.WSTrustRequestTypes) WSTrust13ConstantsAdapter._trust13RequestTypes;
      }
    }

    internal class WSTrust13ElementNames : WSTrustConstantsAdapter.WSTrustElementNames
    {
      internal string KeyWrapAlgorithm = nameof (KeyWrapAlgorithm);
      internal string SecondaryParameters = nameof (SecondaryParameters);
      internal string RequestSecurityTokenResponseCollection = nameof (RequestSecurityTokenResponseCollection);
      internal string ValidateTarget = nameof (ValidateTarget);
    }

    internal class WSTrust13Actions : WSTrustConstantsAdapter.WSTrustActions
    {
      internal string CancelResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal";
      internal string IssueResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal";
      internal string RenewResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal";
      internal string ValidateResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal";

      internal WSTrust13Actions()
      {
        this.Cancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel";
        this.CancelResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel";
        this.Issue = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue";
        this.IssueResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue";
        this.Renew = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew";
        this.RenewResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew";
        this.RequestSecurityContextToken = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/SCT";
        this.RequestSecurityContextTokenCancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/SCT-Cancel";
        this.RequestSecurityContextTokenResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/SCT";
        this.RequestSecurityContextTokenResponseCancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/SCT-Cancel";
        this.Validate = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate";
        this.ValidateResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate";
      }
    }

    internal class WSTrust13ComputedKeyAlgorithm : 
      WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm
    {
      internal WSTrust13ComputedKeyAlgorithm() => this.Psha1 = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/CK/PSHA1";
    }

    internal class WSTrust13KeyTypes : WSTrustConstantsAdapter.WSTrustKeyTypes
    {
      internal WSTrust13KeyTypes()
      {
        this.Asymmetric = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/PublicKey";
        this.Bearer = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer";
        this.Symmetric = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/SymmetricKey";
      }
    }

    internal class WSTrust13RequestTypes : WSTrustConstantsAdapter.WSTrustRequestTypes
    {
      internal WSTrust13RequestTypes()
      {
        this.Cancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Cancel";
        this.Issue = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue";
        this.Renew = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Renew";
        this.Validate = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Validate";
      }
    }
  }
}
