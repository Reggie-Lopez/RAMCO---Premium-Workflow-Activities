// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenHandlerCollectionManager
  {
    private Dictionary<string, SecurityTokenHandlerCollection> _collections = new Dictionary<string, SecurityTokenHandlerCollection>();
    private string _serviceName = "";

    public static SecurityTokenHandlerCollectionManager CreateEmptySecurityTokenHandlerCollectionManager() => new SecurityTokenHandlerCollectionManager("");

    public static SecurityTokenHandlerCollectionManager CreateDefaultSecurityTokenHandlerCollectionManager()
    {
      SecurityTokenHandlerCollection handlerCollection = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
      SecurityTokenHandlerCollectionManager collectionManager = new SecurityTokenHandlerCollectionManager("");
      collectionManager._collections.Clear();
      collectionManager._collections.Add("", handlerCollection);
      return collectionManager;
    }

    private SecurityTokenHandlerCollectionManager()
      : this("")
    {
    }

    public SecurityTokenHandlerCollection this[string usage]
    {
      get => usage != null ? this._collections[usage] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (usage));
      set
      {
        if (usage == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (usage));
        this._collections[usage] = value;
      }
    }

    public bool ContainsKey(string usage) => this._collections.ContainsKey(usage);

    public int Count => this._collections.Count;

    public string ServiceName => this._serviceName;

    public IEnumerable<SecurityTokenHandlerCollection> SecurityTokenHandlerCollections => (IEnumerable<SecurityTokenHandlerCollection>) this._collections.Values;

    public SecurityTokenHandlerCollectionManager(string serviceName) => this._serviceName = serviceName != null ? serviceName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceName));

    public static class Usage
    {
      public const string Default = "";
      public const string ActAs = "ActAs";
      public const string OnBehalfOf = "OnBehalfOf";
    }
  }
}
