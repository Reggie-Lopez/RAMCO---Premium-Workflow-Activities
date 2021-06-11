// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.DefaultTokenReplayCache
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class DefaultTokenReplayCache : TokenReplayCache
  {
    private BoundedCache<SecurityToken> _internalCache;

    public DefaultTokenReplayCache()
      : this(SecurityTokenHandlerConfiguration.DefaultTokenReplayCacheCapacity, SecurityTokenHandlerConfiguration.DefaultTokenReplayCachePurgeInterval)
    {
    }

    public DefaultTokenReplayCache(int capacity, TimeSpan purgeInterval) => this._internalCache = new BoundedCache<SecurityToken>(capacity, purgeInterval, (IEqualityComparer<string>) StringComparer.Ordinal);

    public override int Capacity
    {
      get => this._internalCache.Capacity;
      set => this._internalCache.Capacity = value > 0 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, Microsoft.IdentityModel.SR.GetString("ID0002"));
    }

    public override void Clear() => this._internalCache.Clear();

    public override int IncreaseCapacity(int size) => size > 0 ? this._internalCache.IncreaseCapacity(size) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (size), (object) size, Microsoft.IdentityModel.SR.GetString("ID0002"));

    public override TimeSpan PurgeInterval
    {
      get => this._internalCache.PurgeInterval;
      set => this._internalCache.PurgeInterval = !(value <= TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, Microsoft.IdentityModel.SR.GetString("ID0016"));
    }

    public override bool TryAdd(string key, SecurityToken securityToken, DateTime expirationTime)
    {
      if (DateTime.Equals(expirationTime, DateTime.MaxValue))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1072"));
      return this._internalCache.TryAdd(key, securityToken, expirationTime);
    }

    public override bool TryFind(string key) => this._internalCache.TryFind(key);

    public override bool TryGet(string key, out SecurityToken securityToken) => this._internalCache.TryGet(key, out securityToken);

    public override bool TryRemove(string key) => this._internalCache.TryRemove(key);
  }
}
