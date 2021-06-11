// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.UserCredentialCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class UserCredentialCollection : KeyedCollection<UserCredentialType, IUserCredential>
  {
    public UserCredentialCollection()
      : this((UserNamePasswordCredential) null, (X509CertificateCredential) null, (SelfIssuedCredentials) null)
    {
    }

    public UserCredentialCollection(UserNamePasswordCredential userName)
      : this(userName, (X509CertificateCredential) null, (SelfIssuedCredentials) null)
    {
    }

    public UserCredentialCollection(X509CertificateCredential x509Certificate)
      : this((UserNamePasswordCredential) null, x509Certificate, (SelfIssuedCredentials) null)
    {
    }

    public UserCredentialCollection(SelfIssuedCredentials selfIssuedCredential)
      : this((UserNamePasswordCredential) null, (X509CertificateCredential) null, selfIssuedCredential)
    {
    }

    public UserCredentialCollection(
      UserNamePasswordCredential userName,
      X509CertificateCredential x509Certificate,
      SelfIssuedCredentials selfIssuedCredential)
    {
      if (userName != null)
        this.Add((IUserCredential) userName);
      if (x509Certificate != null)
        this.Add((IUserCredential) x509Certificate);
      if (selfIssuedCredential == null)
        return;
      this.Add((IUserCredential) selfIssuedCredential);
    }

    public UserCredentialCollection(IEnumerable<IUserCredential> collection) => this.AddRange(collection);

    protected override UserCredentialType GetKeyForItem(IUserCredential item) => item.CredentialType;

    public void AddRange(IEnumerable<IUserCredential> collection)
    {
      if (collection == null)
        return;
      foreach (IUserCredential userCredential in collection)
        this.Add(userCredential);
    }
  }
}
