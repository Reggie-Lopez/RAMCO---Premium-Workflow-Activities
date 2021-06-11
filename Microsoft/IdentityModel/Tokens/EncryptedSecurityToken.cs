// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.EncryptedSecurityToken
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class EncryptedSecurityToken : SecurityToken
  {
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;
    private SecurityToken _realToken;

    public EncryptedSecurityToken(SecurityToken token, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      this._encryptingCredentials = encryptingCredentials != null ? encryptingCredentials : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encryptingCredentials));
      this._realToken = token;
    }

    public override bool CanCreateKeyIdentifierClause<T>() => this._realToken.CanCreateKeyIdentifierClause<T>();

    public override T CreateKeyIdentifierClause<T>() => this._realToken.CreateKeyIdentifierClause<T>();

    public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials => this._encryptingCredentials;

    public override string Id => this._realToken.Id;

    public override bool MatchesKeyIdentifierClause(SecurityKeyIdentifierClause keyIdentifierClause) => this._realToken.MatchesKeyIdentifierClause(keyIdentifierClause);

    public override SecurityKey ResolveKeyIdentifierClause(
      SecurityKeyIdentifierClause keyIdentifierClause)
    {
      return this._realToken.ResolveKeyIdentifierClause(keyIdentifierClause);
    }

    public override ReadOnlyCollection<SecurityKey> SecurityKeys => this._realToken.SecurityKeys;

    public SecurityToken Token => this._realToken;

    public override DateTime ValidFrom => this._realToken.ValidFrom;

    public override DateTime ValidTo => this._realToken.ValidTo;
  }
}
