// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.X509CertificateStoreTokenResolver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class X509CertificateStoreTokenResolver : SecurityTokenResolver
  {
    private string _storeName;
    private StoreLocation _storeLocation;

    public X509CertificateStoreTokenResolver()
      : this(System.Security.Cryptography.X509Certificates.StoreName.My, StoreLocation.LocalMachine)
    {
    }

    public X509CertificateStoreTokenResolver(System.Security.Cryptography.X509Certificates.StoreName storeName, StoreLocation storeLocation)
      : this(Enum.GetName(typeof (System.Security.Cryptography.X509Certificates.StoreName), (object) storeName), storeLocation)
    {
    }

    public X509CertificateStoreTokenResolver(string storeName, StoreLocation storeLocation)
    {
      this._storeName = !string.IsNullOrEmpty(storeName) ? storeName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (storeName));
      this._storeLocation = storeLocation;
    }

    public string StoreName => this._storeName;

    public StoreLocation StoreLocation => this._storeLocation;

    protected override bool TryResolveSecurityKeyCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityKey key)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      key = (SecurityKey) null;
      if (keyIdentifierClause is EncryptedKeyIdentifierClause identifierClause)
      {
        SecurityKeyIdentifier encryptingKeyIdentifier = identifierClause.EncryptingKeyIdentifier;
        if (encryptingKeyIdentifier != null && encryptingKeyIdentifier.Count > 0)
        {
          for (int index = 0; index < encryptingKeyIdentifier.Count; ++index)
          {
            SecurityKey key1 = (SecurityKey) null;
            if (this.TryResolveSecurityKey(encryptingKeyIdentifier[index], out key1))
            {
              byte[] encryptedKey = identifierClause.GetEncryptedKey();
              string encryptionMethod = identifierClause.EncryptionMethod;
              byte[] symmetricKey = key1.DecryptKey(encryptionMethod, encryptedKey);
              key = (SecurityKey) new InMemorySymmetricSecurityKey(symmetricKey, false);
              return true;
            }
          }
        }
      }
      else
      {
        SecurityToken token = (SecurityToken) null;
        if (this.TryResolveToken(keyIdentifierClause, out token) && token.SecurityKeys.Count > 0)
        {
          key = token.SecurityKeys[0];
          return true;
        }
      }
      return false;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifier keyIdentifier,
      out SecurityToken token)
    {
      if (keyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifier));
      token = (SecurityToken) null;
      foreach (SecurityKeyIdentifierClause keyIdentifierClause in keyIdentifier)
      {
        if (this.TryResolveToken(keyIdentifierClause, out token))
          return true;
      }
      return false;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityToken token)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      token = (SecurityToken) null;
      X509Store x509Store = (X509Store) null;
      X509Certificate2Collection certificate2Collection = (X509Certificate2Collection) null;
      try
      {
        x509Store = new X509Store(this._storeName, this._storeLocation);
        x509Store.Open(OpenFlags.ReadOnly);
        certificate2Collection = x509Store.Certificates;
        X509Certificate2Enumerator enumerator = certificate2Collection.GetEnumerator();
        while (enumerator.MoveNext())
        {
          X509Certificate2 current = enumerator.Current;
          switch (keyIdentifierClause)
          {
            case X509ThumbprintKeyIdentifierClause identifierClause when identifierClause.Matches(current):
              token = (SecurityToken) new X509SecurityToken(current);
              return true;
            case X509IssuerSerialKeyIdentifierClause identifierClause when identifierClause.Matches(current):
              token = (SecurityToken) new X509SecurityToken(current);
              return true;
            case X509SubjectKeyIdentifierClause identifierClause when identifierClause.Matches(current):
              token = (SecurityToken) new X509SecurityToken(current);
              return true;
            case X509RawDataKeyIdentifierClause identifierClause when identifierClause.Matches(current):
              token = (SecurityToken) new X509SecurityToken(current);
              return true;
            default:
              continue;
          }
        }
      }
      finally
      {
        if (certificate2Collection != null)
        {
          for (int index = 0; index < certificate2Collection.Count; ++index)
            certificate2Collection[index].Reset();
        }
        x509Store?.Close();
      }
      return false;
    }
  }
}
