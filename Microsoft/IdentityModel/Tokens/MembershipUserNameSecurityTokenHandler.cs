// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.MembershipUserNameSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class MembershipUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
  {
    private MembershipProvider _provider;

    public MembershipUserNameSecurityTokenHandler()
    {
      this._provider = Membership.Provider;
      if (this._provider == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4040"));
    }

    public MembershipUserNameSecurityTokenHandler(MembershipProvider provider) => this._provider = provider != null ? provider : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (provider));

    public MembershipUserNameSecurityTokenHandler(XmlNodeList customConfigElements)
    {
      List<XmlElement> xmlElementList = customConfigElements != null ? XmlUtil.GetXmlElements(customConfigElements) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customConfigElements));
      XmlElement xmlElement = 1 == xmlElementList.Count ? xmlElementList[0] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7005", (object) "userNameSecurityTokenHandlerRequirement")));
      if (!StringComparer.Ordinal.Equals(xmlElement.LocalName, "userNameSecurityTokenHandlerRequirement"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7006", (object) "userNameSecurityTokenHandlerRequirement", (object) xmlElement.LocalName)));
      this._provider = Membership.Provider;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlElement.Attributes)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "membershipProviderName"))
          this._provider = Membership.Providers[attribute.Value.ToString()];
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7004", (object) attribute.LocalName, (object) xmlElement.LocalName)));
      }
      if (this._provider == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4064"));
    }

    public override bool CanValidateToken => true;

    public MembershipProvider MembershipProvider
    {
      get => this._provider;
      set => this._provider = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (!(token is UserNameSecurityToken nameSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (UserNameSecurityToken)));
      if (!this._provider.ValidateUser(nameSecurityToken.UserName, nameSecurityToken.Password))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4058", (object) nameSecurityToken.UserName)));
      IClaimsIdentity claimsIdentity = (IClaimsIdentity) new ClaimsIdentity((IEnumerable<Claim>) new Claim[1]
      {
        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", nameSecurityToken.UserName)
      }, "Password");
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
      if (this.Configuration.SaveBootstrapTokens)
        claimsIdentity.BootstrapToken = !this.RetainPassword ? (SecurityToken) new UserNameSecurityToken(nameSecurityToken.UserName, (string) null) : (SecurityToken) nameSecurityToken;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        claimsIdentity
      });
    }
  }
}
