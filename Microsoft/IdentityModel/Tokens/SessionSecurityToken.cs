// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SessionSecurityToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SessionSecurityToken : SecurityToken
  {
    private SecurityContextSecurityToken _securityContextToken;
    private bool _securityContextTokenWrapper;
    private SecureConversationVersion _scVersion = SessionSecurityTokenHandler.DefaultSecureConversationVersion;
    private string _context;
    private bool _isPersistent;
    private IClaimsPrincipal _claimsPrincipal;
    private object _claimsPrincipalLock = new object();
    private string _endpointId;
    private bool _isSessionMode;

    public SessionSecurityToken(IClaimsPrincipal claimsPrincipal)
      : this(claimsPrincipal, (string) null)
    {
    }

    public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, TimeSpan lifetime)
      : this(claimsPrincipal, (string) null, new DateTime?(DateTime.UtcNow), new DateTime?(DateTimeUtil.AddNonNegative(DateTime.UtcNow, lifetime)))
    {
    }

    public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, string context)
      : this(claimsPrincipal, context, new DateTime?(DateTime.UtcNow), new DateTime?(DateTimeUtil.AddNonNegative(DateTime.UtcNow, SessionSecurityTokenHandler.DefaultTokenLifetime)))
    {
    }

    public SessionSecurityToken(
      IClaimsPrincipal claimsPrincipal,
      string context,
      DateTime? validFrom,
      DateTime? validTo)
      : this(claimsPrincipal, new System.Xml.UniqueId(), context, string.Empty, validFrom, validTo, (SymmetricSecurityKey) null)
    {
    }

    public SessionSecurityToken(
      IClaimsPrincipal claimsPrincipal,
      string context,
      string endpointId,
      DateTime? validFrom,
      DateTime? validTo)
      : this(claimsPrincipal, new System.Xml.UniqueId(), context, endpointId, validFrom, validTo, (SymmetricSecurityKey) null)
    {
    }

    public SessionSecurityToken(
      IClaimsPrincipal claimsPrincipal,
      System.Xml.UniqueId contextId,
      string context,
      string endpointId,
      TimeSpan lifetime,
      SymmetricSecurityKey key)
      : this(claimsPrincipal, contextId, context, endpointId, DateTime.UtcNow, lifetime, key)
    {
    }

    public SessionSecurityToken(
      IClaimsPrincipal claimsPrincipal,
      System.Xml.UniqueId contextId,
      string context,
      string endpointId,
      DateTime validFrom,
      TimeSpan lifetime,
      SymmetricSecurityKey key)
      : this(claimsPrincipal, contextId, context, endpointId, new DateTime?(validFrom), new DateTime?(DateTimeUtil.AddNonNegative(validFrom, lifetime)), key)
    {
    }

    public SessionSecurityToken(
      IClaimsPrincipal claimsPrincipal,
      System.Xml.UniqueId contextId,
      string context,
      string endpointId,
      DateTime? validFrom,
      DateTime? validTo,
      SymmetricSecurityKey key)
    {
      if (claimsPrincipal == null || claimsPrincipal.Identities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsPrincipal));
      if (contextId == (System.Xml.UniqueId) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (contextId));
      DateTime dateTime1 = !validFrom.HasValue ? DateTime.UtcNow : DateTimeUtil.ToUniversalTime(validFrom.Value);
      DateTime dateTime2 = !validTo.HasValue ? DateTimeUtil.Add(dateTime1, SessionSecurityTokenHandler.DefaultTokenLifetime) : DateTimeUtil.ToUniversalTime(validTo.Value);
      if (dateTime1 >= dateTime2)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (validFrom));
      if (dateTime2 < DateTime.UtcNow)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (validTo));
      if (endpointId == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointId));
      this._claimsPrincipal = claimsPrincipal;
      IAuthorizationPolicy[] authorizationPolicyArray = new IAuthorizationPolicy[2]
      {
        (IAuthorizationPolicy) new AuthorizationPolicy(claimsPrincipal.Identities),
        (IAuthorizationPolicy) new EndpointAuthorizationPolicy(endpointId)
      };
      byte[] key1 = key == null ? KeyGenerator.GenerateSymmetricKey(128) : key.GetSymmetricKey();
      this._securityContextToken = new SecurityContextSecurityToken(contextId, Microsoft.IdentityModel.UniqueId.CreateUniqueId(), key1, dateTime1, dateTime2, new System.Xml.UniqueId(), dateTime1, dateTime2, new ReadOnlyCollection<IAuthorizationPolicy>((IList<IAuthorizationPolicy>) authorizationPolicyArray));
      this._context = context;
      this._endpointId = endpointId;
    }

    internal SessionSecurityToken(
      System.Xml.UniqueId contextId,
      string id,
      string context,
      byte[] key,
      string endpointId,
      DateTime validFrom,
      DateTime validTo,
      System.Xml.UniqueId keyGeneration,
      DateTime keyEffectiveTime,
      DateTime keyExpirationTime,
      ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies)
    {
      if (key == null)
        key = KeyGenerator.GenerateSymmetricKey(128);
      if (endpointId == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointId));
      this._securityContextToken = new SecurityContextSecurityToken(contextId, id, key, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, authorizationPolicies);
      this._context = context;
      this._endpointId = endpointId;
    }

    public SessionSecurityToken(
      SecurityContextSecurityToken securityContextToken,
      SecureConversationVersion version)
    {
      this._securityContextToken = securityContextToken != null ? securityContextToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityContextToken));
      this._endpointId = this.GetEndpointId(this._securityContextToken) ?? string.Empty;
      this._securityContextTokenWrapper = true;
      this._scVersion = version;
      this._isSessionMode = !securityContextToken.IsCookieMode;
    }

    public IClaimsPrincipal ClaimsPrincipal
    {
      get
      {
        if (this._claimsPrincipal == null)
        {
          lock (this._claimsPrincipalLock)
          {
            if (this._claimsPrincipal == null)
            {
              if (this._securityContextToken.AuthorizationPolicies != null && this._securityContextToken.AuthorizationPolicies.Count > 0)
              {
                authorizationPolicy = (AuthorizationPolicy) null;
                int index = 0;
                while (index < this._securityContextToken.AuthorizationPolicies.Count && !(this._securityContextToken.AuthorizationPolicies[index] is AuthorizationPolicy authorizationPolicy))
                  ++index;
                if (authorizationPolicy != null && authorizationPolicy.IdentityCollection != null)
                  this._claimsPrincipal = Microsoft.IdentityModel.Claims.ClaimsPrincipal.CreateFromIdentities(authorizationPolicy.IdentityCollection);
              }
              if (this._claimsPrincipal == null)
                this._claimsPrincipal = !this._securityContextTokenWrapper ? Microsoft.IdentityModel.Claims.ClaimsPrincipal.AnonymousPrincipal : (IClaimsPrincipal) new Microsoft.IdentityModel.Claims.ClaimsPrincipal();
            }
          }
        }
        return this._claimsPrincipal;
      }
    }

    public string Context => this._context;

    public System.Xml.UniqueId ContextId => this._securityContextToken.ContextId;

    public string EndpointId => this._endpointId;

    public DateTime KeyEffectiveTime => this._securityContextToken.KeyEffectiveTime;

    public DateTime KeyExpirationTime => this._securityContextToken.KeyExpirationTime;

    public System.Xml.UniqueId KeyGeneration => this._securityContextToken.KeyGeneration;

    public override string Id => this._securityContextToken.Id;

    public bool IsPersistent
    {
      get => this._isPersistent;
      set => this._isPersistent = value;
    }

    internal bool IsSecurityContextSecurityTokenWrapper => this._securityContextTokenWrapper;

    public bool IsSessionMode
    {
      get => this._isSessionMode;
      set => this._isSessionMode = value;
    }

    public SecureConversationVersion SecureConversationVersion => this._scVersion;

    internal SecurityContextSecurityToken SecurityContextSecurityToken => this._securityContextToken;

    public override ReadOnlyCollection<SecurityKey> SecurityKeys => this._securityContextToken.SecurityKeys;

    public override DateTime ValidFrom => this._securityContextToken.ValidFrom;

    public override DateTime ValidTo => this._securityContextToken.ValidTo;

    private string GetEndpointId(SecurityContextSecurityToken sct)
    {
      if (sct == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sct));
      for (int index = 0; index < sct.AuthorizationPolicies.Count; ++index)
      {
        if (sct.AuthorizationPolicies[index] is EndpointAuthorizationPolicy authorizationPolicy)
          return authorizationPolicy.EndpointId;
      }
      return (string) null;
    }
  }
}
