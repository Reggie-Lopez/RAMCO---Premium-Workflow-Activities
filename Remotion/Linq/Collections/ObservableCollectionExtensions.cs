// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.ObservableCollectionExtensions
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Remotion.Linq.Collections
{
  internal static class ObservableCollectionExtensions
  {
    public static IEnumerable<T> AsChangeResistantEnumerable<T>(
      this ObservableCollection<T> collection)
    {
      return (IEnumerable<T>) new ObservableCollectionExtensions.ChangeResistantEnumerable<T>(collection);
    }

    public static IEnumerable<IndexValuePair<T>> AsChangeResistantEnumerableWithIndex<T>(
      this ObservableCollection<T> collection)
    {
      using (ChangeResistantObservableCollectionEnumerator<T> enumerator = (ChangeResistantObservableCollectionEnumerator<T>) collection.AsChangeResistantEnumerable<T>().GetEnumerator())
      {
        while (enumerator.MoveNext())
          yield return new IndexValuePair<T>(enumerator);
      }
    }

    private class ChangeResistantEnumerable<T> : IEnumerable<T>, IEnumerable
    {
      private readonly ObservableCollection<T> _collection;

      public ChangeResistantEnumerable(ObservableCollection<T> collection)
      {
        ArgumentUtility.CheckNotNull<ObservableCollection<T>>(nameof (collection), collection);
        this._collection = collection;
      }

      public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) new ChangeResistantObservableCollectionEnumerator<T>(this._collection);

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
