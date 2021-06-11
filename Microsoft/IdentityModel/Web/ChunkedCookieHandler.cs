// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.ChunkedCookieHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public sealed class ChunkedCookieHandler : CookieHandler
  {
    public const int DefaultChunkSize = 2000;
    public const int MinimumChunkSize = 1000;
    private int _chunkSize;

    public ChunkedCookieHandler()
      : this(2000)
    {
    }

    public ChunkedCookieHandler(int chunkSize) => this._chunkSize = chunkSize >= 1000 ? chunkSize : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (chunkSize), Microsoft.IdentityModel.SR.GetString("ID1016", (object) 1000));

    public int ChunkSize => this._chunkSize;

    protected override void DeleteCore(
      string name,
      string path,
      string domain,
      HttpContext context)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this.DeleteInternal(name, path, domain, context.Request.Cookies, context.Response.Cookies);
    }

    internal void DeleteInternal(
      string name,
      string path,
      string domain,
      HttpCookieCollection requestCookies,
      HttpCookieCollection responseCookies)
    {
      foreach (HttpCookie cookieChunk in this.GetCookieChunks(name, requestCookies))
      {
        HttpCookie cookie = new HttpCookie(cookieChunk.Name, (string) null);
        cookie.Path = path;
        cookie.Expires = DateTime.UtcNow.AddDays(-1.0);
        if (!string.IsNullOrEmpty(domain))
          cookie.Domain = domain;
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, (string) null, (TraceRecord) new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Deleting, cookieChunk, path), (Exception) null);
        responseCookies.Set(cookie);
      }
    }

    private IEnumerable<KeyValuePair<string, string>> GetCookieChunks(
      string baseName,
      string cookieValue)
    {
      int chunksRequired = CryptoUtil.CeilingDivide(cookieValue.Length, this._chunkSize);
      if (chunksRequired > 20 && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8008"));
      for (int i = 0; i < chunksRequired; ++i)
        yield return new KeyValuePair<string, string>(ChunkedCookieHandler.GetChunkName(baseName, i), cookieValue.Substring(i * this._chunkSize, Math.Min(cookieValue.Length - i * this._chunkSize, this._chunkSize)));
    }

    private IEnumerable<HttpCookie> GetCookieChunks(
      string baseName,
      HttpCookieCollection cookies)
    {
      int chunkIndex = 0;
      HttpCookie cookie;
      for (string chunkName = ChunkedCookieHandler.GetChunkName(baseName, chunkIndex); (cookie = cookies[chunkName]) != null; chunkName = ChunkedCookieHandler.GetChunkName(baseName, ++chunkIndex))
        yield return cookie;
    }

    protected override byte[] ReadCore(string name, HttpContext context)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      return this.ReadInternal(name, context.Request.Cookies);
    }

    internal byte[] ReadInternal(string name, HttpCookieCollection requestCookies)
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      foreach (HttpCookie cookieChunk in this.GetCookieChunks(name, requestCookies))
      {
        if (stringBuilder == null)
          stringBuilder = new StringBuilder();
        stringBuilder.Append(cookieChunk.Value);
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, (string) null, (TraceRecord) new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Reading, cookieChunk, cookieChunk.Path), (Exception) null);
      }
      return stringBuilder != null ? Convert.FromBase64String(stringBuilder.ToString()) : (byte[]) null;
    }

    protected override void WriteCore(
      byte[] value,
      string name,
      string path,
      string domain,
      DateTime expirationTime,
      bool secure,
      bool httpOnly,
      HttpContext context)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this.WriteInternal(value, name, path, domain, expirationTime, secure, httpOnly, context.Request.Cookies, context.Response.Cookies);
    }

    internal void WriteInternal(
      byte[] value,
      string name,
      string path,
      string domain,
      DateTime expirationTime,
      bool secure,
      bool httpOnly,
      HttpCookieCollection requestCookies,
      HttpCookieCollection responseCookies)
    {
      string base64String = Convert.ToBase64String(value);
      this.DeleteInternal(name, path, domain, requestCookies, responseCookies);
      foreach (KeyValuePair<string, string> cookieChunk in this.GetCookieChunks(name, base64String))
      {
        HttpCookie cookie = new HttpCookie(cookieChunk.Key, cookieChunk.Value);
        cookie.Secure = secure;
        cookie.HttpOnly = httpOnly;
        cookie.Path = path;
        if (!string.IsNullOrEmpty(domain))
          cookie.Domain = domain;
        if (expirationTime != DateTime.MinValue)
          cookie.Expires = expirationTime;
        responseCookies.Set(cookie);
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, (string) null, (TraceRecord) new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Writing, cookie, cookie.Path), (Exception) null);
      }
    }

    private static string GetChunkName(string baseName, int chunkIndex) => chunkIndex != 0 ? baseName + chunkIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture) : baseName;
  }
}
