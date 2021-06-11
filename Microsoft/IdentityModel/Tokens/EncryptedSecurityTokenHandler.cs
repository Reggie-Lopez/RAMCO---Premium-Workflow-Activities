// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.EncryptedSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.XmlEncryption;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class EncryptedSecurityTokenHandler : SecurityTokenHandler
  {
    private static string[] _tokenTypeIdentifiers = new string[1];
    private SecurityTokenSerializer _keyInfoSerializer;
    private object _syncObject = new object();

    public override bool CanReadKeyIdentifierClause(XmlReader reader) => reader != null ? reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public override bool CanReadToken(XmlReader reader) => EncryptedDataElement.CanReadFrom(reader);

    public override bool CanWriteToken => true;

    public SecurityTokenSerializer KeyInfoSerializer
    {
      get
      {
        if (this._keyInfoSerializer == null)
        {
          lock (this._syncObject)
          {
            if (this._keyInfoSerializer == null)
              this._keyInfoSerializer = (SecurityTokenSerializer) new SecurityTokenSerializerAdapter(this.ContainingCollection != null ? this.ContainingCollection : SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(), SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, false, (SamlSerializer) null, (SecurityStateEncoder) null, (IEnumerable<Type>) null);
          }
        }
        return this._keyInfoSerializer;
      }
      set => this._keyInfoSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public override SecurityToken ReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.ServiceTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4276"));
      EncryptedDataElement encryptedDataElement = new EncryptedDataElement(this.KeyInfoSerializer);
      encryptedDataElement.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
      SecurityKey key = (SecurityKey) null;
      foreach (SecurityKeyIdentifierClause keyIdentifierClause in encryptedDataElement.KeyIdentifier)
      {
        this.Configuration.ServiceTokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key);
        if (key != null)
          break;
      }
      if (key == null && encryptedDataElement.KeyIdentifier.CanCreateKey)
        key = encryptedDataElement.KeyIdentifier.CreateKey();
      if (key == null)
      {
        if (encryptedDataElement.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out EncryptedKeyIdentifierClause _))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new EncryptedTokenDecryptionFailedException(Microsoft.IdentityModel.SR.GetString("ID4036", (object) XmlUtil.SerializeSecurityKeyIdentifier(encryptedDataElement.KeyIdentifier, (SecurityTokenSerializer) WSSecurityTokenSerializer.DefaultInstance))));
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new EncryptedTokenDecryptionFailedException(Microsoft.IdentityModel.SR.GetString("ID4036", (object) encryptedDataElement.KeyIdentifier.ToString())));
      }
      if (!(key is SymmetricSecurityKey symmetricSecurityKey))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4023")));
      byte[] buffer;
      using (SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedDataElement.Algorithm))
        buffer = encryptedDataElement.Decrypt(symmetricAlgorithm);
      using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(buffer, XmlDictionaryReaderQuotas.Max))
        return this.ContainingCollection != null && this.ContainingCollection.CanReadToken(textReader) ? this.ContainingCollection.ReadToken(textReader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4014", (object) textReader.LocalName, (object) textReader.NamespaceURI));
    }

    public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#"))
      {
        EncryptedKeyElement encryptedKeyElement = new EncryptedKeyElement(this.KeyInfoSerializer);
        encryptedKeyElement.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
        return (SecurityKeyIdentifierClause) new EncryptedKeyIdentifierClause(encryptedKeyElement.CipherData.CipherValue, encryptedKeyElement.Algorithm, encryptedKeyElement.KeyIdentifier);
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3275", (object) reader.Name, (object) reader.NamespaceURI)));
    }

    [Conditional("DEBUG")]
    private static void DebugEncryptedTokenClearText(byte[] bytes, Encoding encoding) => encoding.GetString(bytes);

    public override Type TokenType => typeof (EncryptedSecurityToken);

    public override string[] GetTokenTypeIdentifiers() => EncryptedSecurityTokenHandler._tokenTypeIdentifiers;

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is EncryptedSecurityToken encryptedSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID4024"));
      if (this.ContainingCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4279"));
      EncryptedDataElement encryptedDataElement = new EncryptedDataElement(this.KeyInfoSerializer);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
          (this.ContainingCollection[encryptedSecurityToken.Token.GetType()] ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4224", (object) encryptedSecurityToken.Token.GetType()))).WriteToken((XmlWriter) textWriter, encryptedSecurityToken.Token);
        Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = encryptedSecurityToken.EncryptingCredentials;
        encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
        encryptedDataElement.KeyIdentifier = encryptingCredentials.SecurityKeyIdentifier;
        encryptedDataElement.Algorithm = encryptingCredentials.Algorithm;
        if (!(encryptingCredentials.SecurityKey is SymmetricSecurityKey securityKey))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID3064")));
        using (SymmetricAlgorithm symmetricAlgorithm = securityKey.GetSymmetricAlgorithm(encryptingCredentials.Algorithm))
        {
          byte[] buffer = memoryStream.GetBuffer();
          encryptedDataElement.Encrypt(symmetricAlgorithm, buffer, 0, (int) memoryStream.Length);
        }
      }
      encryptedDataElement.WriteXml(writer, this.KeyInfoSerializer);
    }
  }
}
