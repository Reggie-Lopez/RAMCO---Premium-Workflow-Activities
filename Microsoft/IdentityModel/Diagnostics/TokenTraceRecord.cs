// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.TokenTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class TokenTraceRecord : TraceRecord
  {
    internal new const string ElementName = "TokenTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/TokenTraceRecord";
    private SecurityToken _securityToken;

    public TokenTraceRecord(SecurityToken securityToken) => this._securityToken = securityToken;

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/TokenTraceRecord";

    private void WriteSessionToken(XmlWriter writer, Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken) => TokenTraceRecord.GetOrCreateSessionSecurityTokenHandler().WriteToken((XmlWriter) XmlDictionaryWriter.CreateDictionaryWriter(writer), (SecurityToken) sessionToken);

    private static Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler GetOrCreateSessionSecurityTokenHandler()
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
      if (!(handlerCollection[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
      {
        securityTokenHandler = new Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler();
        handlerCollection.AddOrReplace((Microsoft.IdentityModel.Tokens.SecurityTokenHandler) securityTokenHandler);
      }
      return securityTokenHandler;
    }

    private static string GenerateSessionIdFromCookie(byte[] cookie)
    {
      byte[] hash;
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewDefaultHash())
        hash = hashAlgorithm.ComputeHash(cookie);
      return Convert.ToBase64String(hash);
    }

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (TokenTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteStartElement("SecurityToken");
      writer.WriteAttributeString("Type", this._securityToken.GetType().ToString());
      if (this._securityToken is Microsoft.IdentityModel.Tokens.SessionSecurityToken)
      {
        this.WriteSessionToken(writer, this._securityToken as Microsoft.IdentityModel.Tokens.SessionSecurityToken);
      }
      else
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
        if (handlerCollection.CanWriteToken(this._securityToken))
          handlerCollection.WriteToken(writer, this._securityToken);
        else
          writer.WriteElementString("Warning", Microsoft.IdentityModel.SR.GetString("TraceUnableToWriteToken", (object) this._securityToken.GetType().ToString()));
      }
      writer.WriteEndElement();
    }
  }
}
