// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.FederatedPassiveSignIn
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web.Configuration;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;

namespace Microsoft.IdentityModel.Web.Controls
{
  [Bindable(false)]
  [Designer(typeof (FederatedPassiveSignInDesigner))]
  [DefaultEvent("SignedIn")]
  [ComVisible(true)]
  public sealed class FederatedPassiveSignIn : SignInControl
  {
    private string _responseReturnUrl;
    private WSFederationAuthenticationModule _activeModule;
    private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;
    private bool _initialized;

    public FederatedPassiveSignIn()
    {
      this._xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas()
      {
        MaxArrayLength = 2097152,
        MaxStringContentLength = 2097152
      };
      this.Init += new EventHandler(this.FederatedPassiveSignIn_Init);
    }

    [WebCategory("Category_FederatedPassive")]
    [UrlProperty]
    [DefaultValue("")]
    [WebDescription("FederatedPassiveSignIn_HomeRealm")]
    public string HomeRealm
    {
      get => (string) this.ViewState[nameof (HomeRealm)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (HomeRealm));
        this.ViewState[nameof (HomeRealm)] = (object) value;
      }
    }

    [WebDescription("FederatedPassiveSignIn_Issuer")]
    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    [UrlProperty]
    public string Issuer
    {
      get => (string) this.ViewState[nameof (Issuer)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Issuer));
        this.ViewState[nameof (Issuer)] = (object) value;
      }
    }

    [DefaultValue("")]
    [WebDescription("FederatedPassiveSignIn_Realm")]
    [WebCategory("Category_FederatedPassive")]
    public string Realm
    {
      get => (string) this.ViewState[nameof (Realm)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Realm));
        this.ViewState[nameof (Realm)] = (object) value;
      }
    }

    [WebDescription("FederatedPassiveSignIn_Reply")]
    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    public string Reply
    {
      get => (string) this.ViewState[nameof (Reply)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Reply));
        this.ViewState[nameof (Reply)] = (object) value;
      }
    }

    [WebCategory("Category_FederatedPassive")]
    [DefaultValue("")]
    [WebDescription("FederatedPassiveSignIn_Resource")]
    public string Resource
    {
      get => (string) this.ViewState[nameof (Resource)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Resource));
        this.ViewState[nameof (Resource)] = (object) value;
      }
    }

    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    [WebDescription("FederatedPassiveSignIn_Request")]
    public string Request
    {
      get => (string) this.ViewState[nameof (Request)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Request));
        this.ViewState[nameof (Request)] = (object) value;
      }
    }

    [WebDescription("FederatedPassiveSignIn_RequestPtr")]
    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    public string RequestPtr
    {
      get => (string) this.ViewState[nameof (RequestPtr)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (RequestPtr));
        this.ViewState[nameof (RequestPtr)] = (object) value;
      }
    }

    [WebDescription("FederatedPassiveSignIn_Freshness")]
    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    public string Freshness
    {
      get => (string) this.ViewState[nameof (Freshness)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Freshness));
        this.ViewState[nameof (Freshness)] = (object) value;
      }
    }

    [DefaultValue("")]
    [WebDescription("FederatedPassiveSignIn_AuthenticationType")]
    [WebCategory("Category_FederatedPassive")]
    public string AuthenticationType
    {
      get => (string) this.ViewState[nameof (AuthenticationType)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (AuthenticationType));
        this.ViewState[nameof (AuthenticationType)] = (object) value;
      }
    }

    [WebDescription("FederatedPassiveSignIn_Policy")]
    [DefaultValue("")]
    [WebCategory("Category_FederatedPassive")]
    public string Policy
    {
      get => (string) this.ViewState[nameof (Policy)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (Policy));
        this.ViewState[nameof (Policy)] = (object) value;
      }
    }

    [DefaultValue("")]
    [WebDescription("FederatedPassiveSignIn_SignInQueryString")]
    [WebCategory("Category_FederatedPassive")]
    public string SignInQueryString
    {
      get => (string) this.ViewState[nameof (SignInQueryString)] ?? string.Empty;
      set
      {
        if (this.DesignMode && this.UseFederationPropertiesFromConfiguration && this._initialized)
          this.ShowFamConfigMessage(nameof (SignInQueryString));
        this.ViewState[nameof (SignInQueryString)] = (object) value;
      }
    }

    [WebCategory("Category_FederatedPassive")]
    [WebDescription("FederatedPassiveSignIn_UseFederationPropertiesFromConfiguration")]
    [DefaultValue(false)]
    [Themeable(false)]
    public bool UseFederationPropertiesFromConfiguration
    {
      get => (bool) (this.ViewState[nameof (UseFederationPropertiesFromConfiguration)] ?? (object) false);
      set
      {
        if (this.DesignMode && this._initialized && value)
        {
          MessageBoxOptions options = (MessageBoxOptions) 0;
          if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
          int num = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("FederatedPassiveSignIn_UseFederationPropertiesSet"), Microsoft.IdentityModel.SR.GetString("FederatedPassiveSignIn_Property_Information"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, options);
        }
        this.ViewState[nameof (UseFederationPropertiesFromConfiguration)] = (object) value;
      }
    }

    protected override string GetSessionTokenContext()
    {
      string parameterValue = this.GetParameterValue("issuer");
      if (string.IsNullOrEmpty(parameterValue))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5002")));
      return WSFederationAuthenticationModule.SessionTokenContextPrefix + WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(parameterValue, string.Empty, string.Empty);
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!this.Enabled)
        return;
      this.Click += new EventHandler(this.OnClick);
      this.CreateWSFederationAuthenticationModule();
    }

    private void RedirectingToIdentityProviderHandler(
      object sender,
      RedirectingToIdentityProviderEventArgs e)
    {
      this.OnRedirectingToIdentityProvider(e);
    }

    internal void CreateWSFederationAuthenticationModule()
    {
      this._activeModule = new WSFederationAuthenticationModule();
      this._activeModule.ServiceConfiguration = this.ServiceConfiguration;
      this._activeModule.RedirectingToIdentityProvider += new EventHandler<RedirectingToIdentityProviderEventArgs>(this.RedirectingToIdentityProviderHandler);
      string parameterValue1 = this.GetParameterValue("homeRealm");
      if (!string.IsNullOrEmpty(parameterValue1))
        this._activeModule.HomeRealm = parameterValue1;
      string parameterValue2 = this.GetParameterValue("issuer");
      if (!string.IsNullOrEmpty(parameterValue2))
        this._activeModule.Issuer = parameterValue2;
      string parameterValue3 = this.GetParameterValue("realm");
      if (!string.IsNullOrEmpty(parameterValue3))
        this._activeModule.Realm = parameterValue3;
      string parameterValue4 = this.GetParameterValue("authenticationType");
      if (!string.IsNullOrEmpty(parameterValue4))
        this._activeModule.AuthenticationType = parameterValue4;
      string parameterValue5 = this.GetParameterValue("freshness");
      if (!string.IsNullOrEmpty(parameterValue5))
        this._activeModule.Freshness = parameterValue5;
      string parameterValue6 = this.GetParameterValue("policy");
      if (!string.IsNullOrEmpty(parameterValue6))
        this._activeModule.Policy = parameterValue6;
      string parameterValue7 = this.GetParameterValue("reply");
      if (!string.IsNullOrEmpty(parameterValue7))
        this._activeModule.Reply = parameterValue7;
      string parameterValue8 = this.GetParameterValue("request");
      if (!string.IsNullOrEmpty(parameterValue8))
        this._activeModule.Request = parameterValue8;
      string parameterValue9 = this.GetParameterValue("requestPtr");
      if (!string.IsNullOrEmpty(parameterValue9))
        this._activeModule.RequestPtr = parameterValue9;
      string parameterValue10 = this.GetParameterValue("resource");
      if (!string.IsNullOrEmpty(parameterValue10))
        this._activeModule.Resource = parameterValue10;
      if (!string.IsNullOrEmpty(this.SignInContext))
        this._activeModule.SignInContext = this.SignInContext;
      string parameterValue11 = this.GetParameterValue("signInQueryString");
      if (!string.IsNullOrEmpty(parameterValue11))
        this._activeModule.SignInQueryString = parameterValue11;
      WSFederationAuthenticationElement famConfiguration = this.GetFAMConfiguration();
      if (this.UseFederationPropertiesFromConfiguration && famConfiguration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5003")));
      this._activeModule.RequireHttps = this.UseFederationPropertiesFromConfiguration ? famConfiguration.RequireHttps : this.RequireHttps;
      this._activeModule.VerifyProperties();
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      if (this.Enabled && StringComparer.OrdinalIgnoreCase.Equals(this.GetParameterValue("requireHttps"), "true"))
      {
        string parameterValue1 = this.GetParameterValue("issuer");
        string parameterValue2 = this.GetParameterValue("reply");
        if (!ControlUtil.IsHttps(parameterValue1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1056"));
        if (!string.IsNullOrEmpty(parameterValue2) && !ControlUtil.IsHttps(parameterValue2))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1057"));
        if (!ControlUtil.IsHttps(this.Context.Request.Url))
        {
          this.Enabled = false;
          this.ToolTip = Microsoft.IdentityModel.SR.GetString("ID5016");
        }
      }
      if (!this.Enabled || !this.AutoSignIn || HttpContext.Current.User.Identity.IsAuthenticated)
        return;
      this.RedirectToIdentityProvider();
    }

    protected override bool SignIn()
    {
      HttpRequest request = this.Page.Request;
      return this._activeModule != null && this._activeModule.CanReadSignInResponse(request, true) && StringComparer.Ordinal.Equals(this.UniqueID, new FederatedPassiveContext(request.Form["wctx"]).ControlId) && this.SignInWithResponseMessage(this._activeModule.GetSignInResponseMessage(request));
    }

    internal bool SignInWithResponseMessage(SignInResponseMessage message)
    {
      string tokenFromMessage = this._activeModule.GetXmlTokenFromMessage(message, (WSFederationSerializer) null);
      TokenReceiver tokenReceiver = new TokenReceiver(this._activeModule.ServiceConfiguration);
      SecurityToken securityToken = tokenReceiver.ReadToken(tokenFromMessage, this._xmlDictionaryReaderQuotas);
      FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(message.Context);
      SecurityTokenReceivedEventArgs e1 = new SecurityTokenReceivedEventArgs(securityToken, federatedPassiveContext.SignInContext);
      this.OnSecurityTokenReceived(e1);
      if (!e1.Cancel)
      {
        IClaimsPrincipal claimsPrincipal = tokenReceiver.AuthenticateToken(e1.SecurityToken, true, HttpContext.Current.Request.RawUrl);
        if (claimsPrincipal != null)
        {
          SecurityTokenValidatedEventArgs e2 = new SecurityTokenValidatedEventArgs(claimsPrincipal);
          this.OnSecurityTokenValidated(e2);
          if (!e2.Cancel)
          {
            SessionAuthenticationModule current = SessionAuthenticationModule.Current;
            DateTime validFrom;
            DateTime validTo;
            tokenReceiver.ComputeSessionTokenLifeTime(e1.SecurityToken, out validFrom, out validTo);
            SessionSecurityTokenCreatedEventArgs e3 = new SessionSecurityTokenCreatedEventArgs(current.CreateSessionSecurityToken(e2.ClaimsPrincipal, this.GetSessionTokenContext(), validFrom, validTo, federatedPassiveContext.RememberMe));
            e3.WriteSessionCookie = this.SignInMode == SignInMode.Session;
            this.OnSessionSecurityTokenCreated(e3);
            this._activeModule.SetPrincipalAndWriteSessionToken(e3.SessionToken, e3.WriteSessionCookie);
            this.OnSignedIn(EventArgs.Empty);
            this._responseReturnUrl = federatedPassiveContext.ReturnUrl;
            return true;
          }
        }
      }
      return false;
    }

    private void FederatedPassiveSignIn_Init(object sender, EventArgs e)
    {
      if (!this.DesignMode)
        return;
      this._initialized = true;
    }

    private void OnClick(object sender, EventArgs e) => this.RedirectToIdentityProvider();

    private void RedirectToIdentityProvider()
    {
      if (this._activeModule == null)
        return;
      this._activeModule.RedirectToIdentityProvider(this.UniqueID, this.Page.Request.QueryString["ReturnUrl"], this.IsPersistentCookie);
      this.Page.Response.End();
    }

    protected override string GetReturnUrl() => !string.IsNullOrEmpty(this._responseReturnUrl) ? this._responseReturnUrl : base.GetReturnUrl();

    private WSFederationAuthenticationElement GetFAMConfiguration()
    {
      WSFederationAuthenticationElement authenticationElement = (WSFederationAuthenticationElement) null;
      ServiceElement defaultServiceElement = MicrosoftIdentityModelSection.DefaultServiceElement;
      if (defaultServiceElement != null)
        authenticationElement = defaultServiceElement.FederatedAuthentication.WSFederation;
      return authenticationElement;
    }

    private string GetParameterValue(string parameterName)
    {
      WSFederationAuthenticationElement famConfiguration = this.GetFAMConfiguration();
      if (this.UseFederationPropertiesFromConfiguration && famConfiguration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5003")));
      switch (parameterName)
      {
        case "authenticationType":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.AuthenticationType : this.AuthenticationType;
        case "freshness":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Freshness : this.Freshness;
        case "homeRealm":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.HomeRealm : this.HomeRealm;
        case "issuer":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Issuer : this.Issuer;
        case "policy":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Policy : this.Policy;
        case "realm":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Realm : this.Realm;
        case "reply":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Reply : this.Reply;
        case "request":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Request : this.Request;
        case "requestPtr":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.RequestPtr : this.RequestPtr;
        case "resource":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.Resource : this.Resource;
        case "signInQueryString":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.SignInQueryString : this.SignInQueryString;
        case "requireHttps":
          return this.UseFederationPropertiesFromConfiguration ? famConfiguration.RequireHttps.ToString() : this.RequireHttps.ToString();
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5005", (object) parameterName)));
      }
    }

    private void ShowFamConfigMessage(string propertyName)
    {
      MessageBoxOptions options = (MessageBoxOptions) 0;
      if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
        options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
      int num = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("FederatedPassiveSignIn_PropertySetWarning", (object) propertyName), Microsoft.IdentityModel.SR.GetString("FederatedPassiveSignIn_Property_Information"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, options);
    }
  }
}
