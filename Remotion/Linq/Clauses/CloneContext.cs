// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.CloneContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Remotion.Utilities;
using System.Runtime.InteropServices;

namespace Remotion.Linq.Clauses
{
  [ComVisible(true)]
  public sealed class CloneContext
  {
    public CloneContext(QuerySourceMapping querySourceMapping)
    {
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      this.QuerySourceMapping = querySourceMapping;
    }

    public QuerySourceMapping QuerySourceMapping { get; private set; }
  }
}
