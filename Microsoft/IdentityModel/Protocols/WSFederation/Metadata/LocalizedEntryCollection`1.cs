﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.LocalizedEntryCollection`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class LocalizedEntryCollection<T> : KeyedCollection<CultureInfo, T>
    where T : LocalizedEntry
  {
    protected override CultureInfo GetKeyForItem(T item) => item.Language;
  }
}
