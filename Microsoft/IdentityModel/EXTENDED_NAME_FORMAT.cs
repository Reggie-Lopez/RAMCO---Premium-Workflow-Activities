// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.EXTENDED_NAME_FORMAT
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel
{
  internal enum EXTENDED_NAME_FORMAT
  {
    NameUnknown = 0,
    NameFullyQualifiedDN = 1,
    NameSamCompatible = 2,
    NameDisplay = 3,
    NameUniqueId = 6,
    NameCanonical = 7,
    NameUserPrincipalName = 8,
    NameCanonicalEx = 9,
    NameServicePrincipalName = 10, // 0x0000000A
    NameDnsDomainName = 12, // 0x0000000C
  }
}
