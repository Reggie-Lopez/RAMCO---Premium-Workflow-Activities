// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.Scope
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class Scope
  {
    private string _appliesToAddress;
    private string _replyToAddress;
    private EncryptingCredentials _encryptingCredentials;
    private SigningCredentials _signingCredentials;
    private bool _symmetricKeyEncryptionRequired = true;
    private bool _tokenEncryptionRequired = true;
    private Dictionary<string, object> _properties = new Dictionary<string, object>();

    public Scope()
      : this((string) null, (SigningCredentials) null, (EncryptingCredentials) null)
    {
    }

    public Scope(string appliesToAddress)
      : this(appliesToAddress, (SigningCredentials) null, (EncryptingCredentials) null)
    {
    }

    public Scope(string appliesToAddress, SigningCredentials signingCredentials)
      : this(appliesToAddress, signingCredentials, (EncryptingCredentials) null)
    {
    }

    public Scope(string appliesToAddress, EncryptingCredentials encryptingCredentials)
      : this(appliesToAddress, (SigningCredentials) null, encryptingCredentials)
    {
    }

    public Scope(
      string appliesToAddress,
      SigningCredentials signingCredentials,
      EncryptingCredentials encryptingCredentials)
    {
      this._appliesToAddress = appliesToAddress;
      this._signingCredentials = signingCredentials;
      this._encryptingCredentials = encryptingCredentials;
    }

    public virtual string AppliesToAddress
    {
      get => this._appliesToAddress;
      set => this._appliesToAddress = value;
    }

    public virtual EncryptingCredentials EncryptingCredentials
    {
      get => this._encryptingCredentials;
      set => this._encryptingCredentials = value;
    }

    public virtual string ReplyToAddress
    {
      get => this._replyToAddress;
      set => this._replyToAddress = value;
    }

    public virtual SigningCredentials SigningCredentials
    {
      get => this._signingCredentials;
      set => this._signingCredentials = value;
    }

    public virtual bool SymmetricKeyEncryptionRequired
    {
      get => this._symmetricKeyEncryptionRequired;
      set => this._symmetricKeyEncryptionRequired = value;
    }

    public virtual bool TokenEncryptionRequired
    {
      get => this._tokenEncryptionRequired;
      set => this._tokenEncryptionRequired = value;
    }

    public virtual Dictionary<string, object> Properties => this._properties;
  }
}
