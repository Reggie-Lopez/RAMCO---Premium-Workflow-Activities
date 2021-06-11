// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.AbsoluteUriCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  internal class AbsoluteUriCollection : Collection<Uri>
  {
    protected override void InsertItem(int index, Uri item)
    {
      if ((Uri) null == item || !item.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (item), Microsoft.IdentityModel.SR.GetString("ID0013"));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, Uri item)
    {
      if ((Uri) null == item || !item.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (item), Microsoft.IdentityModel.SR.GetString("ID0013"));
      base.SetItem(index, item);
    }
  }
}
