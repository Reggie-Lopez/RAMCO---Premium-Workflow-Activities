// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.NotifyPropertyChangedInvocatorAttribute
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  internal sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
  {
    public NotifyPropertyChangedInvocatorAttribute()
    {
    }

    public NotifyPropertyChangedInvocatorAttribute(string parameterName) => this.ParameterName = parameterName;

    public string ParameterName { get; private set; }
  }
}
