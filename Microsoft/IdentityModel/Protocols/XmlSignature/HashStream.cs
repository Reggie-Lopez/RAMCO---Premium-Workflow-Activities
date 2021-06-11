// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.HashStream
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class HashStream : Stream
  {
    private HashAlgorithm _hash;
    private long _length;
    private bool _hashNeedsReset;
    private MemoryStream _logStream;
    private TraceEventType _traceEventType = TraceEventType.Verbose;
    private bool _disposed;

    public HashStream(HashAlgorithm hash)
    {
      if (hash == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (hash));
      this.Reset(hash);
    }

    public override bool CanRead => false;

    public override bool CanWrite => true;

    public override bool CanSeek => false;

    public HashAlgorithm Hash => this._hash;

    public override long Length => this._length;

    public override long Position
    {
      get => this._length;
      set => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());
    }

    public override void Flush()
    {
    }

    public void FlushHash() => this.FlushHash((MemoryStream) null);

    public void FlushHash(MemoryStream preCanonicalBytes)
    {
      this._hash.TransformFinalBlock(new byte[0], 0, 0);
      if (this._logStream == null)
        return;
      DiagnosticUtil.TraceUtil.Trace(this._traceEventType, TraceCode.Diagnostics, (string) null, (TraceRecord) new HashTraceRecord(Convert.ToBase64String(this._hash.Hash), this._logStream.ToArray(), preCanonicalBytes == null ? (byte[]) null : preCanonicalBytes.ToArray()), (Exception) null);
    }

    public byte[] FlushHashAndGetValue() => this.FlushHashAndGetValue((MemoryStream) null);

    public byte[] FlushHashAndGetValue(MemoryStream preCanonicalBytes)
    {
      this.FlushHash(preCanonicalBytes);
      return this._hash.Hash;
    }

    public override int Read(byte[] buffer, int offset, int count) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public void Reset()
    {
      if (this._hashNeedsReset)
      {
        this._hash.Initialize();
        this._hashNeedsReset = false;
      }
      this._length = 0L;
      if (!DiagnosticUtil.TraceUtil.ShouldTrace(this._traceEventType))
        return;
      this._logStream = new MemoryStream();
    }

    public void Reset(HashAlgorithm hash)
    {
      this._hash = hash;
      this._hashNeedsReset = false;
      this._length = 0L;
      if (!DiagnosticUtil.TraceUtil.ShouldTrace(this._traceEventType))
        return;
      this._logStream = new MemoryStream();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this._hash.TransformBlock(buffer, offset, count, buffer, offset);
      this._length += (long) count;
      this._hashNeedsReset = true;
      if (this._logStream == null)
        return;
      this._logStream.Write(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public override void SetLength(long length) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this._disposed)
        return;
      if (disposing && this._logStream != null)
      {
        this._logStream.Dispose();
        this._logStream = (MemoryStream) null;
      }
      this._disposed = true;
    }
  }
}
