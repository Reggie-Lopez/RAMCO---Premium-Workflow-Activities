// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.X509CertificateChain
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel
{
  internal class X509CertificateChain
  {
    public const uint DefaultChainPolicyOID = 1;
    private bool _useMachineContext;
    private X509ChainPolicy _chainPolicy;
    private uint _chainPolicyOID = 1;

    public X509CertificateChain()
      : this(false)
    {
    }

    public X509CertificateChain(bool useMachineContext) => this._useMachineContext = useMachineContext;

    public X509CertificateChain(bool useMachineContext, uint chainPolicyOID)
    {
      this._useMachineContext = useMachineContext;
      this._chainPolicyOID = chainPolicyOID;
    }

    public X509ChainPolicy ChainPolicy
    {
      get
      {
        if (this._chainPolicy == null)
          this._chainPolicy = new X509ChainPolicy();
        return this._chainPolicy;
      }
      set => this._chainPolicy = value;
    }

    public X509ChainStatus[] ChainStatus => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public bool Build(X509Certificate2 certificate)
    {
      if (certificate == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      if (certificate.Handle == IntPtr.Zero)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (certificate), SR.GetString("ID4071"));
      SafeCertChainHandle ppChainContext = SafeCertChainHandle.InvalidHandle;
      X509ChainPolicy chainPolicy = this.ChainPolicy;
      X509CertificateChain.BuildChain(this._useMachineContext ? new IntPtr(1L) : new IntPtr(0L), certificate.Handle, chainPolicy.ExtraStore, chainPolicy.ApplicationPolicy, chainPolicy.CertificatePolicy, chainPolicy.RevocationMode, chainPolicy.RevocationFlag, chainPolicy.VerificationTime, chainPolicy.UrlRetrievalTimeout, out ppChainContext);
      CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA pPolicyPara = new CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof (CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA)));
      CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS pPolicyStatus = new CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof (CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS)));
      pPolicyPara.dwFlags = (uint) (chainPolicy.VerificationFlags | (X509VerificationFlags) 4096);
      if (!CryptoUtil.CAPI.CertVerifyCertificateChainPolicy(new IntPtr((long) this._chainPolicyOID), ppChainContext, ref pPolicyPara, ref pPolicyStatus))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Marshal.GetLastWin32Error()));
      if (pPolicyStatus.dwError != 0U)
      {
        int dwError = (int) pPolicyStatus.dwError;
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(SR.GetString("ID4070", (object) X509Util.GetCertificateId(certificate), (object) new CryptographicException(dwError).Message)));
      }
      return true;
    }

    private static unsafe void BuildChain(
      IntPtr hChainEngine,
      IntPtr pCertContext,
      X509Certificate2Collection extraStore,
      OidCollection applicationPolicy,
      OidCollection certificatePolicy,
      X509RevocationMode revocationMode,
      X509RevocationFlag revocationFlag,
      DateTime verificationTime,
      TimeSpan timeout,
      out SafeCertChainHandle ppChainContext)
    {
      SafeCertStoreHandle memoryStore = X509CertificateChain.ExportToMemoryStore(extraStore, pCertContext);
      CryptoUtil.CAPI.CERT_CHAIN_PARA pChainPara = new CryptoUtil.CAPI.CERT_CHAIN_PARA();
      pChainPara.cbSize = (uint) Marshal.SizeOf(typeof (CryptoUtil.CAPI.CERT_CHAIN_PARA));
      SafeHGlobalHandle safeHglobalHandle1 = SafeHGlobalHandle.InvalidHandle;
      SafeHGlobalHandle safeHglobalHandle2 = SafeHGlobalHandle.InvalidHandle;
      try
      {
        if (applicationPolicy != null && applicationPolicy.Count > 0)
        {
          pChainPara.RequestedUsage.dwType = 0U;
          pChainPara.RequestedUsage.Usage.cUsageIdentifier = (uint) applicationPolicy.Count;
          safeHglobalHandle1 = X509CertificateChain.CopyOidsToUnmanagedMemory(applicationPolicy);
          pChainPara.RequestedUsage.Usage.rgpszUsageIdentifier = safeHglobalHandle1.DangerousGetHandle();
        }
        if (certificatePolicy != null && certificatePolicy.Count > 0)
        {
          pChainPara.RequestedIssuancePolicy.dwType = 0U;
          pChainPara.RequestedIssuancePolicy.Usage.cUsageIdentifier = (uint) certificatePolicy.Count;
          safeHglobalHandle2 = X509CertificateChain.CopyOidsToUnmanagedMemory(certificatePolicy);
          pChainPara.RequestedIssuancePolicy.Usage.rgpszUsageIdentifier = safeHglobalHandle2.DangerousGetHandle();
        }
        pChainPara.dwUrlRetrievalTimeout = (uint) timeout.Milliseconds;
        System.Runtime.InteropServices.ComTypes.FILETIME pTime = new System.Runtime.InteropServices.ComTypes.FILETIME();
        *(long*) &pTime = verificationTime.ToFileTime();
        uint dwFlags = X509CertificateChain.MapRevocationFlags(revocationMode, revocationFlag);
        if (!CryptoUtil.CAPI.CertGetCertificateChain(hChainEngine, pCertContext, ref pTime, memoryStore, ref pChainPara, dwFlags, IntPtr.Zero, out ppChainContext))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Marshal.GetLastWin32Error()));
      }
      finally
      {
        safeHglobalHandle1?.Dispose();
        safeHglobalHandle2?.Dispose();
        memoryStore.Close();
      }
    }

    private static SafeCertStoreHandle ExportToMemoryStore(
      X509Certificate2Collection collection,
      IntPtr pCertContext)
    {
      CryptoUtil.CAPI.CERT_CONTEXT structure1 = (CryptoUtil.CAPI.CERT_CONTEXT) Marshal.PtrToStructure(pCertContext, typeof (CryptoUtil.CAPI.CERT_CONTEXT));
      if ((collection == null || collection.Count <= 0) && structure1.hCertStore == IntPtr.Zero)
        return SafeCertStoreHandle.InvalidHandle;
      SafeCertStoreHandle hCertStore = CryptoUtil.CAPI.CertOpenStore(new IntPtr(2L), 65537U, IntPtr.Zero, 8704U, (string) null);
      if (hCertStore == null || hCertStore.IsInvalid)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Marshal.GetLastWin32Error()));
      if (collection != null && collection.Count > 0)
      {
        foreach (X509Certificate2 x509Certificate2 in collection)
        {
          if (!CryptoUtil.CAPI.CertAddCertificateLinkToStore(hCertStore, x509Certificate2.Handle, 4U, SafeCertContextHandle.InvalidHandle))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Marshal.GetLastWin32Error()));
        }
      }
      using (SafeCertContextHandle certificateContext = CryptoUtil.CAPI.CertCreateCertificateContext(structure1.dwCertEncodingType, structure1.pbCertEncoded, structure1.cbCertEncoded))
      {
        CryptoUtil.CAPI.CERT_CONTEXT structure2 = (CryptoUtil.CAPI.CERT_CONTEXT) Marshal.PtrToStructure(new X509Certificate2(certificateContext.DangerousGetHandle()).Handle, typeof (CryptoUtil.CAPI.CERT_CONTEXT));
        if (structure2.hCertStore != IntPtr.Zero)
        {
          X509Certificate2Collection certificates = (X509Certificate2Collection) null;
          X509Store x509Store = new X509Store(structure2.hCertStore);
          try
          {
            certificates = x509Store.Certificates;
            X509Certificate2Enumerator enumerator = certificates.GetEnumerator();
            while (enumerator.MoveNext())
            {
              X509Certificate2 current = enumerator.Current;
              if (!CryptoUtil.CAPI.CertAddCertificateLinkToStore(hCertStore, current.Handle, 4U, SafeCertContextHandle.InvalidHandle))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Marshal.GetLastWin32Error()));
            }
          }
          finally
          {
            CryptoUtil.ResetAllCertificates(certificates);
            x509Store.Close();
          }
        }
      }
      return hCertStore;
    }

    private static SafeHGlobalHandle CopyOidsToUnmanagedMemory(OidCollection oids)
    {
      SafeHGlobalHandle invalidHandle = SafeHGlobalHandle.InvalidHandle;
      if (oids == null || oids.Count == 0)
        return invalidHandle;
      int num1 = checked (oids.Count * Marshal.SizeOf(typeof (IntPtr)));
      int num2 = 0;
      foreach (Oid oid in oids)
        checked { num2 += oid.Value.Length + 1; }
      SafeHGlobalHandle safeHglobalHandle = SafeHGlobalHandle.AllocHGlobal(num1 + num2);
      IntPtr num3 = new IntPtr((long) safeHglobalHandle.DangerousGetHandle() + (long) num1);
      for (int index = 0; index < oids.Count; ++index)
      {
        Marshal.WriteIntPtr(new IntPtr((long) safeHglobalHandle.DangerousGetHandle() + (long) (index * Marshal.SizeOf(typeof (IntPtr)))), num3);
        byte[] bytes = Encoding.ASCII.GetBytes(oids[index].Value);
        Marshal.Copy(bytes, 0, num3, bytes.Length);
        num3 = new IntPtr((long) num3 + (long) oids[index].Value.Length + 1L);
      }
      return safeHglobalHandle;
    }

    private static uint MapRevocationFlags(
      X509RevocationMode revocationMode,
      X509RevocationFlag revocationFlag)
    {
      uint num1 = 0;
      switch (revocationMode)
      {
        case X509RevocationMode.NoCheck:
          return num1;
        case X509RevocationMode.Offline:
          num1 |= 2147483648U;
          break;
      }
      uint num2;
      switch (revocationFlag)
      {
        case X509RevocationFlag.EndCertificateOnly:
          num2 = num1 | 268435456U;
          break;
        case X509RevocationFlag.EntireChain:
          num2 = num1 | 536870912U;
          break;
        default:
          num2 = num1 | 1073741824U;
          break;
      }
      return num2;
    }
  }
}
