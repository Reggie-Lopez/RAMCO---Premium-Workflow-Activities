// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.X509Util
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel
{
  internal static class X509Util
  {
    internal static RSA EnsureAndGetPrivateRSAKey(X509Certificate2 certificate)
    {
      if (!certificate.HasPrivateKey)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID1001", (object) certificate.Thumbprint)));
      AsymmetricAlgorithm privateKey;
      try
      {
        privateKey = certificate.PrivateKey;
      }
      catch (CryptographicException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID1039", (object) certificate.Thumbprint), (Exception) ex));
      }
      return privateKey is RSA rsa ? rsa : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID1002", (object) certificate.Thumbprint)));
    }

    internal static X509Certificate2 ResolveCertificate(
      CertificateReferenceElement element)
    {
      return X509Util.ResolveCertificate(element.StoreName, element.StoreLocation, element.X509FindType, (object) element.FindValue);
    }

    internal static bool TryResolveCertificate(
      CertificateReferenceElement element,
      out X509Certificate2 certificate)
    {
      return X509Util.TryResolveCertificate(element.StoreName, element.StoreLocation, element.X509FindType, (object) element.FindValue, out certificate);
    }

    internal static X509Certificate2 ResolveCertificate(
      StoreName storeName,
      StoreLocation storeLocation,
      X509FindType findType,
      object findValue)
    {
      X509Certificate2 certificate = (X509Certificate2) null;
      if (!X509Util.TryResolveCertificate(storeName, storeLocation, findType, findValue, out certificate))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(SR.GetString("ID1025", (object) storeName, (object) storeLocation, (object) findType, findValue)));
      return certificate;
    }

    internal static bool TryResolveCertificate(
      StoreName storeName,
      StoreLocation storeLocation,
      X509FindType findType,
      object findValue,
      out X509Certificate2 certificate)
    {
      X509Store x509Store = new X509Store(storeName, storeLocation);
      x509Store.Open(OpenFlags.ReadOnly);
      certificate = (X509Certificate2) null;
      X509Certificate2Collection certificates1 = (X509Certificate2Collection) null;
      X509Certificate2Collection certificates2 = (X509Certificate2Collection) null;
      try
      {
        certificates1 = x509Store.Certificates;
        certificates2 = certificates1.Find(findType, findValue, false);
        if (certificates2.Count == 1)
        {
          certificate = new X509Certificate2((X509Certificate) certificates2[0]);
          return true;
        }
      }
      finally
      {
        CryptoUtil.ResetAllCertificates(certificates2);
        CryptoUtil.ResetAllCertificates(certificates1);
        x509Store.Close();
      }
      return false;
    }

    internal static string GetCertificateId(X509Certificate2 certificate)
    {
      if (certificate == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      string str = certificate.SubjectName.Name;
      if (string.IsNullOrEmpty(str))
        str = certificate.Thumbprint;
      return str;
    }

    internal static string GetCertificateIssuerName(
      X509Certificate2 certificate,
      Microsoft.IdentityModel.Tokens.IssuerNameRegistry issuerNameRegistry)
    {
      if (certificate == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      if (issuerNameRegistry == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (issuerNameRegistry));
      X509Chain x509Chain = new X509Chain();
      x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
      x509Chain.Build(certificate);
      X509ChainElementCollection chainElements = x509Chain.ChainElements;
      string str = (string) null;
      if (chainElements.Count > 1)
      {
        using (X509SecurityToken x509SecurityToken = new X509SecurityToken(chainElements[1].Certificate))
          str = issuerNameRegistry.GetIssuerName((SecurityToken) x509SecurityToken);
      }
      else
      {
        using (X509SecurityToken x509SecurityToken = new X509SecurityToken(certificate))
          str = issuerNameRegistry.GetIssuerName((SecurityToken) x509SecurityToken);
      }
      for (int index = 1; index < chainElements.Count; ++index)
        chainElements[index].Certificate.Reset();
      return str;
    }

    internal static X509CertificateValidator CreateCertificateValidator(
      X509CertificateValidationMode certificateValidationMode,
      X509RevocationMode revocationMode,
      StoreLocation trustedStoreLocation)
    {
      return (X509CertificateValidator) new X509CertificateValidatorEx(certificateValidationMode, revocationMode, trustedStoreLocation);
    }
  }
}
