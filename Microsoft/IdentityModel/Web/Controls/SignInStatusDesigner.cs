// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.SignInStatusDesigner
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.Design.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  internal class SignInStatusDesigner : CompositeControlDesigner
  {
    private bool _loggedIn;
    private FederatedPassiveSignInStatus _loginStatus;

    public override DesignerActionListCollection ActionLists
    {
      get
      {
        DesignerActionListCollection actionListCollection = new DesignerActionListCollection();
        actionListCollection.AddRange(base.ActionLists);
        actionListCollection.Add((DesignerActionList) new SignInStatusDesigner.LoginStatusDesignerActionList(this));
        return actionListCollection;
      }
    }

    protected override bool UsePreviewControl => true;

    public override string GetDesignTimeHtml()
    {
      IDictionary data = (IDictionary) new HybridDictionary(2);
      data[(object) "LoggedIn"] = (object) this._loggedIn;
      FederatedPassiveSignInStatus viewControl = (FederatedPassiveSignInStatus) this.ViewControl;
      ((IControlDesignerAccessor) viewControl).SetDesignModeState(data);
      if (this._loggedIn)
      {
        string signOutText = viewControl.SignOutText;
        if (signOutText == null || signOutText.Length == 0 || signOutText == " ")
          viewControl.SignOutText = "[" + viewControl.ID + "]";
      }
      else
      {
        string signInText = viewControl.SignInText;
        if (signInText == null || signInText.Length == 0 || signInText == " ")
          viewControl.SignInText = "[" + viewControl.ID + "]";
      }
      return base.GetDesignTimeHtml();
    }

    public override void Initialize(IComponent component)
    {
      this._loginStatus = (FederatedPassiveSignInStatus) component;
      base.Initialize((IComponent) this._loginStatus);
    }

    private class LoginStatusDesignerActionList : DesignerActionList
    {
      private SignInStatusDesigner _designer;

      public LoginStatusDesignerActionList(SignInStatusDesigner designer)
        : base(designer.Component)
        => this._designer = designer;

      [TypeConverter(typeof (SignInStatusDesigner.LoginStatusDesignerActionList.SignInStatusViewTypeConverter))]
      public string View
      {
        get => this._designer._loggedIn ? Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedInView") : Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedOutView");
        set
        {
          if (string.Compare(value, Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedInView"), StringComparison.Ordinal) == 0)
            this._designer._loggedIn = true;
          else if (string.Compare(value, Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedOutView"), StringComparison.Ordinal) == 0)
            this._designer._loggedIn = false;
          this._designer.UpdateDesignTimeHtml();
        }
      }

      public override DesignerActionItemCollection GetSortedActionItems() => new DesignerActionItemCollection()
      {
        (DesignerActionItem) new DesignerActionPropertyItem("View", Microsoft.IdentityModel.SR.GetString("WebControls_Views"), string.Empty, Microsoft.IdentityModel.SR.GetString("WebControls_ViewsDescription"))
      };

      private class SignInStatusViewTypeConverter : TypeConverter
      {
        public override TypeConverter.StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {
          return new TypeConverter.StandardValuesCollection((ICollection) new string[2]
          {
            Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedOutView"),
            Microsoft.IdentityModel.SR.GetString("SignInStatus_SignedInView")
          });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
      }
    }
  }
}
