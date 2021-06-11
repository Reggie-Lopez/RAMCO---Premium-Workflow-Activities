// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Attribute
  {
    private string _friendlyName;
    private string _name;
    private Uri _nameFormat;
    private Collection<string> _values = new Collection<string>();
    private string _originalIssuer;
    private string _attributeValueXsiType = "http://www.w3.org/2001/XMLSchema#string";

    public Saml2Attribute(string name) => this._name = !string.IsNullOrEmpty(name) ? name : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));

    public Saml2Attribute(string name, string value)
      : this(name, (IEnumerable<string>) new string[1]
      {
        value
      })
    {
    }

    public Saml2Attribute(string name, IEnumerable<string> values)
      : this(name)
    {
      if (values == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (values));
      foreach (string str in values)
        this._values.Add(str);
    }

    public string FriendlyName
    {
      get => this._friendlyName;
      set => this._friendlyName = XmlUtil.NormalizeEmptyString(value);
    }

    public string Name
    {
      get => this._name;
      set => this._name = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentNullException(nameof (value)));
    }

    public Uri NameFormat
    {
      get => this._nameFormat;
      set => this._nameFormat = !((Uri) null != value) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("error", Microsoft.IdentityModel.SR.GetString("ID0013"));
    }

    public string OriginalIssuer
    {
      get => this._originalIssuer;
      set => this._originalIssuer = !(value == string.Empty) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4251"));
    }

    public string AttributeValueXsiType
    {
      get => this._attributeValueXsiType;
      set
      {
        int length = !string.IsNullOrEmpty(value) ? value.IndexOf('#') : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4254"));
        if (length == -1)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4254"));
        if (value.Substring(0, length).Length == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4254"));
        if (value.Substring(length + 1).Length == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4254"));
        this._attributeValueXsiType = value;
      }
    }

    public Collection<string> Values => this._values;
  }
}
