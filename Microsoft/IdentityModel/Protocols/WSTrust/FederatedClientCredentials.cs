// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.FederatedClientCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class FederatedClientCredentials : ClientCredentials
  {
    private SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

    public SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => this._securityTokenHandlerCollectionManager;

    public FederatedClientCredentials()
      : this(SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
    {
    }

    public FederatedClientCredentials(
      SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
    {
      this._securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager != null ? securityTokenHandlerCollectionManager : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollectionManager));
    }

    public FederatedClientCredentials(ClientCredentials other)
      : this(other, FederatedClientCredentials.GetSecurityTokenHandlerCollectionManagerForCredentials(other))
    {
    }

    public FederatedClientCredentials(
      ClientCredentials other,
      SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
      : base(other)
    {
      this._securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager != null ? securityTokenHandlerCollectionManager : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollectionManager));
    }

    protected override ClientCredentials CloneCore() => (ClientCredentials) new FederatedClientCredentials((ClientCredentials) this, this._securityTokenHandlerCollectionManager);

    public override SecurityTokenManager CreateSecurityTokenManager() => (SecurityTokenManager) new FederatedClientCredentialsSecurityTokenManager(this);

    private static SecurityTokenHandlerCollectionManager GetSecurityTokenHandlerCollectionManagerForCredentials(
      ClientCredentials other)
    {
      return other is FederatedClientCredentials clientCredentials ? clientCredentials._securityTokenHandlerCollectionManager : SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager();
    }
  }
}
