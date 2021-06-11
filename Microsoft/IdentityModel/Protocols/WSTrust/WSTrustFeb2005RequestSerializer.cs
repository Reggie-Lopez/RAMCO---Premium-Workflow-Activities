// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustFeb2005RequestSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustFeb2005RequestSerializer : WSTrustRequestSerializer
  {
    public override RequestSecurityToken ReadXml(
      XmlReader reader,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      return WSTrustSerializationHelper.CreateRequest(reader, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void ReadXmlElement(
      XmlReader reader,
      RequestSecurityToken rst,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.ReadRSTXml(reader, rst, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteKnownRequestElement(
      RequestSecurityToken rst,
      XmlWriter writer,
      WSTrustSerializationContext context)
    {
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteKnownRequestElement(rst, writer, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteXml(
      RequestSecurityToken request,
      XmlWriter writer,
      WSTrustSerializationContext context)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteRequest(request, writer, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override void WriteXmlElement(
      XmlWriter writer,
      string elementName,
      object elementValue,
      RequestSecurityToken rst,
      WSTrustSerializationContext context)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (string.IsNullOrEmpty(elementName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (elementName));
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      WSTrustSerializationHelper.WriteRSTXml(writer, elementName, elementValue, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
    }

    public override bool CanRead(XmlReader reader) => reader != null ? reader.IsStartElement("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
  }
}
