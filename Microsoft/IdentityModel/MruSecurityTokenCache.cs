// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.MruSecurityTokenCache
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public class MruSecurityTokenCache : SecurityTokenCache
  {
    public const int DefaultTokenCacheSize = 10000;
    private Dictionary<object, MruSecurityTokenCache.CacheEntry> _items;
    private int _maximumSize;
    private MruSecurityTokenCache.CacheEntry _mruEntry;
    private LinkedList<object> _mruList;
    private int _sizeAfterPurge;
    private object _syncRoot = new object();

    public MruSecurityTokenCache()
      : this(10000)
    {
    }

    public MruSecurityTokenCache(int maximumSize)
      : this(maximumSize, (IEqualityComparer<object>) null)
    {
    }

    public MruSecurityTokenCache(int maximumSize, IEqualityComparer<object> comparer)
      : this(maximumSize / 5 * 4, maximumSize, comparer)
    {
    }

    public MruSecurityTokenCache(int sizeAfterPurge, int maximumSize)
      : this(sizeAfterPurge, maximumSize, (IEqualityComparer<object>) null)
    {
    }

    public MruSecurityTokenCache(
      int sizeAfterPurge,
      int maximumSize,
      IEqualityComparer<object> comparer)
    {
      if (sizeAfterPurge < 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID0008"), nameof (sizeAfterPurge)));
      this._items = sizeAfterPurge < maximumSize ? new Dictionary<object, MruSecurityTokenCache.CacheEntry>(maximumSize, comparer) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID0009"), nameof (sizeAfterPurge)));
      this._maximumSize = maximumSize;
      this._mruList = new LinkedList<object>();
      this._sizeAfterPurge = sizeAfterPurge;
    }

    public int MaximumSize => this._maximumSize;

    public override void ClearEntries()
    {
      lock (this._syncRoot)
      {
        this._mruList.Clear();
        this._items.Clear();
        this._mruEntry.value = (SecurityToken) null;
        this._mruEntry.node = (LinkedListNode<object>) null;
      }
    }

    public override bool TryRemoveEntry(object key)
    {
      if (key == null)
        return false;
      lock (this._syncRoot)
      {
        MruSecurityTokenCache.CacheEntry cacheEntry;
        if (this._items.TryGetValue(key, out cacheEntry))
        {
          this._items.Remove(key);
          this._mruList.Remove(cacheEntry.node);
          if (object.ReferenceEquals((object) this._mruEntry.node, (object) cacheEntry.node))
          {
            this._mruEntry.value = (SecurityToken) null;
            this._mruEntry.node = (LinkedListNode<object>) null;
          }
          return true;
        }
      }
      return false;
    }

    public override bool TryRemoveAllEntries(object key)
    {
      if (key == null)
        return false;
      Dictionary<object, MruSecurityTokenCache.CacheEntry> dictionary = new Dictionary<object, MruSecurityTokenCache.CacheEntry>();
      lock (this._syncRoot)
      {
        foreach (object key1 in this._items.Keys)
        {
          if (key1.Equals(key))
            dictionary.Add(key1, this._items[key1]);
        }
        foreach (object key1 in dictionary.Keys)
        {
          this._items.Remove(key1);
          MruSecurityTokenCache.CacheEntry cacheEntry = dictionary[key1];
          this._mruList.Remove((object) cacheEntry);
          if (object.ReferenceEquals((object) this._mruEntry.node, (object) cacheEntry.node))
          {
            this._mruEntry.value = (SecurityToken) null;
            this._mruEntry.node = (LinkedListNode<object>) null;
          }
        }
      }
      return dictionary.Count > 0;
    }

    public override bool TryAddEntry(object key, SecurityToken value)
    {
      bool entry;
      lock (this._syncRoot)
      {
        entry = this.TryGetEntry(key, out SecurityToken _);
        if (!entry)
          this.Add(key, value);
      }
      return !entry;
    }

    public override bool TryGetEntry(object key, out SecurityToken value)
    {
      bool flag;
      lock (this._syncRoot)
      {
        if (this._mruEntry.node != null && key != null && key.Equals(this._mruEntry.node.Value))
        {
          value = this._mruEntry.value;
          return true;
        }
        MruSecurityTokenCache.CacheEntry cacheEntry;
        flag = this._items.TryGetValue(key, out cacheEntry);
        value = cacheEntry.value;
        if (flag)
        {
          if (this._mruList.Count > 1)
          {
            if (!object.ReferenceEquals((object) this._mruList.First, (object) cacheEntry.node))
            {
              this._mruList.Remove(cacheEntry.node);
              this._mruList.AddFirst(cacheEntry.node);
              this._mruEntry = cacheEntry;
            }
          }
        }
      }
      return flag;
    }

    public override bool TryGetAllEntries(object key, out IList<SecurityToken> tokens)
    {
      tokens = (IList<SecurityToken>) new List<SecurityToken>();
      lock (this._syncRoot)
      {
        foreach (object key1 in this._items.Keys)
        {
          if (key1.Equals(key))
          {
            MruSecurityTokenCache.CacheEntry cacheEntry = this._items[key1];
            if (this._mruList.Count > 1 && !object.ReferenceEquals((object) this._mruList.First, (object) cacheEntry.node))
            {
              this._mruList.Remove(cacheEntry.node);
              this._mruList.AddFirst(cacheEntry.node);
              this._mruEntry = cacheEntry;
            }
            tokens.Add(cacheEntry.value);
          }
        }
        return tokens.Count > 0;
      }
    }

    public override bool TryReplaceEntry(object key, SecurityToken newValue)
    {
      lock (this._syncRoot)
        return this.TryRemoveEntry(key) && this.TryAddEntry(key, newValue);
    }

    internal Dictionary<object, MruSecurityTokenCache.CacheEntry> Items => this._items;

    private void Add(object key, SecurityToken value)
    {
      if (key == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (key));
      bool flag = false;
      lock (this._syncRoot)
      {
        try
        {
          if (this._items.Count == this._maximumSize)
          {
            int num = this._maximumSize - this._sizeAfterPurge;
            for (int index = 0; index < num; ++index)
            {
              object key1 = this._mruList.Last.Value;
              this._mruList.RemoveLast();
              this._items.Remove(key1);
            }
            flag = true;
          }
          MruSecurityTokenCache.CacheEntry cacheEntry;
          cacheEntry.node = this._mruList.AddFirst(key);
          cacheEntry.value = value;
          this._items.Add(key, cacheEntry);
          this._mruEntry = cacheEntry;
        }
        catch
        {
          throw;
        }
      }
      if (!flag || !DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        return;
      DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("ID8003", (object) this._maximumSize, (object) this._sizeAfterPurge));
    }

    internal struct CacheEntry
    {
      public SecurityToken value;
      public LinkedListNode<object> node;
    }
  }
}
