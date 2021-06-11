// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.UniqueIdentifierGenerator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Remotion.Linq
{
  [ComVisible(true)]
  public sealed class UniqueIdentifierGenerator
  {
    private readonly HashSet<string> _knownIdentifiers = new HashSet<string>();
    private int _identifierCounter;

    public void AddKnownIdentifier(string identifier)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (identifier), identifier);
      this._knownIdentifiers.Add(identifier);
    }

    private bool IsKnownIdentifier(string identifier)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (identifier), identifier);
      return this._knownIdentifiers.Contains(identifier);
    }

    public void Reset()
    {
      this._knownIdentifiers.Clear();
      this._identifierCounter = 0;
    }

    public string GetUniqueIdentifier(string prefix)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (prefix), prefix);
      string identifier;
      do
      {
        identifier = prefix + (object) this._identifierCounter;
        ++this._identifierCounter;
      }
      while (this.IsKnownIdentifier(identifier));
      return identifier;
    }
  }
}
