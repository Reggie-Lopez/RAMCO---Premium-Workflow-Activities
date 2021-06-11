// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.WSFederationSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
  [ComVisible(true)]
  public class WSFederationSerializer
  {
    private WSTrustRequestSerializer _requestSerializer;
    private WSTrustResponseSerializer _responseSerializer;

    public WSFederationSerializer()
      : this((WSTrustRequestSerializer) new WSTrustFeb2005RequestSerializer(), (WSTrustResponseSerializer) new WSTrustFeb2005ResponseSerializer())
    {
    }

    public WSFederationSerializer(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (reader.NamespaceURI == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
      {
        this._requestSerializer = (WSTrustRequestSerializer) new WSTrust13RequestSerializer();
        this._responseSerializer = (WSTrustResponseSerializer) new WSTrust13ResponseSerializer();
      }
      else if (reader.NamespaceURI == "http://schemas.xmlsoap.org/ws/2005/02/trust")
      {
        this._requestSerializer = (WSTrustRequestSerializer) new WSTrustFeb2005RequestSerializer();
        this._responseSerializer = (WSTrustResponseSerializer) new WSTrustFeb2005ResponseSerializer();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID5004", (object) reader.NamespaceURI));
    }

    public WSFederationSerializer(
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer)
    {
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      this._requestSerializer = requestSerializer;
      this._responseSerializer = responseSerializer;
    }

    public virtual RequestSecurityToken CreateRequest(
      WSFederationMessage message,
      WSTrustSerializationContext context)
    {
      if (message == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (message));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      string s = message is SignInRequestMessage inRequestMessage ? inRequestMessage.Request : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID3005"), nameof (message)));
      bool flag = !string.IsNullOrEmpty(inRequestMessage.RequestPtr);
      if (!string.IsNullOrEmpty(s))
      {
        if (flag && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID3211", (object) "request", (object) "wreq", (object) "wreqptr"));
      }
      else if (flag)
        s = this.GetReferencedRequest(inRequestMessage.RequestPtr);
      RequestSecurityToken requestSecurityToken;
      if (!string.IsNullOrEmpty(s))
      {
        using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(s), XmlDictionaryReaderQuotas.Max))
        {
          try
          {
            requestSecurityToken = this._requestSerializer.ReadXml((XmlReader) textReader, context);
          }
          catch (XmlException ex)
          {
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3273"), (Exception) ex));
          }
        }
      }
      else
        requestSecurityToken = new RequestSecurityToken();
      if (string.IsNullOrEmpty(requestSecurityToken.RequestType))
        requestSecurityToken.RequestType = "http://schemas.microsoft.com/idfx/requesttype/issue";
      if (!string.IsNullOrEmpty(inRequestMessage.AuthenticationType) && string.IsNullOrEmpty(requestSecurityToken.AuthenticationType))
        requestSecurityToken.AuthenticationType = UriUtil.CanCreateValidUri(inRequestMessage.AuthenticationType, UriKind.Absolute) ? inRequestMessage.AuthenticationType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3143", (object) "wauth", (object) inRequestMessage.AuthenticationType)));
      if (string.IsNullOrEmpty(requestSecurityToken.KeyType))
        requestSecurityToken.KeyType = "http://schemas.microsoft.com/idfx/keytype/bearer";
      string context1 = inRequestMessage.Context;
      if (!string.IsNullOrEmpty(context1) && string.IsNullOrEmpty(requestSecurityToken.Context))
        requestSecurityToken.Context = context1;
      string realm = inRequestMessage.Realm;
      if (!string.IsNullOrEmpty(realm) && requestSecurityToken.AppliesTo == (EndpointAddress) null)
      {
        if (!UriUtil.CanCreateValidUri(inRequestMessage.Realm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSFederationMessageException(Microsoft.IdentityModel.SR.GetString("ID3143", (object) "wtrealm", (object) inRequestMessage.Realm)));
        requestSecurityToken.AppliesTo = new EndpointAddress(realm);
      }
      requestSecurityToken.ReplyTo = string.Empty;
      if (!string.IsNullOrEmpty(inRequestMessage.Reply))
        requestSecurityToken.ReplyTo = inRequestMessage.Reply;
      return requestSecurityToken;
    }

    public virtual RequestSecurityTokenResponse CreateResponse(
      WSFederationMessage message,
      WSTrustSerializationContext context)
    {
      if (message == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (message));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      string s = message is SignInResponseMessage inResponseMessage ? inResponseMessage.Result : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID3005"), nameof (message)));
      bool flag = !string.IsNullOrEmpty(inResponseMessage.ResultPtr);
      if (!string.IsNullOrEmpty(s))
      {
        if (flag && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID3211", (object) "result", (object) "wresult", (object) "wresultptr"));
      }
      else
      {
        if (!flag)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID3019")));
        s = this.GetReferencedResult(inResponseMessage.ResultPtr);
      }
      using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(s), XmlDictionaryReaderQuotas.Max))
      {
        int content = (int) textReader.MoveToContent();
        return this._responseSerializer.ReadXml((XmlReader) textReader, context);
      }
    }

    public virtual string GetReferencedRequest(string wreqptr) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3210", (object) nameof (wreqptr))));

    public virtual string GetReferencedResult(string wresultptr) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3210", (object) nameof (wresultptr))));

    public virtual string GetRequestAsString(
      RequestSecurityToken request,
      WSTrustSerializationContext context)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      StringBuilder sb = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter) stringWriter))
        {
          this._requestSerializer.WriteXml(request, (XmlWriter) xmlTextWriter, context);
          xmlTextWriter.Flush();
        }
      }
      return sb.ToString();
    }

    public virtual string GetResponseAsString(
      RequestSecurityTokenResponse response,
      WSTrustSerializationContext context)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      StringBuilder sb = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter) stringWriter))
        {
          this._responseSerializer.WriteXml(response, (XmlWriter) xmlTextWriter, context);
          xmlTextWriter.Flush();
        }
      }
      return sb.ToString();
    }

    public virtual bool CanReadRequest(string trustMessage)
    {
      if (string.IsNullOrEmpty(trustMessage))
        return false;
      try
      {
        using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(trustMessage), XmlDictionaryReaderQuotas.Max))
          return this._requestSerializer.CanRead(textReader);
      }
      catch (XmlException ex)
      {
        return false;
      }
    }

    public virtual bool CanReadResponse(string trustMessage)
    {
      if (string.IsNullOrEmpty(trustMessage))
        return false;
      try
      {
        using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(trustMessage), XmlDictionaryReaderQuotas.Max))
          return this._responseSerializer.CanRead(textReader);
      }
      catch (XmlException ex)
      {
        return false;
      }
    }
  }
}
