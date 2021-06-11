// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.TOKEN_GROUPS
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct TOKEN_GROUPS
  {
    internal uint GroupCount;
    internal SID_AND_ATTRIBUTES Groups;
  }
}
