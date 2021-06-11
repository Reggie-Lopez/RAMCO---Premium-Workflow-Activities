// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.MultiDictionaryExtensions
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remotion.Linq.Collections
{
  internal static class MultiDictionaryExtensions
  {
    public static void Add<TKey, TValue>(
      this IDictionary<TKey, ICollection<TValue>> dictionary,
      TKey key,
      TValue item)
    {
      ArgumentUtility.CheckNotNull<IDictionary<TKey, ICollection<TValue>>>(nameof (dictionary), dictionary);
      ArgumentUtility.CheckNotNull<TKey>(nameof (key), key);
      ArgumentUtility.CheckNotNull<TValue>(nameof (item), item);
      ICollection<TValue> objs;
      if (!dictionary.TryGetValue(key, out objs))
      {
        objs = (ICollection<TValue>) new List<TValue>();
        dictionary.Add(key, objs);
      }
      objs.Add(item);
    }

    public static int CountValues<TKey, TValue>(
      this IDictionary<TKey, ICollection<TValue>> dictionary)
    {
      ArgumentUtility.CheckNotNull<IDictionary<TKey, ICollection<TValue>>>(nameof (dictionary), dictionary);
      return dictionary.Sum<KeyValuePair<TKey, ICollection<TValue>>>((Func<KeyValuePair<TKey, ICollection<TValue>>, int>) (kvp => kvp.Value.Count));
    }
  }
}
