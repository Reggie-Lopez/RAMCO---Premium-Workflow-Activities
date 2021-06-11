// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.DisplayClaim
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class DisplayClaim
  {
    private static Dictionary<string, string> _claimDescriptionMap = DisplayClaim.PopulateClaimDescriptionMap();
    private static Dictionary<string, string> _claimTagMap = DisplayClaim.PopulateClaimTagMap();
    private string _claimType;
    private string _displayTag;
    private string _displayValue;
    private string _description;
    private bool _optional;

    private static Dictionary<string, string> PopulateClaimTagMap() => new Dictionary<string, string>()
    {
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country",
        SR.GetString("CountryText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth",
        SR.GetString("DateOfBirthText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
        SR.GetString("EmailAddressText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender",
        SR.GetString("GenderText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
        SR.GetString("GivenNameText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone",
        SR.GetString("HomePhoneText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality",
        SR.GetString("LocalityText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone",
        SR.GetString("MobilePhoneText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
        SR.GetString("NameText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone",
        SR.GetString("OtherPhoneText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode",
        SR.GetString("PostalCodeText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier",
        SR.GetString("PPIDText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince",
        SR.GetString("StateOrProvinceText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress",
        SR.GetString("StreetAddressText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname",
        SR.GetString("SurnameText")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage",
        SR.GetString("WebPageText")
      },
      {
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        SR.GetString("RoleText")
      }
    };

    private static Dictionary<string, string> PopulateClaimDescriptionMap() => new Dictionary<string, string>()
    {
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country",
        SR.GetString("CountryDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth",
        SR.GetString("DateOfBirthDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
        SR.GetString("EmailAddressDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender",
        SR.GetString("GenderDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
        SR.GetString("GivenNameDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone",
        SR.GetString("HomePhoneDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality",
        SR.GetString("LocalityDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone",
        SR.GetString("MobilePhoneDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
        SR.GetString("NameDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone",
        SR.GetString("OtherPhoneDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode",
        SR.GetString("PostalCodeDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier",
        SR.GetString("PPIDDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince",
        SR.GetString("StateOrProvinceDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress",
        SR.GetString("StreetAddressDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname",
        SR.GetString("SurnameDescription")
      },
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage",
        SR.GetString("WebPageDescription")
      },
      {
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        SR.GetString("RoleDescription")
      }
    };

    private static string ClaimTagForClaimType(string claimType)
    {
      string str = (string) null;
      DisplayClaim._claimTagMap.TryGetValue(claimType, out str);
      return str;
    }

    private static string ClaimDescriptionForClaimType(string claimType)
    {
      string str = (string) null;
      DisplayClaim._claimDescriptionMap.TryGetValue(claimType, out str);
      return str;
    }

    public static DisplayClaim CreateDisplayClaimFromClaimType(string claimType) => new DisplayClaim(claimType)
    {
      DisplayTag = DisplayClaim.ClaimTagForClaimType(claimType),
      Description = DisplayClaim.ClaimDescriptionForClaimType(claimType)
    };

    public DisplayClaim(string claimType)
      : this(claimType, (string) null, (string) null, (string) null)
    {
    }

    public DisplayClaim(string claimType, string displayTag, string description)
      : this(claimType, displayTag, description, (string) null)
    {
    }

    public DisplayClaim(
      string claimType,
      string displayTag,
      string description,
      string displayValue)
      : this(claimType, displayTag, description, displayValue, true)
    {
    }

    public DisplayClaim(
      string claimType,
      string displayTag,
      string description,
      string displayValue,
      bool optional)
    {
      this._claimType = !string.IsNullOrEmpty(claimType) ? claimType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimType));
      this._displayTag = displayTag;
      this._description = description;
      this._displayValue = displayValue;
      this._optional = optional;
    }

    public string ClaimType => this._claimType;

    public string DisplayTag
    {
      get => this._displayTag;
      set => this._displayTag = value;
    }

    public string DisplayValue
    {
      get => this._displayValue;
      set => this._displayValue = value;
    }

    public string Description
    {
      get => this._description;
      set => this._description = value;
    }

    public bool Optional
    {
      get => this._optional;
      set => this._optional = value;
    }
  }
}
