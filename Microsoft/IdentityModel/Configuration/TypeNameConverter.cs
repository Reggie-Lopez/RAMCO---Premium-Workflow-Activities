// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.TypeNameConverter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Compilation;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public sealed class TypeNameConverter : ConfigurationConverterBase
  {
    public override object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if ((object) (value as Type) == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID7008", value));
      string str = (string) null;
      if (value != null)
        str = ((Type) value).AssemblyQualifiedName;
      return (object) str;
    }

    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      Type type = BuildManager.GetType((string) value, true);
      return (object) type != null ? (object) type : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID7007", value));
    }
  }
}
