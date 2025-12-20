/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Domain Layer                          *
*  Assembly : Empiria.Procurement.Core.dll                 Pattern   : Value Type                            *
*  Type     : SupplierType                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds a static list of supplier types.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Procurement.Suppliers {

  /// <summary>Holds a static list of supplier types.</summary>
  public class SupplierType : NamedEntity {

    private static readonly FixedList<SupplierType> _supplierTypes = null;

    #region Constructors and parsers

    static SupplierType() {
      _supplierTypes = ReadSupplierTypes();
    }

    private SupplierType(string name) : base(name, name) {

    }

    static public SupplierType Parse(string uid) {
      return _supplierTypes.Find(x => x.UID == uid);
    }

    static public FixedList<SupplierType> GetSupplierTypes() {
      return _supplierTypes;
    }

    static public NamedEntity Unknown => Parse("Desconocido");

    #endregion Constructors and parsers

    static private FixedList<SupplierType> ReadSupplierTypes() {

      var storedJson = StoredJson.Parse("SupplierTypes");

      var types = storedJson.Value.GetFixedList<string>("types");

      return types.Select(x => new SupplierType(x))
                  .ToFixedList();
    }

  }  // class SupplierType

}  // namespace Empiria.Procurement.Suppliers
