// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustSerializationContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustSerializationContext
  {
    private SecurityTokenResolver _securityTokenResolver;
    private SecurityTokenResolver _useKeyTokenResolver;
    private SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;
    private SecurityTokenSerializer _securityTokenSerializer;

    public WSTrustSerializationContext()
      : this(SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
    {
    }

    public WSTrustSerializationContext(
      SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
      : this(securityTokenHandlerCollectionManager, Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance, Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance)
    {
    }

    public WSTrustSerializationContext(
      SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager,
      SecurityTokenResolver securityTokenResolver,
      SecurityTokenResolver useKeyTokenResolver)
    {
      if (securityTokenHandlerCollectionManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollectionManager));
      if (securityTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenResolver));
      if (useKeyTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (useKeyTokenResolver));
      this._securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
      this._securityTokenSerializer = (SecurityTokenSerializer) new SecurityTokenSerializerAdapter(securityTokenHandlerCollectionManager[""]);
      this._securityTokenResolver = securityTokenResolver;
      this._useKeyTokenResolver = useKeyTokenResolver;
    }

    public SecurityTokenResolver TokenResolver
    {
      get => this._securityTokenResolver;
      set => this._securityTokenResolver = value;
    }

    public SecurityTokenResolver UseKeyTokenResolver
    {
      get => this._useKeyTokenResolver;
      set => this._useKeyTokenResolver = value;
    }

    public SecurityTokenSerializer SecurityTokenSerializer
    {
      get => this._securityTokenSerializer;
      set => this._securityTokenSerializer = value;
    }

    public SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager
    {
      get => this._securityTokenHandlerCollectionManager;
      set => this._securityTokenHandlerCollectionManager = value;
    }

    public SecurityTokenHandlerCollection SecurityTokenHandlers => this._securityTokenHandlerCollectionManager[""];
  }
}
