// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.TransformFactory
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal class TransformFactory
  {
    private static TransformFactory _instance = new TransformFactory();

    protected TransformFactory()
    {
    }

    internal static TransformFactory Instance => TransformFactory._instance;

    public virtual Transform CreateTransform(string transformAlgorithmUri)
    {
      if (transformAlgorithmUri == "http://www.w3.org/2001/10/xml-exc-c14n#")
        return (Transform) new ExclusiveCanonicalizationTransform();
      if (transformAlgorithmUri == "http://www.w3.org/2000/09/xmldsig#enveloped-signature")
        return (Transform) new EnvelopedSignatureTransform();
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6021")));
    }
  }
}
