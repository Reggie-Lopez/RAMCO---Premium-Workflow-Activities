// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.AudienceUriModeConverter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class AudienceUriModeConverter : ConfigurationConverterBase
  {
    public override object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if (!(value is AudienceUriMode))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID7008", value));
      string str = (string) null;
      if (value != null)
        str = Enum.GetName(typeof (AudienceUriMode), value);
      return (object) str;
    }

    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      return value is string str ? (object) (AudienceUriMode) Enum.Parse(typeof (AudienceUriMode), str) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID7008", value));
    }
  }
}
