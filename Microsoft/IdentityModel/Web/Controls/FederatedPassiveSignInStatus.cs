// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.FederatedPassiveSignInStatus
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  [Bindable(false)]
  [DefaultEvent("SigningOut")]
  [Designer(typeof (SignInStatusDesigner))]
  [ComVisible(true)]
  public class FederatedPassiveSignInStatus : CompositeControl
  {
    private static readonly object EventSigningOut = new object();
    private static readonly object EventSignedOut = new object();
    private static readonly object EventSignOutError = new object();
    private LinkButton _signInLinkButton;
    private ImageButton _signInImageButton;
    private Button _signInPushButton;
    private LinkButton _signOutLinkButton;
    private ImageButton _signOutImageButton;
    private Button _signOutPushButton;
    private bool _loggedIn;
    private ServiceConfiguration _serviceConfiguration;

    public FederatedPassiveSignInStatus() => this._serviceConfiguration = FederatedAuthentication.ServiceConfiguration;

    [WebDescription("SignInStatus_SignedOut")]
    [WebCategory("Category_Action")]
    public event EventHandler SignedOut
    {
      add => this.Events.AddHandler(FederatedPassiveSignInStatus.EventSignedOut, (Delegate) value);
      remove => this.Events.RemoveHandler(FederatedPassiveSignInStatus.EventSignedOut, (Delegate) value);
    }

    [WebCategory("Category_Action")]
    [WebDescription("SignInStatus_SigningOut")]
    public event EventHandler<CancelEventArgs> SigningOut
    {
      add => this.Events.AddHandler(FederatedPassiveSignInStatus.EventSigningOut, (Delegate) value);
      remove => this.Events.RemoveHandler(FederatedPassiveSignInStatus.EventSigningOut, (Delegate) value);
    }

    [WebDescription("SignInStatus_SignOutError")]
    [WebCategory("Category_Action")]
    public event EventHandler<ErrorEventArgs> SignOutError
    {
      add => this.Events.AddHandler(FederatedPassiveSignInStatus.EventSignOutError, (Delegate) value);
      remove => this.Events.RemoveHandler(FederatedPassiveSignInStatus.EventSignOutError, (Delegate) value);
    }

    [WebCategory("Category_Appearance")]
    [WebDescription("SignInStatus_SignInButtonType")]
    [DefaultValue(ButtonType.Link)]
    public virtual ButtonType SignInButtonType
    {
      get => (ButtonType) (this.ViewState[nameof (SignInButtonType)] ?? (object) ButtonType.Link);
      set => this.ViewState[nameof (SignInButtonType)] = value >= ButtonType.Button && value <= ButtonType.Link ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [DefaultValue("")]
    [WebDescription("SignInStatus_SignInImageUrl")]
    [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))]
    [UrlProperty]
    [WebCategory("Category_Appearance")]
    public virtual string SignInImageUrl
    {
      get => (string) this.ViewState[nameof (SignInImageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (SignInImageUrl)] = (object) value;
    }

    [Localizable(true)]
    [WebDescription("SignInStatus_SignInText")]
    [WebCategory("Category_Appearance")]
    [WebDefaultValue("SignInStatus_DefaultSignInText")]
    public virtual string SignInText
    {
      get => (string) this.ViewState[nameof (SignInText)] ?? Microsoft.IdentityModel.SR.GetString("SignInStatus_DefaultSignInText");
      set => this.ViewState[nameof (SignInText)] = (object) value;
    }

    [DefaultValue(SignOutAction.Refresh)]
    [WebDescription("SignInStatus_SignOutAction")]
    [WebCategory("Category_Behavior")]
    [Themeable(false)]
    public virtual SignOutAction SignOutAction
    {
      get => (SignOutAction) (this.ViewState[nameof (SignOutAction)] ?? (object) SignOutAction.Refresh);
      set => this.ViewState[nameof (SignOutAction)] = value >= SignOutAction.Refresh && value <= SignOutAction.FederatedPassiveSignOut ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [DefaultValue("")]
    [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))]
    [UrlProperty]
    [WebCategory("Category_Appearance")]
    [WebDescription("SignInStatus_SignOutImageUrl")]
    public virtual string SignOutImageUrl
    {
      get => (string) this.ViewState[nameof (SignOutImageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (SignOutImageUrl)] = (object) value;
    }

    [UrlProperty]
    [WebCategory("Category_Behavior")]
    [DefaultValue("")]
    [WebDescription("SignInStatus_SignOutPageUrl")]
    [Editor(typeof (UrlEditor), typeof (UITypeEditor))]
    [Themeable(false)]
    public virtual string SignOutPageUrl
    {
      get => (string) this.ViewState[nameof (SignOutPageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (SignOutPageUrl)] = (object) value;
    }

    [WebDescription("SignInStatus_SignOutText")]
    [WebCategory("Category_Appearance")]
    [Localizable(true)]
    [WebDefaultValue("SignInStatus_DefaultSignOutText")]
    public virtual string SignOutText
    {
      get => (string) this.ViewState[nameof (SignOutText)] ?? Microsoft.IdentityModel.SR.GetString("SignInStatus_DefaultSignOutText");
      set => this.ViewState[nameof (SignOutText)] = (object) value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ServiceConfiguration ServiceConfiguration
    {
      get => this._serviceConfiguration;
      set => this._serviceConfiguration = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      if (this.DesignMode)
        return;
      ControlUtil.EnsureSessionAuthenticationModule();
    }

    protected override void CreateChildControls()
    {
      this.Controls.Clear();
      this._signInLinkButton = new LinkButton();
      this._signInLinkButton.ID = "signinLink";
      this._signInImageButton = new ImageButton();
      this._signInImageButton.ID = "signinImage";
      this._signInPushButton = new Button();
      this._signInPushButton.ID = "signinButton";
      this._signOutLinkButton = new LinkButton();
      this._signOutLinkButton.ID = "signoutLink";
      this._signOutImageButton = new ImageButton();
      this._signOutImageButton.ID = "signoutImage";
      this._signOutPushButton = new Button();
      this._signOutPushButton.ID = "signoutButton";
      this._signInLinkButton.EnableViewState = this._signInLinkButton.EnableTheming = this._signInLinkButton.CausesValidation = false;
      this._signInImageButton.EnableViewState = this._signInImageButton.EnableTheming = this._signInImageButton.CausesValidation = false;
      this._signInPushButton.EnableViewState = this._signInPushButton.EnableTheming = this._signInPushButton.CausesValidation = false;
      this._signOutLinkButton.EnableViewState = this._signOutLinkButton.EnableTheming = this._signOutLinkButton.CausesValidation = false;
      this._signOutImageButton.EnableViewState = this._signOutImageButton.EnableTheming = this._signOutImageButton.CausesValidation = false;
      this._signOutPushButton.EnableViewState = this._signOutPushButton.EnableTheming = this._signOutPushButton.CausesValidation = false;
      CommandEventHandler commandEventHandler1 = new CommandEventHandler(this.SignOutClicked);
      this._signOutLinkButton.Command += commandEventHandler1;
      this._signOutImageButton.Command += commandEventHandler1;
      this._signOutPushButton.Command += commandEventHandler1;
      CommandEventHandler commandEventHandler2 = new CommandEventHandler(this.SignInClicked);
      this._signInLinkButton.Command += commandEventHandler2;
      this._signInImageButton.Command += commandEventHandler2;
      this._signInPushButton.Command += commandEventHandler2;
      this.Controls.Add((Control) this._signOutLinkButton);
      this.Controls.Add((Control) this._signOutImageButton);
      this.Controls.Add((Control) this._signOutPushButton);
      this.Controls.Add((Control) this._signInLinkButton);
      this.Controls.Add((Control) this._signInImageButton);
      this.Controls.Add((Control) this._signInPushButton);
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      WSFederationAuthenticationModule authenticationModule = new WSFederationAuthenticationModule();
      authenticationModule.ServiceConfiguration = this.ServiceConfiguration;
      authenticationModule.CanReadSignInResponse(this.Page.Request, true);
      this._loggedIn = this.Page.Request.IsAuthenticated;
    }

    protected override void Render(HtmlTextWriter writer) => this.RenderContents(writer);

    protected override void RenderContents(HtmlTextWriter writer)
    {
      if (this.Page != null)
        this.Page.VerifyRenderingInServerForm((Control) this);
      this.SetChildProperties();
      base.RenderContents(writer);
    }

    private void SetChildProperties()
    {
      this.EnsureChildControls();
      this._signInLinkButton.Visible = this._signInLinkButton.Enabled = false;
      this._signInImageButton.Visible = this._signInImageButton.Enabled = false;
      this._signInPushButton.Visible = this._signInPushButton.Enabled = false;
      this._signOutLinkButton.Visible = this._signOutLinkButton.Enabled = false;
      this._signOutImageButton.Visible = this._signOutImageButton.Enabled = false;
      this._signOutPushButton.Visible = this._signOutPushButton.Enabled = false;
      WebControl webControl = (WebControl) null;
      Type type = this.GetType();
      if (this._loggedIn)
      {
        switch (this.SignInButtonType)
        {
          case ButtonType.Button:
            this._signOutPushButton.Text = this.SignOutText;
            webControl = (WebControl) this._signOutPushButton;
            break;
          case ButtonType.Image:
            this._signOutImageButton.AlternateText = this.SignOutText;
            this._signOutImageButton.ToolTip = this.ToolTip;
            this._signOutImageButton.ImageUrl = !string.IsNullOrEmpty(this.SignOutImageUrl) ? this.SignOutImageUrl : this.Page.ClientScript.GetWebResourceUrl(type, type.FullName + "SignOut.png");
            webControl = (WebControl) this._signOutImageButton;
            break;
          case ButtonType.Link:
            this._signOutLinkButton.Text = this.SignOutText;
            webControl = (WebControl) this._signOutLinkButton;
            break;
        }
      }
      else
      {
        switch (this.SignInButtonType)
        {
          case ButtonType.Button:
            this._signInPushButton.Text = this.SignInText;
            webControl = (WebControl) this._signInPushButton;
            break;
          case ButtonType.Image:
            this._signInImageButton.AlternateText = this.SignInText;
            this._signInImageButton.ToolTip = this.ToolTip;
            this._signInImageButton.ImageUrl = !string.IsNullOrEmpty(this.SignInImageUrl) ? this.SignInImageUrl : this.Page.ClientScript.GetWebResourceUrl(type, type.FullName + "SignIn.png");
            webControl = (WebControl) this._signInImageButton;
            break;
          case ButtonType.Link:
            this._signInLinkButton.Text = this.SignInText;
            webControl = (WebControl) this._signInLinkButton;
            break;
        }
      }
      if (webControl == null)
        return;
      webControl.Visible = webControl.Enabled = true;
      webControl.CopyBaseAttributes((WebControl) this);
      webControl.ApplyStyle(this.ControlStyle);
    }

    private string ResolveSignOutPageUrl(string urlValue)
    {
      string empty = string.Empty;
      if (string.IsNullOrEmpty(urlValue))
        urlValue = FormsAuthentication.LoginUrl;
      string str1;
      if (!UriUtil.CanCreateValidUri(urlValue, UriKind.Absolute))
      {
        string str2 = this.ResolveUrl(urlValue);
        str1 = new UriBuilder(HttpContext.Current.Request.Url)
        {
          Path = str2
        }.Uri.OriginalString;
      }
      else
        str1 = urlValue;
      return str1;
    }

    private void SignOutClicked(object sender, CommandEventArgs e) => this.SignOut();

    private void SignOut()
    {
      try
      {
        CancelEventArgs e = new CancelEventArgs();
        this.OnSigningOut(e);
        if (e.Cancel)
          return;
        try
        {
          FormsAuthentication.SignOut();
        }
        finally
        {
          FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
        }
        this.OnSignedOut(EventArgs.Empty);
      }
      catch (Exception ex)
      {
        if (DiagnosticUtil.IsFatal(ex))
        {
          throw;
        }
        else
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8012", (object) this.ID, (object) ex));
          ErrorEventArgs e = new ErrorEventArgs(true, ex);
          this.OnSignOutError(e);
          if (!e.Cancel)
            throw;
        }
      }
      string relativeUrl = this.SignOutAction != SignOutAction.FederatedPassiveSignOut ? FederatedPassiveSignInStatus.GetSignOutUrl(this.SignOutAction, this.ResolveClientUrl(this.SignOutPageUrl)) : FederatedPassiveSignInStatus.GetSignOutUrl(this.SignOutAction, this.ResolveSignOutPageUrl(this.SignOutPageUrl));
      if (string.IsNullOrEmpty(relativeUrl))
        return;
      this.Page.Response.Redirect(this.ResolveClientUrl(relativeUrl));
    }

    internal static string GetSignOutUrl(SignOutAction signOutAction, string signOutPageUrl) => FederatedPassiveSignInStatus.GetSignOutUrl(HttpContext.Current.Request, signOutAction, signOutPageUrl);

    internal static string GetSignOutUrl(
      HttpRequest request,
      SignOutAction signOutAction,
      string signOutPageUrl)
    {
      string str;
      switch (signOutAction)
      {
        case SignOutAction.Redirect:
          str = string.IsNullOrEmpty(signOutPageUrl) ? FormsAuthentication.LoginUrl : signOutPageUrl;
          break;
        case SignOutAction.RedirectToLoginPage:
          str = FormsAuthentication.LoginUrl;
          break;
        case SignOutAction.FederatedPassiveSignOut:
          str = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl((Uri) null, new Uri(string.IsNullOrEmpty(signOutPageUrl) ? FormsAuthentication.LoginUrl : signOutPageUrl)).AbsoluteUri;
          break;
        default:
          str = !string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) ? request.RawUrl : request.Path;
          break;
      }
      return str;
    }

    private void SignInClicked(object Source, CommandEventArgs e) => this.Page.Response.Redirect(this.ResolveClientUrl(ControlUtil.GetLoginPage(this.Context, (string) null, true)), false);

    protected virtual void OnSigningOut(CancelEventArgs e)
    {
      EventHandler<CancelEventArgs> eventHandler = (EventHandler<CancelEventArgs>) this.Events[FederatedPassiveSignInStatus.EventSigningOut];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSignedOut(EventArgs e)
    {
      EventHandler eventHandler = (EventHandler) this.Events[FederatedPassiveSignInStatus.EventSignedOut];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSignOutError(ErrorEventArgs e)
    {
      EventHandler<ErrorEventArgs> eventHandler = (EventHandler<ErrorEventArgs>) this.Events[FederatedPassiveSignInStatus.EventSignOutError];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected override void SetDesignModeState(IDictionary data)
    {
      if (data == null)
        return;
      object obj = data[(object) "LoggedIn"];
      if (obj == null)
        return;
      this._loggedIn = (bool) obj;
    }
  }
}
