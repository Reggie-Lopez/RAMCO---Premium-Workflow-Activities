// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
  internal class ClaimsSerializer
  {
    private SessionDictionary _sd;

    public ClaimsSerializer(SessionDictionary sessionDictionary) => this._sd = sessionDictionary != null ? sessionDictionary : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionDictionary));

    public void ReadClaims(XmlDictionaryReader dictionaryReader, ClaimCollection claims)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (claims == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claims));
      if (!dictionaryReader.IsStartElement(this._sd.ClaimCollection, this._sd.EmptyString))
        return;
      dictionaryReader.ReadStartElement();
      while (dictionaryReader.IsStartElement(this._sd.Claim, this._sd.EmptyString))
      {
        Claim claim = new Claim(dictionaryReader.GetAttribute(this._sd.Type, this._sd.EmptyString), dictionaryReader.GetAttribute(this._sd.Value, this._sd.EmptyString), dictionaryReader.GetAttribute(this._sd.ValueType, this._sd.EmptyString), dictionaryReader.GetAttribute(this._sd.Issuer, this._sd.EmptyString), dictionaryReader.GetAttribute(this._sd.OriginalIssuer, this._sd.EmptyString));
        dictionaryReader.ReadFullStartElement();
        if (dictionaryReader.IsStartElement(this._sd.ClaimProperties, this._sd.EmptyString))
          this.ReadClaimProperties(dictionaryReader, claim.Properties);
        dictionaryReader.ReadEndElement();
        claims.Add(claim);
      }
      dictionaryReader.ReadEndElement();
    }

    private void ReadClaimProperties(
      XmlDictionaryReader dictionaryReader,
      IDictionary<string, string> properties)
    {
      if (properties == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (properties));
      dictionaryReader.ReadStartElement();
      while (dictionaryReader.IsStartElement(this._sd.ClaimProperty, this._sd.EmptyString))
      {
        string attribute1 = dictionaryReader.GetAttribute(this._sd.ClaimPropertyName, this._sd.EmptyString);
        string attribute2 = dictionaryReader.GetAttribute(this._sd.ClaimPropertyValue, this._sd.EmptyString);
        if (string.IsNullOrEmpty(attribute1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4249")));
        if (string.IsNullOrEmpty(attribute2))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4250")));
        properties.Add(new KeyValuePair<string, string>(attribute1, attribute2));
        dictionaryReader.ReadFullStartElement();
        dictionaryReader.ReadEndElement();
      }
      dictionaryReader.ReadEndElement();
    }

    public void WriteClaims(XmlDictionaryWriter dictionaryWriter, IEnumerable<Claim> claims)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (claims == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claims));
      dictionaryWriter.WriteStartElement(this._sd.ClaimCollection, this._sd.EmptyString);
      foreach (Claim claim in claims)
      {
        if (claim != null)
        {
          dictionaryWriter.WriteStartElement(this._sd.Claim, this._sd.EmptyString);
          if (!string.IsNullOrEmpty(claim.Issuer))
            dictionaryWriter.WriteAttributeString(this._sd.Issuer, this._sd.EmptyString, claim.Issuer);
          if (!string.IsNullOrEmpty(claim.OriginalIssuer))
            dictionaryWriter.WriteAttributeString(this._sd.OriginalIssuer, this._sd.EmptyString, claim.OriginalIssuer);
          dictionaryWriter.WriteAttributeString(this._sd.Type, this._sd.EmptyString, claim.ClaimType);
          dictionaryWriter.WriteAttributeString(this._sd.Value, this._sd.EmptyString, claim.Value);
          dictionaryWriter.WriteAttributeString(this._sd.ValueType, this._sd.EmptyString, claim.ValueType);
          if (claim.Properties != null && claim.Properties.Count > 0)
            this.WriteClaimProperties(dictionaryWriter, claim.Properties);
          dictionaryWriter.WriteEndElement();
        }
      }
      dictionaryWriter.WriteEndElement();
    }

    private void WriteClaimProperties(
      XmlDictionaryWriter dictionaryWriter,
      IDictionary<string, string> properties)
    {
      if (properties == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (properties));
      if (properties.Count <= 0)
        return;
      dictionaryWriter.WriteStartElement(this._sd.ClaimProperties, this._sd.EmptyString);
      foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) properties)
      {
        dictionaryWriter.WriteStartElement(this._sd.ClaimProperty, this._sd.EmptyString);
        dictionaryWriter.WriteAttributeString(this._sd.ClaimPropertyName, this._sd.EmptyString, property.Key);
        dictionaryWriter.WriteAttributeString(this._sd.ClaimPropertyValue, this._sd.EmptyString, property.Value);
        dictionaryWriter.WriteEndElement();
      }
      dictionaryWriter.WriteEndElement();
    }
  }
}
