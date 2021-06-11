// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsIdentitySerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
  internal class ClaimsIdentitySerializer
  {
    public const string ActorKey = "_actor";
    public const string AuthenticationTypeKey = "_authenticationType";
    public const string BootstrapTokenKey = "_bootstrapToken";
    public const string ClaimsKey = "_claims";
    public const string LabelKey = "_label";
    public const string NameClaimTypeKey = "_nameClaimType";
    public const string RoleClaimTypeKey = "_roleClaimType";
    private SerializationInfo _info;
    private StreamingContext _context;

    public static SecurityToken DeserializeBootstrapTokenFromString(
      string bootstrapTokenString)
    {
      if (!string.IsNullOrEmpty(bootstrapTokenString))
      {
        byte[] buffer = Convert.FromBase64String(bootstrapTokenString);
        using (XmlDictionaryReader binaryReader = XmlDictionaryReader.CreateBinaryReader(buffer, 0, buffer.Length, (IXmlDictionary) SessionDictionary.Instance, XmlDictionaryReaderQuotas.Max))
        {
          Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = ServiceConfiguration.GetCurrent().SecurityTokenHandlers;
          int content1 = (int) binaryReader.MoveToContent();
          using (StringReader stringReader = new StringReader(binaryReader.ReadOuterXml()))
          {
            using (XmlDictionaryReader dictionaryReader = (XmlDictionaryReader) new IdentityModelWrappedXmlDictionaryReader((XmlReader) new XmlTextReader((TextReader) stringReader)
            {
              Normalization = false,
              XmlResolver = (XmlResolver) null,
              ProhibitDtd = true
            }, XmlDictionaryReaderQuotas.Max))
            {
              int content2 = (int) dictionaryReader.MoveToContent();
              if (securityTokenHandlers.CanReadToken((XmlReader) dictionaryReader))
                return securityTokenHandlers.ReadToken((XmlReader) dictionaryReader);
              if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
                DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8001"));
            }
          }
        }
      }
      return (SecurityToken) null;
    }

    public ClaimsIdentitySerializer(SerializationInfo info, StreamingContext context)
    {
      this._info = info;
      this._context = context;
    }

    public IClaimsIdentity DeserializeActor()
    {
      string s = this._info.GetString("_actor");
      if (s == null)
        return (IClaimsIdentity) null;
      using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(s)))
        return (IClaimsIdentity) new BinaryFormatter((ISurrogateSelector) null, this._context).Deserialize((Stream) memoryStream);
    }

    public void SerializeActor(IClaimsIdentity actor) => this._info.AddValue("_actor", (object) this.SerializeActorToString(actor));

    public string DeserializeAuthenticationType() => this._info.GetString("_authenticationType");

    public void SerializeAuthenticationType(string authenticationType) => this._info.AddValue("_authenticationType", (object) authenticationType);

    public string GetSerializedBootstrapTokenString() => this._info.GetString("_bootstrapToken");

    public void SerializeBootstrapToken(SecurityToken bootstrapToken) => this._info.AddValue("_bootstrapToken", (object) this.SerializeBootstrapTokenToString(bootstrapToken));

    public void DeserializeClaims(ClaimCollection claims)
    {
      string claimsString = this._info.GetString("_claims");
      this.DeserializeClaimsFromString(claims, claimsString);
    }

    public void SerializeClaims(IEnumerable<Claim> claims)
    {
      IList<Claim> claimList = claims as IList<Claim>;
      if (claims == null || claimList != null && claimList.Count == 0)
        this._info.AddValue("_claims", (object) string.Empty);
      else
        this._info.AddValue("_claims", (object) this.SerializeClaimsToString(claims));
    }

    public string DeserializeLabel() => this._info.GetString("_label");

    public void SerializeLabel(string label) => this._info.AddValue("_label", (object) label);

    public string DeserializeNameClaimType() => this._info.GetString("_nameClaimType");

    public void SerializeNameClaimType(string nameClaimType) => this._info.AddValue("_nameClaimType", (object) nameClaimType);

    public string DeserializeRoleClaimType() => this._info.GetString("_roleClaimType");

    public void SerializeRoleClaimType(string roleClaimType) => this._info.AddValue("_roleClaimType", (object) roleClaimType);

    private void DeserializeClaimsFromString(ClaimCollection claims, string claimsString)
    {
      if (string.IsNullOrEmpty(claimsString))
        return;
      byte[] buffer = Convert.FromBase64String(claimsString);
      SessionDictionary instance = SessionDictionary.Instance;
      using (XmlDictionaryReader binaryReader = XmlDictionaryReader.CreateBinaryReader(buffer, 0, buffer.Length, (IXmlDictionary) instance, XmlDictionaryReaderQuotas.Max))
        new ClaimsSerializer(instance).ReadClaims(binaryReader, claims);
    }

    private string SerializeActorToString(IClaimsIdentity actor)
    {
      if (actor == null)
        return (string) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter((ISurrogateSelector) null, this._context).Serialize((Stream) memoryStream, (object) actor);
        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
      }
    }

    private string SerializeBootstrapTokenToString(SecurityToken bootstrapToken)
    {
      if (bootstrapToken == null)
        return string.Empty;
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = ServiceConfiguration.GetCurrent().SecurityTokenHandlers;
      if (securityTokenHandlers.CanWriteToken(bootstrapToken))
      {
        MemoryStream memoryStream = new MemoryStream();
        using (XmlDictionaryWriter binaryWriter = XmlDictionaryWriter.CreateBinaryWriter((Stream) memoryStream))
        {
          securityTokenHandlers.WriteToken((XmlWriter) binaryWriter, bootstrapToken);
          binaryWriter.Flush();
          return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
        }
      }
      else
      {
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8000", (object) bootstrapToken.GetType().ToString()));
        return string.Empty;
      }
    }

    private string SerializeClaimsToString(IEnumerable<Claim> claims)
    {
      MemoryStream memoryStream = new MemoryStream();
      using (XmlDictionaryWriter binaryWriter = XmlDictionaryWriter.CreateBinaryWriter((Stream) memoryStream))
      {
        new ClaimsSerializer(SessionDictionary.Instance).WriteClaims(binaryWriter, claims);
        binaryWriter.Flush();
        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
      }
    }
  }
}
