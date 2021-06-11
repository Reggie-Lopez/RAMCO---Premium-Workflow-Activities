// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.TokenService
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class TokenService
  {
    private TokenServiceEndpoint _endpoint;
    private IUserCredential _userCredential;
    private static IUserCredential _defaultUserCredential = (IUserCredential) new KerberosCredential();

    public static IUserCredential DefaultUserCredential => TokenService._defaultUserCredential;

    public static UserCredentialType DefaultUserCredentialType => UserCredentialType.KerberosV5Credential;

    public TokenService(TokenServiceEndpoint endpoint)
      : this(endpoint, TokenService.DefaultUserCredential)
    {
    }

    public TokenService(EndpointAddress endpointAddress)
      : this(endpointAddress, TokenService.DefaultUserCredential)
    {
    }

    public TokenService(EndpointAddress endpointAddress, IUserCredential userCredential)
      : this(new TokenServiceEndpoint(endpointAddress, userCredential == null ? TokenService.DefaultUserCredentialType : userCredential.CredentialType), userCredential)
    {
    }

    public TokenService(
      EndpointAddress endpointAddress,
      IUserCredential userCredential,
      string displayCredentialHint)
      : this(new TokenServiceEndpoint(endpointAddress, userCredential == null ? TokenService.DefaultUserCredentialType : userCredential.CredentialType, displayCredentialHint), userCredential)
    {
    }

    public TokenService(TokenServiceEndpoint endpoint, IUserCredential userCredential)
    {
      if (endpoint == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpoint));
      if (userCredential == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (userCredential));
      if (endpoint.UserCredentialType != userCredential.CredentialType)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (endpoint), Microsoft.IdentityModel.SR.GetString("ID2048", (object) endpoint.UserCredentialType, (object) userCredential.CredentialType));
      if (endpoint.MetadataSet == null || endpoint.MetadataSet.MetadataSections == null || endpoint.MetadataSet.MetadataSections.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (endpoint), Microsoft.IdentityModel.SR.GetString("ID2049"));
      for (int index = 0; index < endpoint.MetadataSet.MetadataSections.Count; ++index)
      {
        MetadataSection metadataSection = endpoint.MetadataSet.MetadataSections[index];
        if (metadataSection.Metadata == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (endpoint), Microsoft.IdentityModel.SR.GetString("ID2049"));
        if (metadataSection.Metadata is MetadataReference metadata)
        {
          if (metadata.Address == (EndpointAddress) null || metadata.Address.Uri == (Uri) null || !metadata.Address.Uri.IsAbsoluteUri)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (endpoint), Microsoft.IdentityModel.SR.GetString("ID2030"));
          if (!StringComparer.OrdinalIgnoreCase.Equals(metadata.Address.Uri.Scheme, Uri.UriSchemeHttps))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (endpoint), Microsoft.IdentityModel.SR.GetString("ID2029"));
        }
      }
      this._endpoint = endpoint;
      this._userCredential = userCredential;
    }

    internal X509Certificate2Collection Certificates => this._endpoint.Certificates;

    public string DisplayCredentialHint => this._endpoint.DisplayCredentialHint;

    public EndpointAddress Address => this._endpoint.Address;

    public MetadataSet MetadataSet => this._endpoint.MetadataSet;

    public MetadataReference MetadataReference => this._endpoint.MetadataReference;

    public IUserCredential UserCredential => this._userCredential;
  }
}
