// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.SimpleCheckBox
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  [DefaultProperty("Text")]
  [ControlValueProperty("Checked")]
  internal class SimpleCheckBox : WebControl, ICheckBoxControl
  {
    public SimpleCheckBox()
      : base(HtmlTextWriterTag.Input)
    {
    }

    [DefaultValue(false)]
    [Themeable(false)]
    public virtual bool Checked
    {
      get => (bool) (this.ViewState[nameof (Checked)] ?? (object) false);
      set => this.ViewState[nameof (Checked)] = (object) value;
    }

    [Bindable(true)]
    [Localizable(true)]
    [WebCategory("Category_Appearance")]
    [DefaultValue("")]
    public virtual string Text
    {
      get => (string) this.ViewState[nameof (Text)] ?? string.Empty;
      set => this.ViewState[nameof (Text)] = (object) value;
    }

    [WebCategory("Category_Action")]
    public event EventHandler CheckedChanged
    {
      add => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID5008")));
      remove => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID5008")));
    }

    protected override void Render(HtmlTextWriter writer)
    {
      bool flag = false;
      if (this.ControlStyleCreated)
      {
        Style controlStyle = this.ControlStyle;
        if (!controlStyle.IsEmpty)
        {
          controlStyle.AddAttributesToRender(writer, (WebControl) this);
          flag = true;
        }
      }
      if (!this.IsEnabled)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
        flag = true;
      }
      string toolTip = this.ToolTip;
      if (toolTip.Length > 0)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Title, toolTip);
        flag = true;
      }
      string onClick = (string) null;
      if (this.HasAttributes)
      {
        System.Web.UI.AttributeCollection attributes = this.Attributes;
        string str = attributes["value"];
        if (str != null)
          attributes.Remove("value");
        onClick = attributes["onclick"];
        if (onClick != null)
        {
          onClick = ControlUtil.EnsureEndWithSemiColon(onClick);
          attributes.Remove("onclick");
        }
        if (attributes.Count != 0)
        {
          attributes.AddAttributes(writer);
          flag = true;
        }
        if (str != null)
          attributes["value"] = str;
      }
      if (flag)
        writer.RenderBeginTag(HtmlTextWriterTag.Span);
      string text = this.Text;
      string clientId = this.ClientID;
      if (text.Length != 0)
      {
        this.RenderInputTag(writer, clientId, onClick);
        this.RenderLabel(writer, text, clientId);
      }
      else
        this.RenderInputTag(writer, clientId, onClick);
      if (!flag)
        return;
      writer.RenderEndTag();
    }

    private void RenderLabel(HtmlTextWriter writer, string text, string clientID)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.For, HttpUtility.HtmlEncode(clientID));
      writer.RenderBeginTag(HtmlTextWriterTag.Label);
      HttpUtility.HtmlEncode(text, (TextWriter) writer);
      writer.RenderEndTag();
    }

    private void RenderInputTag(HtmlTextWriter writer, string clientID, string onClick)
    {
      if (clientID != null)
        writer.AddAttribute(HtmlTextWriterAttribute.Id, HttpUtility.HtmlEncode(clientID));
      writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
      if (this.UniqueID != null)
        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
      if (this.Checked)
        writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
      if (!this.IsEnabled)
        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
      if (onClick != null)
        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, HttpUtility.HtmlEncode(onClick));
      if (this.AccessKey.Length > 0)
        writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, HttpUtility.HtmlEncode(this.AccessKey));
      if (this.TabIndex != (short) 0)
        writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
      writer.RenderBeginTag(HtmlTextWriterTag.Input);
      writer.RenderEndTag();
    }
  }
}
