// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustRequestProcessingErrorEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustRequestProcessingErrorEventArgs : EventArgs
  {
    private Exception _exception;
    private string _requestType;

    public WSTrustRequestProcessingErrorEventArgs(string requestType, Exception exception)
    {
      this._exception = exception;
      this._requestType = requestType;
    }

    public Exception Exception => this._exception;

    public string RequestType => this._requestType;
  }
}
