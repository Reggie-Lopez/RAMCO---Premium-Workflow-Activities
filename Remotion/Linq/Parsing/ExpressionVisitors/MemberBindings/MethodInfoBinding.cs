// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MethodInfoBinding
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings
{
  [ComVisible(true)]
  public class MethodInfoBinding : MemberBinding
  {
    public MethodInfoBinding(MethodInfo boundMember, Expression associatedExpression)
      : base((MemberInfo) boundMember, associatedExpression)
    {
    }

    public override bool MatchesReadAccess(MemberInfo readMember)
    {
      if (readMember == this.BoundMember)
        return true;
      PropertyInfo propertyInfo = readMember as PropertyInfo;
      return propertyInfo != (PropertyInfo) null && propertyInfo.CanRead && (MemberInfo) propertyInfo.GetGetMethod(true) == this.BoundMember;
    }
  }
}
