// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.FederatedClientCredentialsSecurityTokenManager
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class FederatedClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
  {
    private FederatedClientCredentials _federatedClientCredentials;

    public FederatedClientCredentialsSecurityTokenManager(
      FederatedClientCredentials federatedClientCredentials)
      : base((ClientCredentials) federatedClientCredentials)
    {
      this._federatedClientCredentials = federatedClientCredentials;
    }

    public override SecurityTokenProvider CreateSecurityTokenProvider(
      SecurityTokenRequirement tokenRequirement)
    {
      FederatedClientCredentialsParameters federatedClientCredentialsParameters = FederatedClientCredentialsSecurityTokenManager.FindIssuedTokenClientCredentialsParameters(tokenRequirement) ?? new FederatedClientCredentialsParameters();
      if (federatedClientCredentialsParameters.IssuedSecurityToken != null && this.IsIssuedSecurityTokenRequirement(tokenRequirement) && !FederatedClientCredentialsSecurityTokenManager.IsNegoOrSCTIssuedToken(tokenRequirement))
        return (SecurityTokenProvider) new SimpleSecurityTokenProvider(federatedClientCredentialsParameters.IssuedSecurityToken, tokenRequirement);
      SecurityTokenProvider securityTokenProvider = base.CreateSecurityTokenProvider(tokenRequirement);
      return securityTokenProvider is IssuedSecurityTokenProvider federatedSecurityTokenProvider ? (SecurityTokenProvider) new FederatedSecurityTokenProvider(federatedClientCredentialsParameters, federatedSecurityTokenProvider, this._federatedClientCredentials.SecurityTokenHandlerCollectionManager) : securityTokenProvider;
    }

    public override SecurityTokenSerializer CreateSecurityTokenSerializer(
      SecurityTokenVersion version)
    {
      if (version == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (version));
      TrustVersion trustVersion = TrustVersion.WSTrust13;
      SecureConversationVersion secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
      SecurityVersion wsSecurity11 = SecurityVersion.WSSecurity11;
      foreach (string securitySpecification in version.GetSecuritySpecifications())
      {
        if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
          trustVersion = TrustVersion.WSTrustFeb2005;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
          trustVersion = TrustVersion.WSTrust13;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/sc"))
          secureConversationVersion = SecureConversationVersion.WSSecureConversationFeb2005;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
          secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
      }
      SecurityVersion securityVersion = Microsoft.IdentityModel.Tokens.FederatedSecurityTokenManager.GetSecurityVersion(version);
      return (SecurityTokenSerializer) new Microsoft.IdentityModel.Tokens.SecurityTokenSerializerAdapter((this.ClientCredentials is FederatedClientCredentials clientCredentials ? clientCredentials.SecurityTokenHandlerCollectionManager : Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())[""], securityVersion, trustVersion, secureConversationVersion, false, (SamlSerializer) null, (SecurityStateEncoder) null, (IEnumerable<System.Type>) null);
    }

    internal static FederatedClientCredentialsParameters FindIssuedTokenClientCredentialsParameters(
      SecurityTokenRequirement tokenRequirement)
    {
      credentialsParameters = (FederatedClientCredentialsParameters) null;
      ChannelParameterCollection result = (ChannelParameterCollection) null;
      if (tokenRequirement.TryGetProperty<ChannelParameterCollection>(ServiceModelSecurityTokenRequirement.ChannelParametersCollectionProperty, out result) && result != null)
      {
        foreach (object obj in (Collection<object>) result)
        {
          if (obj is FederatedClientCredentialsParameters credentialsParameters)
            break;
        }
      }
      return credentialsParameters;
    }

    internal static bool IsNegoOrSCTIssuedToken(SecurityTokenRequirement tokenRequirement)
    {
      string tokenType = tokenRequirement.TokenType;
      return tokenType == ServiceModelSecurityTokenTypes.AnonymousSslnego || tokenType == ServiceModelSecurityTokenTypes.MutualSslnego || tokenType == ServiceModelSecurityTokenTypes.SecureConversation || tokenType == ServiceModelSecurityTokenTypes.Spnego;
    }
  }
}
