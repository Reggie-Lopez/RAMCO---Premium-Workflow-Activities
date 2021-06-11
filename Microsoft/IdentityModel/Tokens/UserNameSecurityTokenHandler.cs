// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.UserNameSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public abstract class UserNameSecurityTokenHandler : SecurityTokenHandler
  {
    private bool _retainPassword;

    public virtual bool RetainPassword
    {
      get => this._retainPassword;
      set => this._retainPassword = value;
    }

    public override bool CanReadToken(XmlReader reader) => reader != null ? reader.IsStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public override bool CanWriteToken => true;

    public override Type TokenType => typeof (UserNameSecurityToken);

    public override string[] GetTokenTypeIdentifiers() => new string[1]
    {
      "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName"
    };

    public override SecurityToken ReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!this.CanReadToken(reader))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "Username", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", (object) reader.LocalName, (object) reader.NamespaceURI)));
      string userName = (string) null;
      string password = (string) null;
      int content = (int) reader.MoveToContent();
      string attribute1 = reader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
      reader.ReadStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
          userName = reader.ReadElementString();
        else if (reader.IsStartElement("Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
        {
          string attribute2 = reader.GetAttribute("Type", (string) null);
          if (!string.IsNullOrEmpty(attribute2) && !StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID4059", (object) attribute2, (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText")));
          password = reader.ReadElementString();
        }
        else if (reader.IsStartElement("Nonce", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
          reader.Skip();
        else if (reader.IsStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
          reader.Skip();
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4060", (object) reader.LocalName, (object) reader.NamespaceURI, (object) "UsernameToken", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")));
      }
      reader.ReadEndElement();
      if (string.IsNullOrEmpty(userName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4061"));
      return !string.IsNullOrEmpty(attribute1) ? (SecurityToken) new UserNameSecurityToken(userName, password, attribute1) : (SecurityToken) new UserNameSecurityToken(userName, password);
    }

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is UserNameSecurityToken nameSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (UserNameSecurityToken)));
      writer.WriteStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      if (!string.IsNullOrEmpty(token.Id))
        writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", token.Id);
      writer.WriteElementString("Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", nameSecurityToken.UserName);
      if (nameSecurityToken.Password != null)
      {
        writer.WriteStartElement("Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        writer.WriteAttributeString("Type", (string) null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
        writer.WriteString(nameSecurityToken.Password);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
      writer.Flush();
    }
  }
}
