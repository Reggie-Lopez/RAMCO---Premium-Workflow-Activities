// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.FederatedSessionExpiredException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  [Serializable]
  public class FederatedSessionExpiredException : FederatedAuthenticationSessionEndingException
  {
    private DateTime _tested;
    private DateTime _expired;

    public FederatedSessionExpiredException()
    {
    }

    public FederatedSessionExpiredException(DateTime tested, DateTime expired)
      : this(Microsoft.IdentityModel.SR.GetString("ID1004", (object) tested, (object) expired))
    {
      this._tested = tested;
      this._expired = expired;
    }

    public FederatedSessionExpiredException(DateTime tested, DateTime expired, Exception inner)
      : this(Microsoft.IdentityModel.SR.GetString("ID1004", (object) tested, (object) expired), inner)
    {
      this._tested = tested;
      this._expired = expired;
    }

    public FederatedSessionExpiredException(string message)
      : base(message)
    {
    }

    public FederatedSessionExpiredException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected FederatedSessionExpiredException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._expired = info != null ? info.GetDateTime(nameof (Expired)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      this._tested = info.GetDateTime(nameof (Tested));
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("Expired", (object) this._expired, typeof (DateTime));
      info.AddValue("Tested", (object) this._tested, typeof (DateTime));
    }

    public DateTime Expired => this._expired;

    public DateTime Tested => this._tested;
  }
}
