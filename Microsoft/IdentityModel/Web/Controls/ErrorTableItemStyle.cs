// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Controls.ErrorTableItemStyle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
  internal sealed class ErrorTableItemStyle : TableItemStyle, ICustomTypeDescriptor
  {
    public ErrorTableItemStyle() => this.ForeColor = Color.Red;

    AttributeCollection ICustomTypeDescriptor.GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

    string ICustomTypeDescriptor.GetClassName() => TypeDescriptor.GetClassName((object) this, true);

    string ICustomTypeDescriptor.GetComponentName() => TypeDescriptor.GetComponentName((object) this, true);

    TypeConverter ICustomTypeDescriptor.GetConverter() => TypeDescriptor.GetConverter((object) this, true);

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => TypeDescriptor.GetDefaultEvent((object) this, true);

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => TypeDescriptor.GetDefaultProperty((object) this, true);

    object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor((object) this, editorBaseType, true);

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => TypeDescriptor.GetEvents((object) this, true);

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(
      Attribute[] attributes)
    {
      return TypeDescriptor.GetEvents((object) this, attributes, true);
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => ((ICustomTypeDescriptor) this).GetProperties((Attribute[]) null);

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(
      Attribute[] attributes)
    {
      PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties(this.GetType(), attributes);
      PropertyDescriptor[] properties2 = new PropertyDescriptor[properties1.Count];
      PropertyDescriptor propertyDescriptor = properties1["ForeColor"];
      for (int index = 0; index < properties1.Count; ++index)
      {
        PropertyDescriptor oldPropertyDescriptor = properties1[index];
        if (oldPropertyDescriptor == propertyDescriptor)
          properties2[index] = TypeDescriptor.CreateProperty(this.GetType(), oldPropertyDescriptor, (Attribute) new DefaultValueAttribute(typeof (Color), "Red"));
        else
          properties2[index] = oldPropertyDescriptor;
      }
      return new PropertyDescriptorCollection(properties2, true);
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => (object) this;
  }
}
