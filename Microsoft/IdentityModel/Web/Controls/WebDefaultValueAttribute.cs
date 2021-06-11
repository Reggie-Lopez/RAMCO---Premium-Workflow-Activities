// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.WebDefaultValueAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;

namespace Microsoft.IdentityModel.Web.Controls
{
  [AttributeUsage(AttributeTargets.All)]
  internal sealed class WebDefaultValueAttribute : DefaultValueAttribute
  {
    private Type _type;
    private bool _localized;

    public WebDefaultValueAttribute(string value)
      : this((Type) null, value)
    {
    }

    public WebDefaultValueAttribute(Type valueType, string value)
      : base(value)
      => this._type = valueType;

    public override object TypeId => (object) typeof (DefaultValueAttribute);

    public override object Value
    {
      get
      {
        if (!this._localized)
        {
          this._localized = true;
          string name = (string) base.Value;
          if (!string.IsNullOrEmpty(name))
          {
            object obj = (object) Microsoft.IdentityModel.SR.GetString(name);
            if ((object) this._type != null)
            {
              try
              {
                obj = TypeDescriptor.GetConverter(this._type).ConvertFromInvariantString((string) obj);
              }
              catch (NotSupportedException ex)
              {
                obj = (object) null;
              }
            }
            this.SetValue(obj);
          }
        }
        return base.Value;
      }
    }
  }
}
