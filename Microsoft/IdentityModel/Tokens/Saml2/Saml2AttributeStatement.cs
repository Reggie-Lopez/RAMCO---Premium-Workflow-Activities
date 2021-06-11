// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AttributeStatement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AttributeStatement : Saml2Statement
  {
    private Collection<Saml2Attribute> _attributes = new Collection<Saml2Attribute>();

    public Saml2AttributeStatement()
    {
    }

    public Saml2AttributeStatement(Saml2Attribute attribute)
      : this((IEnumerable<Saml2Attribute>) new Saml2Attribute[1]
      {
        attribute
      })
    {
    }

    public Saml2AttributeStatement(IEnumerable<Saml2Attribute> attributes)
    {
      if (attributes == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (attributes));
      foreach (Saml2Attribute attribute in attributes)
      {
        if (attribute == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (attributes));
        this._attributes.Add(attribute);
      }
    }

    public Collection<Saml2Attribute> Attributes => this._attributes;
  }
}
