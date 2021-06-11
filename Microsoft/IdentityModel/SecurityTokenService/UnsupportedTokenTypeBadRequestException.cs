// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.UnsupportedTokenTypeBadRequestException
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
  public class UnsupportedTokenTypeBadRequestException : BadRequestException
  {
    private const string TokenTypeProperty = "TokenType";
    private string _tokenType;

    public UnsupportedTokenTypeBadRequestException() => this._tokenType = string.Empty;

    public UnsupportedTokenTypeBadRequestException(string tokenType)
      : base(Microsoft.IdentityModel.SR.GetString("ID2014", (object) tokenType))
      => this._tokenType = tokenType;

    public UnsupportedTokenTypeBadRequestException(string message, Exception exception)
      : base(message, exception)
    {
    }

    protected UnsupportedTokenTypeBadRequestException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
      this._tokenType = info != null ? info.GetValue(nameof (TokenType), typeof (string)) as string : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      info.AddValue("TokenType", (object) this.TokenType);
      base.GetObjectData(info, context);
    }

    public string TokenType
    {
      get => this._tokenType;
      set => this._tokenType = value;
    }
  }
}
