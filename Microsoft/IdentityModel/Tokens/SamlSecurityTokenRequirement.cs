// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Text;
using System.Web.Compilation;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SamlSecurityTokenRequirement
  {
    private static X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;
    private static X509CertificateValidationMode DefaultValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
    private static StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;
    private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private bool _mapToWindows;
    private bool _useWindowsTokenService;
    private X509CertificateValidator _certificateValidator;

    public SamlSecurityTokenRequirement()
    {
    }

    public SamlSecurityTokenRequirement(XmlElement element)
    {
      if (element == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      if (element.LocalName != "samlSecurityTokenRequirement")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7000", (object) "samlSecurityTokenRequirement", (object) element.LocalName));
      bool flag = false;
      X509RevocationMode revocationMode = SamlSecurityTokenRequirement.DefaultRevocationMode;
      X509CertificateValidationMode certificateValidationMode = SamlSecurityTokenRequirement.DefaultValidationMode;
      StoreLocation trustedStoreLocation = SamlSecurityTokenRequirement.DefaultStoreLocation;
      string typeName1 = (string) null;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) element.Attributes)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "mapToWindows"))
        {
          bool result = false;
          if (!bool.TryParse(attribute.Value, out result))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7022", (object) attribute.Value));
          this.MapToWindows = result;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
        {
          bool result = false;
          if (!bool.TryParse(attribute.Value, out result))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7023", (object) attribute.Value));
          this.UseWindowsTokenService = result;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateValidator"))
          typeName1 = attribute.Value.ToString();
        else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateRevocationMode"))
        {
          flag = true;
          string x = attribute.Value.ToString();
          if (StringComparer.OrdinalIgnoreCase.Equals(x, "NoCheck"))
            revocationMode = X509RevocationMode.NoCheck;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "Offline"))
            revocationMode = X509RevocationMode.Offline;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "Online"))
            revocationMode = X509RevocationMode.Online;
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) element.LocalName)));
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateValidationMode"))
        {
          flag = true;
          string x = attribute.Value.ToString();
          if (StringComparer.OrdinalIgnoreCase.Equals(x, "ChainTrust"))
            certificateValidationMode = X509CertificateValidationMode.ChainTrust;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "PeerOrChainTrust"))
            certificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "PeerTrust"))
            certificateValidationMode = X509CertificateValidationMode.PeerTrust;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "None"))
            certificateValidationMode = X509CertificateValidationMode.None;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "Custom"))
            certificateValidationMode = X509CertificateValidationMode.Custom;
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) element.LocalName)));
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateTrustedStoreLocation"))
        {
          flag = true;
          string x = attribute.Value.ToString();
          if (StringComparer.OrdinalIgnoreCase.Equals(x, "CurrentUser"))
            trustedStoreLocation = StoreLocation.CurrentUser;
          else if (StringComparer.OrdinalIgnoreCase.Equals(x, "LocalMachine"))
            trustedStoreLocation = StoreLocation.LocalMachine;
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) element.LocalName)));
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7004", (object) attribute.LocalName, (object) element.LocalName)));
      }
      foreach (XmlElement xmlElement in XmlUtil.GetXmlElements(element.ChildNodes))
      {
        if (StringComparer.Ordinal.Equals(xmlElement.LocalName, "nameClaimType"))
        {
          if (xmlElement.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(xmlElement.Attributes[0].LocalName, "value"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7001", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", (object) element.LocalName, (object) xmlElement.LocalName), (object) "value"));
          this.NameClaimType = xmlElement.Attributes[0].Value;
        }
        else if (StringComparer.Ordinal.Equals(xmlElement.LocalName, "roleClaimType"))
        {
          if (xmlElement.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(xmlElement.Attributes[0].LocalName, "value"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7001", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", (object) element.LocalName, (object) xmlElement.LocalName), (object) "value"));
          this.RoleClaimType = xmlElement.Attributes[0].Value;
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7002", (object) xmlElement.LocalName, (object) "samlSecurityTokenRequirement"));
      }
      if (certificateValidationMode == X509CertificateValidationMode.Custom)
      {
        Type typeName2 = !string.IsNullOrEmpty(typeName1) ? BuildManager.GetType(typeName1, true) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7028"));
        this._certificateValidator = (object) typeName2 != null ? CustomTypeElement.Resolve<X509CertificateValidator>(new CustomTypeElement(typeName2)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", Microsoft.IdentityModel.SR.GetString("ID7007", (object) typeName2));
      }
      else
      {
        if (!flag)
          return;
        this._certificateValidator = X509Util.CreateCertificateValidator(certificateValidationMode, revocationMode, trustedStoreLocation);
      }
    }

    public X509CertificateValidator CertificateValidator
    {
      get => this._certificateValidator;
      set => this._certificateValidator = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public string NameClaimType
    {
      get => this._nameClaimType;
      set => this._nameClaimType = value;
    }

    public string RoleClaimType
    {
      get => this._roleClaimType;
      set => this._roleClaimType = value;
    }

    public bool MapToWindows
    {
      get => this._mapToWindows;
      set => this._mapToWindows = value;
    }

    public bool UseWindowsTokenService
    {
      get => this._useWindowsTokenService;
      set => this._useWindowsTokenService = value;
    }

    public virtual bool ShouldEnforceAudienceRestriction(
      AudienceUriMode audienceUriMode,
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      switch (audienceUriMode)
      {
        case AudienceUriMode.Never:
          return false;
        case AudienceUriMode.Always:
          return true;
        case AudienceUriMode.BearerKeyOnly:
          return token.SecurityKeys == null || 0 == token.SecurityKeys.Count;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4025", (object) audienceUriMode)));
      }
    }

    public virtual void ValidateAudienceRestriction(
      IList<Uri> allowedAudienceUris,
      IList<Uri> tokenAudiences)
    {
      if (allowedAudienceUris == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (allowedAudienceUris));
      if (tokenAudiences == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenAudiences));
      if (tokenAudiences.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1036")));
      if (allowedAudienceUris.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1043")));
      bool flag1 = false;
      foreach (Uri tokenAudience in (IEnumerable<Uri>) tokenAudiences)
      {
        if (tokenAudience != (Uri) null)
        {
          Uri uri1;
          if (tokenAudience.IsAbsoluteUri)
          {
            uri1 = new Uri(tokenAudience.GetLeftPart(UriPartial.Path));
          }
          else
          {
            Uri baseUri = new Uri("http://www.example.com");
            Uri uri2 = new Uri(baseUri, tokenAudience);
            uri1 = baseUri.MakeRelativeUri(new Uri(uri2.GetLeftPart(UriPartial.Path)));
          }
          if (allowedAudienceUris.Contains(uri1))
          {
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
        return;
      if (1 == tokenAudiences.Count || (Uri) null != tokenAudiences[0])
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1038", (object) tokenAudiences[0].OriginalString)));
      StringBuilder stringBuilder = new StringBuilder(Microsoft.IdentityModel.SR.GetString("ID8007"));
      bool flag2 = true;
      foreach (Uri tokenAudience in (IEnumerable<Uri>) tokenAudiences)
      {
        if (tokenAudience != (Uri) null)
        {
          if (flag2)
            flag2 = false;
          else
            stringBuilder.Append(", ");
          stringBuilder.Append(tokenAudience.OriginalString);
        }
      }
      DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Error, stringBuilder.ToString());
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1037")));
    }
  }
}
