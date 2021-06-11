// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.LocalizationRequiredAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  internal sealed class LocalizationRequiredAttribute : Attribute
  {
    public LocalizationRequiredAttribute()
      : this(true)
    {
    }

    public LocalizationRequiredAttribute(bool required) => this.Required = required;

    public bool Required { get; private set; }

    public override bool Equals(object obj) => obj is LocalizationRequiredAttribute requiredAttribute && requiredAttribute.Required == this.Required;

    public override int GetHashCode() => base.GetHashCode();
  }
}
