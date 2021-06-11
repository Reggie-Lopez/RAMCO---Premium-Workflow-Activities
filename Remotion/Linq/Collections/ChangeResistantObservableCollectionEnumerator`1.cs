// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.ChangeResistantObservableCollectionEnumerator`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Remotion.Linq.Collections
{
  internal class ChangeResistantObservableCollectionEnumerator<T> : 
    IEnumerator<T>,
    IDisposable,
    IEnumerator
  {
    private readonly ObservableCollection<T> _collection;
    private int _index;
    private bool _disposed;

    public ChangeResistantObservableCollectionEnumerator(ObservableCollection<T> collection)
    {
      ArgumentUtility.CheckNotNull<ObservableCollection<T>>(nameof (collection), collection);
      this._collection = collection;
      this._index = -1;
      this._disposed = false;
      this._collection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Collection_CollectionChanged);
    }

    public int Index
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException("enumerator");
        return this._index;
      }
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      this._collection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Collection_CollectionChanged);
    }

    public bool MoveNext()
    {
      if (this._disposed)
        throw new ObjectDisposedException("enumerator");
      ++this._index;
      return this._index < this._collection.Count;
    }

    public void Reset()
    {
      if (this._disposed)
        throw new ObjectDisposedException("enumerator");
      this._index = -1;
    }

    public T Current
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException("enumerator");
        if (this._index < 0)
          throw new InvalidOperationException("MoveNext must be called before invoking Current.");
        if (this._index >= this._collection.Count)
          throw new InvalidOperationException("After MoveNext returned false, Current must not be invoked any more.");
        return this._collection[this._index];
      }
    }

    object IEnumerator.Current => (object) this.Current;

    private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      ArgumentUtility.CheckNotNull<NotifyCollectionChangedEventArgs>(nameof (e), e);
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          if (e.NewStartingIndex > this._index)
            break;
          this._index += e.NewItems.Count;
          break;
        case NotifyCollectionChangedAction.Remove:
          if (e.OldStartingIndex > this._index)
            break;
          this._index -= e.OldItems.Count;
          break;
        case NotifyCollectionChangedAction.Replace:
          break;
        case NotifyCollectionChangedAction.Move:
          break;
        case NotifyCollectionChangedAction.Reset:
          this._index = 0;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
