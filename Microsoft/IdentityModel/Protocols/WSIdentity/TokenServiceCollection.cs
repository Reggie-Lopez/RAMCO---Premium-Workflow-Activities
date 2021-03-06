// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.TokenServiceCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class TokenServiceCollection : KeyedCollection<string, TokenService>
  {
    public TokenServiceCollection()
    {
    }

    public TokenServiceCollection(IEnumerable<TokenService> collection) => this.AddRange(collection);

    public void AddRange(IEnumerable<TokenService> collection)
    {
      if (collection == null)
        return;
      foreach (TokenService tokenService in collection)
        this.Add(tokenService);
    }

    protected override string GetKeyForItem(TokenService item) => item.Address.Uri.AbsoluteUri;
  }
}
