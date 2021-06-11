// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.RequestSecurityToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class RequestSecurityToken : WSTrustMessage
  {
    private AdditionalContext _additionalContext;
    private RequestClaimCollection _claims;
    private string _computedKeyAlgorithm;
    private Renewing _renewing;
    private SecurityTokenElement _renewTarget;
    private bool _requestDisplayToken;
    private string _displayTokenLanguage;
    private InformationCardReference _informationCardReference;
    private string _clientPseudonym;
    private SecurityTokenElement _proofEncryption;
    private RequestSecurityToken _secondaryParameters;
    private SecurityTokenElement _onBehalfOf;
    private EndpointAddress _onBehalfOfIssuer;
    private SecurityTokenElement _actAs;
    private SecurityTokenElement _delegateTo;
    private bool? _forwardable;
    private bool? _delegatable;
    private SecurityTokenElement _cancelTarget;
    private SecurityTokenElement _validateTarget;
    private Participants _participants;
    private SecurityTokenElement _encryption;

    public RequestSecurityToken()
      : this((string) null, (string) null)
    {
    }

    public RequestSecurityToken(string requestType)
      : this(requestType, (string) null)
    {
    }

    public RequestSecurityToken(string requestType, string keyType)
    {
      this.RequestType = requestType;
      if (keyType == "http://schemas.microsoft.com/idfx/keytype/symmetric")
      {
        this.Entropy = new Entropy(256);
        this.KeySizeInBits = new int?(256);
      }
      else if (keyType == "http://schemas.microsoft.com/idfx/keytype/bearer")
        this.KeySizeInBits = new int?(0);
      else if (keyType == "http://schemas.microsoft.com/idfx/keytype/asymmetric")
        this.KeySizeInBits = new int?(1024);
      this.KeyType = keyType;
    }

    public RequestClaimCollection Claims
    {
      get
      {
        if (this._claims == null)
          this._claims = new RequestClaimCollection();
        return this._claims;
      }
    }

    public string ClientPseudonym
    {
      get => this._clientPseudonym;
      set => this._clientPseudonym = value;
    }

    public SecurityTokenElement Encryption
    {
      get => this._encryption;
      set => this._encryption = value;
    }

    public string ComputedKeyAlgorithm
    {
      get => this._computedKeyAlgorithm;
      set => this._computedKeyAlgorithm = value;
    }

    public bool? Delegatable
    {
      get => this._delegatable;
      set => this._delegatable = value;
    }

    public SecurityTokenElement DelegateTo
    {
      get => this._delegateTo;
      set => this._delegateTo = value;
    }

    public bool RequestDisplayToken
    {
      get => this._requestDisplayToken;
      set => this._requestDisplayToken = value;
    }

    public string DisplayTokenLanguage
    {
      get => this._displayTokenLanguage;
      set => this._displayTokenLanguage = value;
    }

    public bool? Forwardable
    {
      get => this._forwardable;
      set => this._forwardable = value;
    }

    public InformationCardReference InformationCardReference
    {
      get => this._informationCardReference;
      set => this._informationCardReference = value;
    }

    public SecurityTokenElement OnBehalfOf
    {
      get => this._onBehalfOf;
      set => this._onBehalfOf = value;
    }

    public Participants Participants
    {
      get => this._participants;
      set => this._participants = value;
    }

    public EndpointAddress Issuer
    {
      get => this._onBehalfOfIssuer;
      set => this._onBehalfOfIssuer = value;
    }

    public AdditionalContext AdditionalContext
    {
      get => this._additionalContext;
      set => this._additionalContext = value;
    }

    public SecurityTokenElement ActAs
    {
      get => this._actAs;
      set => this._actAs = value;
    }

    public SecurityTokenElement CancelTarget
    {
      get => this._cancelTarget;
      set => this._cancelTarget = value;
    }

    public SecurityTokenElement ProofEncryption
    {
      get => this._proofEncryption;
      set => this._proofEncryption = value;
    }

    public Renewing Renewing
    {
      get => this._renewing;
      set => this._renewing = value;
    }

    public SecurityTokenElement RenewTarget
    {
      get => this._renewTarget;
      set => this._renewTarget = value;
    }

    public RequestSecurityToken SecondaryParameters
    {
      get => this._secondaryParameters;
      set => this._secondaryParameters = value;
    }

    public SecurityTokenElement ValidateTarget
    {
      get => this._validateTarget;
      set => this._validateTarget = value;
    }
  }
}
