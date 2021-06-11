// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.TokenReplayCache
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public abstract class TokenReplayCache
  {
    public abstract int Capacity { get; set; }

    public abstract void Clear();

    public abstract int IncreaseCapacity(int size);

    public abstract TimeSpan PurgeInterval { get; set; }

    public abstract bool TryAdd(string key, SecurityToken securityToken, DateTime expirationTime);

    public abstract bool TryFind(string key);

    public abstract bool TryGet(string key, out SecurityToken securityToken);

    public abstract bool TryRemove(string key);
  }
}
