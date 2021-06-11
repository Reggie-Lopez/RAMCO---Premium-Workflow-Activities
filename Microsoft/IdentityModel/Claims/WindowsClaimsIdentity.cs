// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.WindowsClaimsIdentity
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.WindowsTokenService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class WindowsClaimsIdentity : WindowsIdentity, IClaimsIdentity, IIdentity, ISerializable
  {
    private IClaimsIdentity _actor;
    private bool _claimsInitialized;
    private bool _nameInitialized;
    private ClaimCollection _claims;
    private string _label;
    private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
    private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    private SecurityToken _bootstrapToken;
    private string _bootstrapTokenString;
    private string _issuerName;

    public WindowsClaimsIdentity(IntPtr userToken, string authenticationType)
      : this(userToken, authenticationType, "LOCAL AUTHORITY")
    {
    }

    public WindowsClaimsIdentity(IntPtr userToken)
      : this(userToken, "Windows")
    {
    }

    public WindowsClaimsIdentity(IntPtr userToken, string authenticationType, string issuerName)
      : base(WindowsClaimsIdentity.GetValidToken(userToken), authenticationType)
      => this._issuerName = issuerName;

    internal WindowsClaimsIdentity(WindowsIdentity identity, string authenticationType)
      : this(WindowsClaimsIdentity.GetValidWindowsIdentity(identity).Token, authenticationType)
    {
    }

    internal WindowsClaimsIdentity(
      WindowsIdentity identity,
      string authenticationType,
      string issuerName)
      : this(WindowsClaimsIdentity.GetValidWindowsIdentity(identity).Token, authenticationType, issuerName)
    {
    }

    internal WindowsClaimsIdentity(WindowsIdentity identity)
      : this(WindowsClaimsIdentity.GetValidWindowsIdentity(identity).Token, identity.AuthenticationType)
    {
    }

    private static WindowsIdentity GetValidWindowsIdentity(WindowsIdentity identity)
    {
      if (identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      WindowsClaimsIdentity.GetValidToken(identity.Token);
      return identity;
    }

    private static IntPtr GetValidToken(IntPtr token) => !(token == IntPtr.Zero) ? token : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID4281"));

    public static WindowsClaimsIdentity GetCurrent()
    {
      using (WindowsIdentity current = WindowsIdentity.GetCurrent())
        return new WindowsClaimsIdentity(current);
    }

    internal static WindowsClaimsIdentity GetCurrent(string authenticationType)
    {
      using (WindowsIdentity current = WindowsIdentity.GetCurrent())
        return new WindowsClaimsIdentity(current, authenticationType);
    }

    protected WindowsClaimsIdentity(SerializationInfo info, StreamingContext context)
      : base(info, context)
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
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SerializationException(Microsoft.IdentityModel.SR.GetString("ID4282", (object) nameof (WindowsClaimsIdentity)), ex));
      }
    }

    public static WindowsClaimsIdentity CreateFromUpn(
      string upn,
      string authenticationType,
      bool useWindowsTokenService)
    {
      return WindowsClaimsIdentity.CreateFromUpn(upn, authenticationType, useWindowsTokenService, "LOCAL AUTHORITY");
    }

    public static WindowsClaimsIdentity CreateFromUpn(
      string upn,
      string authenticationType,
      bool useWindowsTokenService,
      string issuerName)
    {
      if (useWindowsTokenService)
      {
        using (WindowsIdentity windowsIdentity = S4UClient.UpnLogon(upn))
          return new WindowsClaimsIdentity(windowsIdentity.Token, authenticationType, issuerName);
      }
      else
      {
        using (WindowsIdentity windowsIdentity = new WindowsIdentity(upn))
          return new WindowsClaimsIdentity(windowsIdentity.Token, authenticationType, issuerName);
      }
    }

    public static WindowsClaimsIdentity CreateFromCertificate(
      X509Certificate2 certificate,
      bool useWindowsTokenService)
    {
      return WindowsClaimsIdentity.CreateFromCertificate(certificate, useWindowsTokenService, "LOCAL AUTHORITY");
    }

    public static WindowsClaimsIdentity CreateFromCertificate(
      X509Certificate2 certificate,
      bool useWindowsTokenService,
      string issuerName)
    {
      if (useWindowsTokenService)
      {
        using (WindowsIdentity windowsIdentity = S4UClient.CertificateLogon(certificate))
          return new WindowsClaimsIdentity(windowsIdentity.Token, "X509", issuerName);
      }
      else
      {
        using (WindowsIdentity windowsIdentity = (WindowsIdentity) WindowsClaimsIdentity.CertificateLogon(certificate))
          return new WindowsClaimsIdentity(windowsIdentity.Token, "X509", issuerName);
      }
    }

    public static WindowsClaimsIdentity CertificateLogon(
      X509Certificate2 x509Certificate)
    {
      if (Environment.OSVersion.Version.Major >= 6)
      {
        using (WindowsIdentity windowsIdentity = Microsoft.IdentityModel.Tokens.X509SecurityTokenHandler.KerberosCertificateLogon(x509Certificate))
          return new WindowsClaimsIdentity(windowsIdentity.Token);
      }
      else
      {
        string nameInfo = x509Certificate.GetNameInfo(X509NameType.UpnName, false);
        if (string.IsNullOrEmpty(nameInfo))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4067", (object) X509Util.GetCertificateId(x509Certificate))));
        using (WindowsIdentity windowsIdentity = new WindowsIdentity(nameInfo))
          return new WindowsClaimsIdentity(windowsIdentity.Token);
      }
    }

    public IClaimsIdentity Actor
    {
      get => this._actor;
      set => this._actor = value == null || !this.IsCircular(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4035"));
    }

    public ClaimCollection Claims
    {
      get
      {
        if (!this._claimsInitialized)
          this.InitializeClaims();
        return this._claims;
      }
    }

    public new string Label
    {
      get => this._label;
      set => this._label = value;
    }

    public new string NameClaimType
    {
      get => this._nameClaimType;
      set => this._nameClaimType = value;
    }

    public override string Name
    {
      get
      {
        string str = (string) null;
        if (!this._nameInitialized)
          this.InitializeName();
        foreach (Claim claim in (IEnumerable<Claim>) this._claims.FindAll(new Predicate<Claim>(this.NameClaimPredicate)))
        {
          if (!string.IsNullOrEmpty(claim.Value))
          {
            str = claim.Value;
            break;
          }
        }
        if (string.IsNullOrEmpty(str))
          str = base.Name;
        return str;
      }
    }

    public new string RoleClaimType
    {
      get => this._roleClaimType;
      set => this._roleClaimType = value;
    }

    private bool NameClaimPredicate(Claim c) => StringComparer.Ordinal.Equals(c.ClaimType, this._nameClaimType);

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
      WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity((WindowsIdentity) this);
      if (this._claims != null)
        windowsClaimsIdentity._claims = this._claims.CopyWithSubject((IClaimsIdentity) windowsClaimsIdentity);
      windowsClaimsIdentity._claimsInitialized = this._claimsInitialized;
      windowsClaimsIdentity._nameInitialized = this._nameInitialized;
      windowsClaimsIdentity._issuerName = this._issuerName;
      windowsClaimsIdentity.Label = this.Label;
      windowsClaimsIdentity.NameClaimType = this.NameClaimType;
      windowsClaimsIdentity.RoleClaimType = this.RoleClaimType;
      windowsClaimsIdentity.BootstrapToken = this.BootstrapToken;
      if (this.Actor != null)
        windowsClaimsIdentity.Actor = !this.IsCircular(this.Actor) ? this.Actor.Copy() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4035"));
      return (IClaimsIdentity) windowsClaimsIdentity;
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => this.GetObjectData(info, context);

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      info.AddValue("m_userToken", (object) this.Token);
      ClaimsIdentitySerializer identitySerializer = new ClaimsIdentitySerializer(info, context);
      identitySerializer.SerializeNameClaimType(this._nameClaimType);
      identitySerializer.SerializeRoleClaimType(this._roleClaimType);
      identitySerializer.SerializeLabel(this._label);
      identitySerializer.SerializeActor(this._actor);
      List<Claim> claimList = (List<Claim>) null;
      if (this._claims != null)
        claimList = new List<Claim>(this._claims.Where<Claim>((Func<Claim, bool>) (c => !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid") && (!StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid")) && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid"))));
      identitySerializer.SerializeClaims((IEnumerable<Claim>) claimList);
      identitySerializer.SerializeBootstrapToken(this._bootstrapToken);
    }

    public override string ToString() => this.Name;

    protected override void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
      base.Dispose(disposing);
    }

    protected Claim CreatePrimarySidClaim()
    {
      SafeHGlobalHandle safeHglobalHandle = SafeHGlobalHandle.InvalidHandle;
      Claim claim = (Claim) null;
      try
      {
        safeHglobalHandle = WindowsClaimsIdentity.GetTokenInformation(this.Token, Microsoft.IdentityModel.TokenInformationClass.TokenUser, out uint _);
        SID_AND_ATTRIBUTES structure = (SID_AND_ATTRIBUTES) Marshal.PtrToStructure(safeHglobalHandle.DangerousGetHandle(), typeof (SID_AND_ATTRIBUTES));
        uint num = 16;
        if (structure.Attributes == 0U)
          claim = new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid", new SecurityIdentifier(structure.Sid).Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName);
        else if (((int) structure.Attributes & (int) num) == 16)
          claim = new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid", new SecurityIdentifier(structure.Sid).Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName);
      }
      finally
      {
        safeHglobalHandle.Close();
      }
      return claim;
    }

    protected ICollection<Claim> GetGroupSidClaims()
    {
      Collection<Claim> collection = new Collection<Claim>();
      if (this.Token != IntPtr.Zero)
      {
        SafeHGlobalHandle safeHglobalHandle1 = SafeHGlobalHandle.InvalidHandle;
        SafeHGlobalHandle safeHglobalHandle2 = SafeHGlobalHandle.InvalidHandle;
        try
        {
          uint dwLength;
          safeHglobalHandle2 = WindowsClaimsIdentity.GetTokenInformation(this.Token, Microsoft.IdentityModel.TokenInformationClass.TokenPrimaryGroup, out dwLength);
          SecurityIdentifier securityIdentifier1 = new SecurityIdentifier(((TOKEN_PRIMARY_GROUP) Marshal.PtrToStructure(safeHglobalHandle2.DangerousGetHandle(), typeof (TOKEN_PRIMARY_GROUP))).PrimaryGroup);
          bool flag = false;
          safeHglobalHandle1 = WindowsClaimsIdentity.GetTokenInformation(this.Token, Microsoft.IdentityModel.TokenInformationClass.TokenGroups, out dwLength);
          int num1 = Marshal.ReadInt32(safeHglobalHandle1.DangerousGetHandle());
          IntPtr ptr = new IntPtr((long) safeHglobalHandle1.DangerousGetHandle() + (long) Marshal.OffsetOf(typeof (TOKEN_GROUPS), "Groups"));
          for (int index = 0; index < num1; ++index)
          {
            SID_AND_ATTRIBUTES structure = (SID_AND_ATTRIBUTES) Marshal.PtrToStructure(ptr, typeof (SID_AND_ATTRIBUTES));
            uint num2 = 3221225492;
            SecurityIdentifier securityIdentifier2 = new SecurityIdentifier(structure.Sid);
            if (((int) structure.Attributes & (int) num2) == 4)
            {
              if (!flag && StringComparer.Ordinal.Equals(securityIdentifier2.Value, securityIdentifier1.Value))
              {
                collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName));
                flag = true;
              }
              collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName));
            }
            else if (((int) structure.Attributes & (int) num2) == 16)
            {
              if (!flag && StringComparer.Ordinal.Equals(securityIdentifier2.Value, securityIdentifier1.Value))
              {
                collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName));
                flag = true;
              }
              collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", this._issuerName));
            }
            ptr = new IntPtr((long) ptr + SID_AND_ATTRIBUTES.SizeOf);
          }
        }
        finally
        {
          safeHglobalHandle1.Close();
          safeHglobalHandle2.Close();
        }
      }
      return (ICollection<Claim>) collection;
    }

    private void InitializeName()
    {
      if (this._nameInitialized)
        return;
      this._nameInitialized = true;
      if (!this.IsAuthenticated)
        return;
      string name = base.Name;
      if (string.IsNullOrEmpty(name))
        return;
      if (this._claims == null)
        this._claims = new ClaimCollection((IClaimsIdentity) this);
      this._claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name, "http://www.w3.org/2001/XMLSchema#string", this._issuerName));
    }

    private void InitializeClaims()
    {
      this._claimsInitialized = true;
      if (this._claims == null)
        this._claims = new ClaimCollection((IClaimsIdentity) this);
      if (!this.IsAuthenticated)
        return;
      this.InitializeName();
      Claim primarySidClaim = this.CreatePrimarySidClaim();
      if (primarySidClaim != null)
        this._claims.Add(primarySidClaim);
      this._claims.AddRange((IEnumerable<Claim>) this.GetGroupSidClaims());
    }

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
    }

    private static SafeHGlobalHandle GetTokenInformation(
      IntPtr tokenHandle,
      Microsoft.IdentityModel.TokenInformationClass tokenInformationClass,
      out uint dwLength)
    {
      SafeHGlobalHandle invalidHandle = SafeHGlobalHandle.InvalidHandle;
      dwLength = (uint) Marshal.SizeOf(typeof (uint));
      Microsoft.IdentityModel.NativeMethods.GetTokenInformation(tokenHandle, (uint) tokenInformationClass, invalidHandle, 0U, out dwLength);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      switch (lastWin32Error1)
      {
        case 24:
        case 122:
          SafeHGlobalHandle tokenInformation1 = SafeHGlobalHandle.AllocHGlobal(dwLength);
          bool tokenInformation2 = Microsoft.IdentityModel.NativeMethods.GetTokenInformation(tokenHandle, (uint) tokenInformationClass, tokenInformation1, dwLength, out dwLength);
          int lastWin32Error2 = Marshal.GetLastWin32Error();
          if (!tokenInformation2)
          {
            tokenInformation1.Close();
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error2));
          }
          return tokenInformation1;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error1));
      }
    }
  }
}
