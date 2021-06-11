// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.FederationMetadataConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public static class FederationMetadataConstants
  {
    public const string Namespace = "http://docs.oasis-open.org/wsfed/federation/200706";
    public const string Prefix = "fed";

    public static class Elements
    {
      public const string ClaimTypesOffered = "ClaimTypesOffered";
      public const string ClaimTypesRequested = "ClaimTypesRequested";
      public const string TargetScopes = "TargetScopes";
      public const string TokenTypesOffered = "TokenTypesOffered";
      public const string ApplicationServiceType = "ApplicationServiceType";
      public const string SecurityTokenServiceType = "SecurityTokenServiceType";
      public const string ApplicationServiceEndpoint = "ApplicationServiceEndpoint";
      public const string PassiveRequestorEndpoint = "PassiveRequestorEndpoint";
      public const string SecurityTokenServiceEndpoint = "SecurityTokenServiceEndpoint";
    }
  }
}
