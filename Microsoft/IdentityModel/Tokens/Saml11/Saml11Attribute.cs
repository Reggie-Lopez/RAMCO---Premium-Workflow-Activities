// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml11.Saml11Attribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
  [ComVisible(true)]
  public class Saml11Attribute : SamlAttribute
  {
    private string _originalIssuer;
    private string _attributeValueXsiType = "http://www.w3.org/2001/XMLSchema#string";

    public Saml11Attribute()
    {
    }

    public Saml11Attribute(
      string attributeNamespace,
      string attributeName,
      IEnumerable<string> attributeValues)
      : base(attributeNamespace, attributeName, attributeValues)
    {
      attributeNamespace = StringUtil.OptimizeString(attributeNamespace);
      attributeName = StringUtil.OptimizeString(attributeName);
    }

    public new string OriginalIssuer
    {
      get => this._originalIssuer;
      set => this._originalIssuer = !(value == string.Empty) ? StringUtil.OptimizeString(value) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID4251"));
    }

    public new string AttributeValueXsiType
    {
      get => this._attributeValueXsiType;
      set
      {
        int length = !string.IsNullOrEmpty(value) ? value.IndexOf('#') : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID4254"));
        if (length == -1)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID4254"));
        if (value.Substring(0, length).Length == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID4254"));
        if (value.Substring(length + 1).Length == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID4254"));
        this._attributeValueXsiType = value;
      }
    }
  }
}
