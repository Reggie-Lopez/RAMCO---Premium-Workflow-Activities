// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SubjectConfirmation
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2SubjectConfirmation
  {
    private Saml2SubjectConfirmationData _data;
    private Uri _method;
    private Saml2NameIdentifier _nameId;

    public Saml2SubjectConfirmation(Uri method)
      : this(method, (Saml2SubjectConfirmationData) null)
    {
    }

    public Saml2SubjectConfirmation(Uri method, Saml2SubjectConfirmationData data)
    {
      if ((Uri) null == method)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (method));
      this._method = method.IsAbsoluteUri ? method : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (method), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._data = data;
    }

    public Uri Method
    {
      get => this._method;
      set
      {
        if ((Uri) null == value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._method = value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
      }
    }

    public Saml2NameIdentifier NameIdentifier
    {
      get => this._nameId;
      set => this._nameId = value;
    }

    public Saml2SubjectConfirmationData SubjectConfirmationData
    {
      get => this._data;
      set => this._data = value;
    }
  }
}
