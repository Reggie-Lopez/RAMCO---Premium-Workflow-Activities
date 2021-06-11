// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.WSFederationMessage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public abstract class WSFederationMessage
  {
    private Dictionary<string, string> _parameters = new Dictionary<string, string>();
    private Uri _baseUri;

    public IDictionary<string, string> Parameters => (IDictionary<string, string>) this._parameters;

    protected WSFederationMessage(Uri baseUrl, string action)
    {
      if (baseUrl == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (baseUrl));
      if (!UriUtil.CanCreateValidUri(baseUrl.AbsoluteUri, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3003")));
      this._parameters["wa"] = !string.IsNullOrEmpty(action) ? action : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (action)));
      this._baseUri = baseUrl;
    }

    public static WSFederationMessage CreateFromUri(Uri requestUri)
    {
      if (requestUri == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestUri));
      WSFederationMessage fedMsg;
      if (WSFederationMessage.TryCreateFromUri(requestUri, out fedMsg))
        return fedMsg;
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3094", (object) requestUri)));
    }

    public static bool TryCreateFromUri(Uri requestUri, out WSFederationMessage fedMsg)
    {
      Uri baseUrl = !(requestUri == (Uri) null) ? WSFederationMessage.GetBaseUrl(requestUri) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestUri));
      fedMsg = WSFederationMessage.CreateFromNameValueCollection(baseUrl, WSFederationMessage.ParseQueryString(requestUri));
      return fedMsg != null;
    }

    public static WSFederationMessage CreateFromFormPost(HttpRequest request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      return WSFederationMessage.CreateFromNameValueCollection(WSFederationMessage.GetBaseUrl(request.Url), request.Form) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3095")));
    }

    public string Context
    {
      get => this.GetParameter("wctx");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wctx");
        else
          this.SetParameter("wctx", value);
      }
    }

    public string Encoding
    {
      get => this.GetParameter("wencoding");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.RemoveParameter("wencoding");
        else
          this.SetParameter("wencoding", value);
      }
    }

    public string Action
    {
      get => this.GetParameter("wa");
      set
      {
        if (string.IsNullOrEmpty(value))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (value)));
        this.SetParameter("wa", value);
      }
    }

    public Uri BaseUri
    {
      get => this._baseUri;
      set
      {
        if (value == (Uri) null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._baseUri = UriUtil.CanCreateValidUri(value.AbsoluteUri, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3003")));
      }
    }

    protected virtual void Validate()
    {
      if (this._baseUri == (Uri) null || !UriUtil.CanCreateValidUri(this._baseUri.AbsoluteUri, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3003")));
    }

    public abstract void Write(TextWriter writer);

    public static WSFederationMessage CreateFromNameValueCollection(
      Uri baseUrl,
      NameValueCollection collection)
    {
      if (baseUrl == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (baseUrl));
      if (collection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (collection));
      WSFederationMessage message = (WSFederationMessage) null;
      string str = collection.Get("wa");
      if (!string.IsNullOrEmpty(str))
      {
        switch (str)
        {
          case "wattr1.0":
            message = (WSFederationMessage) new AttributeRequestMessage(baseUrl);
            break;
          case "wpseudo1.0":
            message = (WSFederationMessage) new PseudonymRequestMessage(baseUrl);
            break;
          case "wsignin1.0":
            string result = collection.Get("wresult");
            string uriString = collection.Get("wresultptr");
            bool flag1 = !string.IsNullOrEmpty(result);
            bool flag2 = !string.IsNullOrEmpty(uriString);
            if (flag1)
            {
              message = (WSFederationMessage) new SignInResponseMessage(baseUrl, result);
              break;
            }
            if (flag2)
            {
              message = (WSFederationMessage) new SignInResponseMessage(baseUrl, new Uri(uriString));
              break;
            }
            string realm = collection.Get("wtrealm");
            string reply = collection.Get("wreply");
            message = (WSFederationMessage) new SignInRequestMessage(baseUrl, realm, reply);
            break;
          case "wsignout1.0":
            message = (WSFederationMessage) new SignOutRequestMessage(baseUrl);
            break;
          case "wsignoutcleanup1.0":
            message = (WSFederationMessage) new SignOutCleanupRequestMessage(baseUrl);
            break;
          default:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3014", (object) str)));
        }
      }
      if (message != null)
      {
        WSFederationMessage.PopulateMessage(message, collection);
        message.Validate();
      }
      return message;
    }

    private static void PopulateMessage(WSFederationMessage message, NameValueCollection collection)
    {
      foreach (string key in collection.Keys)
      {
        if (string.IsNullOrEmpty(collection[key]))
        {
          switch (key)
          {
            case "wtrealm":
            case "wresult":
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3261", (object) key)));
            default:
              continue;
          }
        }
        else
          message.SetParameter(key, collection[key]);
      }
    }

    public static NameValueCollection ParseQueryString(Uri data) => !(data == (Uri) null) ? HttpUtility.ParseQueryString(data.Query) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));

    public string GetParameter(string parameter)
    {
      if (string.IsNullOrEmpty(parameter))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (parameter)));
      string str = (string) null;
      this._parameters.TryGetValue(parameter, out str);
      return str;
    }

    public void SetParameter(string parameter, string value)
    {
      if (string.IsNullOrEmpty(parameter))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (parameter)));
      this._parameters[parameter] = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (value)));
    }

    public void SetUriParameter(string parameter, string value)
    {
      if (string.IsNullOrEmpty(value))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (value)));
      if (!UriUtil.CanCreateValidUri(value, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0013"), nameof (value)));
      this.SetParameter(parameter, value);
    }

    public void RemoveParameter(string parameter)
    {
      if (string.IsNullOrEmpty(parameter))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (parameter)));
      if (!this.Parameters.Keys.Contains(parameter))
        return;
      this._parameters.Remove(parameter);
    }

    public static Uri GetBaseUrl(Uri uri)
    {
      string uriString = !(uri == (Uri) null) ? uri.AbsoluteUri : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (uri));
      int length = uriString.IndexOf("?", 0, StringComparison.Ordinal);
      if (length > -1)
        uriString = uriString.Substring(0, length);
      return new Uri(uriString);
    }

    public virtual string WriteQueryString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this._baseUri.AbsoluteUri);
      stringBuilder.Append("?");
      bool flag = true;
      foreach (KeyValuePair<string, string> parameter in this._parameters)
      {
        if (!flag)
          stringBuilder.Append("&");
        stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}={1}", (object) HttpUtility.UrlEncode(parameter.Key), (object) HttpUtility.UrlEncode(parameter.Value)));
        flag = false;
      }
      return stringBuilder.ToString();
    }

    public virtual string WriteFormPost()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<html><head><title>{0}</title></head><body><form method=\"POST\" name=\"hiddenform\" action=\"{1}\">", (object) Microsoft.IdentityModel.SR.GetString("HtmlPostTitle"), (object) HttpUtility.HtmlAttributeEncode(this._baseUri.AbsoluteUri)));
      foreach (KeyValuePair<string, string> parameter in this._parameters)
        stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", (object) HttpUtility.HtmlAttributeEncode(parameter.Key), (object) HttpUtility.HtmlAttributeEncode(parameter.Value)));
      stringBuilder.Append("<noscript><p>");
      stringBuilder.Append(Microsoft.IdentityModel.SR.GetString("HtmlPostNoScriptMessage"));
      stringBuilder.Append("</p><input type=\"submit\" value=\"");
      stringBuilder.Append(Microsoft.IdentityModel.SR.GetString("HtmlPostNoScriptButtonText"));
      stringBuilder.Append("\" /></noscript>");
      stringBuilder.Append("</form><script language=\"javascript\">window.setTimeout('document.forms[0].submit()', 0);</script></body></html>");
      return stringBuilder.ToString();
    }
  }
}
