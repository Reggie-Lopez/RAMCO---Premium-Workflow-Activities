// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SigningOutEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class SigningOutEventArgs : EventArgs
  {
    private static SigningOutEventArgs _ipInitiated = new SigningOutEventArgs(true);
    private static SigningOutEventArgs _rpInitiated = new SigningOutEventArgs(false);
    private bool _isIPInitiated;

    public static SigningOutEventArgs IPInitiated => SigningOutEventArgs._ipInitiated;

    public static SigningOutEventArgs RPInitiated => SigningOutEventArgs._rpInitiated;

    public SigningOutEventArgs(bool isIPInitiated) => this._isIPInitiated = isIPInitiated;

    public bool IsIPInitiated => this._isIPInitiated;
  }
}
