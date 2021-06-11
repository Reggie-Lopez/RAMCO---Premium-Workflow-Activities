// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.RequestInformationCardsResponse
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class RequestInformationCardsResponse
  {
    private Collection<string> _informationCards = new Collection<string>();

    public RequestInformationCardsResponse()
    {
    }

    public RequestInformationCardsResponse(IEnumerable<string> informationCards)
    {
      if (informationCards == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (informationCards));
      foreach (string informationCard in informationCards)
      {
        if (string.IsNullOrEmpty(informationCard))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (informationCards), SR.GetString("ID3282"));
        this._informationCards.Add(informationCard);
      }
    }

    public Collection<string> InformationCards => this._informationCards;
  }
}
