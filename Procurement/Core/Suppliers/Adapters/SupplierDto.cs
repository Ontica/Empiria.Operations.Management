/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Ouptut DTO                              *
*  Type     : SupplierDto, SupplierDescriptor            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return suppliers.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Output DTO used to return minimal suppliers data to be used in lists.</summary>
  public class SupplierDescriptor {

    public string UID {
      get; internal set;
    }

    public string TypeUID {
      get; internal set;
    }

    public string SupplierTypeName {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string CommonName {
      get; internal set;
    }

    public string TaxCode {
      get; internal set;
    }

    public string TaxEntityName {
      get; internal set;
    }

    public string TaxZipCode {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class SupplierDescriptor

}  // namespace Empiria.Procurement.Suppliers.Adapters
