// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.WSSecurity10Constants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public static class WSSecurity10Constants
  {
    public const string FragmentBaseAddress = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0";
    public const string Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
    public const string Prefix = "wsse";
    public const string Base64EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
    public const string HexBinaryEncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary";
    public const string KerberosTokenType1510 = "http://docs.oasis-open.org/wss/oasis-wss-kerberos-token-profile-1.1#GSS_Kerberosv5_AP_REQ1510";
    public const string KerberosTokenTypeGSS = "http://docs.oasis-open.org/wss/oasis-wss-kerberos-token-profile-1.1#GSS_Kerberosv5_AP_REQ";
    public const string TextEncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Text";
    public const string X509TokenType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
    public const string UPTokenPasswordTextValue = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText";

    public static class Attributes
    {
      public const string ValueType = "ValueType";
      public const string EncodingType = "EncodingType";
      public const string URI = "URI";
      public const string Type = "Type";
    }

    public static class Elements
    {
      public const string BinarySecurityToken = "BinarySecurityToken";
      public const string Reference = "Reference";
      public const string KeyIdentifier = "KeyIdentifier";
      public const string SecurityTokenReference = "SecurityTokenReference";
      public const string UsernameToken = "UsernameToken";
      public const string Username = "Username";
      public const string Password = "Password";
      public const string Nonce = "Nonce";
      public const string Created = "Created";
    }

    public static class EncodingTypes
    {
      public const string Base64 = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
      public const string HexBinary = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary";
      public const string Text = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Text";
    }

    public static class FaultCodes
    {
      public const string FailedAuthentication = "FailedAuthentication";
      public const string FailedCheck = "FailedCheck";
      public const string InvalidSecurity = "InvalidSecurity";
      public const string InvalidSecurityToken = "InvalidSecurityToken";
      public const string MessageExpired = "MessageExpired";
      public const string SecurityTokenUnavailable = "SecurityTokenUnavailable";
      public const string UnsupportedAlgorithm = "UnsupportedAlgorithm";
      public const string UnsupportedSecurityToken = "UnsupportedSecurityToken";
    }
  }
}
