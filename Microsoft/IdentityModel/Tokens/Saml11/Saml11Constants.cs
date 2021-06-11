// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml11.Saml11Constants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
  [ComVisible(true)]
  public static class Saml11Constants
  {
    public const string AssertionIdPrefix = "SamlSecurityToken-";
    public const string Namespace = "urn:oasis:names:tc:SAML:1.0:assertion";
    public const string Prefix = "saml";
    public const int MajorVersion = 1;
    public const int MinorVersion = 1;

    public static class AuthenticationMethods
    {
      public const string HardwareTokenString = "URI:urn:oasis:names:tc:SAML:1.0:am:HardwareToken";
      public const string KerberosString = "urn:ietf:rfc:1510";
      public const string PasswordString = "urn:oasis:names:tc:SAML:1.0:am:password";
      public const string PgpString = "urn:oasis:names:tc:SAML:1.0:am:PGP";
      public const string SecureRemotePasswordString = "urn:ietf:rfc:2945";
      public const string SignatureString = "urn:ietf:rfc:3075";
      public const string SpkiString = "urn:oasis:names:tc:SAML:1.0:am:SPKI";
      public const string TlsClientString = "urn:ietf:rfc:2246";
      public const string UnspecifiedString = "urn:oasis:names:tc:SAML:1.0:am:unspecified";
      public const string WindowsString = "urn:federation:authentication:windows";
      public const string X509String = "urn:oasis:names:tc:SAML:1.0:am:X509-PKI";
      public const string XkmsString = "urn:oasis:names:tc:SAML:1.0:am:XKMS";
    }

    public static class ElementNames
    {
      public const string Action = "Action";
      public const string Advice = "Advice";
      public const string Assertion = "Assertion";
      public const string AssertionIdReference = "AssertionIDReference";
      public const string Attribute = "Attribute";
      public const string AttributeStatement = "AttributeStatement";
      public const string AttributeValue = "AttributeValue";
      public const string Audience = "Audience";
      public const string AudienceRestrictionCondition = "AudienceRestrictionCondition";
      public const string AuthenticationStatement = "AuthenticationStatement";
      public const string AuthorityBinding = "AuthorityBinding";
      public const string AuthorizationDecisionStatement = "AuthorizationDecisionStatement";
      public const string Conditions = "Conditions";
      public const string DoNotCacheCondition = "DoNotCacheCondition";
      public const string Evidence = "Evidence";
      public const string NameIdentifier = "NameIdentifier";
      public const string SubjectConfirmation = "SubjectConfirmation";
      public const string Subject = "Subject";
      public const string SubjectConfirmationData = "SubjectConfirmationData";
      public const string SubjectConfirmationMethod = "ConfirmationMethod";
      public const string SubjectLocality = "SubjectLocality";
    }

    public static class AttributeNames
    {
      public const string AssertionId = "AssertionID";
      public const string AttributeName = "AttributeName";
      public const string AttributeNamespace = "AttributeNamespace";
      public const string AuthenticationInstant = "AuthenticationInstant";
      public const string AuthenticationMethod = "AuthenticationMethod";
      public const string AuthorityBinding = "AuthorityBinding";
      public const string AuthorityKind = "AuthorityKind";
      public const string Binding = "Binding";
      public const string Decision = "Decision";
      public const string Issuer = "Issuer";
      public const string IssueInstant = "IssueInstant";
      public const string Location = "Location";
      public const string MajorVersion = "MajorVersion";
      public const string MinorVersion = "MinorVersion";
      public const string OriginalIssuer = "OriginalIssuer";
      public const string NamespaceAttributePrefix = "xmlns";
      public const string NameIdentifierFormat = "Format";
      public const string NameIdentifierNameQualifier = "NameQualifier";
      public const string Namespace = "Namespace";
      public const string NotBefore = "NotBefore";
      public const string NotOnOrAfter = "NotOnOrAfter";
      public const string Resource = "Resource";
      public const string SubjectLocalityDNSAddress = "DNSAddress";
      public const string SubjectLocalityIPAddress = "IPAddress";
    }
  }
}
