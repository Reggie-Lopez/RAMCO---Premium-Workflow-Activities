// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.KeyTypes
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public static class KeyTypes
  {
    public const string Symmetric = "http://schemas.microsoft.com/idfx/keytype/symmetric";
    public const string Asymmetric = "http://schemas.microsoft.com/idfx/keytype/asymmetric";
    public const string Bearer = "http://schemas.microsoft.com/idfx/keytype/bearer";
  }
}
