// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.RsaSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class RsaSecurityTokenHandler : SecurityTokenHandler
  {
    private static string[] _tokenTypeIdentifiers = new string[1]
    {
      "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Rsa"
    };

    public override bool CanReadToken(XmlReader reader) => reader != null ? reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public override bool CanValidateToken => true;

    public override bool CanWriteToken => true;

    public override string[] GetTokenTypeIdentifiers() => RsaSecurityTokenHandler._tokenTypeIdentifiers;

    public override SecurityToken ReadToken(XmlReader reader)
    {
      XmlDictionaryReader dictionaryReader = reader != null ? XmlDictionaryReader.CreateDictionaryReader(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!dictionaryReader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "KeyInfo", (object) "http://www.w3.org/2000/09/xmldsig#", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI)));
      dictionaryReader.ReadStartElement();
      if (!dictionaryReader.IsStartElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "KeyValue", (object) "http://www.w3.org/2000/09/xmldsig#", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI)));
      dictionaryReader.ReadStartElement();
      RSA rsa = (RSA) new RSACryptoServiceProvider();
      rsa.FromXmlString(dictionaryReader.ReadOuterXml());
      dictionaryReader.ReadEndElement();
      dictionaryReader.ReadEndElement();
      return (SecurityToken) new RsaSecurityToken(rsa);
    }

    public override Type TokenType => typeof (RsaSecurityToken);

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      RsaSecurityToken rsaSecurityToken = token != null ? (RsaSecurityToken) token : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (rsaSecurityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (RsaSecurityToken)));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      IClaimsIdentity claimsIdentity = (IClaimsIdentity) new RsaClaimsIdentity((IEnumerable<Claim>) new Claim[1]
      {
        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa", rsaSecurityToken.Rsa.ToXmlString(false), "http://www.w3.org/2000/09/xmldsig#RSAKeyValue", "LOCAL AUTHORITY")
      }, "Signature");
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature"));
      if (this.Configuration.SaveBootstrapTokens)
        claimsIdentity.BootstrapToken = token;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        claimsIdentity
      });
    }

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      RSAParameters rsaParameters = token is RsaSecurityToken rsaSecurityToken ? rsaSecurityToken.Rsa.ExportParameters(false) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (RsaSecurityToken)));
      writer.WriteStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteStartElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteStartElement("RsaKeyValue", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteStartElement("Modulus", "http://www.w3.org/2000/09/xmldsig#");
      byte[] modulus = rsaParameters.Modulus;
      writer.WriteBase64(modulus, 0, modulus.Length);
      writer.WriteEndElement();
      writer.WriteStartElement("Exponent", "http://www.w3.org/2000/09/xmldsig#");
      byte[] exponent = rsaParameters.Exponent;
      writer.WriteBase64(exponent, 0, exponent.Length);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
