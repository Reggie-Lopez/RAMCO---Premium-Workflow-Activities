// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml11;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenHandlerCollection : Collection<SecurityTokenHandler>
  {
    internal static int _defaultHandlerCollectionCount = 8;
    private Dictionary<string, SecurityTokenHandler> _byIdentifier = new Dictionary<string, SecurityTokenHandler>();
    private Dictionary<Type, SecurityTokenHandler> _byType = new Dictionary<Type, SecurityTokenHandler>();
    private SecurityTokenHandlerConfiguration _configuration;

    public static SecurityTokenHandlerCollection CreateDefaultSecurityTokenHandlerCollection() => SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(new SecurityTokenHandlerConfiguration());

    public static SecurityTokenHandlerCollection CreateDefaultSecurityTokenHandlerCollection(
      SecurityTokenHandlerConfiguration configuration)
    {
      SecurityTokenHandlerCollection handlerCollection = new SecurityTokenHandlerCollection((IEnumerable<SecurityTokenHandler>) new SecurityTokenHandler[8]
      {
        (SecurityTokenHandler) new KerberosSecurityTokenHandler(),
        (SecurityTokenHandler) new RsaSecurityTokenHandler(),
        (SecurityTokenHandler) new Saml11SecurityTokenHandler(),
        (SecurityTokenHandler) new Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler(),
        (SecurityTokenHandler) new WindowsUserNameSecurityTokenHandler(),
        (SecurityTokenHandler) new X509SecurityTokenHandler(),
        (SecurityTokenHandler) new EncryptedSecurityTokenHandler(),
        (SecurityTokenHandler) new SessionSecurityTokenHandler()
      }, configuration);
      SecurityTokenHandlerCollection._defaultHandlerCollectionCount = handlerCollection.Count;
      return handlerCollection;
    }

    public SecurityTokenHandlerCollection()
      : this(new SecurityTokenHandlerConfiguration())
    {
    }

    public SecurityTokenHandlerCollection(SecurityTokenHandlerConfiguration configuration) => this._configuration = configuration != null ? configuration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (configuration));

    public SecurityTokenHandlerCollection(IEnumerable<SecurityTokenHandler> handlers)
      : this(handlers, new SecurityTokenHandlerConfiguration())
    {
    }

    public SecurityTokenHandlerCollection(
      IEnumerable<SecurityTokenHandler> handlers,
      SecurityTokenHandlerConfiguration configuration)
    {
      if (handlers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (handlers));
      this._configuration = configuration;
      foreach (SecurityTokenHandler handler in handlers)
        this.Add(handler);
    }

    public SecurityTokenHandler this[string tokenTypeIdentifier]
    {
      get
      {
        if (string.IsNullOrEmpty(tokenTypeIdentifier))
          return (SecurityTokenHandler) null;
        SecurityTokenHandler securityTokenHandler;
        this._byIdentifier.TryGetValue(tokenTypeIdentifier, out securityTokenHandler);
        return securityTokenHandler;
      }
    }

    public SecurityTokenHandler this[SecurityToken token] => token == null ? (SecurityTokenHandler) null : this[token.GetType()];

    public SecurityTokenHandler this[Type tokenType]
    {
      get
      {
        SecurityTokenHandler securityTokenHandler = (SecurityTokenHandler) null;
        if ((object) tokenType != null)
          this._byType.TryGetValue(tokenType, out securityTokenHandler);
        return securityTokenHandler;
      }
    }

    public SecurityTokenHandlerConfiguration Configuration => this._configuration;

    public IEnumerable<Type> TokenTypes => (IEnumerable<Type>) this._byType.Keys;

    public IEnumerable<string> TokenTypeIdentifiers => (IEnumerable<string>) this._byIdentifier.Keys;

    public void AddOrReplace(SecurityTokenHandler handler)
    {
      Type type = handler != null ? handler.TokenType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (handler));
      if ((object) type != null && this._byType.ContainsKey(type))
      {
        this.Remove(this[type]);
      }
      else
      {
        string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
        if (tokenTypeIdentifiers != null)
        {
          foreach (string str in tokenTypeIdentifiers)
          {
            if (str != null && this._byIdentifier.ContainsKey(str))
            {
              this.Remove(this[str]);
              break;
            }
          }
        }
      }
      this.Add(handler);
    }

    private void AddToDictionaries(SecurityTokenHandler handler)
    {
      if (handler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (handler));
      bool flag = false;
      string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
      if (tokenTypeIdentifiers != null)
      {
        foreach (string key in tokenTypeIdentifiers)
        {
          if (key != null)
          {
            this._byIdentifier.Add(key, handler);
            flag = true;
          }
        }
      }
      Type tokenType = handler.TokenType;
      if ((object) handler.TokenType != null)
      {
        try
        {
          this._byType.Add(tokenType, handler);
        }
        catch
        {
          if (flag)
            this.RemoveFromDictionaries(handler);
          throw;
        }
      }
      handler.ContainingCollection = this;
      if (handler.Configuration != null)
        return;
      handler.Configuration = this._configuration;
    }

    private void RemoveFromDictionaries(SecurityTokenHandler handler)
    {
      string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
      if (tokenTypeIdentifiers != null)
      {
        foreach (string key in tokenTypeIdentifiers)
        {
          if (key != null)
            this._byIdentifier.Remove(key);
        }
      }
      Type tokenType = handler.TokenType;
      if ((object) tokenType != null && this._byType.ContainsKey(tokenType))
        this._byType.Remove(tokenType);
      handler.ContainingCollection = (SecurityTokenHandlerCollection) null;
      handler.Configuration = (SecurityTokenHandlerConfiguration) null;
    }

    protected override void ClearItems()
    {
      base.ClearItems();
      this._byIdentifier.Clear();
      this._byType.Clear();
    }

    protected override void InsertItem(int index, SecurityTokenHandler item)
    {
      base.InsertItem(index, item);
      try
      {
        this.AddToDictionaries(item);
      }
      catch
      {
        base.RemoveItem(index);
        throw;
      }
    }

    protected override void RemoveItem(int index)
    {
      SecurityTokenHandler handler = this.Items[index];
      base.RemoveItem(index);
      this.RemoveFromDictionaries(handler);
    }

    protected override void SetItem(int index, SecurityTokenHandler item)
    {
      SecurityTokenHandler handler = this.Items[index];
      base.SetItem(index, item);
      this.RemoveFromDictionaries(handler);
      try
      {
        this.AddToDictionaries(item);
      }
      catch
      {
        base.SetItem(index, handler);
        this.AddToDictionaries(handler);
        throw;
      }
    }

    public bool CanReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this)
      {
        if (securityTokenHandler != null && securityTokenHandler.CanReadToken(reader))
          return true;
      }
      return false;
    }

    public bool CanWriteToken(SecurityToken token)
    {
      SecurityTokenHandler securityTokenHandler = token != null ? this[token] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      return securityTokenHandler != null && securityTokenHandler.CanWriteToken;
    }

    public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor) => ((tokenDescriptor != null ? this[tokenDescriptor.TokenType] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor))) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4020", (object) tokenDescriptor.TokenType)))).CreateToken(tokenDescriptor);

    public ClaimsIdentityCollection ValidateToken(SecurityToken token)
    {
      SecurityTokenHandler securityTokenHandler = token != null ? this[token] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      return securityTokenHandler != null && securityTokenHandler.CanValidateToken ? securityTokenHandler.ValidateToken(token) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4011", (object) token.GetType())));
    }

    public SecurityToken ReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this)
      {
        if (securityTokenHandler != null && securityTokenHandler.CanReadToken(reader))
          return securityTokenHandler.ReadToken(reader);
      }
      return (SecurityToken) null;
    }

    public void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      SecurityTokenHandler securityTokenHandler = token != null ? this[token] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (securityTokenHandler == null || !securityTokenHandler.CanWriteToken)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4010", (object) token.GetType())));
      securityTokenHandler.WriteToken(writer, token);
    }
  }
}
