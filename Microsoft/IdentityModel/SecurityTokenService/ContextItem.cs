// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.ContextItem
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class ContextItem
  {
    private Uri _name;
    private Uri _scope;
    private string _value;

    public ContextItem(Uri name)
      : this(name, (string) null)
    {
    }

    public ContextItem(Uri name, string value)
      : this(name, value, (Uri) null)
    {
    }

    public ContextItem(Uri name, string value, Uri scope)
    {
      if (name == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));
      if (!name.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (name), Microsoft.IdentityModel.SR.GetString("ID0013"));
      if (scope != (Uri) null && !scope.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (scope), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._name = name;
      this._scope = scope;
      this._value = value;
    }

    public Uri Name
    {
      get => this._name;
      set => this._name = value;
    }

    public Uri Scope
    {
      get => this._scope;
      set => this._scope = !(value != (Uri) null) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
    }

    public string Value
    {
      get => this._value;
      set => this._value = value;
    }
  }
}
