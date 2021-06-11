// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrust13ResponseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrust13ResponseSerializer : WSTrustResponseSerializer
  {
    public override RequestSecurityTokenResponse ReadXml(
      XmlReader reader,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      bool flag = false;
      if (reader.IsStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
      {
        reader.ReadStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
        flag = true;
      }
      RequestSecurityTokenResponse response = WSTrustSerializationHelper.CreateResponse(reader, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
      response.IsFinal = flag;
      if (flag)
        reader.ReadEndElement();
      return response;
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
      if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
        rstr.KeyWrapAlgorithm = reader.ReadElementContentAsString();
      else
        WSTrustSerializationHelper.ReadRSTRXml(reader, rstr, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
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
      WSTrustSerializationHelper.WriteKnownResponseElement(rstr, writer, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
      if (string.IsNullOrEmpty(rstr.KeyWrapAlgorithm))
        return;
      this.WriteXmlElement(writer, "KeyWrapAlgorithm", (object) rstr.KeyWrapAlgorithm, rstr, context);
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
      if (response.IsFinal)
        writer.WriteStartElement("trust", "RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      WSTrustSerializationHelper.WriteResponse(response, writer, context, (WSTrustResponseSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
      if (!response.IsFinal)
        return;
      writer.WriteEndElement();
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
      if (StringComparer.Ordinal.Equals(elementName, "KeyWrapAlgorithm"))
        writer.WriteElementString("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (string) elementValue);
      else
        WSTrustSerializationHelper.WriteRSTRXml(writer, elementName, elementValue, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
    }

    public override bool CanRead(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return reader.IsStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512") || reader.IsStartElement("RequestSecurityTokenResponse", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
    }
  }
}
