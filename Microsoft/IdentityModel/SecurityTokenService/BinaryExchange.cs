// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.BinaryExchange
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class BinaryExchange
  {
    private byte[] _binaryData;
    private Uri _valueType;
    private Uri _encodingType;

    public BinaryExchange(byte[] binaryData, Uri valueType)
      : this(binaryData, valueType, new Uri("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
    {
    }

    public BinaryExchange(byte[] binaryData, Uri valueType, Uri encodingType)
    {
      if (binaryData == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (binaryData));
      if (valueType == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (valueType));
      if (encodingType == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encodingType));
      if (!valueType.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (valueType), Microsoft.IdentityModel.SR.GetString("ID0013"));
      if (!encodingType.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (encodingType), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._binaryData = binaryData;
      this._valueType = valueType;
      this._encodingType = encodingType;
    }

    public byte[] BinaryData => this._binaryData;

    public Uri ValueType => this._valueType;

    public Uri EncodingType => this._encodingType;
  }
}
