// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ConfigurationElementInterceptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ConfigurationElementInterceptor : ConfigurationElement
  {
    private XmlDocument elementXml;

    protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
    {
      this.elementXml = new XmlDocument();
      this.elementXml.LoadXml(reader.ReadOuterXml());
      using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(this.elementXml.DocumentElement.OuterXml), XmlDictionaryReaderQuotas.Max))
      {
        textReader.Read();
        base.DeserializeElement(textReader, serializeCollectionKey);
      }
    }

    protected override bool OnDeserializeUnrecognizedAttribute(string name, string value) => true;

    protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader) => true;

    protected override void Reset(ConfigurationElement parentElement)
    {
      base.Reset(parentElement);
      this.Reset((ConfigurationElementInterceptor) parentElement);
    }

    public XmlElement ElementAsXml => this.elementXml != null ? this.elementXml.DocumentElement : (XmlElement) null;

    public XmlNodeList ChildNodes => this.elementXml != null && this.ElementAsXml.ChildNodes.Count != 0 ? this.ElementAsXml.ChildNodes : (XmlNodeList) null;

    private void Reset(ConfigurationElementInterceptor parentElement) => this.elementXml = parentElement.elementXml;
  }
}
