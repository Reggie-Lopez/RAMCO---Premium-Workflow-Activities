// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings.MemberBinding
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings
{
  [ComVisible(true)]
  public abstract class MemberBinding
  {
    private readonly MemberInfo _boundMember;
    private readonly Expression _associatedExpression;

    public static MemberBinding Bind(
      MemberInfo boundMember,
      Expression associatedExpression)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (boundMember), boundMember);
      ArgumentUtility.CheckNotNull<Expression>(nameof (associatedExpression), associatedExpression);
      MethodInfo boundMember1 = boundMember as MethodInfo;
      if (boundMember1 != (MethodInfo) null)
        return (MemberBinding) new MethodInfoBinding(boundMember1, associatedExpression);
      PropertyInfo boundMember2 = boundMember as PropertyInfo;
      return boundMember2 != (PropertyInfo) null ? (MemberBinding) new PropertyInfoBinding(boundMember2, associatedExpression) : (MemberBinding) new FieldInfoBinding((FieldInfo) boundMember, associatedExpression);
    }

    public MemberInfo BoundMember => this._boundMember;

    public Expression AssociatedExpression => this._associatedExpression;

    public MemberBinding(MemberInfo boundMember, Expression associatedExpression)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (boundMember), boundMember);
      ArgumentUtility.CheckNotNull<Expression>(nameof (associatedExpression), associatedExpression);
      this._boundMember = boundMember;
      this._associatedExpression = associatedExpression;
    }

    public abstract bool MatchesReadAccess(MemberInfo member);
  }
}
