// Decompiled with JetBrains decompiler
// Type: PremiumWorkflowActivities.Plugins.BaseWorkflow
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Globalization;

namespace PremiumWorkflowActivities.Plugins
{
  public abstract class BaseWorkflow : CodeActivity
  {
    protected abstract void ExecuteInternal(BaseWorkflow.LocalWorkflowContext context);

    protected override void Execute(CodeActivityContext context)
    {
      using (BaseWorkflow.LocalWorkflowContext context1 = new BaseWorkflow.LocalWorkflowContext(context))
      {
        context1.Trace("Calling ExecuteInternal");
        try
        {
          this.ExecuteInternal(context1);
        }
        catch (Exception ex)
        {
          context1.Trace(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Exception: {0}", (object) ex.ToString()));
          throw;
        }
        finally
        {
          context1.Trace("ExecuteInternal finished");
        }
      }
    }

    protected class LocalWorkflowContext : IDisposable
    {
      public CodeActivityContext CodeActivityContext { get; private set; }

      public IWorkflowContext WorkflowContext { get; private set; }

      public IOrganizationServiceFactory ServiceFactory { get; private set; }

      public OrganizationServiceContext CrmContext { get; private set; }

      public IOrganizationService OrganizationService { get; private set; }

      public ITracingService TracingService { get; private set; }

      internal LocalWorkflowContext(CodeActivityContext codeActivityContext)
      {
        this.CodeActivityContext = codeActivityContext != null ? codeActivityContext : throw new ArgumentNullException(nameof (codeActivityContext));
        this.WorkflowContext = codeActivityContext.GetExtension<IWorkflowContext>();
        this.TracingService = codeActivityContext.GetExtension<ITracingService>();
        this.ServiceFactory = codeActivityContext.GetExtension<IOrganizationServiceFactory>();
        this.OrganizationService = this.ServiceFactory.CreateOrganizationService(new Guid?(((IExecutionContext) this.WorkflowContext).get_UserId()));
        this.CrmContext = new OrganizationServiceContext(this.OrganizationService);
      }

      internal void Trace(string message)
      {
        if (string.IsNullOrWhiteSpace(message) || this.TracingService == null)
          return;
        if (this.WorkflowContext == null)
          this.TracingService.Trace(message, (object[]) Array.Empty<object>());
        else
          this.TracingService.Trace("{0}, Correlation Id: {1}, Initiating User: {2}", new object[3]
          {
            (object) message,
            (object) ((IExecutionContext) this.WorkflowContext).get_CorrelationId(),
            (object) ((IExecutionContext) this.WorkflowContext).get_InitiatingUserId()
          });
      }

      public void Dispose()
      {
        if (this.CrmContext == null)
          return;
        this.CrmContext.Dispose();
      }
    }
  }
}
