// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.BoundedCache`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace Microsoft.IdentityModel
{
  internal class BoundedCache<T> where T : class
  {
    private Dictionary<string, BoundedCache<T>.ExpirableItem<T>> _items;
    private int _capacity;
    private TimeSpan _purgeInterval;
    private ReaderWriterLock _readWriteLock;
    private DateTime _lastPurgeTime = DateTime.UtcNow;

    public BoundedCache(int capacity, TimeSpan purgeInterval)
      : this(capacity, purgeInterval, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
    }

    public BoundedCache(
      int capacity,
      TimeSpan purgeInterval,
      IEqualityComparer<string> keyComparer)
    {
      if (capacity <= 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (capacity), (object) capacity, SR.GetString("ID0002"));
      if (purgeInterval <= TimeSpan.Zero)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (purgeInterval), (object) purgeInterval, SR.GetString("ID0016"));
      if (keyComparer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyComparer));
      this._capacity = capacity;
      this._purgeInterval = purgeInterval;
      this._items = new Dictionary<string, BoundedCache<T>.ExpirableItem<T>>(keyComparer);
      this._readWriteLock = new ReaderWriterLock();
    }

    protected ReaderWriterLock CacheLock => this._readWriteLock;

    public virtual int Capacity
    {
      get => this._capacity;
      set => this._capacity = value > 0 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, SR.GetString("ID0002"));
    }

    public virtual void Clear()
    {
      this._readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        this._items.Clear();
      }
      finally
      {
        this._readWriteLock.ReleaseWriterLock();
      }
    }

    private void EnforceQuota()
    {
      if (this._capacity != int.MaxValue && this._items.Count >= this._capacity)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new QuotaExceededException(SR.GetString("ID0021", (object) this._capacity)));
    }

    public virtual int IncreaseCapacity(int size)
    {
      if (size <= 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (size), (object) size, SR.GetString("ID0002"));
      this._readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        if (int.MaxValue - size <= this._capacity)
          this._capacity = int.MaxValue;
        else
          this._capacity += size;
        return this._capacity;
      }
      finally
      {
        this._readWriteLock.ReleaseWriterLock();
      }
    }

    protected Dictionary<string, BoundedCache<T>.ExpirableItem<T>> Items => this._items;

    private void Purge()
    {
      if (DateTime.UtcNow < DateTimeUtil.Add(this._lastPurgeTime, this._purgeInterval))
        return;
      this._lastPurgeTime = DateTime.UtcNow;
      this._readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        List<string> stringList = new List<string>();
        foreach (string key in this._items.Keys)
        {
          if (this._items[key].IsExpired())
            stringList.Add(key);
        }
        for (int index = 0; index < stringList.Count; ++index)
          this._items.Remove(stringList[index]);
      }
      finally
      {
        this._readWriteLock.ReleaseWriterLock();
      }
    }

    public TimeSpan PurgeInterval
    {
      get => this._purgeInterval;
      set => this._purgeInterval = !(value <= TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, SR.GetString("ID0016"));
    }

    public virtual bool TryAdd(string key, T item, DateTime expirationTime)
    {
      this.Purge();
      this._readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
      this.EnforceQuota();
      try
      {
        if (this._items.ContainsKey(key))
          return false;
        this._items[key] = new BoundedCache<T>.ExpirableItem<T>(item, expirationTime);
        return true;
      }
      finally
      {
        this._readWriteLock.ReleaseWriterLock();
      }
    }

    public virtual bool TryFind(string key)
    {
      this.Purge();
      this._readWriteLock.AcquireReaderLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        return this._items.ContainsKey(key);
      }
      finally
      {
        this._readWriteLock.ReleaseReaderLock();
      }
    }

    public virtual bool TryGet(string key, out T item)
    {
      this.Purge();
      item = default (T);
      this._readWriteLock.AcquireReaderLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        if (!this._items.ContainsKey(key))
          return false;
        item = this._items[key].Item;
        return true;
      }
      finally
      {
        this._readWriteLock.ReleaseReaderLock();
      }
    }

    public virtual bool TryRemove(string key)
    {
      this.Purge();
      this._readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
      try
      {
        if (!this._items.ContainsKey(key))
          return false;
        this._items.Remove(key);
        return true;
      }
      finally
      {
        this._readWriteLock.ReleaseWriterLock();
      }
    }

    protected class ExpirableItem<ET>
    {
      private DateTime _expirationTime;
      private ET _item;

      public ExpirableItem(ET item, DateTime expirationTime)
      {
        this._item = item;
        if (expirationTime.Kind != DateTimeKind.Utc)
          this._expirationTime = DateTimeUtil.ToUniversalTime(expirationTime);
        else
          this._expirationTime = expirationTime;
      }

      public bool IsExpired() => this._expirationTime <= DateTime.UtcNow;

      public ET Item => this._item;
    }

    [System.Flags]
    internal enum CachingMode
    {
      Time = 0,
      MRU = 1,
      FIFO = 2,
    }
  }
}
