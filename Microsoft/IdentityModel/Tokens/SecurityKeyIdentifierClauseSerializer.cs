// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityKeyIdentifierClauseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public abstract class SecurityKeyIdentifierClauseSerializer
  {
    public abstract bool CanReadKeyIdentifierClause(XmlReader reader);

    public abstract bool CanWriteKeyIdentifierClause(
      SecurityKeyIdentifierClause securityKeyIdentifierClause);

    public abstract SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader);

    public abstract void WriteKeyIdentifierClause(
      XmlWriter writer,
      SecurityKeyIdentifierClause securityKeyIdentifierClause);
  }
}
