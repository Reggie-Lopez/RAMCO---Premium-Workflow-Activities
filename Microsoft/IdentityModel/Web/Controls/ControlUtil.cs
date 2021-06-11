// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.ControlUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSFederation;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Microsoft.IdentityModel.Web.Controls
{
  internal static class ControlUtil
  {
    public const string ReturnUrl = "ReturnUrl";

    public static bool IsHttps(Uri url) => url.IsAbsoluteUri && StringComparer.OrdinalIgnoreCase.Equals(url.Scheme, Uri.UriSchemeHttps);

    public static bool IsHttps(string urlPath)
    {
      Uri result;
      return UriUtil.TryCreateValidUri(urlPath, UriKind.Absolute, out result) && ControlUtil.IsHttps(result);
    }

    public static void EnsureSessionAuthenticationModule()
    {
      if (FederatedAuthentication.SessionAuthenticationModule == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1060")));
    }

    public static bool OnLoginPage(HttpContext context)
    {
      string loginUrl = FormsAuthentication.LoginUrl;
      if (string.IsNullOrEmpty(loginUrl) || string.IsNullOrEmpty(loginUrl))
        return false;
      int length = loginUrl.IndexOf('?');
      if (length >= 0)
        loginUrl = loginUrl.Substring(0, length);
      string completeLoginUrl = ControlUtil.GetCompleteLoginUrl(loginUrl);
      string path = context.Request.Path;
      if (StringComparer.OrdinalIgnoreCase.Equals(path, completeLoginUrl))
        return true;
      if (completeLoginUrl.IndexOf('%') >= 0)
      {
        string y1 = HttpUtility.UrlDecode(completeLoginUrl);
        if (StringComparer.OrdinalIgnoreCase.Equals(path, y1))
          return true;
        string y2 = HttpUtility.UrlDecode(completeLoginUrl, context.Request.ContentEncoding);
        if (StringComparer.OrdinalIgnoreCase.Equals(path, y2))
          return true;
      }
      return false;
    }

    public static string GetPathAndQuery(WSFederationMessage request)
    {
      StringBuilder sb = new StringBuilder(128);
      using (StringWriter stringWriter = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        request.Write((TextWriter) stringWriter);
        return sb.ToString();
      }
    }

    public static string GetCompleteLoginUrl(string loginUrl)
    {
      if (string.IsNullOrEmpty(loginUrl))
        return string.Empty;
      if (VirtualPathUtility.IsAppRelative(loginUrl))
        loginUrl = VirtualPathUtility.ToAbsolute(loginUrl);
      return loginUrl;
    }

    public static string EnsureEndWithSemiColon(string value)
    {
      if (value != null)
      {
        int length = value.Length;
        if (length > 0 && value[length - 1] != ';')
          return value + ";";
      }
      return value;
    }

    public static bool IsAppRelative(string path)
    {
      HttpRequest request = HttpContext.Current.Request;
      string domainAppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
      return ControlUtil.IsAppRelative(new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port, domainAppVirtualPath.EndsWith("/", StringComparison.Ordinal) ? domainAppVirtualPath : domainAppVirtualPath + "/").Uri, path);
    }

    public static bool IsAppRelative(Uri basePath, string path)
    {
      Uri result;
      if (!UriUtil.TryCreateValidUri(path, UriKind.RelativeOrAbsolute, out result))
        return false;
      Uri uri;
      if (result.IsAbsoluteUri)
        uri = new UriBuilder(result)
        {
          Scheme = basePath.Scheme,
          Port = basePath.Port
        }.Uri;
      else
        uri = new Uri(basePath, result);
      return uri.AbsoluteUri.StartsWith(basePath.AbsoluteUri, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsDangerousUrl(string s)
    {
      if (string.IsNullOrEmpty(s))
        return false;
      Uri result;
      if (!UriUtil.TryCreateValidUri(s, UriKind.RelativeOrAbsolute, out result))
        return true;
      return result.IsAbsoluteUri && !(result.Scheme == Uri.UriSchemeHttp) && !(result.Scheme == Uri.UriSchemeHttps);
    }

    public static void CopyBaseAttributesToInnerControl(WebControl control, WebControl child)
    {
      short tabIndex = control.TabIndex;
      string accessKey = control.AccessKey;
      try
      {
        control.AccessKey = string.Empty;
        control.TabIndex = (short) 0;
        child.CopyBaseAttributes(control);
      }
      finally
      {
        control.TabIndex = tabIndex;
        control.AccessKey = accessKey;
      }
    }

    public static void SetTableCellStyle(System.Web.UI.Control control, Style style) => ((WebControl) control.Parent)?.ApplyStyle(style);

    public static void SetTableCellVisible(System.Web.UI.Control control, bool visible)
    {
      System.Web.UI.Control parent = control.Parent;
      if (parent == null)
        return;
      parent.Visible = visible;
    }

    public static void CopyBorderStyles(WebControl control, Style style)
    {
      if (style == null || style.IsEmpty)
        return;
      control.BorderStyle = style.BorderStyle;
      control.BorderColor = style.BorderColor;
      control.BorderWidth = style.BorderWidth;
      control.BackColor = style.BackColor;
      control.CssClass = style.CssClass;
    }

    public static void CopyStyleToInnerControl(WebControl control, Style style)
    {
      if (style == null || style.IsEmpty)
        return;
      control.ForeColor = style.ForeColor;
      control.Font.CopyFrom(style.Font);
    }

    public static Table CreateChildTable(bool convertingToTemplate) => convertingToTemplate ? new Table() : (Table) new ControlUtil.ChildTable(2);

    public static bool EnsureCrossAppRedirect(string toUri, HttpContext context, bool throwIfFail)
    {
      if (string.IsNullOrEmpty(toUri) || FormsAuthentication.EnableCrossAppRedirects || ControlUtil.IsPathOnSameServer(toUri, context.Request.Url))
        return true;
      if (throwIfFail)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5015", (object) toUri)));
      return false;
    }

    internal static bool IsPathOnSameServer(string absUriOrLocalPath, Uri currentRequestUri)
    {
      if (UriUtil.CanCreateValidUri(absUriOrLocalPath, UriKind.Relative))
        return true;
      Uri result;
      if (!UriUtil.TryCreateValidUri(absUriOrLocalPath, UriKind.Absolute, out result))
        return false;
      return result.IsLoopback || string.Equals(currentRequestUri.Host, result.Host, StringComparison.OrdinalIgnoreCase);
    }

    public static string GetLoginPage(
      HttpContext context,
      string extraQueryString,
      bool reuseReturnUrl)
    {
      string strUrl = FormsAuthentication.LoginUrl;
      if (strUrl.IndexOf('?') >= 0)
        strUrl = ControlUtil.RemoveQueryStringVariableFromUrl(strUrl, "ReturnUrl");
      int num = strUrl.IndexOf('?');
      if (num < 0)
        strUrl += "?";
      else if (num < strUrl.Length - 1)
        strUrl += "&";
      string str1 = (string) null;
      if (reuseReturnUrl)
      {
        Encoding contentEncoding = context.Request.ContentEncoding;
        Encoding e = contentEncoding.Equals((object) Encoding.Unicode) ? Encoding.UTF8 : contentEncoding;
        str1 = HttpUtility.UrlEncode(ControlUtil.GetReturnUrl(context, false), e);
      }
      if (str1 == null)
        str1 = HttpUtility.UrlEncode(context.Request.Url.PathAndQuery, context.Request.ContentEncoding);
      string str2 = strUrl + "ReturnUrl=" + str1;
      if (!string.IsNullOrEmpty(extraQueryString))
        str2 = str2 + "&" + extraQueryString;
      return str2;
    }

    public static string RemoveQueryStringVariableFromUrl(string strUrl, string QSVar)
    {
      int posQ = strUrl.IndexOf('?');
      if (posQ < 0)
        return strUrl;
      string sep1 = "&";
      string str1 = "?";
      string token1 = sep1 + QSVar + "=";
      ControlUtil.RemoveQSVar(ref strUrl, posQ, token1, sep1, sep1.Length);
      string token2 = str1 + QSVar + "=";
      ControlUtil.RemoveQSVar(ref strUrl, posQ, token2, sep1, str1.Length);
      string sep2 = HttpUtility.UrlEncode("&");
      string str2 = HttpUtility.UrlEncode("?");
      string token3 = sep2 + HttpUtility.UrlEncode(QSVar + "=");
      ControlUtil.RemoveQSVar(ref strUrl, posQ, token3, sep2, sep2.Length);
      string token4 = str2 + HttpUtility.UrlEncode(QSVar + "=");
      ControlUtil.RemoveQSVar(ref strUrl, posQ, token4, sep2, str2.Length);
      return strUrl;
    }

    internal static string GetReturnUrl(HttpContext context, bool useDefaultIfAbsent)
    {
      string str = context.Request.QueryString["ReturnUrl"];
      if (str == null)
      {
        str = context.Request.Form["ReturnUrl"];
        if (!string.IsNullOrEmpty(str) && !str.Contains("/") && str.Contains("%"))
          str = HttpUtility.UrlDecode(str);
      }
      if (!ControlUtil.EnsureCrossAppRedirect(str, context, false))
        str = (string) null;
      if (!string.IsNullOrEmpty(str) && ControlUtil.IsDangerousUrl(str))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new HttpException(Microsoft.IdentityModel.SR.GetString("ID5007")));
      return str != null || !useDefaultIfAbsent ? str : FormsAuthentication.DefaultUrl;
    }

    public static void RemoveQSVar(
      ref string strUrl,
      int posQ,
      string token,
      string sep,
      int lenAtStartToLeave)
    {
      for (int length = strUrl.LastIndexOf(token, StringComparison.Ordinal); length >= posQ; length = strUrl.LastIndexOf(token, StringComparison.Ordinal))
      {
        int startIndex = strUrl.IndexOf(sep, length + token.Length, StringComparison.Ordinal) + sep.Length;
        strUrl = startIndex < sep.Length || startIndex >= strUrl.Length ? strUrl.Substring(0, length) : strUrl.Substring(0, length + lenAtStartToLeave) + strUrl.Substring(startIndex);
      }
    }

    public static DialogResult MessageBoxError(string text, string caption)
    {
      MessageBoxOptions options = (MessageBoxOptions) 0;
      if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
        options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
      return MessageBox.Show((IWin32Window) null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, options);
    }

    internal static void EnsureAutoSignInNotSetOnMultipleControls(Page page)
    {
      bool flag = false;
      foreach (System.Web.UI.Control control in page.Form.Controls)
      {
        if (control is SignInControl signInControl && signInControl.AutoSignIn)
          flag = !flag ? true : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5022")));
      }
    }

    internal class DisappearingTableRow : TableRow
    {
      protected override void Render(HtmlTextWriter writer)
      {
        bool flag = false;
        foreach (System.Web.UI.Control cell in this.Cells)
        {
          if (cell.Visible)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
        base.Render(writer);
      }
    }

    private class ChildTable : Table
    {
      private int _parentLevel;

      internal ChildTable()
        : this(1)
      {
      }

      internal ChildTable(int parentLevel) => this._parentLevel = parentLevel;

      protected override void AddAttributesToRender(HtmlTextWriter writer)
      {
        base.AddAttributesToRender(writer);
        string parentId = this.GetParentID();
        if (parentId == null)
          return;
        writer.AddAttribute(HtmlTextWriterAttribute.Id, parentId);
      }

      private string GetParentID()
      {
        if (this.ID != null)
          return (string) null;
        System.Web.UI.Control control = (System.Web.UI.Control) this;
        for (int index = 0; index < this._parentLevel; ++index)
        {
          control = control.Parent;
          if (control == null)
            break;
        }
        return control != null && !string.IsNullOrEmpty(control.ID) ? control.ClientID : (string) null;
      }
    }
  }
}
