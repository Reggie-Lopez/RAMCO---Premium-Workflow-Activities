// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.DateTimeUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace Microsoft.IdentityModel
{
  internal static class DateTimeUtil
  {
    public static DateTime Add(DateTime time, TimeSpan timespan)
    {
      if (timespan >= TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
        return DateTimeUtil.GetMaxValue(time.Kind);
      return timespan <= TimeSpan.Zero && DateTime.MinValue - time >= timespan ? DateTimeUtil.GetMinValue(time.Kind) : time + timespan;
    }

    public static DateTime AddNonNegative(DateTime time, TimeSpan timespan) => !(timespan <= TimeSpan.Zero) ? DateTimeUtil.Add(time, timespan) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(SR.GetString("ID2082")));

    public static DateTime GetMaxValue(DateTimeKind kind) => new DateTime(DateTime.MaxValue.Ticks, kind);

    public static DateTime GetMinValue(DateTimeKind kind) => new DateTime(DateTime.MinValue.Ticks, kind);

    public static DateTime? ToUniversalTime(DateTime? value) => !value.HasValue || value.Value.Kind == DateTimeKind.Utc ? value : new DateTime?(DateTimeUtil.ToUniversalTime(value.Value));

    public static DateTime ToUniversalTime(DateTime value) => value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
  }
}
