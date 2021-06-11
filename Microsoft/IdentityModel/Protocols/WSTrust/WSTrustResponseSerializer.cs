// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustResponseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public abstract class WSTrustResponseSerializer
  {
    public abstract RequestSecurityTokenResponse ReadXml(
      XmlReader reader,
      WSTrustSerializationContext context);

    public abstract void ReadXmlElement(
      XmlReader reader,
      RequestSecurityTokenResponse rstr,
      WSTrustSerializationContext context);

    public abstract void WriteKnownResponseElement(
      RequestSecurityTokenResponse rstr,
      XmlWriter writer,
      WSTrustSerializationContext context);

    public abstract void WriteXml(
      RequestSecurityTokenResponse response,
      XmlWriter writer,
      WSTrustSerializationContext context);

    public abstract void WriteXmlElement(
      XmlWriter writer,
      string elementName,
      object elementValue,
      RequestSecurityTokenResponse rstr,
      WSTrustSerializationContext context);

    public virtual RequestSecurityTokenResponse CreateInstance() => new RequestSecurityTokenResponse();

    public virtual void Validate(RequestSecurityTokenResponse rstr)
    {
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
    }

    public abstract bool CanRead(XmlReader reader);
  }
}
