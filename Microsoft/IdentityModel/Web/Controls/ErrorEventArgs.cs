// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.ErrorEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Controls
{
  [ComVisible(true)]
  public class ErrorEventArgs : CancelEventArgs
  {
    private Exception _exception;

    public ErrorEventArgs(Exception exception)
      : this(false, exception)
    {
    }

    public ErrorEventArgs(bool cancel, Exception exception)
      : base(cancel)
      => this._exception = exception != null ? exception : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exception));

    public Exception Exception => this._exception;
  }
}
