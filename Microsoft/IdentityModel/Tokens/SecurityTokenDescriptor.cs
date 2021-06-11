// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenDescriptor
  {
    private SecurityKeyIdentifierClause _attachedReference;
    private AuthenticationInformation _authenticationInfo;
    private string _tokenIssuerName;
    private ProofDescriptor _proofDescriptor;
    private IClaimsIdentity _subject;
    private SecurityToken _token;
    private string _tokenType;
    private SecurityKeyIdentifierClause _unattachedReference;
    private Lifetime _lifetime;
    private DisplayToken _displayToken;
    private string _appliesToAddress;
    private string _replyToAddress;
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;
    private SigningCredentials _signingCredentials;
    private Dictionary<string, object> _properties = new Dictionary<string, object>();

    public void AddAuthenticationClaims(string authType) => this.AddAuthenticationClaims(authType, DateTime.UtcNow);

    public void AddAuthenticationClaims(string authType, DateTime time)
    {
      this.Subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authType, "http://www.w3.org/2001/XMLSchema#string"));
      this.Subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(time.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
    }

    public virtual void ApplyTo(RequestSecurityTokenResponse response)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (this._tokenType != null)
        response.TokenType = this._tokenType;
      if (this._token != null)
        response.RequestedSecurityToken = new RequestedSecurityToken(this._token);
      if (this._attachedReference != null)
        response.RequestedAttachedReference = this._attachedReference;
      if (this._unattachedReference != null)
        response.RequestedUnattachedReference = this._unattachedReference;
      if (this._lifetime != null)
        response.Lifetime = this._lifetime;
      if (this._displayToken != null)
        response.RequestedDisplayToken = this._displayToken;
      if (this._proofDescriptor == null)
        return;
      this._proofDescriptor.ApplyTo(response);
    }

    public string AppliesToAddress
    {
      get => this._appliesToAddress;
      set => this._appliesToAddress = string.IsNullOrEmpty(value) || UriUtil.CanCreateValidUri(value, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2002")));
    }

    public string ReplyToAddress
    {
      get => this._replyToAddress;
      set => this._replyToAddress = value;
    }

    public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials
    {
      get => this._encryptingCredentials;
      set => this._encryptingCredentials = value;
    }

    public SigningCredentials SigningCredentials
    {
      get => this._signingCredentials;
      set => this._signingCredentials = value;
    }

    public SecurityKeyIdentifierClause AttachedReference
    {
      get => this._attachedReference;
      set => this._attachedReference = value;
    }

    public AuthenticationInformation AuthenticationInfo
    {
      get => this._authenticationInfo;
      set => this._authenticationInfo = value;
    }

    public string TokenIssuerName
    {
      get => this._tokenIssuerName;
      set => this._tokenIssuerName = value;
    }

    public ProofDescriptor Proof
    {
      get => this._proofDescriptor;
      set => this._proofDescriptor = value;
    }

    public Dictionary<string, object> Properties => this._properties;

    public SecurityToken Token
    {
      get => this._token;
      set => this._token = value;
    }

    public string TokenType
    {
      get => this._tokenType;
      set => this._tokenType = value;
    }

    public SecurityKeyIdentifierClause UnattachedReference
    {
      get => this._unattachedReference;
      set => this._unattachedReference = value;
    }

    public Lifetime Lifetime
    {
      get => this._lifetime;
      set => this._lifetime = value;
    }

    public DisplayToken DisplayToken
    {
      get => this._displayToken;
      set => this._displayToken = value;
    }

    public IClaimsIdentity Subject
    {
      get => this._subject;
      set => this._subject = value;
    }
  }
}
