// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityKeyElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityKeyElement : SecurityKey
  {
    private SecurityKey _securityKey;
    private object _keyLock;
    private SecurityTokenResolver _securityTokenResolver;
    private SecurityKeyIdentifier _securityKeyIdentifier;

    public SecurityKeyElement(
      SecurityKeyIdentifierClause securityKeyIdentifierClause,
      SecurityTokenResolver securityTokenResolver)
    {
      if (securityKeyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifierClause));
      this.Initialize(new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        securityKeyIdentifierClause
      }), securityTokenResolver);
    }

    public SecurityKeyElement(
      SecurityKeyIdentifier securityKeyIdentifier,
      SecurityTokenResolver securityTokenResolver)
    {
      if (securityKeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifier));
      this.Initialize(securityKeyIdentifier, securityTokenResolver);
    }

    private void Initialize(
      SecurityKeyIdentifier securityKeyIdentifier,
      SecurityTokenResolver securityTokenResolver)
    {
      this._keyLock = new object();
      this._securityKeyIdentifier = securityKeyIdentifier;
      this._securityTokenResolver = securityTokenResolver;
    }

    public override byte[] DecryptKey(string algorithm, byte[] keyData)
    {
      if (this._securityKey == null)
        this.ResolveKey();
      return this._securityKey.DecryptKey(algorithm, keyData);
    }

    public override byte[] EncryptKey(string algorithm, byte[] keyData)
    {
      if (this._securityKey == null)
        this.ResolveKey();
      return this._securityKey.EncryptKey(algorithm, keyData);
    }

    public override bool IsAsymmetricAlgorithm(string algorithm)
    {
      switch (algorithm)
      {
        case "http://www.w3.org/2000/09/xmldsig#dsa-sha1":
        case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
        case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
        case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
        case "http://www.w3.org/2001/04/xmlenc#rsa-1_5":
          return true;
        default:
          return false;
      }
    }

    public override bool IsSupportedAlgorithm(string algorithm)
    {
      if (this._securityKey == null)
        this.ResolveKey();
      return this._securityKey.IsSupportedAlgorithm(algorithm);
    }

    public override bool IsSymmetricAlgorithm(string algorithm)
    {
      switch (algorithm)
      {
        case "http://www.w3.org/2000/09/xmldsig#dsa-sha1":
        case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
        case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
        case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
        case "http://www.w3.org/2001/04/xmlenc#rsa-1_5":
          return false;
        case "http://www.w3.org/2000/09/xmldsig#hmac-sha1":
        case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256":
        case "http://www.w3.org/2001/04/xmlenc#aes128-cbc":
        case "http://www.w3.org/2001/04/xmlenc#aes192-cbc":
        case "http://www.w3.org/2001/04/xmlenc#aes256-cbc":
        case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
        case "http://www.w3.org/2001/04/xmlenc#kw-aes128":
        case "http://www.w3.org/2001/04/xmlenc#kw-aes192":
        case "http://www.w3.org/2001/04/xmlenc#kw-aes256":
        case "http://www.w3.org/2001/04/xmlenc#kw-tripledes":
        case "http://schemas.xmlsoap.org/ws/2005/02/sc/dk/p_sha1":
        case "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/dk/p_sha1":
          return true;
        default:
          return false;
      }
    }

    public override int KeySize
    {
      get
      {
        if (this._securityKey == null)
          this.ResolveKey();
        return this._securityKey.KeySize;
      }
    }

    private void ResolveKey()
    {
      if (this._securityKeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("ski");
      if (this._securityKey != null)
        return;
      lock (this._keyLock)
      {
        if (this._securityKey != null)
          return;
        if (this._securityTokenResolver != null)
        {
          for (int index = 0; index < this._securityKeyIdentifier.Count; ++index)
          {
            if (this._securityTokenResolver.TryResolveSecurityKey(this._securityKeyIdentifier[index], out this._securityKey))
              return;
          }
        }
        this._securityKey = this._securityKeyIdentifier.CanCreateKey ? this._securityKeyIdentifier.CreateKey() : throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID2080", this._securityTokenResolver == null ? (object) "null" : (object) this._securityTokenResolver.ToString(), this._securityKeyIdentifier == null ? (object) "null" : (object) this._securityKeyIdentifier.ToString())), TraceEventType.Error);
      }
    }
  }
}
