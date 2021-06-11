// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Threading.AsyncResult
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.IdentityModel.Threading
{
  [ComVisible(true)]
  public abstract class AsyncResult : IAsyncResult, IDisposable
  {
    private AsyncCallback _callback;
    private bool _completed;
    private bool _completedSync;
    private bool _disposed;
    private bool _endCalled;
    private Exception _exception;
    private ManualResetEvent _event;
    private object _state;
    private object _thisLock;

    public static void End(IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      if (!(result is AsyncResult asyncResult))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID4001"), nameof (result)));
      asyncResult._endCalled = !asyncResult._endCalled ? true : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4002")));
      if (!asyncResult._completed)
        asyncResult.AsyncWaitHandle.WaitOne();
      if (asyncResult._event != null)
        asyncResult._event.Dispose();
      if (asyncResult._exception != null)
        throw asyncResult._exception;
    }

    protected AsyncResult()
      : this((AsyncCallback) null, (object) null)
    {
    }

    protected AsyncResult(object state)
      : this((AsyncCallback) null, state)
    {
    }

    protected AsyncResult(AsyncCallback callback, object state)
    {
      this._thisLock = new object();
      this._callback = callback;
      this._state = state;
    }

    ~AsyncResult() => this.Dispose(false);

    protected void Complete(bool completedSynchronously) => this.Complete(completedSynchronously, (Exception) null);

    protected void Complete(bool completedSynchronously, Exception exception)
    {
      if (this._completed)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AsynchronousOperationException(Microsoft.IdentityModel.SR.GetString("ID4005")));
      this._completedSync = completedSynchronously;
      this._exception = exception;
      if (completedSynchronously)
      {
        this._completed = true;
      }
      else
      {
        lock (this._thisLock)
        {
          this._completed = true;
          if (this._event != null)
            this._event.Set();
        }
      }
      try
      {
        if (this._callback == null)
          return;
        this._callback((IAsyncResult) this);
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (AsynchronousOperationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new AsynchronousOperationException(Microsoft.IdentityModel.SR.GetString("ID4003"), ex));
      }
    }

    protected virtual void Dispose(bool isExplicitDispose)
    {
      if (this._disposed || !isExplicitDispose)
        return;
      lock (this._thisLock)
      {
        if (this._disposed)
          return;
        this._disposed = true;
        if (this._event == null)
          return;
        this._event.Close();
      }
    }

    public object AsyncState => this._state;

    public virtual WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this._event == null)
        {
          bool completed = this._completed;
          lock (this._thisLock)
          {
            if (this._event == null)
              this._event = new ManualResetEvent(this._completed);
          }
          if (!completed && this._completed)
            this._event.Set();
        }
        return (WaitHandle) this._event;
      }
    }

    public bool CompletedSynchronously => this._completedSync;

    public bool IsCompleted => this._completed;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
