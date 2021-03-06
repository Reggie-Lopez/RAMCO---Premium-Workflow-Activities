// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.LocalizedUri
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class LocalizedUri : LocalizedEntry
  {
    private Uri _uri;

    public LocalizedUri()
      : this((Uri) null, (CultureInfo) null)
    {
    }

    public LocalizedUri(Uri uri, CultureInfo language)
      : base(language)
      => this.Uri = uri;

    public Uri Uri
    {
      get => this._uri;
      set => this._uri = value;
    }
  }
}
