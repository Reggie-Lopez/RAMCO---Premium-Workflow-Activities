// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.InformationCard
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class InformationCard
  {
    public const long DefaultCardVersion = 1;
    private Dictionary<string, object> _properties = new Dictionary<string, object>();
    private CardImage _cardImage;
    private InformationCardReference _cardReference;
    private Uri _cardType;
    private string _cardName;
    private string _issuer;
    private string _issuerName;
    private PrivacyNotice _privacyNotice;
    private Microsoft.IdentityModel.Protocols.WSIdentity.AppliesToOption? _appliesToOptions;
    private Collection<string> _tokenTypeCollections;
    private DisplayClaimCollection _displayClaimCollection;
    private TokenServiceCollection _tokenServiceCollection;
    private DateTime? _timeExpires;
    private DateTime? _timeIssued;
    private string _language;
    private SigningCredentials _signingCredentials;
    private bool? _requireStrongRecipientIdentity;
    private Collection<Microsoft.IdentityModel.Protocols.WSIdentity.IssuerInformation> _issuerInfo = new Collection<Microsoft.IdentityModel.Protocols.WSIdentity.IssuerInformation>();

    public InformationCard(string issuer)
      : this((SigningCredentials) null, issuer)
    {
    }

    public InformationCard(X509Certificate2 certificate, string issuer)
      : this((SigningCredentials) new Microsoft.IdentityModel.SecurityTokenService.X509SigningCredentials(certificate, "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2000/09/xmldsig#sha1"), issuer)
    {
    }

    public InformationCard(SigningCredentials signingCredentials, string issuer)
      : this(signingCredentials, issuer, new InformationCardReference(), DateTime.UtcNow)
    {
    }

    public InformationCard(
      X509Certificate2 certificate,
      string issuer,
      InformationCardReference reference,
      DateTime timeIssued)
      : this((SigningCredentials) new Microsoft.IdentityModel.SecurityTokenService.X509SigningCredentials(certificate, "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2000/09/xmldsig#sha1"), issuer, reference, timeIssued)
    {
    }

    public InformationCard(
      SigningCredentials signingCredentials,
      string issuer,
      InformationCardReference reference,
      DateTime timeIssued)
    {
      this._signingCredentials = signingCredentials;
      this.InformationCardReference = reference;
      this.Issuer = issuer;
      this.TimeIssued = new DateTime?(timeIssued);
      this.AppliesToOption = new Microsoft.IdentityModel.Protocols.WSIdentity.AppliesToOption?(Microsoft.IdentityModel.Protocols.WSIdentity.AppliesToOption.Required);
    }

    public Microsoft.IdentityModel.Protocols.WSIdentity.AppliesToOption? AppliesToOption
    {
      get => this._appliesToOptions;
      set => this._appliesToOptions = value;
    }

    public CardImage CardImage
    {
      get => this._cardImage;
      set => this._cardImage = value;
    }

    public string CardName
    {
      get => this._cardName;
      set => this._cardName = value;
    }

    public Uri CardType
    {
      get => this._cardType;
      set => this._cardType = value;
    }

    public InformationCardReference InformationCardReference
    {
      get => this._cardReference;
      set => this._cardReference = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public string Issuer
    {
      get => this._issuer;
      set => this._issuer = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public string IssuerName
    {
      get => this._issuerName;
      set => this._issuerName = value == null || value.Length >= 1 && value.Length <= 64 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID3259"));
    }

    public Collection<Microsoft.IdentityModel.Protocols.WSIdentity.IssuerInformation> IssuerInformation => this._issuerInfo;

    public string Language
    {
      get => this._language;
      set => this._language = value;
    }

    public PrivacyNotice PrivacyNotice
    {
      get => this._privacyNotice;
      set => this._privacyNotice = value;
    }

    public Dictionary<string, object> Properties => this._properties;

    public bool? RequireStrongRecipientIdentity
    {
      get => this._requireStrongRecipientIdentity;
      set => this._requireStrongRecipientIdentity = value;
    }

    public DisplayClaimCollection SupportedClaimTypeList
    {
      get
      {
        if (this._displayClaimCollection == null)
          this._displayClaimCollection = new DisplayClaimCollection();
        return this._displayClaimCollection;
      }
    }

    public Collection<string> SupportedTokenTypeList
    {
      get
      {
        if (this._tokenTypeCollections == null)
          this._tokenTypeCollections = new Collection<string>();
        return this._tokenTypeCollections;
      }
    }

    public TokenServiceCollection TokenServiceList
    {
      get
      {
        if (this._tokenServiceCollection == null)
          this._tokenServiceCollection = new TokenServiceCollection();
        return this._tokenServiceCollection;
      }
    }

    public DateTime? TimeExpires
    {
      get => this._timeExpires;
      set
      {
        if (value.HasValue)
          value = new DateTime?(value.Value.ToUniversalTime());
        this._timeExpires = value;
      }
    }

    public DateTime? TimeIssued
    {
      get => this._timeIssued;
      set
      {
        if (value.HasValue)
          value = new DateTime?(value.Value.ToUniversalTime());
        this._timeIssued = value;
      }
    }

    public SigningCredentials SigningCredentials
    {
      get => this._signingCredentials;
      set => this._signingCredentials = value;
    }
  }
}
