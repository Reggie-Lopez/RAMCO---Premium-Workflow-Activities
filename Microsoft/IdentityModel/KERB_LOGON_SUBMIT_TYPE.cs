// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.KERB_LOGON_SUBMIT_TYPE
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel
{
  internal enum KERB_LOGON_SUBMIT_TYPE
  {
    KerbInteractiveLogon = 2,
    KerbSmartCardLogon = 6,
    KerbWorkstationUnlockLogon = 7,
    KerbSmartCardUnlockLogon = 8,
    KerbProxyLogon = 9,
    KerbTicketLogon = 10, // 0x0000000A
    KerbTicketUnlockLogon = 11, // 0x0000000B
    KerbS4ULogon = 12, // 0x0000000C
    KerbCertificateLogon = 13, // 0x0000000D
    KerbCertificateS4ULogon = 14, // 0x0000000E
    KerbCertificateUnlockLogon = 15, // 0x0000000F
  }
}
