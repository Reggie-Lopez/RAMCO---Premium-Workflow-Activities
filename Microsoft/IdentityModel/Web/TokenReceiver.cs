// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.TokenReceiver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;

namespace Microsoft.IdentityModel.Web
{
  internal class TokenReceiver
  {
    private ServiceConfiguration _serviceConfiguration;

    public TokenReceiver(ServiceConfiguration serviceConfiguration) => this._serviceConfiguration = serviceConfiguration != null ? serviceConfiguration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceConfiguration));

    public SecurityToken ReadToken(XmlReader reader)
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = this._serviceConfiguration.SecurityTokenHandlers;
      return securityTokenHandlers.CanReadToken(reader) ? securityTokenHandlers.ReadToken(reader) : (SecurityToken) null;
    }

    public SecurityToken ReadToken(
      string tokenXml,
      XmlDictionaryReaderQuotas readerQuotas)
    {
      if (string.IsNullOrEmpty(tokenXml))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (tokenXml));
      try
      {
        using (StringReader stringReader1 = new StringReader(tokenXml))
        {
          StringReader stringReader2 = stringReader1;
          XmlReaderSettings settings = new XmlReaderSettings()
          {
            XmlResolver = (XmlResolver) null
          };
          using (XmlDictionaryReader dictionaryReader = (XmlDictionaryReader) new IdentityModelWrappedXmlDictionaryReader(XmlReader.Create((TextReader) stringReader2, settings), readerQuotas))
          {
            int content = (int) dictionaryReader.MoveToContent();
            string localName = dictionaryReader.LocalName;
            string namespaceUri = dictionaryReader.NamespaceURI;
            return this.ReadToken((XmlReader) dictionaryReader) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4014", (object) localName, (object) namespaceUri)));
          }
        }
      }
      catch (Microsoft.IdentityModel.Tokens.EncryptedTokenDecryptionFailedException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID1044", (object) (this._serviceConfiguration.ServiceCertificate != null ? "[Thumbprint] " + this._serviceConfiguration.ServiceCertificate.Thumbprint : Microsoft.IdentityModel.SR.GetString("NoCert"))), (Exception) ex));
      }
    }

    public IClaimsPrincipal AuthenticateToken(
      SecurityToken token,
      bool ensureBearerToken,
      string endpointUri)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (ensureBearerToken && token.SecurityKeys != null && token.SecurityKeys.Count != 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID1020")));
      ClaimsIdentityCollection identities = this._serviceConfiguration.SecurityTokenHandlers.ValidateToken(token);
      return this._serviceConfiguration.ClaimsAuthenticationManager.Authenticate(endpointUri, ClaimsPrincipal.CreateFromIdentities(identities));
    }

    public TimeSpan ConfiguredSessionTokenLifeTime
    {
      get
      {
        TimeSpan timeSpan = Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler.DefaultTokenLifetime;
        if (this._serviceConfiguration.SecurityTokenHandlers != null && this._serviceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler)
          timeSpan = securityTokenHandler.TokenLifetime;
        return timeSpan;
      }
    }

    public void ComputeSessionTokenLifeTime(
      SecurityToken securityToken,
      out DateTime validFrom,
      out DateTime validTo)
    {
      TimeSpan sessionTokenLifeTime = this.ConfiguredSessionTokenLifeTime;
      validFrom = DateTime.UtcNow;
      validTo = DateTimeUtil.AddNonNegative(validFrom, sessionTokenLifeTime);
      if (securityToken == null)
        return;
      if (validFrom < securityToken.ValidFrom)
        validFrom = securityToken.ValidFrom;
      if (!(validTo > securityToken.ValidTo))
        return;
      validTo = securityToken.ValidTo;
    }
  }
}
