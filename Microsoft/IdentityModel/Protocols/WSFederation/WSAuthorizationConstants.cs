// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.WSAuthorizationConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public static class WSAuthorizationConstants
  {
    public const string Prefix = "auth";
    public const string Namespace = "http://docs.oasis-open.org/wsfed/authorization/200706";
    public const string Dialect = "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims";
    public const string Action = "http://docs.oasis-open.org/wsfed/authorization/200706/claims/action";

    public static class Attributes
    {
      public const string Name = "Name";
      public const string Scope = "Scope";
    }

    public static class Elements
    {
      public const string AdditionalContext = "AdditionalContext";
      public const string ClaimType = "ClaimType";
      public const string ContextItem = "ContextItem";
      public const string Description = "Description";
      public const string DisplayName = "DisplayName";
      public const string Value = "Value";
    }
  }
}
