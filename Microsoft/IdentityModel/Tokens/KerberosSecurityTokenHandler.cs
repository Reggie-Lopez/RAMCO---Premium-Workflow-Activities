// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.KerberosSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class KerberosSecurityTokenHandler : SecurityTokenHandler
  {
    private static string[] _tokenTypeIdentifiers = new string[1]
    {
      "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Kerberos"
    };

    public override bool CanValidateToken => true;

    public override Type TokenType => typeof (KerberosReceiverSecurityToken);

    public override string[] GetTokenTypeIdentifiers() => KerberosSecurityTokenHandler._tokenTypeIdentifiers;

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is KerberosReceiverSecurityToken receiverSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (KerberosReceiverSecurityToken)));
      string str = this.Configuration != null ? this.Configuration.IssuerNameRegistry.GetWindowsIssuerName() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(receiverSecurityToken.WindowsIdentity.Token, "Kerberos", str);
      windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "http://www.w3.org/2001/XMLSchema#string"));
      windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", str));
      if (this.Configuration.SaveBootstrapTokens)
        windowsClaimsIdentity.BootstrapToken = token;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        (IClaimsIdentity) windowsClaimsIdentity
      });
    }
  }
}
