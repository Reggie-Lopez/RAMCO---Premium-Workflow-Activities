﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.WSAddressing10Constants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public static class WSAddressing10Constants
  {
    public const string Prefix = "wsa";
    public const string NamespaceUri = "http://www.w3.org/2005/08/addressing";

    public static class Elements
    {
      public const string Action = "Action";
      public const string Address = "Address";
      public const string ReplyTo = "ReplyTo";
    }
  }
}
