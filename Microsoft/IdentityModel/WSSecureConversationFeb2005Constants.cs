// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.WSSecureConversationFeb2005Constants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public static class WSSecureConversationFeb2005Constants
  {
    public const string Namespace = "http://schemas.xmlsoap.org/ws/2005/02/sc";
    public const string TokenTypeURI = "http://schemas.xmlsoap.org/ws/2005/02/sc/sct";
    public const int DefaultDerivedKeyLength = 32;

    public static class ElementNames
    {
      public const string Name = "SecurityContextToken";
      public const string Identifier = "Identifier";
      public const string Instance = "Instance";
    }

    public static class Attributes
    {
      public const string Length = "Length";
      public const string Nonce = "Nonce";
      public const string Instance = "Instance";
    }
  }
}
