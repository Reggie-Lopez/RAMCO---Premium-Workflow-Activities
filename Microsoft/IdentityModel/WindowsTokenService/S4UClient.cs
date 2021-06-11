// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.WindowsTokenService.S4UClient
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.WindowsTokenService
{
  [ComVisible(true)]
  public static class S4UClient
  {
    private static ChannelFactory<S4UClient.IS4UService_dup> _channelFactory = new ChannelFactory<S4UClient.IS4UService_dup>((Binding) new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport), new UriBuilder()
    {
      Scheme = Uri.UriSchemeNetPipe,
      Host = "localhost",
      Path = "/s4u/022694f3-9fbd-422b-b4b2-312e25dae2a2"
    }.Uri.ToString());

    public static WindowsIdentity UpnLogon(string upn) => S4UClient.CallService((Func<S4UClient.IS4UService_dup, IntPtr>) (channel => channel.UpnLogon(upn, Process.GetCurrentProcess().Id)));

    public static WindowsIdentity CertificateLogon(X509Certificate2 certificate) => S4UClient.CallService((Func<S4UClient.IS4UService_dup, IntPtr>) (channel => channel.CertificateLogon(certificate.RawData, Process.GetCurrentProcess().Id)));

    private static WindowsIdentity CallService(
      Func<S4UClient.IS4UService_dup, IntPtr> contractOperation)
    {
      S4UClient.IS4UService_dup channel = S4UClient._channelFactory.CreateChannel();
      ICommunicationObject communicationObject = (ICommunicationObject) channel;
      bool flag = false;
      try
      {
        IntPtr num = contractOperation(channel);
        using (new S4UClient.SafeKernelObjectHandle(num, true))
        {
          communicationObject.Close();
          flag = true;
          return new WindowsIdentity(num);
        }
      }
      finally
      {
        if (!flag)
          communicationObject.Abort();
      }
    }

    [ServiceContract(Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/wts")]
    private interface IS4UService_dup
    {
      [OperationContract(Action = "urn:IS4UService-UpnLogon", ReplyAction = "urn:IS4UService-UpnLogon-Response")]
      IntPtr UpnLogon(string upn, int pid);

      [OperationContract(Action = "urn:IS4UService-CertificateLogon", ReplyAction = "urn:IS4UService-CertificateLogon-Response")]
      IntPtr CertificateLogon(byte[] certData, int pid);
    }

    private class SafeKernelObjectHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      public SafeKernelObjectHandle()
        : base(true)
      {
      }

      public SafeKernelObjectHandle(IntPtr handle)
        : this(handle, true)
      {
      }

      public SafeKernelObjectHandle(IntPtr handle, bool takeOwnership)
        : base(takeOwnership)
        => this.SetHandle(handle);

      protected override bool ReleaseHandle() => S4UClient.SafeKernelObjectHandle.CloseHandle(this.handle);

      [SuppressUnmanagedCodeSecurity]
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool CloseHandle(IntPtr handle);
    }
  }
}
