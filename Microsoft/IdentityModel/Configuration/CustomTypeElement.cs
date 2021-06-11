// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.CustomTypeElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class CustomTypeElement : ConfigurationElementInterceptor
  {
    public CustomTypeElement()
    {
    }

    internal CustomTypeElement(Type typeName) => this.TypeName = typeName;

    public static T Resolve<T>(CustomTypeElement customTypeElement, params object[] arguments) where T : class
    {
      if (customTypeElement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customTypeElement));
      try
      {
        Type typeName = customTypeElement.TypeName;
        if (!typeof (T).IsAssignableFrom(typeName))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) customTypeElement, "type", Microsoft.IdentityModel.SR.GetString("ID1029", (object) typeName.AssemblyQualifiedName, (object) typeof (T)));
        object[] args = (object[]) null;
        if (customTypeElement.ElementAsXml != null)
        {
          foreach (XmlNode childNode in customTypeElement.ElementAsXml.ChildNodes)
          {
            if (childNode.NodeType != XmlNodeType.Element)
              customTypeElement.ElementAsXml.RemoveChild(childNode);
          }
        }
        if (customTypeElement.ElementAsXml == null || customTypeElement.ElementAsXml.ChildNodes.Count == 0)
        {
          if (arguments != null && arguments.Length > 0)
            args = arguments;
        }
        else if (arguments != null && arguments.Length > 0)
        {
          args = new object[1]
          {
            (object) customTypeElement.ElementAsXml.ChildNodes
          };
        }
        else
        {
          args = new object[1 + arguments.Length];
          args[0] = (object) customTypeElement.ElementAsXml.ChildNodes;
          for (int index = 0; index < arguments.Length; ++index)
            args[index + 1] = arguments[index];
        }
        return (T) Activator.CreateInstance(typeName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, args, (CultureInfo) null);
      }
      catch (Exception ex)
      {
        if (ex is ConfigurationErrorsException || DiagnosticUtil.ExceptionUtil.IsFatal(ex))
        {
          throw;
        }
        else
        {
          if (ex is TargetInvocationException)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ConfigurationErrorsException(Microsoft.IdentityModel.SR.GetString("ID0012", (object) customTypeElement.TypeName.AssemblyQualifiedName), ex));
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) customTypeElement, "type", ex);
        }
      }
    }

    public bool IsConfigured => (object) this.TypeName != null;

    [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
    [TypeConverter(typeof (TypeNameConverter))]
    public Type TypeName
    {
      get => (Type) this["type"];
      set => this["type"] = (object) value;
    }
  }
}
