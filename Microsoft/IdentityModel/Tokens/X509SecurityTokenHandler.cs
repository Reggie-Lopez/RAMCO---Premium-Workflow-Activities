// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.X509SecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Web.Compilation;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class X509SecurityTokenHandler : SecurityTokenHandler
  {
    private static X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;
    private static X509CertificateValidationMode DefaultValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
    private static StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;
    private bool _mapToWindows;
    private bool _useWindowsTokenService;
    private X509CertificateValidator _certificateValidator;
    private bool _writeXmlDSigDefinedClauseTypes;
    private X509DataSecurityKeyIdentifierClauseSerializer _x509DataKeyIdentifierClauseSerializer = new X509DataSecurityKeyIdentifierClauseSerializer();

    public X509SecurityTokenHandler()
      : this(false, (X509CertificateValidator) null)
    {
    }

    public X509SecurityTokenHandler(X509CertificateValidator certificateValidator)
      : this(false, certificateValidator)
    {
    }

    public X509SecurityTokenHandler(bool mapToWindows)
      : this(mapToWindows, (X509CertificateValidator) null)
    {
    }

    public X509SecurityTokenHandler(
      bool mapToWindows,
      X509CertificateValidator certificateValidator)
    {
      this._mapToWindows = mapToWindows;
      this._certificateValidator = certificateValidator;
    }

    public X509SecurityTokenHandler(XmlNodeList customConfigElements)
    {
      List<XmlElement> xmlElementList = customConfigElements != null ? XmlUtil.GetXmlElements(customConfigElements) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customConfigElements));
      bool flag1 = false;
      bool flag2 = false;
      X509RevocationMode revocationMode = X509SecurityTokenHandler.DefaultRevocationMode;
      X509CertificateValidationMode certificateValidationMode = X509SecurityTokenHandler.DefaultValidationMode;
      StoreLocation trustedStoreLocation = X509SecurityTokenHandler.DefaultStoreLocation;
      string typeName1 = (string) null;
      foreach (XmlElement xmlElement in xmlElementList)
      {
        if (StringComparer.Ordinal.Equals(xmlElement.LocalName, "x509SecurityTokenHandlerRequirement"))
        {
          if (flag1)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7026", (object) "x509SecurityTokenHandlerRequirement"));
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlElement.Attributes)
          {
            if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "mapToWindows"))
              this._mapToWindows = XmlConvert.ToBoolean(attribute.Value.ToLowerInvariant());
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
              this._useWindowsTokenService = XmlConvert.ToBoolean(attribute.Value.ToLowerInvariant());
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "certificateValidator"))
              typeName1 = attribute.Value.ToString();
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "revocationMode"))
            {
              flag2 = true;
              string x = attribute.Value.ToString();
              if (StringComparer.OrdinalIgnoreCase.Equals(x, "NoCheck"))
                revocationMode = X509RevocationMode.NoCheck;
              else if (StringComparer.OrdinalIgnoreCase.Equals(x, "Offline"))
                revocationMode = X509RevocationMode.Offline;
              else if (StringComparer.OrdinalIgnoreCase.Equals(x, "Online"))
                revocationMode = X509RevocationMode.Online;
              else
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) xmlElement.LocalName)));
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "certificateValidationMode"))
            {
              flag2 = true;
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
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) xmlElement.LocalName)));
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "trustedStoreLocation"))
            {
              flag2 = true;
              string x = attribute.Value.ToString();
              if (StringComparer.OrdinalIgnoreCase.Equals(x, "CurrentUser"))
                trustedStoreLocation = StoreLocation.CurrentUser;
              else if (StringComparer.OrdinalIgnoreCase.Equals(x, "LocalMachine"))
                trustedStoreLocation = StoreLocation.LocalMachine;
              else
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7011", (object) attribute.LocalName, (object) xmlElement.LocalName)));
            }
            else
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7004", (object) attribute.LocalName, (object) xmlElement.LocalName)));
          }
          flag1 = true;
        }
      }
      if (certificateValidationMode == X509CertificateValidationMode.Custom)
      {
        Type typeName2 = !string.IsNullOrEmpty(typeName1) ? BuildManager.GetType(typeName1, true) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7028"));
        this._certificateValidator = (object) typeName2 != null ? CustomTypeElement.Resolve<X509CertificateValidator>(new CustomTypeElement(typeName2)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", Microsoft.IdentityModel.SR.GetString("ID7007", (object) typeName2));
      }
      else
      {
        if (!flag2)
          return;
        this._certificateValidator = X509Util.CreateCertificateValidator(certificateValidationMode, revocationMode, trustedStoreLocation);
      }
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

    public X509CertificateValidator CertificateValidator
    {
      get
      {
        if (this._certificateValidator != null)
          return this._certificateValidator;
        return this.Configuration != null ? this.Configuration.CertificateValidator : (X509CertificateValidator) null;
      }
      set => this._certificateValidator = value;
    }

    public bool WriteXmlDSigDefinedClauseTypes
    {
      get => this._writeXmlDSigDefinedClauseTypes;
      set => this._writeXmlDSigDefinedClauseTypes = value;
    }

    public override bool CanReadKeyIdentifierClause(XmlReader reader) => reader != null ? this._x509DataKeyIdentifierClauseSerializer.CanReadKeyIdentifierClause(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public override bool CanReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return reader.IsStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd") && StringComparer.Ordinal.Equals(reader.GetAttribute("ValueType", (string) null), "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
    }

    public override bool CanValidateToken => true;

    public override bool CanWriteKeyIdentifierClause(
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      if (securityKeyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifierClause));
      return this._writeXmlDSigDefinedClauseTypes && this._x509DataKeyIdentifierClauseSerializer.CanWriteKeyIdentifierClause(securityKeyIdentifierClause);
    }

    public override bool CanWriteToken => true;

    public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader)
    {
      return reader != null ? this._x509DataKeyIdentifierClauseSerializer.ReadKeyIdentifierClause(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
    }

    public override SecurityToken ReadToken(XmlReader reader)
    {
      XmlDictionaryReader dictionaryReader = reader != null ? XmlDictionaryReader.CreateDictionaryReader(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      string x = dictionaryReader.IsStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd") ? dictionaryReader.GetAttribute("ValueType", (string) null) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "BinarySecurityToken", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI)));
      if (!StringComparer.Ordinal.Equals(x, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4066", (object) "BinarySecurityToken", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", (object) "ValueType", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3", (object) x)));
      string attribute1 = dictionaryReader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
      string attribute2 = dictionaryReader.GetAttribute("EncodingType", (string) null);
      byte[] rawData;
      if (attribute2 == null || StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
      {
        rawData = dictionaryReader.ReadElementContentAsBase64();
      }
      else
      {
        if (!StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4068")));
        rawData = SoapHexBinary.Parse(dictionaryReader.ReadElementContentAsString()).Value;
      }
      return !string.IsNullOrEmpty(attribute1) ? (SecurityToken) new X509SecurityToken(new X509Certificate2(rawData), attribute1) : (SecurityToken) new X509SecurityToken(new X509Certificate2(rawData));
    }

    public override Type TokenType => typeof (X509SecurityToken);

    public override string[] GetTokenTypeIdentifiers() => new string[1]
    {
      "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/X509Certificate"
    };

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is X509SecurityToken x509SecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (X509SecurityToken)));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      try
      {
        this.CertificateValidator.Validate(x509SecurityToken.Certificate);
      }
      catch (SecurityTokenValidationException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4257", (object) X509Util.GetCertificateId(x509SecurityToken.Certificate)), (Exception) ex));
      }
      if (this.Configuration.IssuerNameRegistry == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4277"));
      string certificateIssuerName = X509Util.GetCertificateIssuerName(x509SecurityToken.Certificate, this.Configuration.IssuerNameRegistry);
      if (string.IsNullOrEmpty(certificateIssuerName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4175")));
      ClaimsIdentity claimsIdentity = new ClaimsIdentity(x509SecurityToken.Certificate, certificateIssuerName);
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
      claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509"));
      if (!this._mapToWindows)
      {
        if (this.Configuration.SaveBootstrapTokens)
          claimsIdentity.BootstrapToken = token;
        return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
        {
          (IClaimsIdentity) claimsIdentity
        });
      }
      WindowsClaimsIdentity windowsClaimsIdentity;
      if (token is X509WindowsSecurityToken windowsSecurityToken)
      {
        windowsClaimsIdentity = new WindowsClaimsIdentity(windowsSecurityToken.WindowsIdentity.Token, "X509", this.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
      }
      else
      {
        new X509NTAuthChainTrustValidator().Validate(x509SecurityToken.Certificate);
        windowsClaimsIdentity = WindowsClaimsIdentity.CreateFromCertificate(x509SecurityToken.Certificate, this._useWindowsTokenService, this.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
      }
      windowsClaimsIdentity.Claims.CopyRange((IEnumerable<Claim>) claimsIdentity.Claims);
      if (this.Configuration.SaveBootstrapTokens)
        windowsClaimsIdentity.BootstrapToken = token;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        (IClaimsIdentity) windowsClaimsIdentity
      });
    }

    internal static unsafe WindowsIdentity KerberosCertificateLogon(
      X509Certificate2 certificate)
    {
      SafeHGlobalHandle safeHglobalHandle1 = (SafeHGlobalHandle) null;
      SafeHGlobalHandle safeHglobalHandle2 = (SafeHGlobalHandle) null;
      SafeHGlobalHandle safeHglobalHandle3 = (SafeHGlobalHandle) null;
      SafeLsaLogonProcessHandle lsaHandle = (SafeLsaLogonProcessHandle) null;
      SafeLsaReturnBufferHandle ProfileBuffer = (SafeLsaReturnBufferHandle) null;
      SafeCloseHandle Token = (SafeCloseHandle) null;
      try
      {
        safeHglobalHandle1 = SafeHGlobalHandle.AllocHGlobal(Microsoft.IdentityModel.NativeMethods.LsaSourceName.Length + 1);
        Marshal.Copy(Microsoft.IdentityModel.NativeMethods.LsaSourceName, 0, safeHglobalHandle1.DangerousGetHandle(), Microsoft.IdentityModel.NativeMethods.LsaSourceName.Length);
        UNICODE_INTPTR_STRING unicodeIntptrString = new UNICODE_INTPTR_STRING(Microsoft.IdentityModel.NativeMethods.LsaSourceName.Length, Microsoft.IdentityModel.NativeMethods.LsaSourceName.Length + 1, safeHglobalHandle1.DangerousGetHandle());
        Microsoft.IdentityModel.Privilege privilege = (Microsoft.IdentityModel.Privilege) null;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          try
          {
            privilege = new Microsoft.IdentityModel.Privilege("SeTcbPrivilege");
            privilege.Enable();
          }
          catch (PrivilegeNotHeldException ex)
          {
            DiagnosticUtil.TraceUtil.Trace(TraceEventType.Warning, TraceCode.Diagnostics, (string) null, (TraceRecord) null, (Exception) ex);
          }
          IntPtr securityMode = IntPtr.Zero;
          int status = Microsoft.IdentityModel.NativeMethods.LsaRegisterLogonProcess(ref unicodeIntptrString, out lsaHandle, out securityMode);
          if (5 == Microsoft.IdentityModel.NativeMethods.LsaNtStatusToWinError(status))
            status = Microsoft.IdentityModel.NativeMethods.LsaConnectUntrusted(out lsaHandle);
          if (status < 0)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Microsoft.IdentityModel.NativeMethods.LsaNtStatusToWinError(status)));
        }
        finally
        {
          int error = -1;
          string str = (string) null;
          try
          {
            error = privilege.Revert();
            if (error != 0)
              str = Microsoft.IdentityModel.SR.GetString("ID4069", (object) new Win32Exception(error));
          }
          finally
          {
            if (error != 0)
            {
              DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Critical, str);
              Environment.FailFast(str);
            }
          }
        }
        safeHglobalHandle2 = SafeHGlobalHandle.AllocHGlobal(Microsoft.IdentityModel.NativeMethods.LsaKerberosName.Length + 1);
        Marshal.Copy(Microsoft.IdentityModel.NativeMethods.LsaKerberosName, 0, safeHglobalHandle2.DangerousGetHandle(), Microsoft.IdentityModel.NativeMethods.LsaKerberosName.Length);
        UNICODE_INTPTR_STRING packageName = new UNICODE_INTPTR_STRING(Microsoft.IdentityModel.NativeMethods.LsaKerberosName.Length, Microsoft.IdentityModel.NativeMethods.LsaKerberosName.Length + 1, safeHglobalHandle2.DangerousGetHandle());
        uint authenticationPackage = 0;
        int status1 = Microsoft.IdentityModel.NativeMethods.LsaLookupAuthenticationPackage(lsaHandle, ref packageName, out authenticationPackage);
        if (status1 < 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Microsoft.IdentityModel.NativeMethods.LsaNtStatusToWinError(status1)));
        TOKEN_SOURCE SourceContext = new TOKEN_SOURCE();
        if (!Microsoft.IdentityModel.NativeMethods.AllocateLocallyUniqueId(out SourceContext.SourceIdentifier))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Marshal.GetLastWin32Error()));
        SourceContext.Name = new char[8];
        SourceContext.Name[0] = 'W';
        SourceContext.Name[1] = 'C';
        SourceContext.Name[2] = 'F';
        byte[] rawData = certificate.RawData;
        int cb = checked (KERB_CERTIFICATE_S4U_LOGON.Size + rawData.Length);
        safeHglobalHandle3 = SafeHGlobalHandle.AllocHGlobal(cb);
        KERB_CERTIFICATE_S4U_LOGON* pointer = (KERB_CERTIFICATE_S4U_LOGON*) safeHglobalHandle3.DangerousGetHandle().ToPointer();
        pointer->MessageType = KERB_LOGON_SUBMIT_TYPE.KerbCertificateS4ULogon;
        pointer->Flags = 2U;
        pointer->UserPrincipalName = new UNICODE_INTPTR_STRING(0, 0, IntPtr.Zero);
        pointer->DomainName = new UNICODE_INTPTR_STRING(0, 0, IntPtr.Zero);
        pointer->CertificateLength = (uint) rawData.Length;
        pointer->Certificate = new IntPtr(safeHglobalHandle3.DangerousGetHandle().ToInt64() + (long) KERB_CERTIFICATE_S4U_LOGON.Size);
        Marshal.Copy(rawData, 0, pointer->Certificate, rawData.Length);
        QUOTA_LIMITS Quotas = new QUOTA_LIMITS();
        LUID LogonId = new LUID();
        int SubStatus = 0;
        int status2 = Microsoft.IdentityModel.NativeMethods.LsaLogonUser(lsaHandle, ref unicodeIntptrString, Microsoft.IdentityModel.SecurityLogonType.Network, authenticationPackage, safeHglobalHandle3.DangerousGetHandle(), (uint) cb, IntPtr.Zero, ref SourceContext, out ProfileBuffer, out uint _, out LogonId, out Token, out Quotas, out SubStatus);
        if (status2 == -1073741714 && SubStatus < 0)
          status2 = SubStatus;
        if (status2 < 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Microsoft.IdentityModel.NativeMethods.LsaNtStatusToWinError(status2)));
        if (SubStatus < 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Microsoft.IdentityModel.NativeMethods.LsaNtStatusToWinError(SubStatus)));
        return new WindowsIdentity(Token.DangerousGetHandle());
      }
      finally
      {
        Token?.Close();
        safeHglobalHandle3?.Close();
        ProfileBuffer?.Close();
        safeHglobalHandle1?.Close();
        safeHglobalHandle2?.Close();
        lsaHandle?.Close();
      }
    }

    public override void WriteKeyIdentifierClause(
      XmlWriter writer,
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (securityKeyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifierClause));
      if (!this._writeXmlDSigDefinedClauseTypes)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4261"));
      this._x509DataKeyIdentifierClauseSerializer.WriteKeyIdentifierClause(writer, securityKeyIdentifierClause);
    }

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is X509SecurityToken x509SecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID0018", (object) typeof (X509SecurityToken)));
      writer.WriteStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      if (!string.IsNullOrEmpty(x509SecurityToken.Id))
        writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", x509SecurityToken.Id);
      writer.WriteAttributeString("ValueType", (string) null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
      writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
      byte[] rawCertData = x509SecurityToken.Certificate.GetRawCertData();
      writer.WriteBase64(rawCertData, 0, rawCertData.Length);
      writer.WriteEndElement();
    }
  }
}
