// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrust13RequestSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrust13RequestSerializer : WSTrustRequestSerializer
  {
    public override RequestSecurityToken ReadXml(
      XmlReader reader,
      WSTrustSerializationContext context)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      return WSTrustSerializationHelper.CreateRequest(reader, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
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
      if (reader.IsStartElement("SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
        rst.SecondaryParameters = this.ReadSecondaryParameters(reader, context);
      else if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
      {
        rst.KeyWrapAlgorithm = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rst.KeyWrapAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) "KeyWrapAlgorithm", (object) "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (object) rst.KeyWrapAlgorithm)));
      }
      else if (reader.IsStartElement("ValidateTarget", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
      {
        if (!reader.IsEmptyElement)
          rst.ValidateTarget = new SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.ValidateTarget == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3221")));
      }
      else
        WSTrustSerializationHelper.ReadRSTXml(reader, rst, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
    }

    protected virtual RequestSecurityToken ReadSecondaryParameters(
      XmlReader reader,
      WSTrustSerializationContext context)
    {
      RequestSecurityToken requestSecurityToken = this.CreateRequestSecurityToken();
      if (reader.IsEmptyElement)
      {
        reader.Read();
        int content = (int) reader.MoveToContent();
        return requestSecurityToken;
      }
      reader.ReadStartElement();
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
        {
          requestSecurityToken.KeyWrapAlgorithm = reader.ReadElementContentAsString();
          if (!UriUtil.CanCreateValidUri(requestSecurityToken.KeyWrapAlgorithm, UriKind.Absolute))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) "KeyWrapAlgorithm", (object) "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (object) requestSecurityToken.KeyWrapAlgorithm)));
        }
        else
        {
          if (reader.IsStartElement("SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3130")));
          WSTrustSerializationHelper.ReadRSTXml(reader, requestSecurityToken, context, WSTrustConstantsAdapter.GetConstantsAdapter(reader.NamespaceURI) ?? (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005);
        }
      }
      reader.ReadEndElement();
      return requestSecurityToken;
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
      WSTrustSerializationHelper.WriteKnownRequestElement(rst, writer, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
      if (!string.IsNullOrEmpty(rst.KeyWrapAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri(rst.KeyWrapAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) "KeyWrapAlgorithm", (object) "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (object) rst.KeyWrapAlgorithm)));
        this.WriteXmlElement(writer, "KeyWrapAlgorithm", (object) rst.KeyWrapAlgorithm, rst, context);
      }
      if (rst.SecondaryParameters != null)
        this.WriteXmlElement(writer, "SecondaryParameters", (object) rst.SecondaryParameters, rst, context);
      if (rst.ValidateTarget == null)
        return;
      this.WriteXmlElement(writer, "ValidateTarget", (object) rst.ValidateTarget, rst, context);
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
      WSTrustSerializationHelper.WriteRequest(request, writer, context, (WSTrustRequestSerializer) this, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
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
      if (StringComparer.Ordinal.Equals(elementName, "SecondaryParameters"))
      {
        if (!(elementValue is RequestSecurityToken rst1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2064", (object) "SecondaryParameters")));
        if (rst1.SecondaryParameters != null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID2055")));
        writer.WriteStartElement("trust", "SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
        this.WriteKnownRequestElement(rst1, writer, context);
        foreach (KeyValuePair<string, object> property in rst1.Properties)
          this.WriteXmlElement(writer, property.Key, property.Value, rst, context);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "KeyWrapAlgorithm"))
        writer.WriteElementString("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (string) elementValue);
      else if (StringComparer.Ordinal.Equals(elementName, "ValidateTarget"))
      {
        if (!(elementValue is SecurityTokenElement securityTokenElement))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) "ValidateTarget", (object) "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (object) typeof (SecurityTokenElement), elementValue));
        writer.WriteStartElement("trust", "ValidateTarget", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else
        WSTrustSerializationHelper.WriteRSTXml(writer, elementName, elementValue, context, (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13);
    }

    public override bool CanRead(XmlReader reader) => reader != null ? reader.IsStartElement("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
  }
}
