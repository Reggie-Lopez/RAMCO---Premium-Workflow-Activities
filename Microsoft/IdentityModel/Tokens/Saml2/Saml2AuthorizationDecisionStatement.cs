// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AuthorizationDecisionStatement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AuthorizationDecisionStatement : Saml2Statement
  {
    public static readonly Uri EmptyResource = new Uri("", UriKind.Relative);
    private Collection<Saml2Action> _actions = new Collection<Saml2Action>();
    private Saml2Evidence _evidence;
    private SamlAccessDecision _decision;
    private Uri _resource;

    public Saml2AuthorizationDecisionStatement(Uri resource, SamlAccessDecision decision)
      : this(resource, decision, (IEnumerable<Saml2Action>) null)
    {
    }

    public Saml2AuthorizationDecisionStatement(
      Uri resource,
      SamlAccessDecision decision,
      IEnumerable<Saml2Action> actions)
    {
      if ((Uri) null == resource)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (resource));
      if (!resource.IsAbsoluteUri && !resource.Equals((object) Saml2AuthorizationDecisionStatement.EmptyResource))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (resource), Microsoft.IdentityModel.SR.GetString("ID4121"));
      if (decision < SamlAccessDecision.Permit || decision > SamlAccessDecision.Indeterminate)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (decision));
      this._resource = resource;
      this._decision = decision;
      if (actions == null)
        return;
      foreach (Saml2Action action in actions)
        this._actions.Add(action);
    }

    public Collection<Saml2Action> Actions => this._actions;

    public SamlAccessDecision Decision
    {
      get => this._decision;
      set => this._decision = value >= SamlAccessDecision.Permit && value <= SamlAccessDecision.Indeterminate ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("decision");
    }

    public Saml2Evidence Evidence
    {
      get => this._evidence;
      set => this._evidence = value;
    }

    public Uri Resource
    {
      get => this._resource;
      set
      {
        if ((Uri) null == value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        if (!value.IsAbsoluteUri && !value.Equals((object) Saml2AuthorizationDecisionStatement.EmptyResource))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4121"));
        this._resource = value;
      }
    }
  }
}
