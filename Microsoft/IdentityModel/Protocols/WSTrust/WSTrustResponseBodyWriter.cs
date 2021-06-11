// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustResponseBodyWriter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustResponseBodyWriter : BodyWriter
  {
    private WSTrustResponseSerializer _serializer;
    private RequestSecurityTokenResponse _rstr;
    private WSTrustSerializationContext _context;

    public WSTrustResponseBodyWriter(
      RequestSecurityTokenResponse rstr,
      WSTrustResponseSerializer serializer,
      WSTrustSerializationContext context)
      : base(true)
    {
      if (serializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serializer));
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this._serializer = serializer;
      this._rstr = rstr;
      this._context = context;
    }

    protected override void OnWriteBodyContents(XmlDictionaryWriter writer) => this._serializer.WriteXml(this._rstr, (XmlWriter) writer, this._context);
  }
}
