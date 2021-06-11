// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.DeflateCookieTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public sealed class DeflateCookieTransform : CookieTransform
  {
    private int _maxDecompressedSize = 1048576;

    public int MaxDecompressedSize
    {
      get => this._maxDecompressedSize;
      set => this._maxDecompressedSize = value;
    }

    public override byte[] Decode(byte[] encoded)
    {
      if (encoded == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encoded));
      if (encoded.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (encoded), Microsoft.IdentityModel.SR.GetString("ID6045"));
      using (DeflateStream deflateStream = new DeflateStream((Stream) new MemoryStream(encoded), CompressionMode.Decompress, false))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          byte[] buffer = new byte[1024];
          int count;
          do
          {
            count = deflateStream.Read(buffer, 0, buffer.Length);
            memoryStream.Write(buffer, 0, count);
            if (memoryStream.Length > (long) this.MaxDecompressedSize)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID1068", (object) this.MaxDecompressedSize)));
          }
          while (count > 0);
          return memoryStream.ToArray();
        }
      }
    }

    public override byte[] Encode(byte[] value)
    {
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (value.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID6044"));
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream, CompressionMode.Compress, true))
          deflateStream.Write(value, 0, value.Length);
        byte[] array = memoryStream.ToArray();
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, (string) null, (TraceRecord) new DeflateCookieTraceRecord(value.Length, array.Length), (Exception) null);
        return array;
      }
    }
  }
}
