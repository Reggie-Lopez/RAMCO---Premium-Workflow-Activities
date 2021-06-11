// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.Claim
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public class Claim
  {
    private string _issuer;
    private string _originalIssuer;
    private Dictionary<string, string> _properties;
    private IClaimsIdentity _subject;
    private string _type;
    private string _value;
    private string _valueType;

    public Claim(string claimType, string value)
      : this(claimType, value, "http://www.w3.org/2001/XMLSchema#string", "LOCAL AUTHORITY")
    {
    }

    public Claim(string claimType, string value, string valueType)
      : this(claimType, value, valueType, "LOCAL AUTHORITY")
    {
    }

    public Claim(string claimType, string value, string valueType, string issuer)
      : this(claimType, value, valueType, issuer, issuer)
    {
    }

    public Claim(
      string claimType,
      string value,
      string valueType,
      string issuer,
      string originalIssuer)
    {
      if (claimType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("type");
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      this._originalIssuer = originalIssuer;
      this._type = claimType;
      this._value = value;
      this._valueType = !string.IsNullOrEmpty(valueType) ? valueType : "http://www.w3.org/2001/XMLSchema#string";
      if (string.IsNullOrEmpty(issuer))
      {
        this._issuer = "LOCAL AUTHORITY";
        if (string.IsNullOrEmpty(originalIssuer))
          this._originalIssuer = "LOCAL AUTHORITY";
      }
      else
        this._issuer = issuer;
      this._type = StringUtil.OptimizeString(this._type);
      this._value = StringUtil.OptimizeString(this._value);
      this._valueType = StringUtil.OptimizeString(this._valueType);
      this._issuer = StringUtil.OptimizeString(this._issuer);
      this._originalIssuer = StringUtil.OptimizeString(this._originalIssuer);
    }

    public Claim(System.IdentityModel.Claims.Claim claim, string issuer)
    {
      if (claim == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claim));
      if (claim.Resource == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4031")));
      this._issuer = !string.IsNullOrEmpty(issuer) ? issuer : "LOCAL AUTHORITY";
      this._originalIssuer = issuer;
      this._valueType = "http://www.w3.org/2001/XMLSchema#string";
      if (claim.Resource is string)
        this.AssignClaimFromStringResourceSysClaim(claim);
      else
        this.AssignClaimFromSysClaim(claim);
      if (this._value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4030", (object) claim.ClaimType, (object) claim.Resource.GetType())));
    }

    public virtual string ClaimType => this._type;

    public virtual string Issuer => this._issuer;

    public virtual string OriginalIssuer => this._originalIssuer;

    public virtual IDictionary<string, string> Properties
    {
      get
      {
        if (this._properties == null)
          this._properties = new Dictionary<string, string>();
        return (IDictionary<string, string>) this._properties;
      }
    }

    public virtual IClaimsIdentity Subject => this._subject;

    public virtual string Value => this._value;

    public virtual string ValueType => this._valueType;

    private void AssignClaimFromStringResourceSysClaim(System.IdentityModel.Claims.Claim claim)
    {
      this._type = claim.ClaimType;
      this._value = (string) claim.Resource;
      if (!StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"))
        return;
      if (claim.Right == Rights.Identity)
        this._type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
      else
        this._type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
    }

    private void AssignClaimFromSysClaim(System.IdentityModel.Claims.Claim claim)
    {
      if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid") && (object) (claim.Resource as SecurityIdentifier) != null)
      {
        this._type = !(claim.Right == Rights.Identity) ? "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid" : "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
        this._value = ((IdentityReference) claim.Resource).Value;
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") && claim.Resource is MailAddress)
      {
        this._type = claim.ClaimType;
        this._value = ((MailAddress) claim.Resource).Address;
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint") && claim.Resource is byte[])
      {
        this._type = claim.ClaimType;
        this._value = Convert.ToBase64String((byte[]) claim.Resource);
        this._valueType = "http://www.w3.org/2001/XMLSchema#base64Binary";
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash") && claim.Resource is byte[])
      {
        this._type = claim.ClaimType;
        this._value = Convert.ToBase64String((byte[]) claim.Resource);
        this._valueType = "http://www.w3.org/2001/XMLSchema#base64Binary";
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") && claim.Resource is SamlNameIdentifierClaimResource)
      {
        this._type = claim.ClaimType;
        this._value = ((SamlNameIdentifierClaimResource) claim.Resource).Name;
        if (((SamlNameIdentifierClaimResource) claim.Resource).Format != null)
        {
          if (this._properties == null)
            this._properties = new Dictionary<string, string>();
          this._properties.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format", ((SamlNameIdentifierClaimResource) claim.Resource).Format);
        }
        if (((SamlNameIdentifierClaimResource) claim.Resource).NameQualifier == null)
          return;
        if (this._properties == null)
          this._properties = new Dictionary<string, string>();
        this._properties.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier", ((SamlNameIdentifierClaimResource) claim.Resource).NameQualifier);
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname") && claim.Resource is X500DistinguishedName)
      {
        this._type = claim.ClaimType;
        this._value = ((X500DistinguishedName) claim.Resource).Name;
        this._valueType = "urn:oasis:names:tc:xacml:1.0:data-type:x500Name";
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri") && (object) (claim.Resource as Uri) != null)
      {
        this._type = claim.ClaimType;
        this._value = ((Uri) claim.Resource).ToString();
      }
      else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa") && claim.Resource is RSA)
      {
        this._type = claim.ClaimType;
        this._value = ((AsymmetricAlgorithm) claim.Resource).ToXmlString(false);
        this._valueType = "http://www.w3.org/2000/09/xmldsig#RSAKeyValue";
      }
      else
      {
        if (!StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid") || (object) (claim.Resource as SecurityIdentifier) == null)
          return;
        this._type = claim.ClaimType;
        this._value = ((IdentityReference) claim.Resource).Value;
      }
    }

    public virtual Claim Copy()
    {
      Claim claim = new Claim(this._type, this._value, this._valueType, this._issuer, this._originalIssuer);
      if (this._properties != null)
      {
        foreach (string key in this._properties.Keys)
          claim.Properties[key] = this._properties[key];
      }
      return claim;
    }

    public virtual void SetSubject(IClaimsIdentity subject) => this._subject = subject;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}: {1}", (object) this._type, (object) this._value);
  }
}
