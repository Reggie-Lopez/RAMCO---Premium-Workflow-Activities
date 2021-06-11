// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustConstantsAdapter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  internal abstract class WSTrustConstantsAdapter
  {
    internal static WSTrustConstantsAdapter.WSTrustAttributeNames _attributeNames;
    internal static WSTrustConstantsAdapter.WSTrustElementNames _elementNames;
    internal static WSTrustConstantsAdapter.FaultCodeValues _faultCodes;
    internal string NamespaceURI;
    internal string Prefix;

    internal abstract WSTrustConstantsAdapter.WSTrustActions Actions { get; }

    internal virtual WSTrustConstantsAdapter.WSTrustAttributeNames Attributes
    {
      get
      {
        if (WSTrustConstantsAdapter._attributeNames == null)
          WSTrustConstantsAdapter._attributeNames = new WSTrustConstantsAdapter.WSTrustAttributeNames();
        return WSTrustConstantsAdapter._attributeNames;
      }
    }

    internal abstract WSTrustConstantsAdapter.WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm { get; }

    internal virtual WSTrustConstantsAdapter.WSTrustElementNames Elements
    {
      get
      {
        if (WSTrustConstantsAdapter._elementNames == null)
          WSTrustConstantsAdapter._elementNames = new WSTrustConstantsAdapter.WSTrustElementNames();
        return WSTrustConstantsAdapter._elementNames;
      }
    }

    internal virtual WSTrustConstantsAdapter.FaultCodeValues FaultCodes
    {
      get
      {
        if (WSTrustConstantsAdapter._faultCodes == null)
          WSTrustConstantsAdapter._faultCodes = new WSTrustConstantsAdapter.FaultCodeValues();
        return WSTrustConstantsAdapter._faultCodes;
      }
    }

    internal abstract WSTrustConstantsAdapter.WSTrustRequestTypes RequestTypes { get; }

    internal abstract WSTrustConstantsAdapter.WSTrustKeyTypes KeyTypes { get; }

    internal static WSTrustFeb2005ConstantsAdapter TrustFeb2005 => WSTrustFeb2005ConstantsAdapter.Instance;

    internal static WSTrust13ConstantsAdapter Trust13 => WSTrust13ConstantsAdapter.Instance;

    internal static WSTrustConstantsAdapter GetConstantsAdapter(string ns)
    {
      if (StringComparer.Ordinal.Equals(ns, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
        return (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005;
      return StringComparer.Ordinal.Equals(ns, "http://docs.oasis-open.org/ws-sx/ws-trust/200512") ? (WSTrustConstantsAdapter) WSTrustConstantsAdapter.Trust13 : (WSTrustConstantsAdapter) null;
    }

    internal abstract class WSTrustActions
    {
      internal string Cancel;
      internal string CancelResponse;
      internal string Issue;
      internal string IssueResponse;
      internal string Renew;
      internal string RenewResponse;
      internal string RequestSecurityContextToken;
      internal string RequestSecurityContextTokenCancel;
      internal string RequestSecurityContextTokenResponse;
      internal string RequestSecurityContextTokenResponseCancel;
      internal string Validate;
      internal string ValidateResponse;
    }

    internal class WSTrustAttributeNames
    {
      internal string Allow = nameof (Allow);
      internal string Context = nameof (Context);
      internal string Dialect = nameof (Dialect);
      internal string EncodingType = nameof (EncodingType);
      internal string OK = nameof (OK);
      internal string Type = nameof (Type);
      internal string ValueType = nameof (ValueType);
    }

    internal abstract class WSTrustComputedKeyAlgorithm
    {
      internal string Psha1;
    }

    internal class WSTrustElementNames
    {
      internal string AllowPostdating = nameof (AllowPostdating);
      internal string AuthenticationType = nameof (AuthenticationType);
      internal string BinarySecret = nameof (BinarySecret);
      internal string BinaryExchange = nameof (BinaryExchange);
      internal string CancelTarget = nameof (CancelTarget);
      internal string Claims = nameof (Claims);
      internal string ComputedKey = nameof (ComputedKey);
      internal string ComputedKeyAlgorithm = nameof (ComputedKeyAlgorithm);
      internal string CanonicalizationAlgorithm = nameof (CanonicalizationAlgorithm);
      internal string Code = nameof (Code);
      internal string Delegatable = nameof (Delegatable);
      internal string DelegateTo = nameof (DelegateTo);
      internal string Encryption = nameof (Encryption);
      internal string EncryptionAlgorithm = nameof (EncryptionAlgorithm);
      internal string EncryptWith = nameof (EncryptWith);
      internal string Entropy = nameof (Entropy);
      internal string Forwardable = nameof (Forwardable);
      internal string Issuer = nameof (Issuer);
      internal string KeySize = nameof (KeySize);
      internal string KeyType = nameof (KeyType);
      internal string Lifetime = nameof (Lifetime);
      internal string OnBehalfOf = nameof (OnBehalfOf);
      internal string Participant = nameof (Participant);
      internal string Participants = nameof (Participants);
      internal string Primary = nameof (Primary);
      internal string ProofEncryption = nameof (ProofEncryption);
      internal string Reason = nameof (Reason);
      internal string Renewing = nameof (Renewing);
      internal string RenewTarget = nameof (RenewTarget);
      internal string RequestedAttachedReference = nameof (RequestedAttachedReference);
      internal string RequestedProofToken = nameof (RequestedProofToken);
      internal string RequestedSecurityToken = nameof (RequestedSecurityToken);
      internal string RequestedTokenCancelled = nameof (RequestedTokenCancelled);
      internal string RequestedUnattachedReference = nameof (RequestedUnattachedReference);
      internal string RequestKeySize = nameof (RequestKeySize);
      internal string RequestSecurityToken = nameof (RequestSecurityToken);
      internal string RequestSecurityTokenResponse = nameof (RequestSecurityTokenResponse);
      internal string RequestType = nameof (RequestType);
      internal string SecurityContextToken = nameof (SecurityContextToken);
      internal string SignWith = nameof (SignWith);
      internal string SignatureAlgorithm = nameof (SignatureAlgorithm);
      internal string Status = nameof (Status);
      internal string TokenType = nameof (TokenType);
      internal string UseKey = nameof (UseKey);
    }

    internal abstract class WSTrustRequestTypes
    {
      internal string Cancel;
      internal string Issue;
      internal string Renew;
      internal string Validate;
    }

    internal abstract class WSTrustKeyTypes
    {
      internal string Asymmetric;
      internal string Bearer;
      internal string Symmetric;
    }

    internal class FaultCodeValues
    {
      internal string AuthenticationBadElements = nameof (AuthenticationBadElements);
      internal string BadRequest = nameof (BadRequest);
      internal string ExpiredData = nameof (ExpiredData);
      internal string FailedAuthentication = nameof (FailedAuthentication);
      internal string InvalidRequest = nameof (InvalidRequest);
      internal string InvalidScope = nameof (InvalidScope);
      internal string InvalidSecurityToken = nameof (InvalidSecurityToken);
      internal string InvalidTimeRange = nameof (InvalidTimeRange);
      internal string RenewNeeded = nameof (RenewNeeded);
      internal string RequestFailed = nameof (RequestFailed);
      internal string UnableToRenew = nameof (UnableToRenew);
    }
  }
}
