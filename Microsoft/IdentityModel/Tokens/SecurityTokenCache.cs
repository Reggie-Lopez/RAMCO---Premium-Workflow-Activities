﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenCache
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public abstract class SecurityTokenCache
  {
    private SecurityTokenHandler _owner;

    public abstract void ClearEntries();

    public abstract bool TryRemoveEntry(object key);

    public abstract bool TryRemoveAllEntries(object key);

    public abstract bool TryAddEntry(object key, SecurityToken value);

    public SecurityTokenHandler Owner
    {
      get => this._owner;
      set => this._owner = value;
    }

    public abstract bool TryGetEntry(object key, out SecurityToken value);

    public abstract bool TryGetAllEntries(object key, out IList<SecurityToken> tokens);

    public abstract bool TryReplaceEntry(object key, SecurityToken newValue);
  }
}
