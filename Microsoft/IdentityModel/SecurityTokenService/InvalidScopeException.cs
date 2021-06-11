// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.InvalidScopeException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  [Serializable]
  public class InvalidScopeException : RequestException
  {
    private const string ScopeProperty = "Scope";
    private string _address;

    public InvalidScopeException()
      : this(string.Empty)
    {
    }

    public InvalidScopeException(string address)
      : base(Microsoft.IdentityModel.SR.GetString("ID2010", (object) address))
      => this._address = address;

    public InvalidScopeException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected InvalidScopeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      => this._address = info != null ? info.GetValue(nameof (Scope), typeof (string)) as string : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      info.AddValue("Scope", (object) this.Scope);
      base.GetObjectData(info, context);
    }

    public string Scope => this._address;
  }
}
