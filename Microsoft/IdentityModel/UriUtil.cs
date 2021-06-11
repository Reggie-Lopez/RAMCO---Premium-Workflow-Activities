// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.UriUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace Microsoft.IdentityModel
{
  internal static class UriUtil
  {
    public static bool CanCreateValidUri(string uriString, UriKind uriKind) => UriUtil.TryCreateValidUri(uriString, uriKind, out Uri _);

    public static bool TryCreateValidUri(string uriString, UriKind uriKind, out Uri result) => Uri.TryCreate(uriString, uriKind, out result);
  }
}
