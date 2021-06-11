// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.SimpleButton
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  [ParseChildren(false)]
  [DefaultProperty("Text")]
  [ControlBuilder(typeof (SimpleButtonControlBuilder))]
  internal class SimpleButton : WebControl
  {
    private Image _img;
    private Label _label;

    public SimpleButton()
      : base(HtmlTextWriterTag.Button)
    {
    }

    [WebCategory("Category_Appearance")]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [WebDescription("SignIn_SignInText")]
    [Localizable(true)]
    [WebDefaultValue("SignIn_DefaultSignInText")]
    public virtual string Text
    {
      get => (string) this.ViewState[nameof (Text)] ?? SR.GetString("SignIn_DefaultSignInText");
      set => this.ViewState[nameof (Text)] = (object) value;
    }

    [WebCategory("Category_Appearance")]
    [WebDescription("SignIn_SignInImageUrl")]
    [UrlProperty]
    [DefaultValue("")]
    [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))]
    public virtual string ImageUrl
    {
      get => (string) this.ViewState[nameof (ImageUrl)] ?? string.Empty;
      set => this.ViewState[nameof (ImageUrl)] = (object) value;
    }

    [WebCategory("Category_Behavior")]
    [DefaultValue("")]
    [Themeable(false)]
    [WebDescription("SimpleButton_OnClientClick")]
    public virtual string OnClientClick
    {
      get => (string) this.ViewState[nameof (OnClientClick)] ?? string.Empty;
      set => this.ViewState[nameof (OnClientClick)] = (object) value;
    }

    protected override void CreateChildControls()
    {
      this.Controls.Clear();
      this._img = new Image();
      this._img.ID = "signinimage";
      this._img.ImageAlign = ImageAlign.AbsMiddle;
      this._img.Style.Add(HtmlTextWriterStyle.MarginRight, "5px");
      this.Controls.Add((Control) this._img);
      this._label = new Label();
      this._label.ID = "signintext";
      this.Controls.Add((Control) this._label);
    }

    protected override void AddAttributesToRender(HtmlTextWriter writer)
    {
      if (this.Enabled && !this.IsEnabled)
        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
      if (this.IsEnabled && !string.IsNullOrEmpty(this.OnClientClick))
        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ControlUtil.EnsureEndWithSemiColon(this.OnClientClick));
      base.AddAttributesToRender(writer);
    }

    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);
      if (savedState == null)
        return;
      string str = (string) this.ViewState["Text"];
      if (str == null)
        return;
      this.Text = str;
    }

    protected override void RenderContents(HtmlTextWriter writer)
    {
      if (this.DesignMode)
        this.EnsureChildControls();
      this._img.Visible = !string.IsNullOrEmpty(this.ImageUrl);
      if (this._img.Visible)
        this._img.ImageUrl = this.ImageUrl;
      this._label.Visible = !string.IsNullOrEmpty(this.Text);
      if (this._label.Visible)
        this._label.Text = this.Text;
      base.RenderContents(writer);
    }
  }
}
