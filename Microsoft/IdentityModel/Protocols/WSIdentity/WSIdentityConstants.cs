// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.WSIdentityConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public static class WSIdentityConstants
  {
    public const string Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity";
    public const string Prefix = "i";
    public const string OasisNamespace = "http://docs.oasis-open.org/imi/ns/identity-200903";
    public const string OasisPrefix = "ic09";
    public const string Dialect = "http://schemas.xmlsoap.org/ws/2005/05/identity";
    public const string InformationCardMimeType = "application/x-informationcard";
    public const string AnonymousIssuer = "http://www.w3.org/2005/08/addressing/anonymous";
    public const string SelfIssuer = "http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self";
    public const string SignatureObjectId = "_Object_InformationCard";
    public const int MaxCardImageLength = 1048576;
    public const int MaxCardNameLength = 255;
    public const int MinDescriptionLength = 1;
    public const int MaxDescriptionLength = 255;
    public const int MaxDisplayCredentialsHintLength = 64;
    public const int MinDisplayTagLength = 1;
    public const int MaxDisplayTagLength = 255;
    public const int MaxIssuerNameLength = 64;
    public const int MaxPrivatePersonalIdentifierLength = 1024;
    public const int MaxSupportedClaimTypeCount = 128;
    public const int MaxSupportedTokenTypeCount = 32;
    public const int MaxTokenServiceCount = 128;
    public const int MaxUsernameLength = 255;
    public const int MinCardNameLength = 1;
    public const int MinCardVersion = 1;
    public const int MinIssuerNameLength = 1;
    public const long MaxCardVersion = 4294967295;
    public const int MinCardPrivacyNoticeVersion = 1;
    public const long MaxCardPrivacyNoticeVersion = 4294967295;
    public const int FriendlyPPIDLength = 10;
    public const string FriendlyPPIDAlphabets = "QL23456789ABCDEFGHJKMNPRSTUVWXYZ";
    public const char FriendlyPPIDSeparator = '-';

    public static class Attributes
    {
      public const string MimeType = "MimeType";
      public const string Optional = "Optional";
      public const string Uri = "Uri";
      public const string Version = "Version";
    }

    public static class ClaimTypes
    {
      private const string ClaimTypeBase = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
      public const string Anonymous = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/anonymous";
      public const string Authentication = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication";
      public const string AuthorizationDecision = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authorizationdecision";
      public const string DenyOnlySid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid";
      public const string Dns = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns";
      public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
      public const string Hash = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash";
      public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
      public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
      public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";
      public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";
      public const string Spn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/spn";
      public const string System = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system";
      public const string Thumbprint = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint";
      public const string Upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
      public const string Uri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri";
      public const string X500DistinguishedName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname";
      public const string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
      public const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
      public const string StreetAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress";
      public const string Locality = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality";
      public const string StateOrProvince = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince";
      public const string PostalCode = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode";
      public const string Country = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country";
      public const string HomePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone";
      public const string OtherPhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone";
      public const string MobilePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone";
      public const string DateOfBirth = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth";
      public const string Gender = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";
      public const string PPID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier";
      public const string PrivatePersonalIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier";
      public const string Webpage = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage";
    }

    public static class Elements
    {
      public const string Id = "id";
      public const string CardId = "CardId";
      public const string CardImage = "CardImage";
      public const string CardName = "CardName";
      public const string CardSignatureFormat = "CardSignatureFormat";
      public const string CardType = "CardType";
      public const string CardVersion = "CardVersion";
      public const string ClaimType = "ClaimType";
      public const string ClientPseudonym = "ClientPseudonym";
      public const string Description = "Description";
      public const string DisplayClaim = "DisplayClaim";
      public const string DisplayCredentialHint = "DisplayCredentialHint";
      public const string DisplayTag = "DisplayTag";
      public const string DisplayToken = "DisplayToken";
      public const string DisplayValue = "DisplayValue";
      public const string EKUPolicy = "EKUPolicy";
      public const string InformationCard = "InformationCard";
      public const string InformationCardReference = "InformationCardReference";
      public const string Issuer = "Issuer";
      public const string IssuerName = "IssuerName";
      public const string KerberosV5Credential = "KerberosV5Credential";
      public const string OID = "OID";
      public const string PPID = "PPID";
      public const string PrincipalName = "PrincipalName";
      public const string PrivacyNotice = "PrivacyNotice";
      public const string PrivatePersonalIdentifier = "PrivatePersonalIdentifier";
      public const string RequestDisplayToken = "RequestDisplayToken";
      public const string RequestedDisplayToken = "RequestedDisplayToken";
      public const string RequestInformationCards = "RequestInformationCards";
      public const string RequestInformationCardsResponse = "RequestInformationCardsResponse";
      public const string RequireAppliesTo = "RequireAppliesTo";
      public const string SelfIssuedCredential = "SelfIssuedCredential";
      public const string SupportedClaimType = "SupportedClaimType";
      public const string SupportedClaimTypeList = "SupportedClaimTypeList";
      public const string SupportedTokenTypeList = "SupportedTokenTypeList";
      public const string TimeExpires = "TimeExpires";
      public const string TimeIssued = "TimeIssued";
      public const string TokenService = "TokenService";
      public const string TokenServiceList = "TokenServiceList";
      public const string TokenType = "TokenType";
      public const string UserCredential = "UserCredential";
      public const string Username = "Username";
      public const string UsernamePasswordCredential = "UsernamePasswordCredential";
      public const string X509Issuer = "X509Issuer";
      public const string X509Principal = "X509Principal";
      public const string X509Subject = "X509Subject";
      public const string X509SubjectAndIssuer = "X509SubjectAndIssuer";
      public const string X509V3Credential = "X509V3Credential";
    }

    public static class FaultCodes
    {
      public const string FailedRequiredClaims = "FailedRequiredClaims";
      public const string InformationCardRefreshRequired = "InformationCardRefreshRequired";
      public const string InternalError = "InternalError";
      public const string InvalidInput = "InvalidInput";
      public const string InvalidProofKey = "InvalidProofKey";
      public const string MissingAppliesTo = "MissingAppliesTo";
      public const string UnauthorizedRequest = "UnauthorizedRequest";
      public const string UnknownInformationCardReference = "UnknownInformationCardReference";
      public const string UnsupportedSignatureFormat = "UnsupportedSignatureFormat";
    }

    public static class CardSignatureFormatTypes
    {
      public const string Enveloped = "Enveloped";
      public const string Enveloping = "Enveloping";
      public const string None = "None";
    }

    public static class ImageTypes
    {
      public const string Bmp = "image/bmp";
      public const string Gif = "image/gif";
      public const string Jpeg = "image/jpeg";
      public const string Png = "image/png";
      public const string Tiff = "image/tiff";
    }
  }
}
