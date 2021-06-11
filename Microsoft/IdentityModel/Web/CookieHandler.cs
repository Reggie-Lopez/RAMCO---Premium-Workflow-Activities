// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.CookieHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public abstract class CookieHandler
  {
    private const string DefaultCookieName = "FedAuth";
    private string _domain;
    private bool _hideFromClientScript = true;
    private string _name = "FedAuth";
    private string _path = CookieHandler.DefaultCookiePath;
    private bool _requireSsl = true;
    private TimeSpan? _persistentSessionLifetime = new TimeSpan?();

    private static string DefaultCookiePath
    {
      get
      {
        string str = Uri.EscapeUriString(HttpRuntime.AppDomainAppVirtualPath ?? string.Empty);
        return !str.EndsWith("/", StringComparison.Ordinal) ? str + "/" : str;
      }
    }

    public void Delete() => this.Delete(HttpContext.Current);

    public void Delete(string name) => this.Delete(name, HttpContext.Current);

    public void Delete(HttpContext context) => this.Delete(this._name, context);

    public void Delete(string name, HttpContext context) => this.Delete(name, this._path, this._domain, context);

    public void Delete(string name, string path, string domain, HttpContext context)
    {
      if (string.IsNullOrEmpty(name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));
      this.DeleteCore(name, path, domain, context);
    }

    protected abstract void DeleteCore(
      string name,
      string path,
      string domain,
      HttpContext context);

    public string Domain
    {
      get => this._domain;
      set => this._domain = value;
    }

    public bool HideFromClientScript
    {
      get => this._hideFromClientScript;
      set => this._hideFromClientScript = value;
    }

    public string Name
    {
      get => this._name;
      set => this._name = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public string Path
    {
      get => this._path;
      set => this._path = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public TimeSpan? PersistentSessionLifetime
    {
      get => this._persistentSessionLifetime;
      set
      {
        TimeSpan? nullable = value;
        TimeSpan zero = TimeSpan.Zero;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() < zero ? 1 : 0) : 0) != 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), Microsoft.IdentityModel.SR.GetString("ID1022"));
        this._persistentSessionLifetime = value;
      }
    }

    public bool RequireSsl
    {
      get => this._requireSsl;
      set => this._requireSsl = value;
    }

    public byte[] Read() => this.Read(HttpContext.Current);

    public byte[] Read(string name) => this.Read(name, HttpContext.Current);

    public byte[] Read(HttpContext context) => this.Read(this._name, context);

    public byte[] Read(string name, HttpContext context) => !string.IsNullOrEmpty(name) ? this.ReadCore(name, context) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (name));

    protected abstract byte[] ReadCore(string name, HttpContext context);

    public void Write(byte[] value, bool isPersistent, DateTime tokenExpirationTime)
    {
      DateTime expirationTime = isPersistent ? (!this._persistentSessionLifetime.HasValue ? tokenExpirationTime : DateTime.UtcNow + this._persistentSessionLifetime.Value) : DateTime.MinValue;
      this.Write(value, this._name, expirationTime, HttpContext.Current);
    }

    public void Write(byte[] value, string name, DateTime expirationTime) => this.Write(value, name, expirationTime, HttpContext.Current);

    public void Write(byte[] value, string name, DateTime expirationTime, HttpContext context) => this.Write(value, name, this._path, this._domain, expirationTime, this._requireSsl, this._hideFromClientScript, context);

    public void Write(
      byte[] value,
      string name,
      string path,
      string domain,
      DateTime expirationTime,
      bool requiresSsl,
      bool hideFromClientScript,
      HttpContext context)
    {
      if (value == null || value.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (string.IsNullOrEmpty(name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (name));
      this.WriteCore(value, name, path, domain, expirationTime, requiresSsl, hideFromClientScript, context);
    }

    protected abstract void WriteCore(
      byte[] value,
      string name,
      string path,
      string domain,
      DateTime expirationTime,
      bool secure,
      bool httpOnly,
      HttpContext context);

    internal static string MatchCookiePath(string targetUrl)
    {
      Uri result;
      if (!string.IsNullOrEmpty(targetUrl) && Uri.TryCreate(targetUrl, UriKind.RelativeOrAbsolute, out result))
      {
        SessionAuthenticationModule authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
        if (authenticationModule != null && authenticationModule.CookieHandler != null)
          return authenticationModule.CookieHandler.MatchCookiePath(HttpContext.Current.Request.Url, result);
      }
      return targetUrl;
    }

    internal string MatchCookiePath(Uri baseUri, Uri targetUri)
    {
      Uri uri = targetUri.IsAbsoluteUri ? targetUri : new Uri(baseUri, targetUri);
      string host = uri.Host;
      string pathAndQuery = uri.PathAndQuery;
      string path = this._path;
      string str = this._domain;
      if (str == null)
        str = host;
      else if (!str.Equals(host, StringComparison.OrdinalIgnoreCase) && !str.StartsWith(".", StringComparison.OrdinalIgnoreCase))
        str = "." + str;
      if (!path.EndsWith("/", StringComparison.Ordinal))
        path += "/";
      if (!host.EndsWith(str, StringComparison.OrdinalIgnoreCase) || !pathAndQuery.StartsWith(path, StringComparison.OrdinalIgnoreCase))
        return targetUri.OriginalString;
      UriBuilder uriBuilder = new UriBuilder(uri);
      uriBuilder.Path = path.Length >= uriBuilder.Path.Length ? path : path + uriBuilder.Path.Substring(path.Length);
      return targetUri.IsAbsoluteUri ? uriBuilder.Uri.AbsoluteUri : uriBuilder.Path + uriBuilder.Fragment + uriBuilder.Query;
    }
  }
}
