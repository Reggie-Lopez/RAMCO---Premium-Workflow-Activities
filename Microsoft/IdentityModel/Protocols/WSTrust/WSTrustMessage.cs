// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.SecurityTokenService;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public abstract class WSTrustMessage : OpenObject
  {
    private bool _allowPostdating;
    private EndpointAddress _appliesTo;
    private string _replyTo;
    private string _authenticationType;
    private string _canonicalizationAlgorithm;
    private string _context;
    private string _encryptionAlgorithm;
    private Entropy _entropy;
    private string _issuedTokenEncryptionAlgorithm;
    private string _keyWrapAlgorithm;
    private string _issuedTokenSignatureAlgorithm;
    private int? _keySizeInBits;
    private string _keyType;
    private Lifetime _lifetime;
    private string _requestType;
    private string _signatureAlgorithm;
    private string _tokenType;
    private UseKey _useKey;
    private BinaryExchange _binaryExchange;

    public bool AllowPostdating
    {
      get => this._allowPostdating;
      set => this._allowPostdating = value;
    }

    public EndpointAddress AppliesTo
    {
      get => this._appliesTo;
      set => this._appliesTo = value;
    }

    public BinaryExchange BinaryExchange
    {
      get => this._binaryExchange;
      set => this._binaryExchange = value;
    }

    public string ReplyTo
    {
      get => this._replyTo;
      set => this._replyTo = value;
    }

    public string AuthenticationType
    {
      get => this._authenticationType;
      set => this._authenticationType = value;
    }

    public string CanonicalizationAlgorithm
    {
      get => this._canonicalizationAlgorithm;
      set => this._canonicalizationAlgorithm = value;
    }

    public string Context
    {
      get => this._context;
      set => this._context = value;
    }

    public string EncryptionAlgorithm
    {
      get => this._encryptionAlgorithm;
      set => this._encryptionAlgorithm = value;
    }

    public Entropy Entropy
    {
      get => this._entropy;
      set => this._entropy = value;
    }

    public string EncryptWith
    {
      get => this._issuedTokenEncryptionAlgorithm;
      set => this._issuedTokenEncryptionAlgorithm = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("IssuedTokenEncryptionAlgorithm");
    }

    public string SignWith
    {
      get => this._issuedTokenSignatureAlgorithm;
      set => this._issuedTokenSignatureAlgorithm = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public int? KeySizeInBits
    {
      get => this._keySizeInBits;
      set
      {
        if (value.HasValue && value.Value < 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
        this._keySizeInBits = value;
      }
    }

    public string KeyType
    {
      get => this._keyType;
      set => this._keyType = value;
    }

    public string KeyWrapAlgorithm
    {
      get => this._keyWrapAlgorithm;
      set => this._keyWrapAlgorithm = value;
    }

    public Lifetime Lifetime
    {
      get => this._lifetime;
      set => this._lifetime = value;
    }

    public string RequestType
    {
      get => this._requestType;
      set => this._requestType = value;
    }

    public string SignatureAlgorithm
    {
      get => this._signatureAlgorithm;
      set => this._signatureAlgorithm = value;
    }

    public string TokenType
    {
      get => this._tokenType;
      set => this._tokenType = value;
    }

    public UseKey UseKey
    {
      get => this._useKey;
      set => this._useKey = value;
    }
  }
}
