// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AuthenticationStatement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AuthenticationStatement : Saml2Statement
  {
    private Saml2AuthenticationContext _authnContext;
    private DateTime _authnInstant;
    private string _sessionIndex;
    private DateTime? _sessionNotOnOrAfter;
    private Saml2SubjectLocality _subjectLocality;

    public Saml2AuthenticationStatement(Saml2AuthenticationContext authenticationContext)
      : this(authenticationContext, DateTime.UtcNow)
    {
    }

    public Saml2AuthenticationStatement(
      Saml2AuthenticationContext authenticationContext,
      DateTime authenticationInstant)
    {
      this._authnContext = authenticationContext != null ? authenticationContext : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (authenticationContext));
      this._authnInstant = DateTimeUtil.ToUniversalTime(authenticationInstant);
    }

    public Saml2AuthenticationContext AuthenticationContext
    {
      get => this._authnContext;
      set => this._authnContext = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public DateTime AuthenticationInstant
    {
      get => this._authnInstant;
      set => this._authnInstant = DateTimeUtil.ToUniversalTime(value);
    }

    public string SessionIndex
    {
      get => this._sessionIndex;
      set => this._sessionIndex = XmlUtil.NormalizeEmptyString(value);
    }

    public DateTime? SessionNotOnOrAfter
    {
      get => this._sessionNotOnOrAfter;
      set => this._sessionNotOnOrAfter = DateTimeUtil.ToUniversalTime(value);
    }

    public Saml2SubjectLocality SubjectLocality
    {
      get => this._subjectLocality;
      set => this._subjectLocality = value;
    }
  }
}
