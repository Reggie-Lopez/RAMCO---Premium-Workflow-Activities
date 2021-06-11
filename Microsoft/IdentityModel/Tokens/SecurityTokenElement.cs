// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenElement
  {
    private SecurityToken _securityToken;
    private XmlElement _securityTokenXml;
    private SecurityTokenHandlerCollection _securityTokenHandlers;
    private ClaimsIdentityCollection _subject;

    public SecurityTokenElement(SecurityToken securityToken)
    {
      if (securityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityToken));
      if (securityToken is GenericXmlSecurityToken xmlSecurityToken)
        this._securityTokenXml = xmlSecurityToken.TokenXml;
      this._securityToken = securityToken;
    }

    public SecurityTokenElement(
      XmlElement securityTokenXml,
      SecurityTokenHandlerCollection securityTokenHandlers)
    {
      if (securityTokenXml == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenXml));
      if (securityTokenHandlers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlers));
      this._securityTokenXml = securityTokenXml;
      this._securityTokenHandlers = securityTokenHandlers;
    }

    public XmlElement SecurityTokenXml => this._securityTokenXml;

    public SecurityToken GetSecurityToken()
    {
      if (this._securityToken == null)
        this._securityToken = this.ReadSecurityToken(this._securityTokenXml, this._securityTokenHandlers);
      return this._securityToken;
    }

    public ClaimsIdentityCollection GetSubject()
    {
      if (this._subject == null)
        this._subject = this.CreateSubject(this._securityTokenXml, this._securityTokenHandlers);
      return this._subject;
    }

    protected virtual ClaimsIdentityCollection CreateSubject(
      XmlElement securityTokenXml,
      SecurityTokenHandlerCollection securityTokenHandlers)
    {
      if (securityTokenXml == null || securityTokenHandlers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4052"));
      SecurityToken securityToken = this.GetSecurityToken();
      return securityTokenHandlers.ValidateToken(securityToken);
    }

    protected virtual SecurityToken ReadSecurityToken(
      XmlElement securityTokenXml,
      SecurityTokenHandlerCollection securityTokenHandlers)
    {
      XmlReader reader = (XmlReader) new XmlNodeReader((XmlNode) securityTokenXml);
      int content = (int) reader.MoveToContent();
      return securityTokenHandlers.ReadToken(reader) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4051", (object) securityTokenXml, (object) reader.LocalName, (object) reader.NamespaceURI)));
    }
  }
}
