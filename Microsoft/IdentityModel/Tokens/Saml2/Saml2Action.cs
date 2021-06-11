// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Action
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Action
  {
    private Uri _namespace;
    private string _value;

    public Saml2Action(string value, Uri actionNamespace)
    {
      if (string.IsNullOrEmpty(value))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if ((Uri) null == actionNamespace)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (actionNamespace));
      this._namespace = actionNamespace.IsAbsoluteUri ? actionNamespace : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (actionNamespace), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._value = value;
    }

    public Uri Namespace
    {
      get => this._namespace;
      set
      {
        if ((Uri) null == value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._namespace = value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
      }
    }

    public string Value
    {
      get => this._value;
      set => this._value = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
