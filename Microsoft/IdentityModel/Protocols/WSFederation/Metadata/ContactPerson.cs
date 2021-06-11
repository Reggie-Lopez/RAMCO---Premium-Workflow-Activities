// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.ContactPerson
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class ContactPerson
  {
    private ContactType _type;
    private string _company;
    private string _givenName;
    private string _surname;
    private Collection<string> _emailAddresses = new Collection<string>();
    private Collection<string> _telephoneNumbers = new Collection<string>();

    public ContactPerson()
    {
    }

    public ContactPerson(ContactType contactType) => this._type = contactType;

    public string Company
    {
      get => this._company;
      set => this._company = value;
    }

    public ICollection<string> EmailAddresses => (ICollection<string>) this._emailAddresses;

    public string GivenName
    {
      get => this._givenName;
      set => this._givenName = value;
    }

    public string Surname
    {
      get => this._surname;
      set => this._surname = value;
    }

    public ICollection<string> TelephoneNumbers => (ICollection<string>) this._telephoneNumbers;

    public ContactType Type
    {
      get => this._type;
      set => this._type = value;
    }
  }
}
