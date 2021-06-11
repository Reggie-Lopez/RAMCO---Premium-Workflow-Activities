// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.IndexValuePair`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;

namespace Remotion.Linq.Collections
{
  internal struct IndexValuePair<T>
  {
    private readonly ChangeResistantObservableCollectionEnumerator<T> _enumerator;

    public IndexValuePair(
      ChangeResistantObservableCollectionEnumerator<T> enumerator)
    {
      ArgumentUtility.CheckNotNull<ChangeResistantObservableCollectionEnumerator<T>>(nameof (enumerator), enumerator);
      this._enumerator = enumerator;
    }

    public int Index => this._enumerator.Index;

    public T Value => this._enumerator.Current;
  }
}
