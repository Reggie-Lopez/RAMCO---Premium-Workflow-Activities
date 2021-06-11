// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.FederatedPassiveSignInDesigner
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;

namespace Microsoft.IdentityModel.Web.Controls
{
  internal class FederatedPassiveSignInDesigner : CompositeControlDesigner
  {
    private const string _wifSDKRegistryPath = "SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5";
    private const string _wifSDKRegistryKey = "InstallPath";
    private const string _fedUtilExe = "FedUtil.exe";
    private const string _fedUtilArgs = "/c";
    private bool _fedUtilInstalled;
    private DesignerActionListCollection _actionLists;

    public override void Initialize(IComponent component)
    {
      this._fedUtilInstalled = this.IsFedUtilInstalled();
      base.Initialize(component);
    }

    public override DesignerActionListCollection ActionLists
    {
      get
      {
        if (!this._fedUtilInstalled)
          return base.ActionLists;
        if (this._actionLists == null)
        {
          this._actionLists = new DesignerActionListCollection();
          this._actionLists.Add((DesignerActionList) new FederatedPassiveSignInDesigner.FederatedPassiveSignInActionList(this));
        }
        return this._actionLists;
      }
    }

    private bool IsFedUtilInstalled()
    {
      bool flag = false;
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5");
      if (registryKey != null)
      {
        string path1 = registryKey.GetValue("InstallPath") as string;
        if (!string.IsNullOrEmpty(path1) && File.Exists(Path.Combine(path1, "FedUtil.exe")))
          flag = true;
      }
      return flag;
    }

    private class FederatedPassiveSignInActionList : DesignerActionList
    {
      private FederatedPassiveSignIn _signInControl;
      private FederatedPassiveSignInDesigner _designer;

      public FederatedPassiveSignInActionList(FederatedPassiveSignInDesigner designer)
        : base(designer.Component)
      {
        this._designer = designer;
        this._signInControl = (FederatedPassiveSignIn) designer.Component;
      }

      public override DesignerActionItemCollection GetSortedActionItems() => new DesignerActionItemCollection()
      {
        (DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "InvokeFedUtil", Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtil"), string.Empty, Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtil"), true)
      };

      private void InvokeFedUtil()
      {
        if (!this._signInControl.UseFederationPropertiesFromConfiguration)
        {
          int num1 = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilErrorConfigProperty"), Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, this.GetMessageBoxOptions());
        }
        else
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5");
          if (registryKey == null)
          {
            int num2 = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, this.GetMessageBoxOptions());
          }
          else
          {
            string path1 = registryKey.GetValue("InstallPath") as string;
            if (string.IsNullOrEmpty(path1))
            {
              int num3 = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, this.GetMessageBoxOptions());
            }
            else
            {
              string str = Path.Combine(path1, "FedUtil.exe");
              if (!File.Exists(str))
              {
                int num4 = (int) MessageBox.Show(Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), Microsoft.IdentityModel.SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, this.GetMessageBoxOptions());
              }
              else
                Process.Start(str, "/c");
            }
          }
        }
      }

      private MessageBoxOptions GetMessageBoxOptions()
      {
        MessageBoxOptions messageBoxOptions = (MessageBoxOptions) 0;
        if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
          messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
        return messageBoxOptions;
      }
    }
  }
}
