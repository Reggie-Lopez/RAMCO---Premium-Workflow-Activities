// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Privilege
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.IdentityModel
{
  internal class Privilege
  {
    public const string SeAuditPrivilege = "SeAuditPrivilege";
    public const string SeTcbPrivilege = "SeTcbPrivilege";
    private const uint SE_PRIVILEGE_DISABLED = 0;
    private const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1;
    private const uint SE_PRIVILEGE_ENABLED = 2;
    private const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648;
    private const int ERROR_SUCCESS = 0;
    private const int ERROR_NO_TOKEN = 1008;
    private const int ERROR_NOT_ALL_ASSIGNED = 1300;
    private static Dictionary<string, LUID> _luids = new Dictionary<string, LUID>();
    private string _privilege;
    private LUID _luid;
    private bool _needToRevert;
    private bool _initialEnabled;
    private bool _isImpersonating;
    private SafeCloseHandle _threadToken;

    public Privilege(string privilege)
    {
      this._privilege = privilege;
      this._luid = Privilege.LuidFromPrivilege(privilege);
    }

    public void Enable()
    {
      this._threadToken = this.GetThreadToken();
      this.EnableTokenPrivilege(this._threadToken);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public int Revert()
    {
      if (!this._isImpersonating)
      {
        if (this._needToRevert && !this._initialEnabled)
        {
          TOKEN_PRIVILEGE newState;
          newState.PrivilegeCount = 1U;
          newState.Privilege.Luid = this._luid;
          newState.Privilege.Attributes = 0U;
          uint returnLength = 0;
          if (!NativeMethods.AdjustTokenPrivileges(this._threadToken, false, ref newState, TOKEN_PRIVILEGE.Size, out TOKEN_PRIVILEGE _, out returnLength))
            return Marshal.GetLastWin32Error();
        }
        this._needToRevert = false;
      }
      else
      {
        if (!NativeMethods.RevertToSelf())
          return Marshal.GetLastWin32Error();
        this._isImpersonating = false;
      }
      if (this._threadToken != null)
      {
        this._threadToken.Close();
        this._threadToken = (SafeCloseHandle) null;
      }
      return 0;
    }

    private SafeCloseHandle GetThreadToken()
    {
      SafeCloseHandle threadToken;
      if (!NativeMethods.OpenThreadToken(NativeMethods.GetCurrentThread(), TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, true, out threadToken))
      {
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        CryptoUtil.CloseInvalidOutSafeHandle((SafeHandle) threadToken);
        if (lastWin32Error1 != 1008)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error1));
        SafeCloseHandle tokenHandle;
        if (!NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), TokenAccessLevels.Duplicate, out tokenHandle))
        {
          int lastWin32Error2 = Marshal.GetLastWin32Error();
          CryptoUtil.CloseInvalidOutSafeHandle((SafeHandle) tokenHandle);
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error2));
        }
        try
        {
          if (!NativeMethods.DuplicateTokenEx(tokenHandle, TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, IntPtr.Zero, SECURITY_IMPERSONATION_LEVEL.Impersonation, TokenType.TokenImpersonation, out threadToken))
          {
            int lastWin32Error2 = Marshal.GetLastWin32Error();
            CryptoUtil.CloseInvalidOutSafeHandle((SafeHandle) threadToken);
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error2));
          }
          this.SetThreadToken(threadToken);
        }
        finally
        {
          tokenHandle.Close();
        }
      }
      return threadToken;
    }

    private void EnableTokenPrivilege(SafeCloseHandle threadToken)
    {
      TOKEN_PRIVILEGE newState;
      newState.PrivilegeCount = 1U;
      newState.Privilege.Luid = this._luid;
      newState.Privilege.Attributes = 2U;
      uint returnLength = 0;
      RuntimeHelpers.PrepareConstrainedRegions();
      TOKEN_PRIVILEGE previousState;
      bool flag = NativeMethods.AdjustTokenPrivileges(threadToken, false, ref newState, TOKEN_PRIVILEGE.Size, out previousState, out returnLength);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag && lastWin32Error == 0)
      {
        this._initialEnabled = 0 != ((int) previousState.Privilege.Attributes & 2);
        this._needToRevert = true;
      }
      if (lastWin32Error == 1300)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new PrivilegeNotHeldException(this._privilege));
      if (!flag)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(lastWin32Error));
    }

    private void SetThreadToken(SafeCloseHandle threadToken)
    {
      int error = 0;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        if (!NativeMethods.SetThreadToken(IntPtr.Zero, threadToken))
          error = Marshal.GetLastWin32Error();
        else
          this._isImpersonating = true;
      }
      if (!this._isImpersonating)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(error));
    }

    private static LUID LuidFromPrivilege(string privilege)
    {
      LUID Luid;
      lock (Privilege._luids)
      {
        if (Privilege._luids.TryGetValue(privilege, out Luid))
          return Luid;
      }
      if (!NativeMethods.LookupPrivilegeValueW((string) null, privilege, out Luid))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      lock (Privilege._luids)
      {
        if (!Privilege._luids.ContainsKey(privilege))
          Privilege._luids[privilege] = Luid;
      }
      return Luid;
    }
  }
}
