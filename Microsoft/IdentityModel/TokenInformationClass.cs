// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.TokenInformationClass
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel
{
  internal enum TokenInformationClass : uint
  {
    TokenUser = 1,
    TokenGroups = 2,
    TokenPrivileges = 3,
    TokenOwner = 4,
    TokenPrimaryGroup = 5,
    TokenDefaultDacl = 6,
    TokenSource = 7,
    TokenType = 8,
    TokenImpersonationLevel = 9,
    TokenStatistics = 10, // 0x0000000A
    TokenRestrictedSids = 11, // 0x0000000B
    TokenSessionId = 12, // 0x0000000C
    TokenGroupsAndPrivileges = 13, // 0x0000000D
    TokenSessionReference = 14, // 0x0000000E
    TokenSandBoxInert = 15, // 0x0000000F
  }
}
