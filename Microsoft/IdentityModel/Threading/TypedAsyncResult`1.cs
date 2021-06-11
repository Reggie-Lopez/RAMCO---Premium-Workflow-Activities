// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Threading.TypedAsyncResult`1
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Threading
{
  [ComVisible(true)]
  public class TypedAsyncResult<T> : AsyncResult
  {
    private T _result;

    public TypedAsyncResult(object state)
      : base(state)
    {
    }

    public TypedAsyncResult(AsyncCallback callback, object state)
      : base(callback, state)
    {
    }

    public void Complete(T result, bool completedSynchronously)
    {
      this._result = result;
      this.Complete(completedSynchronously);
    }

    public void Complete(T result, bool completedSynchronously, Exception exception)
    {
      this._result = result;
      this.Complete(completedSynchronously, exception);
    }

    public static T End(IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      if (!(result is TypedAsyncResult<T> typedAsyncResult))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (result), Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (TypedAsyncResult<T>), (object) result.GetType()));
      AsyncResult.End((IAsyncResult) typedAsyncResult);
      return typedAsyncResult.Result;
    }

    public T Result => this._result;
  }
}
