// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Subject
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Subject
  {
    private Saml2NameIdentifier _nameId;
    private Collection<Saml2SubjectConfirmation> _subjectConfirmations = new Collection<Saml2SubjectConfirmation>();

    public Saml2Subject()
    {
    }

    public Saml2Subject(Saml2NameIdentifier nameId) => this._nameId = nameId;

    public Saml2Subject(Saml2SubjectConfirmation subjectConfirmation)
    {
      if (subjectConfirmation == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subjectConfirmation));
      this._subjectConfirmations.Add(subjectConfirmation);
    }

    public Saml2NameIdentifier NameId
    {
      get => this._nameId;
      set => this._nameId = value;
    }

    public Collection<Saml2SubjectConfirmation> SubjectConfirmations => this._subjectConfirmations;
  }
}
