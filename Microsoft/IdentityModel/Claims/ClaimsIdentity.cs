// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsIdentity
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Security;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class ClaimsIdentity : IClaimsIdentity, IIdentity, ISerializable
  {
    public const string DefaultNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    public const string DefaultIssuer = "LOCAL AUTHORITY";
    private string _authenticationType;
    private ClaimCollection _claims;
    private IClaimsIdentity _actor;
    private string _label;
    private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private SecurityToken _bootstrapToken;
    private string _bootstrapTokenString;

    public ClaimsIdentity()
      : this((IEnumerable<Claim>) null, (string) null, (SecurityToken) null)
    {
    }

    public ClaimsIdentity(IIdentity identity)
      : this((IEnumerable<Claim>) null, (string) null, (SecurityToken) null)
    {
      this._authenticationType = identity != null ? identity.AuthenticationType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      if (identity is IClaimsIdentity claimsIdentity)
      {
        if (claimsIdentity.Claims != null)
          this._claims = claimsIdentity.Claims.CopyWithSubject((IClaimsIdentity) this);
        this._label = claimsIdentity.Label;
        this._nameClaimType = claimsIdentity.NameClaimType;
        this._roleClaimType = claimsIdentity.RoleClaimType;
        this._bootstrapToken = claimsIdentity.BootstrapToken;
        if (claimsIdentity.Actor == null)
          return;
        this._actor = !this.IsCircular(claimsIdentity.Actor) ? claimsIdentity.Actor.Copy() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4035"));
      }
      else
      {
        if (!identity.IsAuthenticated || string.IsNullOrEmpty(identity.Name))
          return;
        this._claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", identity.Name));
      }
    }

    public ClaimsIdentity(IEnumerable<Claim> claims)
      : this(claims, (string) null, (SecurityToken) null)
    {
    }

    public ClaimsIdentity(IEnumerable<Claim> claims, SecurityToken bootstrapToken)
      : this(claims, (string) null, bootstrapToken)
    {
    }

    public ClaimsIdentity(string authenticationType)
      : this((IEnumerable<Claim>) null, authenticationType, (SecurityToken) null)
    {
    }

    public ClaimsIdentity(X509Certificate2 certificate, string issuer)
      : this(ClaimsIdentity.GetClaimsFromCertificate(certificate, issuer), "X509")
    {
    }

    public ClaimsIdentity(X509Certificate2 certificate, string issuer, string authenticationType)
      : this(ClaimsIdentity.GetClaimsFromCertificate(certificate, issuer), authenticationType)
    {
    }

    public ClaimsIdentity(IEnumerable<Claim> claims, string authenticationType)
      : this(claims, authenticationType, (SecurityToken) null)
    {
    }

    public ClaimsIdentity(
      IEnumerable<Claim> claims,
      string authenticationType,
      SecurityToken bootstrapToken)
    {
      this._claims = new ClaimCollection((IClaimsIdentity) this);
      this._authenticationType = authenticationType;
      if (claims != null)
        this._claims.AddRange(claims);
      this._bootstrapToken = bootstrapToken;
    }

    public ClaimsIdentity(string authenticationType, string nameClaimType, string roleClaimType)
      : this((IEnumerable<Claim>) null, authenticationType, nameClaimType, roleClaimType)
    {
    }

    public ClaimsIdentity(
      IEnumerable<Claim> claims,
      string authenticationType,
      string nameClaimType,
      string roleClaimType)
      : this(claims, authenticationType, nameClaimType, roleClaimType, (SecurityToken) null)
    {
    }

    public ClaimsIdentity(
      IEnumerable<Claim> claims,
      string authenticationType,
      string nameClaimType,
      string roleClaimType,
      SecurityToken bootstrapToken)
      : this(claims, authenticationType, bootstrapToken)
    {
      this._nameClaimType = nameClaimType;
      this._roleClaimType = roleClaimType;
    }

    protected ClaimsIdentity(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      try
      {
        this.Deserialize(info, context);
      }
      catch (Exception ex)
      {
        if (DiagnosticUtil.IsFatal(ex))
          throw;
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SerializationException(Microsoft.IdentityModel.SR.GetString("ID4282", (object) nameof (ClaimsIdentity)), ex));
      }
    }

    internal ClaimsIdentity(ClaimSet claimSet)
      : this(claimSet, (string) null)
    {
    }

    internal ClaimsIdentity(ClaimSet claimSet, string authenticationType)
    {
      if (claimSet == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimSet));
      this._claims = new ClaimCollection((IClaimsIdentity) this);
      this._authenticationType = authenticationType;
      string issuer = (string) null;
      if (claimSet.Issuer == null)
      {
        issuer = "LOCAL AUTHORITY";
      }
      else
      {
        foreach (System.IdentityModel.Claims.Claim claim in claimSet.Issuer.FindClaims("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", Rights.Identity))
        {
          if (claim != null && claim.Resource is string)
          {
            issuer = claim.Resource as string;
            break;
          }
        }
      }
      for (int index = 0; index < claimSet.Count; ++index)
      {
        if (string.Equals(claimSet[index].Right, Rights.PossessProperty, StringComparison.Ordinal))
          this._claims.Add(new Claim(claimSet[index], issuer));
      }
    }

    internal ClaimsIdentity(
      ClaimSet claimSet,
      string authenticationType,
      string nameClaimType,
      string roleClaimType)
      : this(claimSet, authenticationType)
    {
      this._nameClaimType = nameClaimType;
      this._roleClaimType = roleClaimType;
    }

    public string AuthenticationType => this._authenticationType != null ? this._authenticationType : string.Empty;

    public bool IsAuthenticated => this._claims.Count > 0;

    public override string ToString() => this.Name;

    public string Name
    {
      get
      {
        string str = (string) null;
        using (IEnumerator<Claim> enumerator = this._claims.FindAll(new Predicate<Claim>(this.NameClaimPredicate)).GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            Claim current = enumerator.Current;
            if (string.IsNullOrEmpty(current.Value))
              return (string) null;
            str = current.Value;
          }
        }
        return str;
      }
    }

    public IClaimsIdentity Actor
    {
      get => this._actor;
      set => this._actor = value == null || !this.IsCircular(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4035"));
    }

    public ClaimCollection Claims => this._claims;

    public string Label
    {
      get => this._label;
      set => this._label = value;
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

    public SecurityToken BootstrapToken
    {
      get
      {
        if (this._bootstrapToken == null && !string.IsNullOrEmpty(this._bootstrapTokenString))
          this._bootstrapToken = ClaimsIdentitySerializer.DeserializeBootstrapTokenFromString(this._bootstrapTokenString);
        return this._bootstrapToken;
      }
      set => this._bootstrapToken = value;
    }

    public IClaimsIdentity Copy()
    {
      ClaimsIdentity claimsIdentity = new ClaimsIdentity(this.AuthenticationType);
      if (this.Claims != null)
        claimsIdentity._claims = this.Claims.CopyWithSubject((IClaimsIdentity) claimsIdentity);
      claimsIdentity.Label = this.Label;
      claimsIdentity.NameClaimType = this.NameClaimType;
      claimsIdentity.RoleClaimType = this.RoleClaimType;
      claimsIdentity.BootstrapToken = this.BootstrapToken;
      if (this.Actor != null)
        claimsIdentity.Actor = !this.IsCircular(this.Actor) ? this.Actor.Copy() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4035"));
      return (IClaimsIdentity) claimsIdentity;
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => this.GetObjectData(info, context);

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      ClaimsIdentitySerializer identitySerializer = info != null ? new ClaimsIdentitySerializer(info, context) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      identitySerializer.SerializeNameClaimType(this._nameClaimType);
      identitySerializer.SerializeRoleClaimType(this._roleClaimType);
      identitySerializer.SerializeLabel(this._label);
      identitySerializer.SerializeActor(this._actor);
      identitySerializer.SerializeClaims((IEnumerable<Claim>) this._claims);
      identitySerializer.SerializeBootstrapToken(this._bootstrapToken);
      identitySerializer.SerializeAuthenticationType(this._authenticationType);
    }

    public static IClaimsIdentity AnonymousIdentity => (IClaimsIdentity) new ClaimsIdentity();

    public static IEnumerable<Claim> GetClaimsFromCertificate(
      X509Certificate2 certificate,
      string issuer)
    {
      if (certificate == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      ICollection<Claim> claims = (ICollection<Claim>) new Collection<Claim>();
      string base64String = Convert.ToBase64String(certificate.GetCertHash());
      claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint", base64String, "http://www.w3.org/2001/XMLSchema#base64Binary", issuer));
      string name = certificate.SubjectName.Name;
      if (!string.IsNullOrEmpty(name))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
      string nameInfo1 = certificate.GetNameInfo(X509NameType.DnsName, false);
      if (!string.IsNullOrEmpty(nameInfo1))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns", nameInfo1, "http://www.w3.org/2001/XMLSchema#string", issuer));
      string nameInfo2 = certificate.GetNameInfo(X509NameType.SimpleName, false);
      if (!string.IsNullOrEmpty(nameInfo2))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", nameInfo2, "http://www.w3.org/2001/XMLSchema#string", issuer));
      string nameInfo3 = certificate.GetNameInfo(X509NameType.EmailName, false);
      if (!string.IsNullOrEmpty(nameInfo3))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", nameInfo3, "http://www.w3.org/2001/XMLSchema#string", issuer));
      string nameInfo4 = certificate.GetNameInfo(X509NameType.UpnName, false);
      if (!string.IsNullOrEmpty(nameInfo4))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", nameInfo4, "http://www.w3.org/2001/XMLSchema#string", issuer));
      string nameInfo5 = certificate.GetNameInfo(X509NameType.UrlName, false);
      if (!string.IsNullOrEmpty(nameInfo5))
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri", nameInfo5, "http://www.w3.org/2001/XMLSchema#string", issuer));
      if (certificate.PublicKey.Key is RSA key)
        claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa", key.ToXmlString(false), "http://www.w3.org/2000/09/xmldsig#RSAKeyValue", issuer));
      if (certificate.PublicKey.Key is DSA key)
        claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa", key.ToXmlString(false), "http://www.w3.org/2000/09/xmldsig#DSAKeyValue", issuer));
      string serialNumber = certificate.SerialNumber;
      if (!string.IsNullOrEmpty(serialNumber))
        claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber", serialNumber, "http://www.w3.org/2001/XMLSchema#string", issuer));
      return (IEnumerable<Claim>) claims;
    }

    internal static IClaimsIdentity CreateFromIdentity(IIdentity identity)
    {
      switch (identity)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
        case IClaimsIdentity claimsIdentity2:
          return claimsIdentity2;
        case WindowsIdentity identity1:
          return (IClaimsIdentity) new WindowsClaimsIdentity(identity1);
        case FormsIdentity formsIdentity:
          ClaimsIdentity claimsIdentity1 = new ClaimsIdentity((IIdentity) formsIdentity);
          FormsAuthenticationTicket ticket = formsIdentity.Ticket;
          if (ticket != null)
          {
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(ticket.IssueDate.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath", ticket.CookiePath));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration", XmlConvert.ToString(ticket.Expiration.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expired", XmlConvert.ToString(ticket.Expired), "http://www.w3.org/2001/XMLSchema#boolean"));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent", XmlConvert.ToString(ticket.IsPersistent), "http://www.w3.org/2001/XMLSchema#boolean"));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata", ticket.UserData));
            claimsIdentity1.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/version", XmlConvert.ToString(ticket.Version), "http://www.w3.org/2001/XMLSchema#integer"));
          }
          return (IClaimsIdentity) claimsIdentity1;
        default:
          return (IClaimsIdentity) new ClaimsIdentity(identity);
      }
    }

    private bool NameClaimPredicate(Claim c) => StringComparer.Ordinal.Equals(c.ClaimType, this._nameClaimType);

    private bool IsCircular(IClaimsIdentity subject)
    {
      if (object.ReferenceEquals((object) this, (object) subject))
        return true;
      for (IClaimsIdentity claimsIdentity = subject; claimsIdentity.Actor != null; claimsIdentity = claimsIdentity.Actor)
      {
        if (object.ReferenceEquals((object) this, (object) claimsIdentity.Actor))
          return true;
      }
      return false;
    }

    private void Deserialize(SerializationInfo info, StreamingContext context)
    {
      ClaimsIdentitySerializer identitySerializer = new ClaimsIdentitySerializer(info, context);
      this._nameClaimType = identitySerializer.DeserializeNameClaimType();
      this._roleClaimType = identitySerializer.DeserializeRoleClaimType();
      this._label = identitySerializer.DeserializeLabel();
      this._actor = identitySerializer.DeserializeActor();
      if (this._claims == null)
        this._claims = new ClaimCollection((IClaimsIdentity) this);
      identitySerializer.DeserializeClaims(this._claims);
      this._bootstrapToken = (SecurityToken) null;
      this._bootstrapTokenString = identitySerializer.GetSerializedBootstrapTokenString();
      this._authenticationType = identitySerializer.DeserializeAuthenticationType();
    }
  }
}
