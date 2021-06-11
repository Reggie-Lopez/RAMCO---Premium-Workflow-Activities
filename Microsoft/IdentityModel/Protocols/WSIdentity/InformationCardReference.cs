// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.InformationCardReference
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class InformationCardReference
  {
    private string _cardId;
    private long _cardVersion;

    public InformationCardReference()
    {
      this._cardId = UniqueId.CreateRandomUri();
      this._cardVersion = 1L;
    }

    public InformationCardReference(string cardId, long cardVersion)
    {
      if (string.IsNullOrEmpty(cardId))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (cardId));
      if (cardVersion < 1L || cardVersion > (long) uint.MaxValue)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(nameof (cardVersion), Microsoft.IdentityModel.SR.GetString("ID2027", (object) (long) uint.MaxValue)));
      this._cardId = cardId;
      this._cardVersion = cardVersion;
    }

    public string CardId => this._cardId;

    public long CardVersion
    {
      get => this._cardVersion;
      set => this._cardVersion = value;
    }
  }
}
