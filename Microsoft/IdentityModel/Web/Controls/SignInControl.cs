// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.SignInControl
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  [ComVisible(true)]
  public abstract class SignInControl : CompositeControl
  {
    private const string ErrorParameterName = "signinfailure";
    private const int ViewStateArrayLength = 5;
    private static readonly object EventRedirectingToIdentityProvider = new object();
    private static readonly object EventSecurityTokenReceived = new object();
    private static readonly object EventSecurityTokenValidated = new object();
    private static readonly object EventSessionSecurityTokenCreated = new object();
    private static readonly object EventSignedIn = new object();
    private static readonly object EventSignInError = new object();
    private TableItemStyle _errorTextStyle;
    private TableItemStyle _titleTextStyle;
    private TableItemStyle _checkBoxStyle;
    private Style _signInButtonStyle;
    private SignInControl.SignInContainer _templateContainer;
    private bool _convertingToTemplate;
    private bool _renderDesignerRegion;
    private IButtonControl _signInButton;
    private ServiceConfiguration _serviceConfiguration;

    protected IButtonControl SignInButton
    {
      get => this._signInButton;
      set => this._signInButton = value;
    }

    protected SignInControl() => this._serviceConfiguration = FederatedAuthentication.ServiceConfiguration;

    [WebCategory("Category_Action")]
    [WebDescription("SignIn_RedirectingToIdentityProvider")]
    public event EventHandler<RedirectingToIdentityProviderEventArgs> RedirectingToIdentityProvider
    {
      add => this.Events.AddHandler(SignInControl.EventRedirectingToIdentityProvider, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventRedirectingToIdentityProvider, (Delegate) value);
    }

    [WebCategory("Category_Action")]
    [WebDescription("SignIn_TokenReceived")]
    public event EventHandler<SecurityTokenReceivedEventArgs> SecurityTokenReceived
    {
      add => this.Events.AddHandler(SignInControl.EventSecurityTokenReceived, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventSecurityTokenReceived, (Delegate) value);
    }

    [WebDescription("SignIn_TokenValidated")]
    [WebCategory("Category_Action")]
    public event EventHandler<SecurityTokenValidatedEventArgs> SecurityTokenValidated
    {
      add => this.Events.AddHandler(SignInControl.EventSecurityTokenValidated, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventSecurityTokenValidated, (Delegate) value);
    }

    [WebCategory("Category_Action")]
    [WebDescription("SignIn_SessionTokenCreated")]
    public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated
    {
      add => this.Events.AddHandler(SignInControl.EventSessionSecurityTokenCreated, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventSessionSecurityTokenCreated, (Delegate) value);
    }

    [WebDescription("SignIn_SignedIn")]
    [WebCategory("Category_Action")]
    public event EventHandler SignedIn
    {
      add => this.Events.AddHandler(SignInControl.EventSignedIn, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventSignedIn, (Delegate) value);
    }

    [WebCategory("Category_Action")]
    [WebDescription("SignIn_SignInError")]
    public event EventHandler<ErrorEventArgs> SignInError
    {
      add => this.Events.AddHandler(SignInControl.EventSignInError, (Delegate) value);
      remove => this.Events.RemoveHandler(SignInControl.EventSignInError, (Delegate) value);
    }

    [WebCategory("Category_Behavior")]
    [Themeable(false)]
    [WebDescription("SignIn_AutoSignIn")]
    [DefaultValue(false)]
    public virtual bool AutoSignIn
    {
      get => (bool) (this.ViewState[nameof (AutoSignIn)] ?? (object) false);
      set => this.ViewState[nameof (AutoSignIn)] = (object) value;
    }

    [WebCategory("Category_Appearance")]
    [WebDescription("SignIn_BorderPadding")]
    [DefaultValue(1)]
    public virtual int BorderPadding
    {
      get => (int) (this.ViewState[nameof (BorderPadding)] ?? (object) 1);
      set => this.ViewState[nameof (BorderPadding)] = value >= -1 ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [UrlProperty]
    [DefaultValue("")]
    [WebDescription("SignIn_DestinationPageUrl")]
    [WebCategory("Category_Behavior")]
    [Editor(typeof (UrlEditor), typeof (UITypeEditor))]
    [Themeable(false)]
    public virtual string DestinationPageUrl
    {
      get => (string) this.ViewState[nameof (DestinationPageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (DestinationPageUrl)] = (object) value;
    }

    [WebDescription("SignIn_Orientation")]
    [WebCategory("Category_Layout")]
    [DefaultValue(Orientation.Vertical)]
    public virtual Orientation Orientation
    {
      get => (Orientation) (this.ViewState[nameof (Orientation)] ?? (object) Orientation.Vertical);
      set
      {
        this.ViewState[nameof (Orientation)] = value >= Orientation.Horizontal && value <= Orientation.Vertical ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
        this.ChildControlsCreated = false;
      }
    }

    [DefaultValue(LoginFailureAction.Refresh)]
    [WebDescription("SignIn_ErrorAction")]
    [WebCategory("Category_Behavior")]
    [Themeable(false)]
    public virtual LoginFailureAction ErrorAction
    {
      get => (LoginFailureAction) (this.ViewState[nameof (ErrorAction)] ?? (object) LoginFailureAction.Refresh);
      set => this.ViewState[nameof (ErrorAction)] = value >= LoginFailureAction.Refresh && value <= LoginFailureAction.RedirectToLoginPage ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [WebDescription("SignIn_ErrorText")]
    [WebCategory("Category_Appearance")]
    [WebDefaultValue("SignIn_DefaultErrorText")]
    [Localizable(true)]
    public virtual string ErrorText
    {
      get => (string) this.ViewState[nameof (ErrorText)] ?? Microsoft.IdentityModel.SR.GetString("SignIn_DefaultErrorText");
      set => this.ViewState[nameof (ErrorText)] = (object) value;
    }

    [DefaultValue(null)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [WebDescription("SignIn_ErrorTextStyle")]
    [WebCategory("Category_Styles")]
    public TableItemStyle ErrorTextStyle
    {
      get
      {
        if (this._errorTextStyle == null)
        {
          this._errorTextStyle = (TableItemStyle) new ErrorTableItemStyle();
          if (this.IsTrackingViewState)
            ((IStateManager) this._errorTextStyle).TrackViewState();
        }
        return this._errorTextStyle;
      }
    }

    [WebDescription("SignIn_SignInButtonStyle")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [WebCategory("Category_Styles")]
    [DefaultValue(null)]
    public Style SignInButtonStyle
    {
      get
      {
        if (this._signInButtonStyle == null)
        {
          this._signInButtonStyle = new Style();
          if (this.IsTrackingViewState)
            ((IStateManager) this._signInButtonStyle).TrackViewState();
        }
        return this._signInButtonStyle;
      }
    }

    [WebCategory("Category_Appearance")]
    [WebDescription("SignIn_SignInImageUrl")]
    [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))]
    [UrlProperty]
    [DefaultValue("")]
    public virtual string SignInImageUrl
    {
      get => (string) this.ViewState[nameof (SignInImageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (SignInImageUrl)] = (object) value;
    }

    [WebDefaultValue("SignIn_DefaultSignInText")]
    [Localizable(true)]
    [WebDescription("SignIn_SignInText")]
    [WebCategory("Category_Appearance")]
    public virtual string SignInText
    {
      get => (string) this.ViewState[nameof (SignInText)] ?? Microsoft.IdentityModel.SR.GetString("SignIn_DefaultSignInText");
      set => this.ViewState[nameof (SignInText)] = (object) value;
    }

    [WebCategory("Category_Appearance")]
    [WebDescription("SignIn_SignInButtonType")]
    [DefaultValue(ButtonType.Image)]
    public virtual ButtonType SignInButtonType
    {
      get => (ButtonType) (this.ViewState[nameof (SignInButtonType)] ?? (object) ButtonType.Image);
      set => this.ViewState[nameof (SignInButtonType)] = value >= ButtonType.Button && value <= ButtonType.Link ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [WebCategory("Category_Appearance")]
    [DefaultValue(true)]
    [WebDescription("SignIn_ShowButtonImage")]
    public virtual bool ShowButtonImage
    {
      get => (bool) (this.ViewState[nameof (ShowButtonImage)] ?? (object) true);
      set => this.ViewState[nameof (ShowButtonImage)] = (object) value;
    }

    [WebCategory("Category_Behavior")]
    [WebDescription("SignIn_SignInContext")]
    [DefaultValue("")]
    public string SignInContext
    {
      get => (string) this.ViewState[nameof (SignInContext)] ?? string.Empty;
      set => this.ViewState[nameof (SignInContext)] = (object) value;
    }

    [WebDescription("SignIn_TitleText")]
    [WebCategory("Category_Appearance")]
    [Localizable(true)]
    [WebDefaultValue("SignIn_DefaultTitleText")]
    public virtual string TitleText
    {
      get => (string) this.ViewState[nameof (TitleText)] ?? Microsoft.IdentityModel.SR.GetString("SignIn_DefaultTitleText");
      set => this.ViewState[nameof (TitleText)] = (object) value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [WebCategory("Category_Styles")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [WebDescription("SignIn_TitleTextStyle")]
    [DefaultValue(null)]
    public TableItemStyle TitleTextStyle
    {
      get
      {
        if (this._titleTextStyle == null)
        {
          this._titleTextStyle = new TableItemStyle();
          if (this.IsTrackingViewState)
            ((IStateManager) this._titleTextStyle).TrackViewState();
        }
        return this._titleTextStyle;
      }
    }

    [Themeable(false)]
    [DefaultValue(true)]
    [WebDescription("SignIn_DisplayRememberMe")]
    [WebCategory("Category_Behavior")]
    public virtual bool DisplayRememberMe
    {
      get => (bool) (this.ViewState[nameof (DisplayRememberMe)] ?? (object) true);
      set => this.ViewState[nameof (DisplayRememberMe)] = (object) value;
    }

    [WebDescription("SignIn_RememberMeSet")]
    [DefaultValue(false)]
    [WebCategory("Category_Behavior")]
    [Themeable(false)]
    public virtual bool RememberMeSet
    {
      get => (bool) (this.ViewState[nameof (RememberMeSet)] ?? (object) false);
      set => this.ViewState[nameof (RememberMeSet)] = (object) value;
    }

    [WebCategory("Category_Appearance")]
    [WebDefaultValue("SignIn_DefaultRememberMeText")]
    [WebDescription("SignIn_RememberMeText")]
    [Localizable(true)]
    public virtual string RememberMeText
    {
      get => (string) this.ViewState[nameof (RememberMeText)] ?? Microsoft.IdentityModel.SR.GetString("SignIn_DefaultRememberMeText");
      set => this.ViewState[nameof (RememberMeText)] = (object) value;
    }

    [DefaultValue(null)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [WebDescription("SignIn_CheckBoxStyle")]
    [WebCategory("Category_Styles")]
    public TableItemStyle CheckBoxStyle
    {
      get
      {
        if (this._checkBoxStyle == null)
        {
          this._checkBoxStyle = new TableItemStyle();
          if (this.IsTrackingViewState)
            ((IStateManager) this._checkBoxStyle).TrackViewState();
        }
        return this._checkBoxStyle;
      }
    }

    [WebCategory("Category_Behavior")]
    [Themeable(false)]
    [WebDescription("SignIn_VisibleWhenSignedIn")]
    [DefaultValue(true)]
    public virtual bool VisibleWhenSignedIn
    {
      get => (bool) (this.ViewState[nameof (VisibleWhenSignedIn)] ?? (object) true);
      set => this.ViewState[nameof (VisibleWhenSignedIn)] = (object) value;
    }

    [WebDescription("SignIn_RequireHttps")]
    [DefaultValue(true)]
    [WebCategory("Category_Behavior")]
    public bool RequireHttps
    {
      get => (bool) (this.ViewState[nameof (RequireHttps)] ?? (object) true);
      set => this.ViewState[nameof (RequireHttps)] = (object) value;
    }

    [DefaultValue(SignInMode.Session)]
    [Themeable(false)]
    [WebDescription("SignIn_SignInMode")]
    [WebCategory("Category_Behavior")]
    public virtual SignInMode SignInMode
    {
      get => (SignInMode) (this.ViewState[nameof (SignInMode)] ?? (object) SignInMode.Session);
      set => this.ViewState[nameof (SignInMode)] = SignInModeHelper.IsDefined(value) ? (object) value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ServiceConfiguration ServiceConfiguration
    {
      get => this._serviceConfiguration;
      set => this._serviceConfiguration = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    protected string ButtonClientId => this._templateContainer.ActiveButton.ClientID;

    protected string RememberMeClientId => this._templateContainer.RememberMeCheckBox.ClientID;

    protected event EventHandler Click;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    private SignInControl.SignInContainer TemplateContainer
    {
      get
      {
        this.EnsureChildControls();
        return this._templateContainer;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string ClientSideSignInFunction => this.UniqueID + "OnClick";

    private bool ConvertingToTemplate => this.DesignMode && this._convertingToTemplate;

    protected WebControl ActiveButton => this.TemplateContainer.ActiveButton;

    protected abstract bool SignIn();

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      if (!this.DesignMode)
        ControlUtil.EnsureSessionAuthenticationModule();
      switch (this.SignInButtonType)
      {
        case ButtonType.Image:
          this.SignInButton = (IButtonControl) new ImageButton();
          this.SignInButton.Click += new EventHandler(this.SignInButton_Click);
          break;
        case ButtonType.Link:
          this.SignInButton = (IButtonControl) new LinkButton();
          this.SignInButton.Click += new EventHandler(this.SignInButton_Click);
          break;
      }
    }

    private void SignInButton_Click(object sender, EventArgs e)
    {
      if (this.Click == null)
        return;
      this.Click((object) this, e);
    }

    protected override void CreateChildControls()
    {
      this.Controls.Clear();
      this._templateContainer = new SignInControl.SignInContainer(this);
      this._templateContainer.RenderDesignerRegion = this._renderDesignerRegion;
      this._templateContainer.EnableViewState = false;
      this._templateContainer.EnableTheming = false;
      switch (this.SignInButtonType)
      {
        case ButtonType.Button:
          this.SignInButton = (IButtonControl) new Button();
          break;
        case ButtonType.Image:
          this.SignInButton = (IButtonControl) new ImageButton();
          this.SignInButton.Click += new EventHandler(this.SignInButton_Click);
          break;
        case ButtonType.Link:
          this.SignInButton = (IButtonControl) new LinkButton();
          this.SignInButton.Click += new EventHandler(this.SignInButton_Click);
          break;
      }
      ((ITemplate) new SignInControl.SignInTemplate(this, this.SignInButton)).InstantiateIn((Control) this._templateContainer);
      this._templateContainer.Visible = true;
      this.Controls.Add((Control) this._templateContainer);
    }

    protected override void OnPreRender(EventArgs e)
    {
      if (!this.DesignMode)
        ControlUtil.EnsureAutoSignInNotSetOnMultipleControls(this.Page);
      if (this.Page.IsPostBack && this.Page.Request.Form["__EVENTTARGET"] == this.UniqueID)
        this.SignInButton_Click((object) this, EventArgs.Empty);
      base.OnPreRender(e);
      try
      {
        if (this.SignIn())
        {
          if (this.SignInMode == SignInMode.Session)
          {
            string redirectUrl = this.GetRedirectUrl();
            if (ControlUtil.IsAppRelative(redirectUrl))
              this.Page.Response.Redirect(CookieHandler.MatchCookiePath(redirectUrl));
            else
              throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new FederationException(Microsoft.IdentityModel.SR.GetString("ID3206", (object) redirectUrl)), TraceEventType.Error);
          }
        }
      }
      catch (Exception ex)
      {
        if (!this.HandleSignInException(ex))
          throw;
      }
      this.TemplateContainer.Visible = this.VisibleWhenSignedIn || !this.Page.Request.IsAuthenticated;
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (this.Page != null)
        this.Page.VerifyRenderingInServerForm((Control) this);
      if (this.DesignMode)
      {
        this.ChildControlsCreated = false;
        this.EnsureChildControls();
      }
      if (this.TemplateContainer.Visible)
      {
        this.SetChildProperties();
        this.RenderContents(writer);
      }
      if (this.DesignMode)
        return;
      if (this.TemplateContainer.Visible)
        writer.Write(this.GetClientScript());
      if (!this.Enabled || !this.AutoSignIn || !string.IsNullOrEmpty(((ITextControl) this.TemplateContainer.ErrorTextLabel).Text))
        return;
      writer.Write(this.GetAutoSignInScript());
    }

    private void SetChildProperties()
    {
      this.SetCommonChildProperties();
      this.SetDefaultTemplateChildProperties();
    }

    private void SetCommonChildProperties()
    {
      SignInControl.SignInContainer templateContainer = this.TemplateContainer;
      ControlUtil.CopyBaseAttributesToInnerControl((WebControl) this, (WebControl) templateContainer);
      templateContainer.ApplyStyle(this.ControlStyle);
      ITextControl errorTextLabel = (ITextControl) templateContainer.ErrorTextLabel;
      string errorText = this.ErrorText;
      if (errorTextLabel == null || errorText.Length <= 0 || !this.RedirectedFromFailedLogin())
        return;
      errorTextLabel.Text = errorText;
    }

    private void SetDefaultTemplateChildProperties()
    {
      SignInControl.SignInContainer templateContainer = this.TemplateContainer;
      templateContainer.BorderTable.CellPadding = this.BorderPadding;
      templateContainer.BorderTable.CellSpacing = 0;
      Literal title = templateContainer.Title;
      string titleText = this.TitleText;
      if (titleText.Length > 0)
      {
        title.Text = titleText;
        if (this._titleTextStyle != null)
          ControlUtil.SetTableCellStyle((Control) title, (Style) this.TitleTextStyle);
        ControlUtil.SetTableCellVisible((Control) title, true);
      }
      else
        ControlUtil.SetTableCellVisible((Control) title, false);
      WebControl rememberMeCheckBox = templateContainer.RememberMeCheckBox;
      if (this.DisplayRememberMe)
      {
        if (rememberMeCheckBox is CheckBox checkBox)
          checkBox.Text = this.RememberMeText;
        else
          ((SimpleCheckBox) rememberMeCheckBox).Text = this.RememberMeText;
        ((ICheckBoxControl) rememberMeCheckBox).Checked = this.RememberMeSet;
        if (this._checkBoxStyle != null)
          ControlUtil.SetTableCellStyle((Control) rememberMeCheckBox, (Style) this.CheckBoxStyle);
        ControlUtil.SetTableCellVisible((Control) rememberMeCheckBox, true);
      }
      else
        ControlUtil.SetTableCellVisible((Control) rememberMeCheckBox, false);
      rememberMeCheckBox.TabIndex = this.TabIndex;
      Type type = this.GetType();
      bool flag1 = this.SignInButtonType == ButtonType.Image;
      string str = (string) null;
      if (!this.DesignMode && !string.IsNullOrEmpty(this.GetClientScript()))
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "javascript:{0}({1});", (object) this.ClientSideSignInFunction, flag1 ? (object) "true" : (object) string.Empty);
      switch (this.SignInButtonType)
      {
        case ButtonType.Button:
          SimpleButton activeButton1 = (SimpleButton) templateContainer.ActiveButton;
          activeButton1.Text = this.SignInText;
          activeButton1.OnClientClick = this.Click == null ? str : string.Format("javascript:__doPostBack('{0}');", (object) this.UniqueID);
          if (this.ShowButtonImage)
          {
            activeButton1.ImageUrl = !string.IsNullOrEmpty(this.SignInImageUrl) ? this.SignInImageUrl : this.Page.ClientScript.GetWebResourceUrl(type, type.FullName + "Button.png");
            break;
          }
          break;
        case ButtonType.Image:
          ImageButton activeButton2 = (ImageButton) templateContainer.ActiveButton;
          activeButton2.ImageUrl = !string.IsNullOrEmpty(this.SignInImageUrl) ? this.SignInImageUrl : this.Page.ClientScript.GetWebResourceUrl(type, type.FullName + ".png");
          activeButton2.AlternateText = this.SignInText;
          activeButton2.ToolTip = this.ToolTip;
          if (!string.IsNullOrEmpty(str))
          {
            activeButton2.OnClientClick = str;
            break;
          }
          break;
        case ButtonType.Link:
          LinkButton activeButton3 = (LinkButton) templateContainer.ActiveButton;
          activeButton3.Text = this.SignInText;
          if (!string.IsNullOrEmpty(str))
          {
            activeButton3.OnClientClick = str;
            break;
          }
          break;
      }
      templateContainer.ActiveButton.TabIndex = this.TabIndex;
      templateContainer.ActiveButton.AccessKey = this.AccessKey;
      if (this._signInButtonStyle != null)
        templateContainer.ActiveButton.ApplyStyle(this.SignInButtonStyle);
      Image createUserIcon = templateContainer.CreateUserIcon;
      HyperLink createUserLink = templateContainer.CreateUserLink;
      LiteralControl userLinkSeparator = templateContainer.CreateUserLinkSeparator;
      HyperLink helpPageLink = templateContainer.HelpPageLink;
      Image helpPageIcon = templateContainer.HelpPageIcon;
      LiteralControl literalControl = new LiteralControl("|");
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = flag3 || flag4;
      bool flag7 = flag2 || flag5;
      helpPageLink.Visible = flag3;
      literalControl.Visible = flag6 && flag7;
      helpPageIcon.Visible = flag4;
      createUserLink.Visible = flag2;
      userLinkSeparator.Visible = flag7;
      createUserIcon.Visible = flag5;
      if (flag7 || flag6)
        ControlUtil.SetTableCellVisible((Control) helpPageLink, true);
      else
        ControlUtil.SetTableCellVisible((Control) helpPageLink, false);
      Control errorTextLabel = templateContainer.ErrorTextLabel;
      if (((ITextControl) errorTextLabel).Text.Length > 0)
      {
        ControlUtil.SetTableCellStyle(errorTextLabel, (Style) this.ErrorTextStyle);
        ControlUtil.SetTableCellVisible(errorTextLabel, true);
      }
      else
        ControlUtil.SetTableCellVisible(errorTextLabel, false);
    }

    protected virtual bool IsPersistentCookie => ((ICheckBoxControl) this._templateContainer.RememberMeCheckBox).Checked;

    protected virtual string GetClientScript() => string.Empty;

    protected virtual string GetAutoSignInScript() => string.Empty;

    protected virtual string GetReturnUrl() => ControlUtil.GetReturnUrl(this.Context, false);

    protected virtual string GetSessionTokenContext() => (string) null;

    protected virtual void OnRedirectingToIdentityProvider(RedirectingToIdentityProviderEventArgs e)
    {
      EventHandler<RedirectingToIdentityProviderEventArgs> eventHandler = (EventHandler<RedirectingToIdentityProviderEventArgs>) this.Events[SignInControl.EventRedirectingToIdentityProvider];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSecurityTokenReceived(SecurityTokenReceivedEventArgs e)
    {
      EventHandler<SecurityTokenReceivedEventArgs> eventHandler = (EventHandler<SecurityTokenReceivedEventArgs>) this.Events[SignInControl.EventSecurityTokenReceived];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSecurityTokenValidated(SecurityTokenValidatedEventArgs e)
    {
      EventHandler<SecurityTokenValidatedEventArgs> eventHandler = (EventHandler<SecurityTokenValidatedEventArgs>) this.Events[SignInControl.EventSecurityTokenValidated];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs e)
    {
      EventHandler<SessionSecurityTokenCreatedEventArgs> eventHandler = (EventHandler<SessionSecurityTokenCreatedEventArgs>) this.Events[SignInControl.EventSessionSecurityTokenCreated];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSignedIn(EventArgs e)
    {
      EventHandler eventHandler = (EventHandler) this.Events[SignInControl.EventSignedIn];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    protected virtual void OnSignInError(ErrorEventArgs e)
    {
      EventHandler<ErrorEventArgs> eventHandler = (EventHandler<ErrorEventArgs>) this.Events[SignInControl.EventSignInError];
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    private bool HandleSignInException(Exception exception)
    {
      if (DiagnosticUtil.IsFatal(exception))
        return false;
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8013", (object) this.ID, (object) exception));
      ErrorEventArgs e = new ErrorEventArgs(true, exception);
      this.OnSignInError(e);
      if (!e.Cancel)
        return false;
      ITextControl errorTextLabel = (ITextControl) this.TemplateContainer.ErrorTextLabel;
      if (errorTextLabel != null)
        errorTextLabel.Text = this.ErrorText;
      return true;
    }

    private string GetRedirectUrl()
    {
      if (ControlUtil.OnLoginPage(this.Context) || this.AutoSignIn)
      {
        string returnUrl = this.GetReturnUrl();
        if (!string.IsNullOrEmpty(returnUrl))
          return returnUrl;
        string destinationPageUrl = this.DestinationPageUrl;
        return !string.IsNullOrEmpty(destinationPageUrl) ? this.ResolveClientUrl(destinationPageUrl) : FormsAuthentication.DefaultUrl;
      }
      string destinationPageUrl1 = this.DestinationPageUrl;
      if (!string.IsNullOrEmpty(destinationPageUrl1))
        return this.ResolveClientUrl(destinationPageUrl1);
      return this.Page.Form != null && string.Equals(this.Page.Form.Method, "get", StringComparison.OrdinalIgnoreCase) ? this.Page.Request.Path : this.Page.Request.RawUrl;
    }

    protected override void SetDesignModeState(IDictionary data)
    {
      if (data == null)
        return;
      object obj1 = data[(object) "ConvertToTemplate"];
      if (obj1 != null)
        this._convertingToTemplate = (bool) obj1;
      object obj2 = data[(object) "RegionEditing"];
      if (obj2 == null)
        return;
      this._renderDesignerRegion = (bool) obj2;
    }

    protected override void LoadViewState(object savedState)
    {
      if (savedState == null)
      {
        base.LoadViewState(savedState);
      }
      else
      {
        object[] objArray = (object[]) savedState;
        base.LoadViewState(objArray[0]);
        if (objArray[1] != null)
          ((IStateManager) this.SignInButtonStyle).LoadViewState(objArray[1]);
        if (objArray[2] != null)
          ((IStateManager) this.TitleTextStyle).LoadViewState(objArray[3]);
        if (objArray[3] != null)
          ((IStateManager) this.ErrorTextStyle).LoadViewState(objArray[4]);
        if (objArray[4] == null)
          return;
        ((IStateManager) this.CheckBoxStyle).LoadViewState(objArray[5]);
      }
    }

    protected override object SaveViewState()
    {
      object[] objArray = new object[5]
      {
        base.SaveViewState(),
        this._signInButtonStyle != null ? ((IStateManager) this._signInButtonStyle).SaveViewState() : (object) null,
        this._titleTextStyle != null ? ((IStateManager) this._titleTextStyle).SaveViewState() : (object) null,
        this._errorTextStyle != null ? ((IStateManager) this._errorTextStyle).SaveViewState() : (object) null,
        this._checkBoxStyle != null ? ((IStateManager) this._checkBoxStyle).SaveViewState() : (object) null
      };
      for (int index = 0; index < 5; ++index)
      {
        if (objArray[index] != null)
          return (object) objArray;
      }
      return (object) null;
    }

    protected override void TrackViewState()
    {
      base.TrackViewState();
      if (this._signInButtonStyle != null)
        ((IStateManager) this._signInButtonStyle).TrackViewState();
      if (this._titleTextStyle != null)
        ((IStateManager) this._titleTextStyle).TrackViewState();
      if (this._errorTextStyle != null)
        ((IStateManager) this._errorTextStyle).TrackViewState();
      if (this._checkBoxStyle == null)
        return;
      ((IStateManager) this._checkBoxStyle).TrackViewState();
    }

    private bool RedirectedFromFailedLogin() => !this.DesignMode && this.Page != null && (!this.Page.IsPostBack && this.Page.Request.QueryString["signinfailure"] != null);

    internal sealed class SignInContainer : WebControlContainer<SignInControl>
    {
      private const string ErrorTextId = "ErrorTextId";
      private HyperLink _createUserLink;
      private LiteralControl _createUserLinkSeparator;
      private Control _errorTextLabel;
      private HyperLink _helpPageLink;
      private IButtonControl _button;
      private Literal _title;
      private Image _createUserIcon;
      private Image _helpPageIcon;
      private WebControl _rememberMeCheckBox;
      private SimpleButton _simpleButton = new SimpleButton();

      public SignInContainer(SignInControl owner)
        : base(owner, owner.DesignMode)
      {
      }

      protected override bool ConvertingToTemplate => this.Owner.ConvertingToTemplate;

      internal HyperLink CreateUserLink
      {
        get => this._createUserLink;
        set => this._createUserLink = value;
      }

      internal LiteralControl CreateUserLinkSeparator
      {
        get => this._createUserLinkSeparator;
        set => this._createUserLinkSeparator = value;
      }

      internal Image HelpPageIcon
      {
        get => this._helpPageIcon;
        set => this._helpPageIcon = value;
      }

      internal Image CreateUserIcon
      {
        get => this._createUserIcon;
        set => this._createUserIcon = value;
      }

      internal Control ErrorTextLabel
      {
        get => this._errorTextLabel;
        set => this._errorTextLabel = value;
      }

      internal HyperLink HelpPageLink
      {
        get => this._helpPageLink;
        set => this._helpPageLink = value;
      }

      internal IButtonControl Button
      {
        get => this._button;
        set => this._button = value;
      }

      internal WebControl RememberMeCheckBox
      {
        get => this._rememberMeCheckBox;
        set => this._rememberMeCheckBox = value;
      }

      internal WebControl ActiveButton
      {
        get
        {
          switch (this.Owner.SignInButtonType)
          {
            case ButtonType.Button:
              return (WebControl) this._simpleButton;
            default:
              return (WebControl) this._button;
          }
        }
      }

      internal Literal Title
      {
        get => this._title;
        set => this._title = value;
      }
    }

    internal class SignInTemplate : ITemplate
    {
      private SignInControl _owner;
      private IButtonControl _button;

      public SignInTemplate(SignInControl owner, IButtonControl button)
      {
        this._owner = owner;
        this._button = button;
      }

      private void CreateControls(SignInControl.SignInContainer loginContainer)
      {
        Literal literal1 = new Literal();
        loginContainer.Title = literal1;
        loginContainer.Button = this._button;
        WebControl webControl = (WebControl) new CheckBox();
        webControl.ID = "rememberMe";
        loginContainer.RememberMeCheckBox = webControl;
        HyperLink hyperLink1 = new HyperLink();
        loginContainer.CreateUserLink = hyperLink1;
        LiteralControl literalControl = new LiteralControl();
        loginContainer.CreateUserLinkSeparator = literalControl;
        HyperLink hyperLink2 = new HyperLink();
        loginContainer.HelpPageLink = hyperLink2;
        Literal literal2 = new Literal();
        loginContainer.ErrorTextLabel = (Control) literal2;
        loginContainer.HelpPageIcon = new Image();
        loginContainer.CreateUserIcon = new Image();
      }

      private void LayoutControls(SignInControl.SignInContainer loginContainer)
      {
        if (this._owner.Orientation == Orientation.Vertical)
          this.LayoutVertical(loginContainer);
        else
          this.LayoutHorizontal(loginContainer);
      }

      private void LayoutHorizontal(SignInControl.SignInContainer loginContainer)
      {
        Table table = new Table();
        table.CellPadding = 0;
        TableRow row1 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell1 = new TableCell();
        cell1.HorizontalAlign = HorizontalAlign.Center;
        cell1.Controls.Add((Control) loginContainer.Title);
        row1.Cells.Add(cell1);
        table.Rows.Add(row1);
        TableCell cell2 = new TableCell();
        cell2.HorizontalAlign = HorizontalAlign.Center;
        cell2.Controls.Add((Control) loginContainer.ActiveButton);
        row1.Cells.Add(cell2);
        table.Rows.Add(row1);
        TableRow row2 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell3 = new TableCell();
        cell3.ColumnSpan = 2;
        cell3.Controls.Add((Control) loginContainer.RememberMeCheckBox);
        row2.Cells.Add(cell3);
        table.Rows.Add(row2);
        TableRow row3 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell4 = new TableCell();
        cell4.ColumnSpan = 2;
        cell4.Controls.Add(loginContainer.ErrorTextLabel);
        row3.Cells.Add(cell4);
        table.Rows.Add(row3);
        TableRow row4 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell5 = new TableCell();
        cell5.ColumnSpan = 2;
        cell5.Controls.Add((Control) loginContainer.CreateUserIcon);
        cell5.Controls.Add((Control) loginContainer.CreateUserLink);
        loginContainer.CreateUserLinkSeparator.Text = " ";
        cell5.Controls.Add((Control) loginContainer.CreateUserLinkSeparator);
        cell5.Controls.Add((Control) loginContainer.HelpPageIcon);
        cell5.Controls.Add((Control) loginContainer.HelpPageLink);
        row4.Cells.Add(cell5);
        table.Rows.Add(row4);
        Table childTable = ControlUtil.CreateChildTable(this._owner.ConvertingToTemplate);
        TableRow row5 = new TableRow();
        TableCell cell6 = new TableCell();
        cell6.Controls.Add((Control) table);
        row5.Cells.Add(cell6);
        childTable.Rows.Add(row5);
        loginContainer.LayoutTable = table;
        loginContainer.BorderTable = childTable;
        loginContainer.Controls.Add((Control) childTable);
      }

      private void LayoutVertical(SignInControl.SignInContainer loginContainer)
      {
        Table table = new Table();
        table.CellPadding = 0;
        TableRow row1 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell1 = new TableCell();
        cell1.HorizontalAlign = HorizontalAlign.Center;
        cell1.Controls.Add((Control) loginContainer.Title);
        row1.Cells.Add(cell1);
        table.Rows.Add(row1);
        TableRow row2 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell2 = new TableCell();
        cell2.HorizontalAlign = HorizontalAlign.Center;
        cell2.Controls.Add((Control) loginContainer.ActiveButton);
        row2.Cells.Add(cell2);
        table.Rows.Add(row2);
        TableRow row3 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell3 = new TableCell();
        cell3.HorizontalAlign = HorizontalAlign.Center;
        cell3.Controls.Add((Control) loginContainer.RememberMeCheckBox);
        row3.Cells.Add(cell3);
        table.Rows.Add(row3);
        TableRow row4 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell4 = new TableCell();
        cell4.HorizontalAlign = HorizontalAlign.Center;
        cell4.Controls.Add(loginContainer.ErrorTextLabel);
        row4.Cells.Add(cell4);
        table.Rows.Add(row4);
        TableRow row5 = (TableRow) new ControlUtil.DisappearingTableRow();
        TableCell cell5 = new TableCell();
        cell5.Controls.Add((Control) loginContainer.CreateUserIcon);
        cell5.Controls.Add((Control) loginContainer.CreateUserLink);
        cell5.Controls.Add((Control) loginContainer.CreateUserLinkSeparator);
        loginContainer.CreateUserLinkSeparator.Text = "<br />";
        cell5.Controls.Add((Control) loginContainer.HelpPageIcon);
        cell5.Controls.Add((Control) loginContainer.HelpPageLink);
        row5.Cells.Add(cell5);
        table.Rows.Add(row5);
        Table childTable = ControlUtil.CreateChildTable(this._owner.ConvertingToTemplate);
        TableRow row6 = new TableRow();
        TableCell cell6 = new TableCell();
        cell6.Controls.Add((Control) table);
        row6.Cells.Add(cell6);
        childTable.Rows.Add(row6);
        loginContainer.LayoutTable = table;
        loginContainer.BorderTable = childTable;
        loginContainer.Controls.Add((Control) childTable);
      }

      void ITemplate.InstantiateIn(Control container)
      {
        SignInControl.SignInContainer loginContainer = (SignInControl.SignInContainer) container;
        this.CreateControls(loginContainer);
        this.LayoutControls(loginContainer);
      }
    }
  }
}
