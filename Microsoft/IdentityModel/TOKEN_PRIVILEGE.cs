// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.TOKEN_PRIVILEGE
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  internal struct TOKEN_PRIVILEGE
  {
    internal uint PrivilegeCount;
    internal LUID_AND_ATTRIBUTES Privilege;
    internal static readonly uint Size = (uint) Marshal.SizeOf(typeof (TOKEN_PRIVILEGE));
  }
}
