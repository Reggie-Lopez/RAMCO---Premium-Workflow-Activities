// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Advice
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Advice
  {
    private Collection<Saml2Id> _assertionIdReferences = new Collection<Saml2Id>();
    private Collection<Saml2Assertion> _assertions = new Collection<Saml2Assertion>();
    private AbsoluteUriCollection _assertionUriReferences = new AbsoluteUriCollection();

    public Collection<Saml2Id> AssertionIdReferences => this._assertionIdReferences;

    public Collection<Saml2Assertion> Assertions => this._assertions;

    public Collection<Uri> AssertionUriReferences => (Collection<Uri>) this._assertionUriReferences;
  }
}
