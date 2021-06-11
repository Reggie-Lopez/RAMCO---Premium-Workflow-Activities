// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.CryptoUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace Microsoft.IdentityModel
{
  internal static class CryptoUtil
  {
    private const string _fipsPolicyRegistryKey = "System\\CurrentControlSet\\Control\\Lsa";
    public const int WindowsVistaMajorNumber = 6;
    private static object _syncObject = new object();
    private static CryptoUtil.FIPS_ALGORTITHM_POLICY _fipsPolicyState = CryptoUtil.FIPS_ALGORTITHM_POLICY.Unknown;
    private static RandomNumberGenerator _random = CryptoUtil.Algorithms.NewRandomNumberGenerator();

    public static bool AreEqual(byte[] a, byte[] b)
    {
      if (a == null || b == null)
        return a == null && null == b;
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      if (a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if ((int) a[index] != (int) b[index])
          return false;
      }
      return true;
    }

    public static int CeilingDivide(int dividend, int divisor)
    {
      int num1 = dividend % divisor;
      int num2 = dividend / divisor;
      if (num1 > 0)
        ++num2;
      return num2;
    }

    internal static void CloseInvalidOutSafeHandle(SafeHandle handle) => handle?.SetHandleAsInvalid();

    public static void GenerateRandomBytes(byte[] data) => CryptoUtil._random.GetNonZeroBytes(data);

    public static byte[] GenerateRandomBytes(int sizeInBits)
    {
      int length = sizeInBits / 8;
      if (sizeInBits <= 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(nameof (sizeInBits), SR.GetString("ID6033", (object) sizeInBits)));
      byte[] data = length * 8 == sizeInBits ? new byte[length] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID6002", (object) sizeInBits), nameof (sizeInBits)));
      CryptoUtil.GenerateRandomBytes(data);
      return data;
    }

    public static byte[] CreateSignatureForSha256(
      AsymmetricSignatureFormatter formatter,
      HashAlgorithm hash)
    {
      if (!CryptoUtil.Algorithms.RequiresFipsCompliance)
        return formatter.CreateSignature(hash);
      formatter.SetHashAlgorithm("SHA256");
      return formatter.CreateSignature(hash.Hash);
    }

    public static bool VerifySignatureForSha256(
      AsymmetricSignatureDeformatter deformatter,
      HashAlgorithm hash,
      byte[] signatureValue)
    {
      if (!CryptoUtil.Algorithms.RequiresFipsCompliance)
        return deformatter.VerifySignature(hash, signatureValue);
      deformatter.SetHashAlgorithm("SHA256");
      return deformatter.VerifySignature(hash.Hash, signatureValue);
    }

    public static AsymmetricSignatureFormatter GetSignatureFormatterForSha256(
      AsymmetricSecurityKey key)
    {
      AsymmetricAlgorithm asymmetricAlgorithm = key.GetAsymmetricAlgorithm("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", true);
      return asymmetricAlgorithm is RSACryptoServiceProvider rsaProvider ? CryptoUtil.GetSignatureFormatterForSha256(rsaProvider) : (AsymmetricSignatureFormatter) new RSAPKCS1SignatureFormatter(asymmetricAlgorithm);
    }

    public static AsymmetricSignatureFormatter GetSignatureFormatterForSha256(
      RSACryptoServiceProvider rsaProvider)
    {
      CspParameters parameters = new CspParameters();
      parameters.ProviderType = 24;
      parameters.KeyContainerName = rsaProvider.CspKeyContainerInfo.KeyContainerName;
      parameters.KeyNumber = (int) rsaProvider.CspKeyContainerInfo.KeyNumber;
      if (24 == rsaProvider.CspKeyContainerInfo.ProviderType)
        parameters.ProviderName = rsaProvider.CspKeyContainerInfo.ProviderName;
      if (rsaProvider.CspKeyContainerInfo.MachineKeyStore)
        parameters.Flags = CspProviderFlags.UseMachineKeyStore;
      rsaProvider = new RSACryptoServiceProvider(parameters);
      return (AsymmetricSignatureFormatter) new RSAPKCS1SignatureFormatter((AsymmetricAlgorithm) rsaProvider);
    }

    public static AsymmetricSignatureDeformatter GetSignatureDeFormatterForSha256(
      AsymmetricSecurityKey key)
    {
      AsymmetricAlgorithm asymmetricAlgorithm = key.GetAsymmetricAlgorithm("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", false);
      return asymmetricAlgorithm is RSACryptoServiceProvider rsaProvider ? CryptoUtil.GetSignatureDeFormatterForSha256(rsaProvider) : (AsymmetricSignatureDeformatter) new RSAPKCS1SignatureDeformatter(asymmetricAlgorithm);
    }

    public static AsymmetricSignatureDeformatter GetSignatureDeFormatterForSha256(
      RSACryptoServiceProvider rsaProvider)
    {
      CspParameters parameters = new CspParameters();
      parameters.ProviderType = 24;
      parameters.KeyNumber = (int) rsaProvider.CspKeyContainerInfo.KeyNumber;
      if (24 == rsaProvider.CspKeyContainerInfo.ProviderType)
        parameters.ProviderName = rsaProvider.CspKeyContainerInfo.ProviderName;
      if (rsaProvider.CspKeyContainerInfo.MachineKeyStore)
        parameters.Flags = CspProviderFlags.UseMachineKeyStore;
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(parameters);
      cryptoServiceProvider.ImportCspBlob(rsaProvider.ExportCspBlob(false));
      return (AsymmetricSignatureDeformatter) new RSAPKCS1SignatureDeformatter((AsymmetricAlgorithm) cryptoServiceProvider);
    }

    public static void ValidateBufferBounds(Array buffer, int offset, int count)
    {
      if (buffer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (buffer));
      if (count < 0 || count > buffer.Length)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID0007", (object) 0, (object) buffer.Length), nameof (count)));
      if (offset < 0 || offset > buffer.Length - count)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID0007", (object) 0, (object) (buffer.Length - count)), nameof (offset)));
    }

    [SecuritySafeCritical]
    [RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Lsa")]
    private static CryptoUtil.FIPS_ALGORTITHM_POLICY GetFipsAlgorithmPolicyKeyFromRegistry()
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa", false))
      {
        object obj = (object) null;
        if (registryKey != null)
          obj = registryKey.GetValue("FIPSAlgorithmPolicy");
        return obj == null || (int) obj == 0 ? CryptoUtil.FIPS_ALGORTITHM_POLICY.Disabled : CryptoUtil.FIPS_ALGORTITHM_POLICY.Enabled;
      }
    }

    public static void ResetAllCertificates(X509Certificate2Collection certificates)
    {
      if (certificates == null)
        return;
      for (int index = 0; index < certificates.Count; ++index)
        certificates[index].Reset();
    }

    private enum FIPS_ALGORTITHM_POLICY
    {
      Unknown,
      Enabled,
      Disabled,
    }

    public static class Algorithms
    {
      public static HashAlgorithm NewDefaultHash() => CryptoUtil.Algorithms.NewSha256();

      public static SymmetricAlgorithm NewDefaultEncryption() => CryptoUtil.Algorithms.CreateAlgorithmFromConfig("http://www.w3.org/2001/04/xmlenc#aes256-cbc") is SymmetricAlgorithm algorithmFromConfig ? algorithmFromConfig : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", (object) "http://www.w3.org/2000/09/xmldsig#hmac-sha1"));

      public static KeyedHashAlgorithm NewHmacSha1() => CryptoUtil.Algorithms.CreateAlgorithmFromConfig("http://www.w3.org/2000/09/xmldsig#hmac-sha1") is KeyedHashAlgorithm algorithmFromConfig ? algorithmFromConfig : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", (object) "http://www.w3.org/2000/09/xmldsig#hmac-sha1"));

      public static RandomNumberGenerator NewRandomNumberGenerator() => RandomNumberGenerator.Create();

      public static RSA NewRsa() => RSA.Create();

      public static HashAlgorithm NewSha1() => CryptoUtil.Algorithms.CreateHashAlgorithm("http://www.w3.org/2000/09/xmldsig#sha1") ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", (object) "http://www.w3.org/2000/09/xmldsig#sha1"));

      public static HashAlgorithm NewSha256() => CryptoUtil.Algorithms.CreateHashAlgorithm("http://www.w3.org/2001/04/xmlenc#sha256") ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", (object) "http://www.w3.org/2001/04/xmlenc#sha256"));

      public static HashAlgorithm CreateHashAlgorithm(string algorithm)
      {
        object obj = !string.IsNullOrEmpty(algorithm) ? CryptoUtil.Algorithms.CreateAlgorithmFromConfig(algorithm) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (algorithm));
        if (obj != null)
        {
          if (obj is SignatureDescription signatureDescription)
            return signatureDescription.CreateDigest() ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(SR.GetString("ID6011", (object) algorithm)));
          return obj is HashAlgorithm hashAlgorithm ? hashAlgorithm : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(SR.GetString("ID6022", (object) algorithm)));
        }
        switch (algorithm)
        {
          case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
            return CryptoUtil.Algorithms.NewSha256();
          case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
            return CryptoUtil.Algorithms.NewSha1();
          default:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(SR.GetString("ID6022", (object) algorithm)));
        }
      }

      public static object CreateAlgorithmFromConfig(string algorithm)
      {
        if (string.IsNullOrEmpty(algorithm))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (algorithm));
        object obj = (object) null;
        try
        {
          obj = CryptoConfig.CreateFromName(algorithm);
        }
        catch (TargetInvocationException ex)
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID6038", (object) algorithm, (object) ex.InnerException));
        }
        if (obj != null)
          return obj;
        switch (algorithm)
        {
          case "http://www.w3.org/2001/04/xmlenc#sha256":
            obj = !CryptoUtil.Algorithms.RequiresFipsCompliance ? (object) HashAlgorithm.Create("http://www.w3.org/2001/04/xmlenc#sha256") : (object) new SHA256CryptoServiceProvider();
            break;
          case "http://www.w3.org/2000/09/xmldsig#sha1":
            obj = !CryptoUtil.Algorithms.RequiresFipsCompliance ? (object) SHA1.Create() : (object) new SHA1CryptoServiceProvider();
            break;
          case "http://www.w3.org/2000/09/xmldsig#hmac-sha1":
            obj = (object) new HMACSHA1(CryptoUtil.GenerateRandomBytes(64), !CryptoUtil.Algorithms.RequiresFipsCompliance);
            break;
        }
        return obj;
      }

      [SecuritySafeCritical]
      internal static bool RequiresFipsCompliance
      {
        get
        {
          if (CryptoUtil._fipsPolicyState == CryptoUtil.FIPS_ALGORTITHM_POLICY.Unknown)
          {
            lock (CryptoUtil._syncObject)
            {
              if (CryptoUtil._fipsPolicyState == CryptoUtil.FIPS_ALGORTITHM_POLICY.Unknown)
              {
                bool pfEnabled;
                CryptoUtil._fipsPolicyState = Environment.OSVersion.Version.Major < 6 ? CryptoUtil.GetFipsAlgorithmPolicyKeyFromRegistry() : (0 != CryptoUtil.CAPI.BCryptGetFipsAlgorithmMode(out pfEnabled) || !pfEnabled ? CryptoUtil.FIPS_ALGORTITHM_POLICY.Disabled : CryptoUtil.FIPS_ALGORTITHM_POLICY.Enabled);
              }
            }
          }
          return CryptoUtil._fipsPolicyState == CryptoUtil.FIPS_ALGORTITHM_POLICY.Enabled;
        }
      }
    }

    [SuppressUnmanagedCodeSecurity]
    public static class CAPI
    {
      internal const string CRYPT32 = "crypt32.dll";
      internal const string BCRYPT = "bcrypt.dll";
      internal const int S_OK = 0;
      internal const int S_FALSE = 1;
      internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512;
      internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192;
      internal const uint CERT_CHAIN_POLICY_BASE = 1;
      internal const uint CERT_STORE_ADD_ALWAYS = 4;
      internal const uint CERT_CHAIN_POLICY_NT_AUTH = 6;
      internal const uint X509_ASN_ENCODING = 1;
      internal const uint PKCS_7_ASN_ENCODING = 65536;
      internal const uint CERT_STORE_PROV_MEMORY = 2;
      internal const uint CERT_INFO_ISSUER_FLAG = 4;
      internal const uint CERT_INFO_SUBJECT_FLAG = 7;
      internal const uint CERT_CHAIN_REVOCATION_CHECK_END_CERT = 268435456;
      internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN = 536870912;
      internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 1073741824;
      internal const uint CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY = 2147483648;
      internal const uint CERT_CHAIN_POLICY_IGNORE_PEER_TRUST_FLAG = 4096;
      internal const uint USAGE_MATCH_TYPE_AND = 0;
      internal const uint HCCE_CURRENT_USER = 0;
      internal const uint HCCE_LOCAL_MACHINE = 1;

      [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      internal static extern SafeCertContextHandle CertCreateCertificateContext(
        [In] uint dwCertEncodingType,
        [In] IntPtr pbCertEncoded,
        [In] uint cbCertEncoded);

      [DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      internal static extern SafeCertStoreHandle CertOpenStore(
        [In] IntPtr lpszStoreProvider,
        [In] uint dwMsgAndCertEncodingType,
        [In] IntPtr hCryptProv,
        [In] uint dwFlags,
        [In] string pvPara);

      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("crypt32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CertCloseStore([In] IntPtr hCertStore, [In] uint dwFlags);

      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("crypt32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

      [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CertAddCertificateLinkToStore(
        [In] SafeCertStoreHandle hCertStore,
        [In] IntPtr pCertContext,
        [In] uint dwAddDisposition,
        [In, Out] SafeCertContextHandle ppStoreContext);

      [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CertGetCertificateChain(
        [In] IntPtr hChainEngine,
        [In] IntPtr pCertContext,
        [In] ref System.Runtime.InteropServices.ComTypes.FILETIME pTime,
        [In] SafeCertStoreHandle hAdditionalStore,
        [In] ref CryptoUtil.CAPI.CERT_CHAIN_PARA pChainPara,
        [In] uint dwFlags,
        [In] IntPtr pvReserved,
        out SafeCertChainHandle ppChainContext);

      [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CertVerifyCertificateChainPolicy(
        [In] IntPtr pszPolicyOID,
        [In] SafeCertChainHandle pChainContext,
        [In] ref CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA pPolicyPara,
        [In, Out] ref CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS pPolicyStatus);

      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("crypt32.dll", SetLastError = true)]
      internal static extern void CertFreeCertificateChain(IntPtr handle);

      [DllImport("bcrypt.dll", SetLastError = true)]
      internal static extern int BCryptGetFipsAlgorithmMode([MarshalAs(UnmanagedType.U1)] out bool pfEnabled);

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_CONTEXT
      {
        internal uint dwCertEncodingType;
        internal IntPtr pbCertEncoded;
        internal uint cbCertEncoded;
        internal IntPtr pCertInfo;
        internal IntPtr hCertStore;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_ENHKEY_USAGE
      {
        internal uint cUsageIdentifier;
        internal IntPtr rgpszUsageIdentifier;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_USAGE_MATCH
      {
        internal uint dwType;
        internal CryptoUtil.CAPI.CERT_ENHKEY_USAGE Usage;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_CHAIN_PARA
      {
        internal uint cbSize;
        internal CryptoUtil.CAPI.CERT_USAGE_MATCH RequestedUsage;
        internal CryptoUtil.CAPI.CERT_USAGE_MATCH RequestedIssuancePolicy;
        internal uint dwUrlRetrievalTimeout;
        internal bool fCheckRevocationFreshnessTime;
        internal uint dwRevocationFreshnessTime;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_CHAIN_POLICY_PARA
      {
        internal uint cbSize;
        internal uint dwFlags;
        internal IntPtr pvExtraPolicyPara;

        internal CERT_CHAIN_POLICY_PARA(int size)
        {
          this.cbSize = (uint) size;
          this.dwFlags = 0U;
          this.pvExtraPolicyPara = IntPtr.Zero;
        }
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CERT_CHAIN_POLICY_STATUS
      {
        internal uint cbSize;
        internal uint dwError;
        internal IntPtr lChainIndex;
        internal IntPtr lElementIndex;
        internal IntPtr pvExtraPolicyStatus;

        internal CERT_CHAIN_POLICY_STATUS(int size)
        {
          this.cbSize = (uint) size;
          this.dwError = 0U;
          this.lChainIndex = IntPtr.Zero;
          this.lElementIndex = IntPtr.Zero;
          this.pvExtraPolicyStatus = IntPtr.Zero;
        }
      }
    }
  }
}
