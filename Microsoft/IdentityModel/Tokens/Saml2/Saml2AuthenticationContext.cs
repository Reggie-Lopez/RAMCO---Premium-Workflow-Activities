// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AuthenticationContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AuthenticationContext
  {
    private Collection<Uri> _authenticatingAuthorities = (Collection<Uri>) new AbsoluteUriCollection();
    private Uri _classReference;
    private Uri _declarationReference;

    public Saml2AuthenticationContext()
      : this((Uri) null, (Uri) null)
    {
    }

    public Saml2AuthenticationContext(Uri classReference)
      : this(classReference, (Uri) null)
    {
    }

    public Saml2AuthenticationContext(Uri classReference, Uri declarationReference)
    {
      if ((Uri) null != classReference && !classReference.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (classReference), Microsoft.IdentityModel.SR.GetString("ID0013"));
      if ((Uri) null != declarationReference && !declarationReference.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (declarationReference), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._classReference = classReference;
      this._declarationReference = declarationReference;
    }

    public Collection<Uri> AuthenticatingAuthorities => this._authenticatingAuthorities;

    public Uri ClassReference
    {
      get => this._classReference;
      set => this._classReference = !((Uri) null != value) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
    }

    public Uri DeclarationReference
    {
      get => this._declarationReference;
      set => this._declarationReference = !((Uri) null != value) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
    }
  }
}
