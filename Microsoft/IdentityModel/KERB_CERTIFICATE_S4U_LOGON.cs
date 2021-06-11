// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.KERB_CERTIFICATE_S4U_LOGON
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct KERB_CERTIFICATE_S4U_LOGON
  {
    internal KERB_LOGON_SUBMIT_TYPE MessageType;
    internal uint Flags;
    internal UNICODE_INTPTR_STRING UserPrincipalName;
    internal UNICODE_INTPTR_STRING DomainName;
    internal uint CertificateLength;
    internal IntPtr Certificate;
    internal static int Size = Marshal.SizeOf(typeof (KERB_CERTIFICATE_S4U_LOGON));
  }
}
