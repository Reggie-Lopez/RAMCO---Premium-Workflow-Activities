// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.RequestedSecurityToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class RequestedSecurityToken
  {
    private XmlElement _tokenAsXml;
    private SecurityToken _requestedToken;

    public RequestedSecurityToken(SecurityToken token) => this._requestedToken = token != null ? token : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));

    public RequestedSecurityToken(XmlElement tokenAsXml) => this._tokenAsXml = tokenAsXml != null ? tokenAsXml : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenAsXml));

    public virtual XmlElement SecurityTokenXml => this._tokenAsXml;

    public SecurityToken SecurityToken => this._requestedToken;
  }
}
