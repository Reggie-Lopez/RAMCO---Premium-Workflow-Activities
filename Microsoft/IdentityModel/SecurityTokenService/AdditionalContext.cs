// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.AdditionalContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class AdditionalContext
  {
    private List<ContextItem> _contextItems = new List<ContextItem>();

    public AdditionalContext()
    {
    }

    public AdditionalContext(IEnumerable<ContextItem> items)
    {
      if (items == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (items));
      foreach (ContextItem contextItem in items)
        this._contextItems.Add(contextItem);
    }

    public IList<ContextItem> Items => (IList<ContextItem>) this._contextItems;
  }
}
