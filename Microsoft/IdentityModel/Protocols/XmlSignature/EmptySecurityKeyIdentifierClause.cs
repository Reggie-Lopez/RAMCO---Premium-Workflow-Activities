// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EmptySecurityKeyIdentifierClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public class EmptySecurityKeyIdentifierClause : SecurityKeyIdentifierClause
  {
    private object _context;

    public EmptySecurityKeyIdentifierClause()
      : this((object) null)
    {
    }

    public EmptySecurityKeyIdentifierClause(object context)
      : base(typeof (EmptySecurityKeyIdentifierClause).ToString())
      => this._context = context;

    public object Context => this._context;
  }
}
