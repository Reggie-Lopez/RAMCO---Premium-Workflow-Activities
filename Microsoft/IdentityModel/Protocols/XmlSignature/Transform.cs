// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.Transform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal abstract class Transform
  {
    public abstract string Algorithm { get; }

    public virtual bool NeedsInclusiveContext => false;

    public abstract object Process(object input);

    public abstract byte[] ProcessAndDigest(object input, string digestAlgorithm);

    public abstract void ReadFrom(XmlDictionaryReader reader);

    public abstract void WriteTo(XmlDictionaryWriter writer);
  }
}
