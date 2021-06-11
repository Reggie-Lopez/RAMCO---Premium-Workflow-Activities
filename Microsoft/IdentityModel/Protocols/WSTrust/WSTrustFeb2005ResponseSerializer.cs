// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustFeb2005ResponseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustFeb2005ResponseSerializer : WSTrustResponseSerializer
  {
    public override RequestSecurityTokenResponse ReadXml(
      XmlReader reader,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      return WSTrustSerializationHelper.CreateResponse(reader, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void ReadXmlElement(
      XmlReader reader,
      RequestSecurityTokenResponse rstr,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.ReadRSTRXml(reader, rstr, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteKnownResponseElement(
      RequestSecurityTokenResponse rstr,
      XmlWriter writer,
      WSTrustSerializationContext context)
    {
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteKnownResponseElement(rstr, writer, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteXml(
      RequestSecurityTokenResponse response,
      XmlWriter writer,
      WSTrustSerializationContext context)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteResponse(response, writer, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteXmlElement(
      XmlWriter writer,
      string elementName,
      object elementValue,
      RequestSecurityTokenResponse rstr,
      WSTrustSerializationContext context)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (string.IsNullOrEmpty(elementName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (elementName));
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteRSTRXml(writer, elementName, elementValue, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override bool CanRead(XmlReader reader) => reader != null ? reader.IsStartElement("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
  }
}
