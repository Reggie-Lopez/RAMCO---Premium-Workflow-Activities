// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.TransformChain
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class TransformChain
  {
    private string _prefix = "ds";
    private IList<Transform> _transforms = (IList<Transform>) new List<Transform>(2);

    public int TransformCount => this._transforms.Count;

    public Transform this[int index] => this._transforms[index];

    public bool NeedsInclusiveContext
    {
      get
      {
        for (int index = 0; index < this.TransformCount; ++index)
        {
          if (this[index].NeedsInclusiveContext)
            return true;
        }
        return false;
      }
    }

    public void Add(Transform transform) => this._transforms.Add(transform);

    public void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
    {
      reader.MoveToStartElement("Transforms", "http://www.w3.org/2000/09/xmldsig#");
      this._prefix = reader.Prefix;
      reader.Read();
      while (reader.IsStartElement("Transform", "http://www.w3.org/2000/09/xmldsig#"))
      {
        string attribute = reader.GetAttribute("Algorithm", (string) null);
        Transform transform = transformFactory.CreateTransform(attribute);
        transform.ReadFrom(reader);
        this.Add(transform);
      }
      int content = (int) reader.MoveToContent();
      reader.ReadEndElement();
      if (this.TransformCount == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6017")));
    }

    public byte[] TransformToDigest(object data, string digestMethod)
    {
      for (int index = 0; index < this.TransformCount - 1; ++index)
        data = this[index].Process(data);
      return this[this.TransformCount - 1].ProcessAndDigest(data, digestMethod);
    }

    public void WriteTo(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement(this._prefix, "Transforms", "http://www.w3.org/2000/09/xmldsig#");
      for (int index = 0; index < this.TransformCount; ++index)
        this[index].WriteTo(writer);
      writer.WriteEndElement();
    }
  }
}
