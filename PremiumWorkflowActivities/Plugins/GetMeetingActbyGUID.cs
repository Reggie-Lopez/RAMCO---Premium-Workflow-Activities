// Decompiled with JetBrains decompiler
// Type: PremiumWorkflowActivities.Plugins.GetMeetingActbyGUID
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace PremiumWorkflowActivities.Plugins
{
  public class GetMeetingActbyGUID : BaseWorkflow
  {
    [RequiredArgument]
    [Input("Primary Key (GUID)")]
    public InArgument<string> RecordId { get; set; }

    protected override void ExecuteInternal(BaseWorkflow.LocalWorkflowContext context)
    {
      EntityReference entityReference = new EntityReference("cobalt_meetingactivity", Guid.Parse(this.RecordId.Get((ActivityContext) context.CodeActivityContext)));
      this.MeetingActDefinedRecordId.Set((ActivityContext) context.CodeActivityContext, entityReference);
    }

    [Output("MeetingAct")]
    [Default("{FBC01159-8540-E111-9C03-BC305B2A71B7}")]
    [ReferenceTarget("cobalt_meetingactivity")]
    public OutArgument<EntityReference> MeetingActDefinedRecordId { get; set; }
  }
}
