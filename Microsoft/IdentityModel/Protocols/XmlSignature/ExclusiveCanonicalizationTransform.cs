// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.ExclusiveCanonicalizationTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal class ExclusiveCanonicalizationTransform : Transform
  {
    private bool _includeComments;
    private string _inclusiveNamespacesPrefixList;
    private string[] _inclusivePrefixes;
    private string _inclusiveListElementPrefix = "ec";
    private string _prefix = "ds";
    private readonly bool _isCanonicalizationMethod;

    public ExclusiveCanonicalizationTransform()
      : this(false)
    {
    }

    public ExclusiveCanonicalizationTransform(bool isCanonicalizationMethod)
      : this(isCanonicalizationMethod, false)
      => this._isCanonicalizationMethod = isCanonicalizationMethod;

    protected ExclusiveCanonicalizationTransform(
      bool isCanonicalizationMethod,
      bool includeComments)
    {
      this._isCanonicalizationMethod = isCanonicalizationMethod;
      this._includeComments = includeComments;
    }

    public override string Algorithm => !this._includeComments ? "http://www.w3.org/2001/10/xml-exc-c14n#" : "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

    public bool IncludeComments => this._includeComments;

    public string InclusiveNamespacesPrefixList
    {
      get => this._inclusiveNamespacesPrefixList;
      set
      {
        this._inclusiveNamespacesPrefixList = value;
        this._inclusivePrefixes = ExclusiveCanonicalizationTransform.TokenizeInclusivePrefixList(value);
      }
    }

    public override bool NeedsInclusiveContext => this.GetInclusivePrefixes() != null;

    public string[] GetInclusivePrefixes() => this._inclusivePrefixes;

    private CanonicalizationDriver GetConfiguredDriver()
    {
      CanonicalizationDriver canonicalizationDriver = new CanonicalizationDriver();
      canonicalizationDriver.IncludeComments = this.IncludeComments;
      canonicalizationDriver.SetInclusivePrefixes(this._inclusivePrefixes);
      return canonicalizationDriver;
    }

    public override object Process(object input)
    {
      if (input is XmlReader reader)
      {
        CanonicalizationDriver configuredDriver = this.GetConfiguredDriver();
        configuredDriver.SetInput(reader);
        return (object) configuredDriver.GetMemoryStream();
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6004", (object) input.GetType())));
    }

    public override byte[] ProcessAndDigest(object input, string digestAlgorithm)
    {
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(digestAlgorithm))
      {
        this.ProcessAndDigest(input, hashAlgorithm);
        return hashAlgorithm.Hash;
      }
    }

    public void ProcessAndDigest(object input, HashAlgorithm hash)
    {
      HashStream hashStream = new HashStream(hash);
      bool flag = false;
      if (input is XmlReader reader)
      {
        this.ProcessReaderInput(reader, hashStream);
        flag = true;
      }
      hashStream.FlushHash();
      hashStream.Dispose();
      if (!flag)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6004", (object) input.GetType())));
    }

    private void ProcessReaderInput(XmlReader reader, HashStream hashStream)
    {
      int content = (int) reader.MoveToContent();
      CanonicalizationDriver configuredDriver = this.GetConfiguredDriver();
      configuredDriver.SetInput(reader);
      configuredDriver.WriteTo((Stream) hashStream);
    }

    public override void ReadFrom(XmlDictionaryReader reader)
    {
      string localName = this._isCanonicalizationMethod ? "CanonicalizationMethod" : "Transform";
      reader.MoveToStartElement(localName, "http://www.w3.org/2000/09/xmldsig#");
      this._prefix = reader.Prefix;
      bool isEmptyElement1 = reader.IsEmptyElement;
      string attribute = reader.GetAttribute("Algorithm", (string) null);
      if (string.IsNullOrEmpty(attribute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Algorithm", (object) reader.LocalName)));
      if (attribute != "http://www.w3.org/2001/10/xml-exc-c14n#" && attribute != "http://www.w3.org/2001/10/xml-exc-c14n#WithComments")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6005", (object) attribute)));
      reader.Read();
      int content1 = (int) reader.MoveToContent();
      if (isEmptyElement1)
        return;
      if (reader.IsStartElement("InclusiveNamespaces", "http://www.w3.org/2001/10/xml-exc-c14n#"))
      {
        this._inclusiveListElementPrefix = reader.Prefix;
        bool isEmptyElement2 = reader.IsEmptyElement;
        this.InclusiveNamespacesPrefixList = reader.GetAttribute("PrefixList", (string) null);
        reader.Read();
        if (!isEmptyElement2)
          reader.ReadEndElement();
      }
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
    }

    public override void WriteTo(XmlDictionaryWriter writer)
    {
      string localName = this._isCanonicalizationMethod ? "CanonicalizationMethod" : "Transform";
      string text = this._includeComments ? "http://www.w3.org/2001/10/xml-exc-c14n#WithComments" : "http://www.w3.org/2001/10/xml-exc-c14n#";
      writer.WriteStartElement(this._prefix, localName, "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteStartAttribute("Algorithm", (string) null);
      if (text != null)
        writer.WriteString(text);
      writer.WriteEndAttribute();
      if (this.InclusiveNamespacesPrefixList != null)
      {
        writer.WriteStartElement(this._inclusiveListElementPrefix, "InclusiveNamespaces", "http://www.w3.org/2001/10/xml-exc-c14n#");
        writer.WriteAttributeString("PrefixList", (string) null, this.InclusiveNamespacesPrefixList);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private static string[] TokenizeInclusivePrefixList(string prefixList)
    {
      if (prefixList == null)
        return (string[]) null;
      string[] strArray1 = prefixList.Split((char[]) null);
      int length = 0;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        string str = strArray1[index];
        if (str == "#default")
          strArray1[length++] = string.Empty;
        else if (str.Length > 0)
          strArray1[length++] = str;
      }
      if (length == 0)
        return (string[]) null;
      if (length == strArray1.Length)
        return strArray1;
      string[] strArray2 = new string[length];
      Array.Copy((Array) strArray1, (Array) strArray2, length);
      return strArray2;
    }
  }
}
