// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustRequestBodyWriter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustRequestBodyWriter : BodyWriter
  {
    private WSTrustSerializationContext _serializationContext;
    private RequestSecurityToken _requestSecurityToken;
    private WSTrustRequestSerializer _serializer;

    public WSTrustRequestBodyWriter(
      RequestSecurityToken requestSecurityToken,
      WSTrustRequestSerializer serializer,
      WSTrustSerializationContext serializationContext)
      : base(true)
    {
      if (requestSecurityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSecurityToken));
      if (serializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serializer));
      if (serializationContext == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serializationContext));
      this._requestSecurityToken = requestSecurityToken;
      this._serializer = serializer;
      this._serializationContext = serializationContext;
    }

    protected override void OnWriteBodyContents(XmlDictionaryWriter writer) => this._serializer.WriteXml(this._requestSecurityToken, (XmlWriter) writer, this._serializationContext);
  }
}
