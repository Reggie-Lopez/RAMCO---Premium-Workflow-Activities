// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.WSFederationConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public static class WSFederationConstants
  {
    public const string Namespace = "http://docs.oasis-open.org/wsfed/federation/200706";

    public static class Actions
    {
      public const string Attribute = "wattr1.0";
      public const string Pseudonym = "wpseudo1.0";
      public const string SignIn = "wsignin1.0";
      public const string SignOut = "wsignout1.0";
      public const string SignOutCleanup = "wsignoutcleanup1.0";
    }

    public static class Parameters
    {
      public const string Action = "wa";
      public const string Attribute = "wattr";
      public const string AttributePtr = "wattrptr";
      public const string AuthenticationType = "wauth";
      public const string Context = "wctx";
      public const string CurrentTime = "wct";
      public const string Encoding = "wencoding";
      public const string Federation = "wfed";
      public const string Freshness = "wfresh";
      public const string HomeRealm = "whr";
      public const string Policy = "wp";
      public const string Pseudonym = "wpseudo";
      public const string PseudonymPtr = "wpseudoptr";
      public const string Realm = "wtrealm";
      public const string Reply = "wreply";
      public const string Request = "wreq";
      public const string RequestPtr = "wreqptr";
      public const string Resource = "wres";
      public const string Result = "wresult";
      public const string ResultPtr = "wresultptr";
    }

    public static class FaultCodeValues
    {
      public const string AlreadySignedIn = "AlreadySignedIn";
      public const string BadRequest = "BadRequest";
      public const string IssuerNameNotSupported = "IssuerNameNotSupported";
      public const string NeedFresherCredentials = "NeedFresherCredentials";
      public const string NoMatchInScope = "NoMatchInScope";
      public const string NoPseudonymInScope = "NoPseudonymInScope";
      public const string NotSignedIn = "NotSignedIn";
      public const string RstParameterNotAccepted = "RstParameterNotAccepted";
      public const string SpecificPolicy = "SpecificPolicy";
      public const string UnsupportedClaimsDialect = "UnsupportedClaimsDialect";
      public const string UnsupportedEncoding = "UnsupportedEncoding";
    }
  }
}
