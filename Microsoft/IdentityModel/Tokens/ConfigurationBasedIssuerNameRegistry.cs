// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class ConfigurationBasedIssuerNameRegistry : IssuerNameRegistry
  {
    private Dictionary<string, string> _configuredTrustedIssuers = new Dictionary<string, string>((IEqualityComparer<string>) new ConfigurationBasedIssuerNameRegistry.ThumbprintKeyComparer());

    public ConfigurationBasedIssuerNameRegistry()
    {
    }

    public ConfigurationBasedIssuerNameRegistry(XmlNodeList customConfiguration)
    {
      List<XmlElement> xmlElementList = customConfiguration != null ? XmlUtil.GetXmlElements(customConfiguration) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customConfiguration));
      XmlElement xmlElement1 = xmlElementList.Count == 1 ? xmlElementList[0] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7019", (object) typeof (ConfigurationBasedIssuerNameRegistry).Name));
      if (!StringComparer.Ordinal.Equals(xmlElement1.LocalName, "trustedIssuers"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7002", (object) xmlElement1.LocalName, (object) "trustedIssuers"));
      foreach (XmlNode childNode in xmlElement1.ChildNodes)
      {
        if (childNode is XmlElement xmlElement2)
        {
          if (StringComparer.Ordinal.Equals(xmlElement2.LocalName, "add"))
          {
            XmlNode namedItem1 = xmlElement2.Attributes.GetNamedItem("thumbprint");
            XmlNode namedItem2 = xmlElement2.Attributes.GetNamedItem("name");
            if (xmlElement2.Attributes.Count != 2 || namedItem1 == null || (namedItem2 == null || string.IsNullOrEmpty(namedItem2.Value)))
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7010", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", (object) xmlElement1.LocalName, (object) xmlElement2.LocalName), (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} and {1}", (object) "thumbprint", (object) "name")));
            this._configuredTrustedIssuers.Add(namedItem1.Value.Replace(" ", ""), string.Intern(namedItem2.Value));
          }
          else if (StringComparer.Ordinal.Equals(xmlElement2.LocalName, "remove"))
          {
            if (xmlElement2.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(xmlElement2.Attributes[0].LocalName, "thumbprint"))
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7010", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", (object) xmlElement1.LocalName, (object) xmlElement2.LocalName), (object) "thumbprint"));
            this._configuredTrustedIssuers.Remove(xmlElement2.Attributes.GetNamedItem("thumbprint").Value.Replace(" ", ""));
          }
          else if (StringComparer.Ordinal.Equals(xmlElement2.LocalName, "clear"))
            this._configuredTrustedIssuers.Clear();
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7002", (object) xmlElement1.LocalName, (object) xmlElement2.LocalName));
        }
      }
    }

    public override string GetIssuerName(SecurityToken securityToken)
    {
      if (securityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityToken));
      if (securityToken is X509SecurityToken x509SecurityToken)
      {
        string thumbprint = x509SecurityToken.Certificate.Thumbprint;
        if (this._configuredTrustedIssuers.ContainsKey(thumbprint))
          return this._configuredTrustedIssuers[thumbprint];
      }
      return (string) null;
    }

    public IDictionary<string, string> ConfiguredTrustedIssuers => (IDictionary<string, string>) this._configuredTrustedIssuers;

    public void AddTrustedIssuer(string certificateThumbprint, string name)
    {
      if (string.IsNullOrEmpty(certificateThumbprint))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (certificateThumbprint));
      if (string.IsNullOrEmpty(name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (name));
      certificateThumbprint = !this._configuredTrustedIssuers.ContainsKey(certificateThumbprint) ? certificateThumbprint.Replace(" ", "") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4265", (object) certificateThumbprint));
      this._configuredTrustedIssuers.Add(certificateThumbprint, name);
    }

    private class ThumbprintKeyComparer : IEqualityComparer<string>
    {
      public bool Equals(string x, string y) => StringComparer.OrdinalIgnoreCase.Equals(x, y);

      public int GetHashCode(string obj) => obj.ToUpper(CultureInfo.InvariantCulture).GetHashCode();
    }
  }
}
