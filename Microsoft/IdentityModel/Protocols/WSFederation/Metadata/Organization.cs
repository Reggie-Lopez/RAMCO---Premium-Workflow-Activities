// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.Organization
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class Organization
  {
    private LocalizedEntryCollection<LocalizedName> _displayNames = new LocalizedEntryCollection<LocalizedName>();
    private LocalizedEntryCollection<LocalizedName> _names = new LocalizedEntryCollection<LocalizedName>();
    private LocalizedEntryCollection<LocalizedUri> _urls = new LocalizedEntryCollection<LocalizedUri>();

    public Organization()
      : this(new LocalizedEntryCollection<LocalizedName>(), new LocalizedEntryCollection<LocalizedName>(), new LocalizedEntryCollection<LocalizedUri>())
    {
    }

    public Organization(
      LocalizedEntryCollection<LocalizedName> names,
      LocalizedEntryCollection<LocalizedName> displayNames,
      LocalizedEntryCollection<LocalizedUri> urls)
    {
      if (names == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (names));
      if (displayNames == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (displayNames));
      if (urls == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (urls));
      this._names = names;
      this._displayNames = displayNames;
      this._urls = urls;
    }

    public LocalizedEntryCollection<LocalizedName> DisplayNames => this._displayNames;

    public LocalizedEntryCollection<LocalizedName> Names => this._names;

    public LocalizedEntryCollection<LocalizedUri> Urls => this._urls;
  }
}
