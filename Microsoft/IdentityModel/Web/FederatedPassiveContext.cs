// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.FederatedPassiveContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  internal class FederatedPassiveContext
  {
    private const string ControlIdKey = "id";
    private const string ReturnUrlKey = "ru";
    private const string SignInContextKey = "cx";
    private const string RememberMeKey = "rm";
    private string _wctx;
    private string _controlId;
    private string _signInContext;
    private string _returnUrl;
    private bool _rememberMe;

    public FederatedPassiveContext(
      string controlId,
      string signInContext,
      string returnUrl,
      bool rememberMe)
    {
      this._controlId = controlId;
      this._signInContext = signInContext;
      this._returnUrl = returnUrl;
      this._rememberMe = rememberMe;
    }

    public FederatedPassiveContext(string wctx) => this._wctx = wctx;

    public string ControlId
    {
      get
      {
        this.Initialize();
        return this._controlId;
      }
    }

    public string SignInContext
    {
      get
      {
        this.Initialize();
        return this._signInContext;
      }
    }

    public string ReturnUrl
    {
      get
      {
        this.Initialize();
        return this._returnUrl;
      }
    }

    public bool RememberMe
    {
      get
      {
        this.Initialize();
        return this._rememberMe;
      }
    }

    public string WCtx
    {
      get
      {
        this.Initialize();
        return this._wctx;
      }
    }

    private void Initialize()
    {
      if (this._wctx == null)
      {
        StringBuilder stringBuilder = new StringBuilder(128);
        stringBuilder.Append("rm");
        stringBuilder.Append('=');
        stringBuilder.Append(this._rememberMe ? '1' : '0');
        stringBuilder.Append('&');
        stringBuilder.Append("id");
        stringBuilder.Append('=');
        stringBuilder.Append(HttpUtility.UrlEncode(this._controlId));
        if (!string.IsNullOrEmpty(this._signInContext))
        {
          stringBuilder.Append('&');
          stringBuilder.Append("cx");
          stringBuilder.Append('=');
          stringBuilder.Append(HttpUtility.UrlEncode(this._signInContext));
        }
        if (!string.IsNullOrEmpty(this._returnUrl))
        {
          stringBuilder.Append('&');
          stringBuilder.Append("ru");
          stringBuilder.Append('=');
          stringBuilder.Append(HttpUtility.UrlEncode(this._returnUrl));
        }
        this._wctx = stringBuilder.ToString();
      }
      else
      {
        if (this._controlId != null)
          return;
        this._controlId = string.Empty;
        NameValueCollection queryString = HttpUtility.ParseQueryString(this._wctx);
        foreach (string key in queryString.Keys)
        {
          if (key == "rm")
            this._rememberMe = queryString[key] != "0";
          else if (key == "id")
            this._controlId = queryString[key];
          else if (key == "cx")
            this._signInContext = queryString[key];
          else if (key == "ru")
            this._returnUrl = queryString[key];
        }
      }
    }
  }
}
