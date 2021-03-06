// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.NameBasedRegistrationInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  [ComVisible(true)]
  public sealed class NameBasedRegistrationInfo
  {
    private readonly string _name;
    private readonly Func<MethodInfo, bool> _filter;

    public NameBasedRegistrationInfo(string name, Func<MethodInfo, bool> filter)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (name), name);
      ArgumentUtility.CheckNotNull<Func<MethodInfo, bool>>(nameof (filter), filter);
      this._name = name;
      this._filter = filter;
    }

    public string Name => this._name;

    public Func<MethodInfo, bool> Filter => this._filter;
  }
}
