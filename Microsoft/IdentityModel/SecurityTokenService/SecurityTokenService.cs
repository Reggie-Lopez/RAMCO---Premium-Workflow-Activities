// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Threading;
using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public abstract class SecurityTokenService
  {
    private SecurityTokenServiceConfiguration _securityTokenServiceConfiguration;
    private IClaimsPrincipal _principal;
    private RequestSecurityToken _request;
    private Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor _tokenDescriptor;

    protected SecurityTokenService(
      SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
    {
      this._securityTokenServiceConfiguration = securityTokenServiceConfiguration != null ? securityTokenServiceConfiguration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenServiceConfiguration));
    }

    public virtual IAsyncResult BeginCancel(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      AsyncCallback callback,
      object state)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request != null ? (object) request.RequestType : (object) "Cancel")));
    }

    protected virtual IAsyncResult BeginGetScope(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      AsyncCallback callback,
      object state)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID2081")));
    }

    public virtual IAsyncResult BeginIssue(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      AsyncCallback callback,
      object state)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      this._principal = principal;
      this._request = request;
      this.ValidateRequest(request);
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.FederatedAsyncState federatedAsyncState = new Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.FederatedAsyncState(request, principal, (IAsyncResult) new TypedAsyncResult<RequestSecurityTokenResponse>(callback, state));
      this.BeginGetScope(principal, request, new AsyncCallback(this.OnGetScopeComplete), (object) federatedAsyncState);
      return federatedAsyncState.Result;
    }

    public virtual IAsyncResult BeginRenew(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      AsyncCallback callback,
      object state)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request == null || request.RequestType == null ? (object) "Renew" : (object) request.RequestType)));
    }

    public virtual IAsyncResult BeginValidate(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      AsyncCallback callback,
      object state)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request == null || request.RequestType == null ? (object) "Validate" : (object) request.RequestType)));
    }

    public virtual RequestSecurityTokenResponse Cancel(
      IClaimsPrincipal principal,
      RequestSecurityToken request)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request == null || request.RequestType == null ? (object) nameof (Cancel) : (object) request.RequestType)));
    }

    protected virtual Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor CreateSecurityTokenDescriptor(
      RequestSecurityToken request,
      Scope scope)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (scope == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (scope));
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor securityTokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor();
      securityTokenDescriptor.AppliesToAddress = scope.AppliesToAddress;
      securityTokenDescriptor.ReplyToAddress = scope.ReplyToAddress;
      securityTokenDescriptor.SigningCredentials = scope.SigningCredentials;
      if (securityTokenDescriptor.SigningCredentials == null)
        securityTokenDescriptor.SigningCredentials = this.SecurityTokenServiceConfiguration.SigningCredentials;
      if (scope.EncryptingCredentials != null && scope.EncryptingCredentials.SecurityKey is AsymmetricSecurityKey && (request.EncryptionAlgorithm == null || request.EncryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#aes256-cbc") && (request.SecondaryParameters == null || request.SecondaryParameters.EncryptionAlgorithm == null || request.SecondaryParameters.EncryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#aes256-cbc"))
        securityTokenDescriptor.EncryptingCredentials = (EncryptingCredentials) new EncryptedKeyEncryptingCredentials(scope.EncryptingCredentials, 256, "http://www.w3.org/2001/04/xmlenc#aes256-cbc");
      return securityTokenDescriptor;
    }

    protected virtual string GetIssuerName() => this.SecurityTokenServiceConfiguration.TokenIssuerName;

    private string GetValidIssuerName()
    {
      string issuerName = this.GetIssuerName();
      return !string.IsNullOrEmpty(issuerName) ? issuerName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2083"));
    }

    protected virtual Microsoft.IdentityModel.Tokens.ProofDescriptor GetProofToken(
      RequestSecurityToken request,
      Scope scope)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (scope == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (scope));
      EncryptingCredentials encryptingCredentials1 = this.GetRequestorProofEncryptingCredentials(request);
      if (scope.EncryptingCredentials != null && !(scope.EncryptingCredentials.SecurityKey is AsymmetricSecurityKey))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4179")));
      EncryptingCredentials encryptingCredentials2 = scope.EncryptingCredentials;
      string x = string.IsNullOrEmpty(request.KeyType) ? "http://schemas.microsoft.com/idfx/keytype/symmetric" : request.KeyType;
      Microsoft.IdentityModel.Tokens.ProofDescriptor proofDescriptor = (Microsoft.IdentityModel.Tokens.ProofDescriptor) null;
      if (StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/asymmetric"))
      {
        if (request.UseKey == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3091")));
        proofDescriptor = (Microsoft.IdentityModel.Tokens.ProofDescriptor) new Microsoft.IdentityModel.Tokens.AsymmetricProofDescriptor(request.UseKey.SecurityKeyIdentifier);
      }
      else if (StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
      {
        if (request.ComputedKeyAlgorithm != null && !StringComparer.Ordinal.Equals(request.ComputedKeyAlgorithm, "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new RequestFailedException(Microsoft.IdentityModel.SR.GetString("ID2011", (object) request.ComputedKeyAlgorithm)));
        if (encryptingCredentials2 == null && scope.SymmetricKeyEncryptionRequired)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new RequestFailedException(Microsoft.IdentityModel.SR.GetString("ID4007")));
        if (!request.KeySizeInBits.HasValue)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new RequestFailedException(Microsoft.IdentityModel.SR.GetString("ID2059")));
        proofDescriptor = request.Entropy == null ? (Microsoft.IdentityModel.Tokens.ProofDescriptor) new Microsoft.IdentityModel.Tokens.SymmetricProofDescriptor(request.KeySizeInBits.Value, encryptingCredentials2, encryptingCredentials1, request.EncryptWith) : (Microsoft.IdentityModel.Tokens.ProofDescriptor) new Microsoft.IdentityModel.Tokens.SymmetricProofDescriptor(request.KeySizeInBits.Value, encryptingCredentials2, encryptingCredentials1, request.Entropy.GetKeyBytes(), request.EncryptWith);
      }
      else
        StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/bearer");
      return proofDescriptor;
    }

    protected virtual EncryptingCredentials GetRequestorProofEncryptingCredentials(
      RequestSecurityToken request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (request.ProofEncryption == null)
        return (EncryptingCredentials) null;
      if (request.ProofEncryption.GetSecurityToken() is X509SecurityToken securityToken)
        return (EncryptingCredentials) new X509EncryptingCredentials(securityToken);
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new RequestFailedException(Microsoft.IdentityModel.SR.GetString("ID2084", (object) request.ProofEncryption.GetSecurityToken())));
    }

    protected virtual Lifetime GetTokenLifetime(Lifetime requestLifetime)
    {
      DateTime dateTime;
      DateTime expires;
      if (requestLifetime == null)
      {
        dateTime = DateTime.UtcNow;
        expires = DateTimeUtil.Add(dateTime, this._securityTokenServiceConfiguration.DefaultTokenLifetime);
      }
      else
      {
        dateTime = !requestLifetime.Created.HasValue ? DateTime.UtcNow : requestLifetime.Created.Value;
        expires = !requestLifetime.Expires.HasValue ? DateTimeUtil.Add(dateTime, this._securityTokenServiceConfiguration.DefaultTokenLifetime) : requestLifetime.Expires.Value;
      }
      this.VerifyComputedLifetime(dateTime, expires);
      return new Lifetime(dateTime, expires);
    }

    private void VerifyComputedLifetime(DateTime created, DateTime expires)
    {
      DateTime utcNow = DateTime.UtcNow;
      if (DateTimeUtil.Add(DateTimeUtil.ToUniversalTime(expires), this._securityTokenServiceConfiguration.MaxClockSkew) < utcNow)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2075", (object) created, (object) expires, (object) utcNow)));
      if (DateTimeUtil.ToUniversalTime(created) > DateTimeUtil.Add(utcNow + TimeSpan.FromDays(1.0), this._securityTokenServiceConfiguration.MaxClockSkew))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2076", (object) created, (object) expires, (object) utcNow)));
      if (expires <= created)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2077", (object) created, (object) expires)));
      if (expires - created > this._securityTokenServiceConfiguration.MaximumTokenLifetime)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2078", (object) created, (object) expires, (object) this._securityTokenServiceConfiguration.MaximumTokenLifetime)));
    }

    protected virtual RequestSecurityTokenResponse GetResponse(
      RequestSecurityToken request,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        return (RequestSecurityTokenResponse) null;
      RequestSecurityTokenResponse response = new RequestSecurityTokenResponse((WSTrustMessage) request);
      tokenDescriptor.ApplyTo(response);
      if (request.ReplyTo != null)
        response.ReplyTo = tokenDescriptor.ReplyToAddress;
      if (!string.IsNullOrEmpty(tokenDescriptor.AppliesToAddress))
        response.AppliesTo = new EndpointAddress(tokenDescriptor.AppliesToAddress);
      return response;
    }

    public virtual RequestSecurityTokenResponse EndCancel(
      IAsyncResult result)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", (object) "Cancel")));
    }

    protected virtual Scope EndGetScope(IAsyncResult result) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID2081")));

    public virtual RequestSecurityTokenResponse EndIssue(
      IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      return result is TypedAsyncResult<RequestSecurityTokenResponse> ? TypedAsyncResult<RequestSecurityTokenResponse>.End(result) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2012", (object) typeof (TypedAsyncResult<RequestSecurityTokenResponse>), (object) result.GetType())));
    }

    public virtual RequestSecurityTokenResponse EndRenew(
      IAsyncResult result)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", (object) "Renew")));
    }

    public virtual RequestSecurityTokenResponse EndValidate(
      IAsyncResult result)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", (object) "Validate")));
    }

    protected abstract Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request);

    protected abstract IClaimsIdentity GetOutputClaimsIdentity(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      Scope scope);

    protected virtual IAsyncResult BeginGetOutputClaimsIdentity(
      IClaimsPrincipal principal,
      RequestSecurityToken request,
      Scope scope,
      AsyncCallback callback,
      object state)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID2081")));
    }

    protected virtual IClaimsIdentity EndGetOutputClaimsIdentity(IAsyncResult result) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID2081")));

    public virtual RequestSecurityTokenResponse Issue(
      IClaimsPrincipal principal,
      RequestSecurityToken request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      this._principal = principal;
      this._request = request;
      this.ValidateRequest(request);
      Scope scope = this.GetScope(principal, request);
      this.Scope = scope != null ? scope : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2013"));
      this.SecurityTokenDescriptor = this.CreateSecurityTokenDescriptor(request, scope);
      if (this.SecurityTokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2003"));
      if (this.SecurityTokenDescriptor.SigningCredentials == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2079"));
      if (this.Scope.TokenEncryptionRequired && this.SecurityTokenDescriptor.EncryptingCredentials == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4184"));
      Microsoft.IdentityModel.Tokens.SecurityTokenHandler securityTokenHandler = this.GetSecurityTokenHandler(request.TokenType);
      if (securityTokenHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID4010", (object) request.TokenType)));
      this._tokenDescriptor.TokenIssuerName = this.GetValidIssuerName();
      this._tokenDescriptor.Lifetime = this.GetTokenLifetime(request.Lifetime);
      this._tokenDescriptor.Proof = this.GetProofToken(request, scope);
      this._tokenDescriptor.Subject = this.GetOutputClaimsIdentity(principal, request, scope);
      if (!string.IsNullOrEmpty(request.TokenType))
      {
        this._tokenDescriptor.TokenType = request.TokenType;
      }
      else
      {
        string[] tokenTypeIdentifiers = securityTokenHandler.GetTokenTypeIdentifiers();
        this._tokenDescriptor.TokenType = tokenTypeIdentifiers != null && tokenTypeIdentifiers.Length != 0 ? tokenTypeIdentifiers[0] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4264", (object) request.TokenType)));
      }
      this._tokenDescriptor.Token = securityTokenHandler.CreateToken(this._tokenDescriptor);
      this._tokenDescriptor.AttachedReference = securityTokenHandler.CreateSecurityTokenReference(this._tokenDescriptor.Token, true);
      this._tokenDescriptor.UnattachedReference = securityTokenHandler.CreateSecurityTokenReference(this._tokenDescriptor.Token, false);
      if (request.RequestDisplayToken)
        this._tokenDescriptor.DisplayToken = this.GetDisplayToken(request.DisplayTokenLanguage, this._tokenDescriptor.Subject);
      return this.GetResponse(request, this._tokenDescriptor);
    }

    protected virtual DisplayToken GetDisplayToken(
      string requestedDisplayTokenLanguage,
      IClaimsIdentity subject)
    {
      return (DisplayToken) null;
    }

    protected virtual Microsoft.IdentityModel.Tokens.SecurityTokenHandler GetSecurityTokenHandler(
      string requestedTokenType)
    {
      return this._securityTokenServiceConfiguration.SecurityTokenHandlers[string.IsNullOrEmpty(requestedTokenType) ? this._securityTokenServiceConfiguration.DefaultTokenType : requestedTokenType];
    }

    private void OnGetScopeComplete(IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      if (!(result.AsyncState is Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.FederatedAsyncState asyncState))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2001")));
      Exception exception = (Exception) null;
      if (!(asyncState.Result is TypedAsyncResult<RequestSecurityTokenResponse> result1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (TypedAsyncResult<RequestSecurityTokenResponse>), (object) asyncState.Result.GetType())));
      RequestSecurityToken request = asyncState.Request;
      try
      {
        Scope scope = this.EndGetScope(result);
        this.Scope = scope != null ? scope : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2013"));
        this.SecurityTokenDescriptor = this.CreateSecurityTokenDescriptor(request, this.Scope);
        if (this.SecurityTokenDescriptor == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2003"));
        if (this.SecurityTokenDescriptor.SigningCredentials == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2079"));
        if (this.Scope.TokenEncryptionRequired && this.SecurityTokenDescriptor.EncryptingCredentials == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4184"));
        asyncState.SecurityTokenHandler = this.GetSecurityTokenHandler(request == null ? (string) null : request.TokenType) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID4010", request == null ? (object) string.Empty : (object) request.TokenType)));
        this._tokenDescriptor.TokenIssuerName = this.GetValidIssuerName();
        this._tokenDescriptor.Lifetime = this.GetTokenLifetime(request == null ? (Lifetime) null : request.Lifetime);
        this._tokenDescriptor.Proof = this.GetProofToken(request, this.Scope);
        this.BeginGetOutputClaimsIdentity(asyncState.ClaimsPrincipal, asyncState.Request, scope, new AsyncCallback(this.OnGetOutputClaimsIdentityComplete), (object) asyncState);
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      if (exception == null)
        return;
      result1.Complete((RequestSecurityTokenResponse) null, result.CompletedSynchronously, exception);
    }

    private void OnGetOutputClaimsIdentityComplete(IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      if (!(result.AsyncState is Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.FederatedAsyncState asyncState))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2001")));
      Microsoft.IdentityModel.Tokens.SecurityTokenHandler securityTokenHandler = asyncState.SecurityTokenHandler;
      if (securityTokenHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2016")));
      Exception exception = (Exception) null;
      RequestSecurityToken request = asyncState.Request;
      RequestSecurityTokenResponse result1 = (RequestSecurityTokenResponse) null;
      if (!(asyncState.Result is TypedAsyncResult<RequestSecurityTokenResponse> result2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (TypedAsyncResult<RequestSecurityTokenResponse>), (object) asyncState.Result.GetType())));
      try
      {
        if (this._tokenDescriptor == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2003"));
        this._tokenDescriptor.Subject = this.EndGetOutputClaimsIdentity(result);
        if (!string.IsNullOrEmpty(request.TokenType))
        {
          this._tokenDescriptor.TokenType = request.TokenType;
        }
        else
        {
          string[] tokenTypeIdentifiers = securityTokenHandler.GetTokenTypeIdentifiers();
          this._tokenDescriptor.TokenType = tokenTypeIdentifiers != null && tokenTypeIdentifiers.Length != 0 ? tokenTypeIdentifiers[0] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4264", (object) request.TokenType)));
        }
        this._tokenDescriptor.Token = securityTokenHandler.CreateToken(this._tokenDescriptor);
        this._tokenDescriptor.AttachedReference = securityTokenHandler.CreateSecurityTokenReference(this._tokenDescriptor.Token, true);
        this._tokenDescriptor.UnattachedReference = securityTokenHandler.CreateSecurityTokenReference(this._tokenDescriptor.Token, false);
        if (request != null && request.RequestDisplayToken)
          this._tokenDescriptor.DisplayToken = this.GetDisplayToken(request.DisplayTokenLanguage, this._tokenDescriptor.Subject);
        result1 = this.GetResponse(request, this._tokenDescriptor);
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      result2.Complete(result1, result2.CompletedSynchronously, exception);
    }

    public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => this._securityTokenServiceConfiguration;

    public IClaimsPrincipal Principal
    {
      get => this._principal;
      set => this._principal = value;
    }

    public RequestSecurityToken Request
    {
      get => this._request;
      set => this._request = value;
    }

    public Scope Scope { get; set; }

    protected Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor SecurityTokenDescriptor
    {
      get => this._tokenDescriptor;
      set => this._tokenDescriptor = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public virtual RequestSecurityTokenResponse Renew(
      IClaimsPrincipal principal,
      RequestSecurityToken request)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request == null || request.RequestType == null ? (object) nameof (Renew) : (object) request.RequestType)));
    }

    public virtual RequestSecurityTokenResponse Validate(
      IClaimsPrincipal principal,
      RequestSecurityToken request)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3141", request == null || request.RequestType == null ? (object) nameof (Validate) : (object) request.RequestType)));
    }

    protected virtual void ValidateRequest(RequestSecurityToken request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2051")));
      if (request.RequestType != null && request.RequestType != "http://schemas.microsoft.com/idfx/requesttype/issue")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2052")));
      if (request.KeyType != null && !Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.IsKnownType(request.KeyType))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2053")));
      if (StringComparer.Ordinal.Equals(request.KeyType, "http://schemas.microsoft.com/idfx/keytype/bearer") && request.KeySizeInBits.HasValue && request.KeySizeInBits.Value != 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2050")));
      if (this.GetSecurityTokenHandler(request.TokenType) == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new UnsupportedTokenTypeBadRequestException(request.TokenType));
      request.KeyType = string.IsNullOrEmpty(request.KeyType) ? "http://schemas.microsoft.com/idfx/keytype/symmetric" : request.KeyType;
      if (!StringComparer.Ordinal.Equals(request.KeyType, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
        return;
      if (request.KeySizeInBits.HasValue)
      {
        if (request.KeySizeInBits.Value > this._securityTokenServiceConfiguration.DefaultMaxSymmetricKeySizeInBits)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID2056", (object) request.KeySizeInBits.Value, (object) this._securityTokenServiceConfiguration.DefaultMaxSymmetricKeySizeInBits)));
      }
      else
        request.KeySizeInBits = new int?(this._securityTokenServiceConfiguration.DefaultSymmetricKeySizeInBits);
    }

    private static bool IsKnownType(string keyType) => StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/symmetric") || StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/asymmetric") || StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/bearer");

    protected class FederatedAsyncState
    {
      private RequestSecurityToken _request;
      private IClaimsPrincipal _claimsPrincipal;
      private Microsoft.IdentityModel.Tokens.SecurityTokenHandler _securityTokenHandler;
      private IAsyncResult _result;

      public FederatedAsyncState(
        Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService.FederatedAsyncState federatedAsyncState)
      {
        this._request = federatedAsyncState != null ? federatedAsyncState.Request : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (FederatedAsyncState));
        this._claimsPrincipal = federatedAsyncState.ClaimsPrincipal;
        this._securityTokenHandler = federatedAsyncState.SecurityTokenHandler;
        this._result = federatedAsyncState.Result;
      }

      public FederatedAsyncState(
        RequestSecurityToken request,
        IClaimsPrincipal principal,
        IAsyncResult result)
      {
        if (request == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
        if (result == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
        this._request = request;
        this._claimsPrincipal = principal;
        this._result = result;
      }

      public RequestSecurityToken Request => this._request;

      public IClaimsPrincipal ClaimsPrincipal => this._claimsPrincipal;

      public Microsoft.IdentityModel.Tokens.SecurityTokenHandler SecurityTokenHandler
      {
        get => this._securityTokenHandler;
        set => this._securityTokenHandler = value;
      }

      public IAsyncResult Result => this._result;
    }
  }
}
