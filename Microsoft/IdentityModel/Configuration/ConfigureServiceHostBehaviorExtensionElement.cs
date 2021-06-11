// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ConfigureServiceHostBehaviorExtensionElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ConfigureServiceHostBehaviorExtensionElement : BehaviorExtensionElement
  {
    private string _serviceName = "";

    public override Type BehaviorType => typeof (ConfigureServiceHostServiceBehavior);

    protected override object CreateBehavior() => (object) new ConfigureServiceHostServiceBehavior(this._serviceName);

    protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(reader.ReadOuterXml());
      XmlAttribute attribute = xmlDocument.DocumentElement.Attributes["name"];
      if (attribute != null)
        this._serviceName = attribute.Value;
      using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xmlDocument.DocumentElement.OuterXml), XmlDictionaryReaderQuotas.Max))
      {
        textReader.Read();
        base.DeserializeElement(textReader, serializeCollectionKey);
      }
    }

    protected override bool OnDeserializeUnrecognizedAttribute(string name, string value) => true;
  }
}
