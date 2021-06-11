// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.WSIdentity2007Constants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public static class WSIdentity2007Constants
  {
    public const string Namespace = "http://schemas.xmlsoap.org/ws/2007/01/identity";
    public const string Prefix = "ic07";
    public const int MinEntryNameLength = 1;
    public const int MaxEntryNameLength = 255;
    public const int MinEntryValueLength = 1;
    public const int MaxEntryValueLength = 255;

    public static class Elements
    {
      public const string IssuerInformation = "IssuerInformation";
      public const string IssuerInformationEntry = "IssuerInformationEntry";
      public const string EntryName = "EntryName";
      public const string EntryValue = "EntryValue";
      public const string RequireStrongRecipientIdentity = "RequireStrongRecipientIdentity";
    }
  }
}
