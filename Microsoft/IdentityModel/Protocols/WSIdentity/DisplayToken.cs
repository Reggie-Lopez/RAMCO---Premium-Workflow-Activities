// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.DisplayToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class DisplayToken
  {
    private string _language;
    private DisplayClaimCollection _displayClaims;

    public DisplayToken(string language, IEnumerable<DisplayClaim> displayClaims)
    {
      if (string.IsNullOrEmpty(language))
        language = CultureInfo.CurrentUICulture.Name;
      if (displayClaims == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (displayClaims));
      this._language = language;
      this._displayClaims = new DisplayClaimCollection(displayClaims);
    }

    public DisplayClaimCollection DisplayClaims => this._displayClaims;

    public string Language => this._language;
  }
}
