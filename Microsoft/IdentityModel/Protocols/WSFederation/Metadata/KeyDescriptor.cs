// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.KeyDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class KeyDescriptor
  {
    private SecurityKeyIdentifier _ski;
    private KeyType _use;
    private Collection<EncryptionMethod> _encryptionMethods = new Collection<EncryptionMethod>();

    public KeyDescriptor()
      : this((SecurityKeyIdentifier) null)
    {
    }

    public KeyDescriptor(SecurityKeyIdentifier ski) => this._ski = ski;

    public SecurityKeyIdentifier KeyInfo
    {
      get => this._ski;
      set => this._ski = value;
    }

    public KeyType Use
    {
      get => this._use;
      set => this._use = value;
    }

    public ICollection<EncryptionMethod> EncryptionMethods => (ICollection<EncryptionMethod>) this._encryptionMethods;
  }
}
