// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.WindowsUserNameSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class WindowsUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
  {
    public override bool CanValidateToken => true;

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is UserNameSecurityToken nameSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (UserNameSecurityToken)));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      string lpszUserName = nameSecurityToken.UserName;
      string password = nameSecurityToken.Password;
      string lpszDomain = (string) null;
      string[] strArray = nameSecurityToken.UserName.Split('\\');
      if (strArray.Length != 1)
      {
        lpszUserName = strArray.Length == 2 && !string.IsNullOrEmpty(strArray[0]) ? strArray[1] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID4062"));
        lpszDomain = strArray[0];
      }
      SafeCloseHandle phToken = (SafeCloseHandle) null;
      try
      {
        if (!Microsoft.IdentityModel.NativeMethods.LogonUser(lpszUserName, lpszDomain, password, 8U, 0U, out phToken))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4063", (object) lpszUserName), (Exception) new Win32Exception(lastWin32Error)));
        }
        string windowsIssuerName = this.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
        WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(phToken.DangerousGetHandle(), "Password", windowsIssuerName);
        windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", windowsIssuerName));
        windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
        if (this.Configuration.SaveBootstrapTokens)
          windowsClaimsIdentity.BootstrapToken = !this.RetainPassword ? (SecurityToken) new UserNameSecurityToken(nameSecurityToken.UserName, (string) null) : (SecurityToken) nameSecurityToken;
        return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
        {
          (IClaimsIdentity) windowsClaimsIdentity
        });
      }
      finally
      {
        phToken?.Close();
      }
    }
  }
}
