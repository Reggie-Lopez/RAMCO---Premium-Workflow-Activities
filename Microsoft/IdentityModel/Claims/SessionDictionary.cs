// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.SessionDictionary
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public sealed class SessionDictionary : XmlDictionary
  {
    private static readonly SessionDictionary instance = new SessionDictionary();
    private XmlDictionaryString _claim;
    private XmlDictionaryString _sct;
    private XmlDictionaryString _issuer;
    private XmlDictionaryString _originalIssuer;
    private XmlDictionaryString _issuerRef;
    private XmlDictionaryString _claimCollection;
    private XmlDictionaryString _actor;
    private XmlDictionaryString _claimProperty;
    private XmlDictionaryString _claimProperties;
    private XmlDictionaryString _value;
    private XmlDictionaryString _valueType;
    private XmlDictionaryString _label;
    private XmlDictionaryString _claimPropertyName;
    private XmlDictionaryString _claimPropertyValue;
    private XmlDictionaryString _type;
    private XmlDictionaryString _subjectId;
    private XmlDictionaryString _contextId;
    private XmlDictionaryString _anonymousIssued;
    private XmlDictionaryString _selfIssued;
    private XmlDictionaryString _authenticationType;
    private XmlDictionaryString _nameClaimType;
    private XmlDictionaryString _roleClaimType;
    private XmlDictionaryString _version;
    private XmlDictionaryString _scVersion;
    private XmlDictionaryString _emptyString;
    private XmlDictionaryString _nullValue;
    private XmlDictionaryString _key;
    private XmlDictionaryString _effectiveTime;
    private XmlDictionaryString _expiryTime;
    private XmlDictionaryString _keyGeneration;
    private XmlDictionaryString _keyEffectiveTime;
    private XmlDictionaryString _keyExpiryTime;
    private XmlDictionaryString _sessionId;
    private XmlDictionaryString _id;
    private XmlDictionaryString _validFrom;
    private XmlDictionaryString _validTo;
    private XmlDictionaryString _sesionToken;
    private XmlDictionaryString _sesionTokenCookie;
    private XmlDictionaryString _bootStrapToken;
    private XmlDictionaryString _context;
    private XmlDictionaryString _claimsPrincipal;
    private XmlDictionaryString _windowsPrincipal;
    private XmlDictionaryString _windowsIdentity;
    private XmlDictionaryString _identity;
    private XmlDictionaryString _identities;
    private XmlDictionaryString _windowsLogonName;
    private XmlDictionaryString _persistentTrue;
    private XmlDictionaryString _sctAuthorizationPolicy;
    private XmlDictionaryString _right;
    private XmlDictionaryString _endpointId;
    private XmlDictionaryString _windowsSidClaim;
    private XmlDictionaryString _denyOnlySidClaim;
    private XmlDictionaryString _x500DistinguishedNameClaim;
    private XmlDictionaryString _x509ThumbprintClaim;
    private XmlDictionaryString _nameClaim;
    private XmlDictionaryString _dnsClaim;
    private XmlDictionaryString _rsaClaim;
    private XmlDictionaryString _mailAddressClaim;
    private XmlDictionaryString _systemClaim;
    private XmlDictionaryString _hashClaim;
    private XmlDictionaryString _spnClaim;
    private XmlDictionaryString _upnClaim;
    private XmlDictionaryString _urlClaim;
    private XmlDictionaryString _sid;
    private XmlDictionaryString _sessionModeTrue;

    private SessionDictionary()
    {
      this._claim = this.Add(nameof (Claim));
      this._sct = this.Add(nameof (SecurityContextToken));
      this._version = this.Add(nameof (Version));
      this._scVersion = this.Add(nameof (SecureConversationVersion));
      this._issuer = this.Add(nameof (Issuer));
      this._originalIssuer = this.Add(nameof (OriginalIssuer));
      this._issuerRef = this.Add(nameof (IssuerRef));
      this._claimCollection = this.Add(nameof (ClaimCollection));
      this._actor = this.Add(nameof (Actor));
      this._claimProperty = this.Add(nameof (ClaimProperty));
      this._claimProperties = this.Add(nameof (ClaimProperties));
      this._value = this.Add(nameof (Value));
      this._valueType = this.Add(nameof (ValueType));
      this._label = this.Add(nameof (Label));
      this._type = this.Add(nameof (Type));
      this._subjectId = this.Add("subjectID");
      this._claimPropertyName = this.Add(nameof (ClaimPropertyName));
      this._claimPropertyValue = this.Add(nameof (ClaimPropertyValue));
      this._anonymousIssued = this.Add("http://www.w3.org/2005/08/addressing/anonymous");
      this._selfIssued = this.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self");
      this._authenticationType = this.Add(nameof (AuthenticationType));
      this._nameClaimType = this.Add(nameof (NameClaimType));
      this._roleClaimType = this.Add(nameof (RoleClaimType));
      this._nullValue = this.Add("Null");
      this._emptyString = this.Add(string.Empty);
      this._key = this.Add(nameof (Key));
      this._effectiveTime = this.Add(nameof (EffectiveTime));
      this._expiryTime = this.Add(nameof (ExpiryTime));
      this._keyGeneration = this.Add(nameof (KeyGeneration));
      this._keyEffectiveTime = this.Add(nameof (KeyEffectiveTime));
      this._keyExpiryTime = this.Add(nameof (KeyExpiryTime));
      this._sessionId = this.Add(nameof (SessionId));
      this._id = this.Add(nameof (Id));
      this._validFrom = this.Add(nameof (ValidFrom));
      this._validTo = this.Add(nameof (ValidTo));
      this._contextId = this.Add(nameof (ContextId));
      this._sesionToken = this.Add(nameof (SessionToken));
      this._sesionTokenCookie = this.Add(nameof (SessionTokenCookie));
      this._bootStrapToken = this.Add("BootStrapToken");
      this._context = this.Add(nameof (Context));
      this._claimsPrincipal = this.Add(nameof (ClaimsPrincipal));
      this._windowsPrincipal = this.Add(nameof (WindowsPrincipal));
      this._windowsIdentity = this.Add("WindowIdentity");
      this._identity = this.Add(nameof (Identity));
      this._identities = this.Add(nameof (Identities));
      this._windowsLogonName = this.Add(nameof (WindowsLogonName));
      this._persistentTrue = this.Add(nameof (PersistentTrue));
      this._sctAuthorizationPolicy = this.Add(nameof (SctAuthorizationPolicy));
      this._right = this.Add(nameof (Right));
      this._endpointId = this.Add(nameof (EndpointId));
      this._windowsSidClaim = this.Add(nameof (WindowsSidClaim));
      this._denyOnlySidClaim = this.Add(nameof (DenyOnlySidClaim));
      this._x500DistinguishedNameClaim = this.Add(nameof (X500DistinguishedNameClaim));
      this._x509ThumbprintClaim = this.Add(nameof (X509ThumbprintClaim));
      this._nameClaim = this.Add(nameof (NameClaim));
      this._dnsClaim = this.Add(nameof (DnsClaim));
      this._rsaClaim = this.Add(nameof (RsaClaim));
      this._mailAddressClaim = this.Add(nameof (MailAddressClaim));
      this._systemClaim = this.Add(nameof (SystemClaim));
      this._hashClaim = this.Add(nameof (HashClaim));
      this._spnClaim = this.Add(nameof (SpnClaim));
      this._upnClaim = this.Add(nameof (UpnClaim));
      this._urlClaim = this.Add(nameof (UrlClaim));
      this._sid = this.Add(nameof (Sid));
      this._sessionModeTrue = this.Add(nameof (SessionModeTrue));
    }

    public static SessionDictionary Instance => SessionDictionary.instance;

    public XmlDictionaryString PersistentTrue => this._persistentTrue;

    public XmlDictionaryString WindowsLogonName => this._windowsLogonName;

    public XmlDictionaryString ClaimsPrincipal => this._claimsPrincipal;

    public XmlDictionaryString WindowsPrincipal => this._windowsPrincipal;

    public XmlDictionaryString AnonymousIssued => this._anonymousIssued;

    public XmlDictionaryString WindowsIdentity => this._windowsIdentity;

    public XmlDictionaryString Identity => this._identity;

    public XmlDictionaryString Identities => this._identities;

    public XmlDictionaryString SessionId => this._sessionId;

    public XmlDictionaryString SessionModeTrue => this._sessionModeTrue;

    public XmlDictionaryString ValidFrom => this._validFrom;

    public XmlDictionaryString ValidTo => this._validTo;

    public XmlDictionaryString EffectiveTime => this._effectiveTime;

    public XmlDictionaryString ExpiryTime => this._expiryTime;

    public XmlDictionaryString KeyEffectiveTime => this._keyEffectiveTime;

    public XmlDictionaryString KeyExpiryTime => this._keyExpiryTime;

    public XmlDictionaryString Claim => this._claim;

    public XmlDictionaryString SelfIssued => this._selfIssued;

    public XmlDictionaryString Issuer => this._issuer;

    public XmlDictionaryString OriginalIssuer => this._originalIssuer;

    public XmlDictionaryString IssuerRef => this._issuerRef;

    public XmlDictionaryString ClaimCollection => this._claimCollection;

    public XmlDictionaryString Actor => this._actor;

    public XmlDictionaryString ClaimProperties => this._claimProperties;

    public XmlDictionaryString ClaimProperty => this._claimProperty;

    public XmlDictionaryString Value => this._value;

    public XmlDictionaryString ValueType => this._valueType;

    public XmlDictionaryString Label => this._label;

    public XmlDictionaryString Type => this._type;

    public XmlDictionaryString SubjectId => this._subjectId;

    public XmlDictionaryString ClaimPropertyName => this._claimPropertyName;

    public XmlDictionaryString ClaimPropertyValue => this._claimPropertyValue;

    public XmlDictionaryString AuthenticationType => this._authenticationType;

    public XmlDictionaryString NameClaimType => this._nameClaimType;

    public XmlDictionaryString RoleClaimType => this._roleClaimType;

    public XmlDictionaryString NullValue => this._nullValue;

    public XmlDictionaryString SecurityContextToken => this._sct;

    public XmlDictionaryString Version => this._version;

    public XmlDictionaryString SecureConversationVersion => this._scVersion;

    public XmlDictionaryString EmptyString => this._emptyString;

    public XmlDictionaryString Key => this._key;

    public XmlDictionaryString KeyGeneration => this._keyGeneration;

    public XmlDictionaryString Id => this._id;

    public XmlDictionaryString ContextId => this._contextId;

    public XmlDictionaryString SessionToken => this._sesionToken;

    public XmlDictionaryString SessionTokenCookie => this._sesionTokenCookie;

    public XmlDictionaryString BootstrapToken => this._bootStrapToken;

    public XmlDictionaryString Context => this._context;

    public XmlDictionaryString SctAuthorizationPolicy => this._sctAuthorizationPolicy;

    public XmlDictionaryString Right => this._right;

    public XmlDictionaryString EndpointId => this._endpointId;

    public XmlDictionaryString WindowsSidClaim => this._windowsSidClaim;

    public XmlDictionaryString DenyOnlySidClaim => this._denyOnlySidClaim;

    public XmlDictionaryString X500DistinguishedNameClaim => this._x500DistinguishedNameClaim;

    public XmlDictionaryString X509ThumbprintClaim => this._x509ThumbprintClaim;

    public XmlDictionaryString NameClaim => this._nameClaim;

    public XmlDictionaryString DnsClaim => this._dnsClaim;

    public XmlDictionaryString RsaClaim => this._rsaClaim;

    public XmlDictionaryString MailAddressClaim => this._mailAddressClaim;

    public XmlDictionaryString SystemClaim => this._systemClaim;

    public XmlDictionaryString HashClaim => this._hashClaim;

    public XmlDictionaryString SpnClaim => this._spnClaim;

    public XmlDictionaryString UpnClaim => this._upnClaim;

    public XmlDictionaryString UrlClaim => this._urlClaim;

    public XmlDictionaryString Sid => this._sid;
  }
}
