// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsIdentityCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class ClaimsIdentityCollection : 
    IList<IClaimsIdentity>,
    ICollection<IClaimsIdentity>,
    IEnumerable<IClaimsIdentity>,
    IEnumerable
  {
    private List<IClaimsIdentity> _collection = new List<IClaimsIdentity>();

    public ClaimsIdentityCollection()
    {
    }

    public ClaimsIdentityCollection(IEnumerable<IClaimsIdentity> subjects)
    {
      if (subjects == null)
        return;
      this._collection.AddRange(subjects);
    }

    public ClaimsIdentityCollection Copy()
    {
      ClaimsIdentityCollection identityCollection = new ClaimsIdentityCollection();
      foreach (IClaimsIdentity claimsIdentity in this)
        identityCollection.Add(claimsIdentity.Copy());
      return identityCollection;
    }

    public void AddRange(IEnumerable<IClaimsIdentity> collection)
    {
      if (collection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (collection));
      this._collection.AddRange(collection);
    }

    public IClaimsIdentity this[int index]
    {
      get => this._collection[index];
      set => this._collection[index] = value;
    }

    public int IndexOf(IClaimsIdentity item) => this._collection.IndexOf(item);

    public void Insert(int index, IClaimsIdentity item)
    {
      if (item == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (item));
      this._collection.Insert(index, item);
    }

    public void RemoveAt(int index) => this._collection.RemoveAt(index);

    public void Add(IClaimsIdentity item)
    {
      if (item == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (item));
      this._collection.Add(item);
    }

    public void Clear() => this._collection.Clear();

    public bool Contains(IClaimsIdentity item) => this._collection.Contains(item);

    public void CopyTo(IClaimsIdentity[] array, int arrayIndex)
    {
      if (array == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (array));
      this._collection.CopyTo(array, arrayIndex);
    }

    public int Count => this._collection.Count;

    public bool IsReadOnly => false;

    public bool Remove(IClaimsIdentity item) => this._collection.Remove(item);

    public IEnumerator<IClaimsIdentity> GetEnumerator() => (IEnumerator<IClaimsIdentity>) this._collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this._collection).GetEnumerator();
  }
}
