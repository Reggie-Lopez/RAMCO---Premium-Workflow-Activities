// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.AuthenticationTypeMaps
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace Microsoft.IdentityModel.Claims
{
  internal static class AuthenticationTypeMaps
  {
    public static AuthenticationTypeMaps.Mapping[] Saml11 = new AuthenticationTypeMaps.Mapping[12]
    {
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken", "URI:urn:oasis:names:tc:SAML:1.0:am:HardwareToken"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/kerberos", "urn:ietf:rfc:1510"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password", "urn:oasis:names:tc:SAML:1.0:am:password"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/pgp", "urn:oasis:names:tc:SAML:1.0:am:PGP"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/secureremotepassword", "urn:ietf:rfc:2945"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature", "urn:ietf:rfc:3075"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/spki", "urn:oasis:names:tc:SAML:1.0:am:SPKI"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/tlsclient", "urn:ietf:rfc:2246"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/unspecified", "urn:oasis:names:tc:SAML:1.0:am:unspecified"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "urn:federation:authentication:windows"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509", "urn:oasis:names:tc:SAML:1.0:am:X509-PKI"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/xkms", "urn:oasis:names:tc:SAML:1.0:am:XKMS")
    };
    public static AuthenticationTypeMaps.Mapping[] Saml2 = new AuthenticationTypeMaps.Mapping[12]
    {
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/kerberos", "urn:oasis:names:tc:SAML:2.0:ac:classes:Kerberos"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password", "urn:oasis:names:tc:SAML:2.0:ac:classes:Password"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/pgp", "urn:oasis:names:tc:SAML:2.0:ac:classes:PGP"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/secureremotepassword", "urn:oasis:names:tc:SAML:2.0:ac:classes:SecureRemotePassword"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature", "urn:oasis:names:tc:SAML:2.0:ac:classes:XMLDSig"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/spki", "urn:oasis:names:tc:SAML:2.0:ac:classes:SPKI"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcard", "urn:oasis:names:tc:SAML:2.0:ac:classes:Smartcard"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcardpki", "urn:oasis:names:tc:SAML:2.0:ac:classes:SmartcardPKI"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/tlsclient", "urn:oasis:names:tc:SAML:2.0:ac:classes:TLSClient"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/unspecified", "urn:oasis:names:tc:SAML:2.0:ac:classes:Unspecified"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509", "urn:oasis:names:tc:SAML:2.0:ac:classes:X509"),
      new AuthenticationTypeMaps.Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "urn:federation:authentication:windows")
    };

    public static string Denormalize(
      string normalizedAuthenticationMethod,
      AuthenticationTypeMaps.Mapping[] mappingTable)
    {
      foreach (AuthenticationTypeMaps.Mapping mapping in mappingTable)
      {
        if (StringComparer.Ordinal.Equals(normalizedAuthenticationMethod, mapping.Normalized))
          return mapping.Unnormalized;
      }
      return normalizedAuthenticationMethod;
    }

    public static string Normalize(
      string unnormalizedAuthenticationMethod,
      AuthenticationTypeMaps.Mapping[] mappingTable)
    {
      foreach (AuthenticationTypeMaps.Mapping mapping in mappingTable)
      {
        if (StringComparer.Ordinal.Equals(unnormalizedAuthenticationMethod, mapping.Unnormalized))
          return mapping.Normalized;
      }
      return unnormalizedAuthenticationMethod;
    }

    public struct Mapping
    {
      public string Normalized;
      public string Unnormalized;

      public Mapping(string normalized, string unnormalized)
      {
        this.Normalized = normalized;
        this.Unnormalized = unnormalized;
      }
    }
  }
}
