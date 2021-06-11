// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public class ClaimCollection : IList<Claim>, ICollection<Claim>, IEnumerable<Claim>, IEnumerable
  {
    private List<Claim> _claims = new List<Claim>();
    private IClaimsIdentity _subject;

    public ClaimCollection(IClaimsIdentity subject) => this._subject = subject != null ? subject : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));

    public void AddRange(IEnumerable<Claim> collection)
    {
      if (collection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (collection));
      List<Claim> claimList = new List<Claim>();
      foreach (Claim claim in collection)
        claimList.Add(claim);
      foreach (Claim claim in claimList)
        this.Add(claim);
    }

    public ClaimCollection CopyWithSubject(IClaimsIdentity subject)
    {
      ClaimCollection claimCollection = new ClaimCollection(subject);
      foreach (Claim claim in this._claims)
        claimCollection.Add(claim.Copy());
      return claimCollection;
    }

    public void CopyRange(IEnumerable<Claim> collection)
    {
      if (collection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (collection));
      this.AddRange(collection.Select<Claim, Claim>((Func<Claim, Claim>) (claim => new Claim(claim.ClaimType, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer))));
    }

    public bool Exists(Predicate<Claim> match) => this._claims.Exists(match);

    public ICollection<Claim> FindAll(Predicate<Claim> match) => (ICollection<Claim>) this._claims.FindAll(match);

    public int IndexOf(Claim item) => this._claims.IndexOf(item);

    public void Insert(int index, Claim item)
    {
      if (index < 0 || index >= this._claims.Count)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (index));
      if (item == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (item));
      if (object.ReferenceEquals((object) item.Subject, (object) this._subject))
        return;
      if (item.Subject != null && item.Subject.Claims != null)
        item.Subject.Claims.Remove(item);
      this._claims.Insert(index, item);
      item.SetSubject(this._subject);
    }

    public void RemoveAt(int index) => this.Remove(this._claims[index]);

    public Claim this[int index]
    {
      get => this._claims[index];
      set
      {
        if (index < 0 || index >= this._claims.Count)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (index));
        if (value == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        if (object.ReferenceEquals((object) value.Subject, (object) this._subject))
          return;
        if (value.Subject != null && value.Subject.Claims != null)
          value.Subject.Claims.Remove(value);
        this._claims[index] = value;
        value.SetSubject(this._subject);
      }
    }

    public int Count => this._claims.Count;

    public bool IsReadOnly => false;

    public void Add(Claim item)
    {
      if (item == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (item));
      if (object.ReferenceEquals((object) item.Subject, (object) this._subject))
        return;
      if (item.Subject != null && item.Subject.Claims != null)
        item.Subject.Claims.Remove(item);
      this._claims.Add(item);
      item.SetSubject(this._subject);
    }

    public void Clear()
    {
      this._claims.ForEach((Action<Claim>) (claim => claim.SetSubject((IClaimsIdentity) null)));
      this._claims.Clear();
    }

    public bool Contains(Claim item) => this._claims.Contains(item);

    public void CopyTo(Claim[] array, int arrayIndex) => this._claims.CopyTo(array, arrayIndex);

    public bool Remove(Claim item)
    {
      if (item == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (item));
      if (item.Subject != this._subject)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4028")));
      if (!this._claims.Remove(item))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4029")));
      item.SetSubject((IClaimsIdentity) null);
      return true;
    }

    public IEnumerator<Claim> GetEnumerator() => (IEnumerator<Claim>) this._claims.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this._claims).GetEnumerator();
  }
}
