// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;
using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens
{
  internal class SamlAttributeKeyComparer : IEqualityComparer<SamlAttributeKeyComparer.AttributeKey>
  {
    public bool Equals(
      SamlAttributeKeyComparer.AttributeKey x,
      SamlAttributeKeyComparer.AttributeKey y)
    {
      return x.Name.Equals(y.Name, StringComparison.Ordinal) && x.FriendlyName.Equals(y.FriendlyName, StringComparison.Ordinal) && (x.ValueType.Equals(y.ValueType, StringComparison.Ordinal) && x.OriginalIssuer.Equals(y.OriginalIssuer, StringComparison.Ordinal)) && x.NameFormat.Equals(y.NameFormat, StringComparison.Ordinal) && x.Namespace.Equals(y.Namespace, StringComparison.Ordinal);
    }

    public int GetHashCode(SamlAttributeKeyComparer.AttributeKey obj) => obj.GetHashCode();

    public class AttributeKey
    {
      private string _friendlyName;
      private int _hashCode;
      private string _name;
      private string _nameFormat;
      private string _namespace;
      private string _valueType;
      private string _originalIssuer;

      internal string FriendlyName => this._friendlyName;

      internal string Name => this._name;

      internal string NameFormat => this._nameFormat;

      internal string Namespace => this._namespace;

      internal string ValueType => this._valueType;

      internal string OriginalIssuer => this._originalIssuer;

      public AttributeKey(Saml11Attribute attribute)
      {
        if (attribute == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (attribute));
        this._friendlyName = string.Empty;
        this._name = attribute.Name;
        this._nameFormat = string.Empty;
        this._namespace = attribute.Namespace ?? string.Empty;
        this._valueType = attribute.AttributeValueXsiType ?? string.Empty;
        this._originalIssuer = attribute.OriginalIssuer ?? string.Empty;
        this.ComputeHashCode();
      }

      public AttributeKey(Saml2Attribute attribute)
      {
        if (attribute == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (attribute));
        this._friendlyName = attribute.FriendlyName ?? string.Empty;
        this._name = attribute.Name;
        this._nameFormat = attribute.NameFormat == (Uri) null ? string.Empty : attribute.NameFormat.AbsoluteUri;
        this._namespace = string.Empty;
        this._valueType = attribute.AttributeValueXsiType ?? string.Empty;
        this._originalIssuer = attribute.OriginalIssuer ?? string.Empty;
        this.ComputeHashCode();
      }

      public override int GetHashCode() => this._hashCode;

      private void ComputeHashCode()
      {
        this._hashCode = this._name.GetHashCode();
        this._hashCode ^= this._friendlyName.GetHashCode();
        this._hashCode ^= this._nameFormat.GetHashCode();
        this._hashCode ^= this._namespace.GetHashCode();
        this._hashCode ^= this._valueType.GetHashCode();
        this._hashCode ^= this._originalIssuer.GetHashCode();
      }
    }
  }
}
