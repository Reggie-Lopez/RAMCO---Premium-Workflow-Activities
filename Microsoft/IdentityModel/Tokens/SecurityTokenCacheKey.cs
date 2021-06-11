// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenCacheKey
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenCacheKey
  {
    private System.Xml.UniqueId _contextId;
    private System.Xml.UniqueId _keyGeneration;
    private string _endpointId;
    private bool _isSessionMode;
    private bool _canIgnoreContextId;
    private bool _canIgnoreKeyGeneration;
    private bool _canIgnoreEndpointId;

    public SecurityTokenCacheKey(
      string endpointId,
      System.Xml.UniqueId contextId,
      System.Xml.UniqueId keyGeneration,
      bool isSessionMode)
    {
      this._endpointId = endpointId;
      this._contextId = contextId;
      this._keyGeneration = keyGeneration;
      this._canIgnoreContextId = false;
      this._canIgnoreKeyGeneration = false;
      this._canIgnoreEndpointId = false;
      this._isSessionMode = isSessionMode;
    }

    public System.Xml.UniqueId ContextId => this._contextId;

    public System.Xml.UniqueId KeyGeneration => this._keyGeneration;

    public string EndpointId => this._endpointId;

    public bool CanIgnoreContextId
    {
      get => this._canIgnoreContextId;
      set => this._canIgnoreContextId = value;
    }

    public bool CanIgnoreKeyGeneration
    {
      get => this._canIgnoreKeyGeneration;
      set => this._canIgnoreKeyGeneration = value;
    }

    public bool CanIgnoreEndpointId
    {
      get => this._canIgnoreEndpointId;
      set => this._canIgnoreEndpointId = value;
    }

    public bool IsSessionMode => this._isSessionMode;

    public override int GetHashCode() => this._keyGeneration == (System.Xml.UniqueId) null ? this._contextId.GetHashCode() : this._contextId.GetHashCode() ^ this._keyGeneration.GetHashCode();

    public override bool Equals(object obj)
    {
      if ((object) (obj as SecurityTokenCacheKey) == null)
        return false;
      SecurityTokenCacheKey securityTokenCacheKey = (SecurityTokenCacheKey) obj;
      bool flag = true;
      if (!this._canIgnoreEndpointId && !securityTokenCacheKey._canIgnoreEndpointId)
        flag = StringComparer.Ordinal.Equals(securityTokenCacheKey.EndpointId, this._endpointId);
      if (flag && !this._canIgnoreContextId && !securityTokenCacheKey._canIgnoreContextId)
        flag = securityTokenCacheKey.ContextId == this._contextId;
      if (flag && !this._canIgnoreKeyGeneration && !securityTokenCacheKey.CanIgnoreKeyGeneration)
        flag = securityTokenCacheKey.KeyGeneration == this._keyGeneration;
      return flag;
    }

    public static bool operator ==(SecurityTokenCacheKey a, SecurityTokenCacheKey b) => object.ReferenceEquals((object) a, (object) null) ? object.ReferenceEquals((object) b, (object) null) : a.Equals((object) b);

    public static bool operator !=(SecurityTokenCacheKey a, SecurityTokenCacheKey b) => !(a == b);
  }
}
