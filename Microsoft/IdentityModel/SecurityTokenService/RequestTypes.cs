// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.RequestTypes
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public static class RequestTypes
  {
    public const string Cancel = "http://schemas.microsoft.com/idfx/requesttype/cancel";
    public const string Issue = "http://schemas.microsoft.com/idfx/requesttype/issue";
    public const string Renew = "http://schemas.microsoft.com/idfx/requesttype/renew";
    public const string Validate = "http://schemas.microsoft.com/idfx/requesttype/validate";
    public const string IssueCard = "http://schemas.microsoft.com/idfx/requesttype/issueCard";
    public const string GetMetadata = "http://schemas.microsoft.com/idfx/requesttype/getMetadata";
  }
}
