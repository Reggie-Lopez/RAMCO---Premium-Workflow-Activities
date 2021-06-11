// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.AuthenticationContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class AuthenticationContext
  {
    private Collection<string> _authorities;
    private string _contextClass;
    private string _contextDeclaration;

    public AuthenticationContext() => this._authorities = new Collection<string>();

    public Collection<string> Authorities => this._authorities;

    public string ContextClass
    {
      get => this._contextClass;
      set => this._contextClass = value;
    }

    public string ContextDeclaration
    {
      get => this._contextDeclaration;
      set => this._contextDeclaration = value;
    }
  }
}
