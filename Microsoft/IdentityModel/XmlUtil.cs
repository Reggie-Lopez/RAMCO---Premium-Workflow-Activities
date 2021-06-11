// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.XmlUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel
{
  internal static class XmlUtil
  {
    public const string LanguageNamespaceUri = "http://www.w3.org/XML/1998/namespace";
    public const string LanguagePrefix = "xml";
    public const string LanguageLocalname = "lang";
    public const string LanguageAttribute = "xml:lang";

    public static void WriteLanguageAttribute(XmlWriter writer, string value) => writer.WriteAttributeString("xml", "lang", (string) null, value);

    public static XmlQualifiedName GetXsiType(XmlReader reader)
    {
      string attribute = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
      reader.MoveToElement();
      return string.IsNullOrEmpty(attribute) ? (XmlQualifiedName) null : XmlUtil.ResolveQName(reader, attribute);
    }

    public static bool EqualsQName(XmlQualifiedName qname, string localName, string namespaceUri) => (XmlQualifiedName) null != qname && StringComparer.Ordinal.Equals(localName, qname.Name) && StringComparer.Ordinal.Equals(namespaceUri, qname.Namespace);

    public static bool IsNil(XmlReader reader)
    {
      string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
      return !string.IsNullOrEmpty(attribute) && XmlConvert.ToBoolean(attribute);
    }

    public static string NormalizeEmptyString(string s) => !string.IsNullOrEmpty(s) ? s : (string) null;

    public static XmlQualifiedName ResolveQName(XmlReader reader, string qstring)
    {
      string name = qstring;
      string prefix = string.Empty;
      int length = qstring.IndexOf(':');
      if (length > -1)
      {
        prefix = qstring.Substring(0, length);
        name = qstring.Substring(length + 1, qstring.Length - (length + 1));
      }
      string ns = reader.LookupNamespace(prefix);
      return new XmlQualifiedName(name, ns);
    }

    public static void ValidateXsiType(
      XmlReader reader,
      string expectedTypeName,
      string expectedTypeNamespace)
    {
      XmlUtil.ValidateXsiType(reader, expectedTypeName, expectedTypeNamespace, false);
    }

    public static void ValidateXsiType(
      XmlReader reader,
      string expectedTypeName,
      string expectedTypeNamespace,
      bool requireDeclaration)
    {
      XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
      if ((XmlQualifiedName) null == xsiType)
      {
        if (requireDeclaration)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4104", (object) reader.LocalName, (object) reader.NamespaceURI));
      }
      else if (!StringComparer.Ordinal.Equals(expectedTypeNamespace, xsiType.Namespace) || !StringComparer.Ordinal.Equals(expectedTypeName, xsiType.Name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4102", (object) expectedTypeName, (object) expectedTypeNamespace, (object) xsiType.Name, (object) xsiType.Namespace));
    }

    public static string SerializeSecurityKeyIdentifier(
      SecurityKeyIdentifier ski,
      SecurityTokenSerializer securityTokenSerializer)
    {
      StringBuilder sb = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (XmlWriter writer = XmlWriter.Create((TextWriter) stringWriter, new XmlWriterSettings()
        {
          OmitXmlDeclaration = true
        }))
          securityTokenSerializer.WriteKeyIdentifier(writer, ski);
      }
      return sb.ToString();
    }

    public static bool IsValidXmlIDValue(string val)
    {
      if (string.IsNullOrEmpty(val))
        return false;
      return val[0] >= 'A' && val[0] <= 'Z' || val[0] >= 'a' && val[0] <= 'z' || val[0] == '_' || val[0] == ':';
    }

    public static void WriteElementStringAsUniqueId(
      XmlDictionaryWriter writer,
      XmlDictionaryString localName,
      XmlDictionaryString ns,
      string id)
    {
      writer.WriteStartElement(localName, ns);
      writer.WriteValue(id);
      writer.WriteEndElement();
    }

    public static void WriteElementContentAsInt64(
      XmlDictionaryWriter writer,
      XmlDictionaryString localName,
      XmlDictionaryString ns,
      long value)
    {
      writer.WriteStartElement(localName, ns);
      writer.WriteValue(value);
      writer.WriteEndElement();
    }

    public static long ReadElementContentAsInt64(XmlDictionaryReader reader)
    {
      reader.ReadFullStartElement();
      long num = reader.ReadContentAsLong();
      reader.ReadEndElement();
      return num;
    }

    public static List<XmlElement> GetXmlElements(XmlNodeList nodeList)
    {
      if (nodeList == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (nodeList));
      List<XmlElement> xmlElementList = new List<XmlElement>();
      foreach (XmlNode node in nodeList)
      {
        if (node is XmlElement xmlElement)
          xmlElementList.Add(xmlElement);
      }
      return xmlElementList;
    }
  }
}
