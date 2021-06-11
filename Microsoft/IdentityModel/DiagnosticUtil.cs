// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.DiagnosticUtil
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace Microsoft.IdentityModel
{
  internal static class DiagnosticUtil
  {
    public static bool IsFatal(Exception exception) => DiagnosticUtil.ExceptionUtil.IsFatal(exception);

    public static class ExceptionUtil
    {
      public static Exception ThrowHelperArgumentNull(string arg) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentNullException(arg));

      public static Exception ThrowHelperArgumentNullOrEmptyString(string arg) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID0006"), arg));

      public static Exception ThrowHelperArgumentOutOfRange(string arg) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(arg));

      public static Exception ThrowHelperArgumentOutOfRange(string arg, string message) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(arg, message));

      public static Exception ThrowHelperArgumentOutOfRange(
        string arg,
        object actualValue,
        string message)
      {
        return DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(arg, actualValue, message));
      }

      public static Exception ThrowHelperArgument(string arg, string message) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(message, arg));

      public static Exception ThrowHelperConfigurationError(
        ConfigurationElement configElement,
        string propertyName,
        Exception inner)
      {
        if (inner == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inner));
        if (configElement == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (configElement));
        if (propertyName == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (propertyName));
        if (configElement.ElementInformation == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0003", (object) "configElement.ElementInformation"));
        if (configElement.ElementInformation.Properties == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0003", (object) "configElement.ElementInformation.Properties"));
        if (configElement.ElementInformation.Properties[propertyName] == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0005", (object) "configElement.ElementInformation.Properties", (object) propertyName));
        return DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ConfigurationErrorsException(SR.GetString("ID1024", (object) propertyName, (object) inner.Message), inner, configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber));
      }

      public static Exception ThrowHelperConfigurationError(
        ConfigurationElement configElement,
        string propertyName,
        string message)
      {
        if (configElement == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (configElement));
        if (propertyName == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (propertyName));
        if (configElement.ElementInformation == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0003", (object) "configElement.ElementInformation"));
        if (configElement.ElementInformation.Properties == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0003", (object) "configElement.ElementInformation.Properties"));
        if (configElement.ElementInformation.Properties[propertyName] == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (configElement), SR.GetString("ID0005", (object) "configElement.ElementInformation.Properties", (object) propertyName));
        return DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ConfigurationErrorsException(message, configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber));
      }

      public static Exception ThrowHelperInvalidOperation(string message) => DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(message));

      public static Exception ThrowHelperError(Exception exception) => DiagnosticUtil.ExceptionUtil.ThrowHelper(exception, TraceEventType.Error);

      public static Exception ThrowHelperError(Exception exception, string description) => DiagnosticUtil.ExceptionUtil.ThrowHelper(exception, TraceEventType.Error);

      public static Exception ThrowHelper(Exception exception, TraceEventType eventType) => DiagnosticUtil.ExceptionUtil.ThrowHelper(exception, eventType, SR.GetString("TraceHandledException"));

      public static Exception ThrowHelper(
        Exception exception,
        TraceEventType eventType,
        string description)
      {
        return DiagnosticUtil.ExceptionUtil.ThrowHelper(exception, eventType, description, (TraceRecord) null);
      }

      public static Exception ThrowHelper(
        Exception exception,
        TraceEventType eventType,
        string description,
        TraceRecord tr)
      {
        DiagnosticUtil.TraceUtil.Trace(eventType, TraceCode.HandledException, description, tr, exception);
        return exception;
      }

      public static Exception ThrowHelperXml(XmlReader reader, string message) => DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, message, (Exception) null);

      public static Exception ThrowHelperXml(
        XmlReader reader,
        string message,
        Exception inner)
      {
        IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
        return DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(message, inner, xmlLineInfo != null ? xmlLineInfo.LineNumber : 0, xmlLineInfo != null ? xmlLineInfo.LinePosition : 0));
      }

      public static bool IsFatal(Exception exception)
      {
        Exception exception1 = exception;
        while (true)
        {
          switch (exception1)
          {
            case null:
              goto label_4;
            case OutOfMemoryException _ when !(exception1 is InsufficientMemoryException):
            case ThreadAbortException _:
            case AccessViolationException _:
            case SEHException _:
              goto label_1;
            default:
              exception1 = exception1.InnerException;
              continue;
          }
        }
label_1:
        return true;
label_4:
        return false;
      }
    }

    public static class TraceUtil
    {
      private const string _critical = "Critical";
      private const string _error = "Error";
      private const string _warning = "Warning";
      private const string _information = "Information";
      private const string _verbose = "Verbose";
      private const string _start = "Start";
      private const string _stop = "Stop";
      private const string _suspend = "Suspend";
      private const string _transfer = "Transfer";
      private const string TraceSourceName = "Microsoft.IdentityModel";
      private const string EventSourceName = "Microsoft.IdentityModel 3.5.0.0";
      private static bool _calledShutdown = false;
      private static string _appDomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;
      private static TraceSource _traceSource = new TraceSource("Microsoft.IdentityModel");
      private static bool _traceEnabled = DiagnosticUtil.TraceUtil.InitializeTraceEnabled();
      private static object _lock = new object();
      private static DateTime _lastFailure = DateTime.MinValue;
      private static TimeSpan _failureBlackout = TimeSpan.FromMinutes(10.0);

      private static void AddDomainEventHandlersForCleanup(TraceSource traceSource)
      {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += new UnhandledExceptionEventHandler(DiagnosticUtil.TraceUtil.UnhandledExceptionHandler);
        currentDomain.DomainUnload += new EventHandler(DiagnosticUtil.TraceUtil.ExitOrUnloadEventHandler);
        currentDomain.ProcessExit += new EventHandler(DiagnosticUtil.TraceUtil.ExitOrUnloadEventHandler);
      }

      private static void AddExceptionToTraceString(XmlWriter xmlWriter, Exception exception)
      {
        xmlWriter.WriteElementString("ExceptionType", DiagnosticUtil.TraceUtil.XmlEncode(exception.GetType().AssemblyQualifiedName));
        xmlWriter.WriteElementString("Message", DiagnosticUtil.TraceUtil.XmlEncode(exception.Message));
        xmlWriter.WriteElementString("StackTrace", DiagnosticUtil.TraceUtil.XmlEncode(DiagnosticUtil.TraceUtil.StackTraceString(exception)));
        xmlWriter.WriteElementString("ExceptionString", DiagnosticUtil.TraceUtil.XmlEncode(exception.ToString()));
        if (exception is Win32Exception win32Exception)
          xmlWriter.WriteElementString("NativeErrorCode", win32Exception.NativeErrorCode.ToString("X", (IFormatProvider) CultureInfo.InvariantCulture));
        if (exception.Data != null && exception.Data.Count > 0)
        {
          xmlWriter.WriteStartElement("DataItems");
          foreach (object key in (IEnumerable) exception.Data.Keys)
          {
            xmlWriter.WriteStartElement("Data");
            xmlWriter.WriteElementString("Key", DiagnosticUtil.TraceUtil.XmlEncode(key.ToString()));
            xmlWriter.WriteElementString("Value", DiagnosticUtil.TraceUtil.XmlEncode(exception.Data[key].ToString()));
            xmlWriter.WriteEndElement();
          }
          xmlWriter.WriteEndElement();
        }
        if (exception.InnerException == null)
          return;
        xmlWriter.WriteStartElement("InnerException");
        DiagnosticUtil.TraceUtil.AddExceptionToTraceString(xmlWriter, exception.InnerException);
        xmlWriter.WriteEndElement();
      }

      private static void ExitOrUnloadEventHandler(object sender, EventArgs e) => DiagnosticUtil.TraceUtil.ShutdownTracing();

      private static void LogTraceFailure(string traceString, Exception e)
      {
        try
        {
          lock (DiagnosticUtil.TraceUtil._lock)
          {
            if (!(DateTime.UtcNow.Subtract(DiagnosticUtil.TraceUtil._lastFailure) >= DiagnosticUtil.TraceUtil._failureBlackout))
              return;
            DiagnosticUtil.TraceUtil._lastFailure = DateTime.UtcNow;
            EventLog eventLog = new EventLog();
            eventLog.Source = "Microsoft.IdentityModel 3.5.0.0";
            StackTrace stackTrace = new StackTrace();
            if (e == null)
              eventLog.WriteEntry(SR.GetString("EventLogTraceFailedWithExceptionMessage", (object) e.ToString(), (object) stackTrace.ToString()), EventLogEntryType.Error);
            else
              eventLog.WriteEntry(SR.GetString("EventLogTraceFailedWithoutExceptionMessage", (object) stackTrace.ToString()), EventLogEntryType.Error);
          }
        }
        catch (Exception ex)
        {
          if (!DiagnosticUtil.IsFatal(ex))
            return;
          throw;
        }
      }

      private static string LookupSeverity(TraceEventType type)
      {
        switch (type)
        {
          case TraceEventType.Critical:
            return "Critical";
          case TraceEventType.Error:
            return "Error";
          case TraceEventType.Warning:
            return "Warning";
          case TraceEventType.Information:
            return "Information";
          case TraceEventType.Verbose:
            return "Verbose";
          case TraceEventType.Start:
            return "Start";
          case TraceEventType.Stop:
            return "Stop";
          case TraceEventType.Suspend:
            return "Suspend";
          case TraceEventType.Transfer:
            return "Transfer";
          default:
            return type.ToString();
        }
      }

      private static bool InitializeTraceEnabled()
      {
        DiagnosticUtil.TraceUtil.AddDomainEventHandlersForCleanup(DiagnosticUtil.TraceUtil._traceSource);
        return DiagnosticUtil.TraceUtil._traceSource.Switch.Level != SourceLevels.Off && DiagnosticUtil.TraceUtil._traceSource.Listeners != null && DiagnosticUtil.TraceUtil._traceSource.Listeners.Count > 0;
      }

      public static string ProcessName
      {
        get
        {
          using (Process currentProcess = Process.GetCurrentProcess())
            return currentProcess.ProcessName;
        }
      }

      public static int ProcessId
      {
        get
        {
          using (Process currentProcess = Process.GetCurrentProcess())
            return currentProcess.Id;
        }
      }

      public static bool ShouldTrace(TraceEventType eventType) => DiagnosticUtil.TraceUtil._traceEnabled && DiagnosticUtil.TraceUtil._traceSource.Switch.ShouldTrace(eventType);

      private static void ShutdownTracing()
      {
        if (DiagnosticUtil.TraceUtil._traceSource == null)
          return;
        if (DiagnosticUtil.TraceUtil._calledShutdown)
          return;
        try
        {
          if (!DiagnosticUtil.TraceUtil._traceEnabled || DiagnosticUtil.TraceUtil._traceSource.Switch.Level == SourceLevels.Off)
            return;
          DiagnosticUtil.TraceUtil._calledShutdown = true;
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
            DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.AppDomainUnload, SR.GetString("TraceAppDomainUnload"), (TraceRecord) new DictionaryTraceRecord((IDictionary) new Dictionary<string, string>(3)
            {
              ["AppDomain.FriendlyName"] = AppDomain.CurrentDomain.FriendlyName,
              ["ProcessName"] = DiagnosticUtil.TraceUtil.ProcessName,
              ["ProcessId"] = DiagnosticUtil.TraceUtil.ProcessId.ToString((IFormatProvider) CultureInfo.CurrentCulture)
            }), (Exception) null);
          DiagnosticUtil.TraceUtil._traceSource.Flush();
        }
        catch (Exception ex)
        {
          if (DiagnosticUtil.IsFatal(ex))
            throw;
          else
            DiagnosticUtil.TraceUtil.LogTraceFailure((string) null, ex);
        }
      }

      private static string StackTraceString(Exception exception)
      {
        string stackTrace = exception.StackTrace;
        if (string.IsNullOrEmpty(stackTrace))
        {
          StackFrame[] frames = new StackTrace().GetFrames();
          int skipFrames = 0;
          bool flag = false;
          foreach (StackFrame stackFrame in frames)
          {
            string name = stackFrame.GetMethod().Name;
            switch (name)
            {
              case "AddExceptionToTraceString":
              case "HandleSecurityTokenProcessingException":
              case nameof (StackTraceString):
              case "Trace":
              case "TraceString":
                ++skipFrames;
                break;
              default:
                if (name.StartsWith("ThrowHelper", StringComparison.Ordinal))
                {
                  ++skipFrames;
                  break;
                }
                flag = true;
                break;
            }
            if (flag)
              break;
          }
          stackTrace = new StackTrace(skipFrames, false).ToString();
        }
        return stackTrace;
      }

      public static void Trace(
        TraceEventType type,
        TraceCode tc,
        string description,
        TraceRecord traceRecord,
        Exception exception)
      {
        try
        {
          PlainXmlWriter plainXmlWriter = new PlainXmlWriter();
          TraceXPathNavigator navigator = plainXmlWriter.Navigator;
          plainXmlWriter.WriteStartElement("TraceRecord");
          plainXmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2009/10/IdentityModel/TraceRecord");
          plainXmlWriter.WriteAttributeString("Severity", DiagnosticUtil.TraceUtil.LookupSeverity(type));
          if (string.IsNullOrEmpty(description))
            plainXmlWriter.WriteElementString("Description", "Microsoft.IdentityModel Diagnostic Trace");
          else
            plainXmlWriter.WriteElementString("Description", description);
          plainXmlWriter.WriteElementString("AppDomain", DiagnosticUtil.TraceUtil._appDomainFriendlyName);
          traceRecord?.WriteTo((XmlWriter) plainXmlWriter);
          if (exception != null)
          {
            plainXmlWriter.WriteStartElement("Exception");
            DiagnosticUtil.TraceUtil.AddExceptionToTraceString((XmlWriter) plainXmlWriter, exception);
            plainXmlWriter.WriteEndElement();
          }
          DiagnosticUtil.TraceUtil._traceSource.TraceData(type, (int) tc, (object) navigator);
          if (DiagnosticUtil.TraceUtil._calledShutdown)
            DiagnosticUtil.TraceUtil._traceSource.Flush();
          DiagnosticUtil.TraceUtil._lastFailure = DateTime.MinValue;
        }
        catch (Exception ex)
        {
          if (!DiagnosticUtil.ExceptionUtil.IsFatal(ex))
            return;
          throw;
        }
      }

      public static void TraceString(
        TraceEventType eventType,
        string formatString,
        params object[] args)
      {
        if (!DiagnosticUtil.TraceUtil.ShouldTrace(eventType))
          return;
        if (args != null && args.Length > 0)
          DiagnosticUtil.TraceUtil.Trace(eventType, TraceCode.Diagnostics, string.Format((IFormatProvider) CultureInfo.InvariantCulture, formatString, args), (TraceRecord) null, (Exception) null);
        else
          DiagnosticUtil.TraceUtil.Trace(eventType, TraceCode.Diagnostics, formatString, (TraceRecord) null, (Exception) null);
      }

      public static string XmlEncode(string text)
      {
        if (string.IsNullOrEmpty(text))
          return text;
        int length = text.Length;
        StringBuilder stringBuilder = new StringBuilder(length + 8);
        for (int index = 0; index < length; ++index)
        {
          char ch = text[index];
          switch (ch)
          {
            case '&':
              stringBuilder.Append("&amp;");
              break;
            case '<':
              stringBuilder.Append("&lt;");
              break;
            case '>':
              stringBuilder.Append("&gt;");
              break;
            default:
              stringBuilder.Append(ch);
              break;
          }
        }
        return stringBuilder.ToString();
      }

      private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
      {
        Exception exceptionObject = (Exception) args.ExceptionObject;
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Critical, TraceCode.UnhandledException, SR.GetString("TraceUnhandledException"), (TraceRecord) null, exceptionObject);
        DiagnosticUtil.TraceUtil.ShutdownTracing();
      }
    }
  }
}
