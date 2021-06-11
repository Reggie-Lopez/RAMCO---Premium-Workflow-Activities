// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.WebCategoryAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;

namespace Microsoft.IdentityModel.Web.Controls
{
  [AttributeUsage(AttributeTargets.All)]
  internal sealed class WebCategoryAttribute : CategoryAttribute
  {
    public WebCategoryAttribute(string category)
      : base(category)
    {
    }

    public override object TypeId => (object) typeof (CategoryAttribute);

    protected override string GetLocalizedString(string value) => base.GetLocalizedString(value) ?? Microsoft.IdentityModel.SR.GetString(value);
  }
}
