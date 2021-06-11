// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.RequestInformationCards
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class RequestInformationCards
  {
    private Uri _cardIdentifier;
    private CardSignatureFormatType _cardSignatureFormat;
    private Uri _cardType;
    private Uri _issuer;
    private SecurityTokenElement _onBehalfOf;

    public Uri CardIdentifier
    {
      get => this._cardIdentifier;
      set => this._cardIdentifier = value;
    }

    public CardSignatureFormatType CardSignatureFormat
    {
      get => this._cardSignatureFormat;
      set => this._cardSignatureFormat = value;
    }

    public Uri CardType
    {
      get => this._cardType;
      set => this._cardType = value;
    }

    public Uri Issuer
    {
      get => this._issuer;
      set => this._issuer = value;
    }

    public SecurityTokenElement OnBehalfOf
    {
      get => this._onBehalfOf;
      set => this._onBehalfOf = value;
    }
  }
}
