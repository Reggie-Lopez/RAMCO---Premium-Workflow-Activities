// Decompiled with JetBrains decompiler
// Type: SS.Crm.Linq.AnonymousBinding
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Linq.Expressions;
using System.Reflection;

namespace SS.Crm.Linq
{
  internal class AnonymousBinding
  {
    public Expression SourceExpression { get; set; }

    public MemberInfo TargetMember { get; set; }
  }
}
