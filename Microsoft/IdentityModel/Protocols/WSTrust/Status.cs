// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Status
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class Status
  {
    private string _code;
    private string _reason;

    public Status(string code, string reason)
    {
      this._code = !string.IsNullOrEmpty(code) ? code : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (code));
      this._reason = reason;
    }

    public string Code
    {
      get => this._code;
      set => this._code = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("code");
    }

    public string Reason
    {
      get => this._reason;
      set => this._reason = value;
    }
  }
}
