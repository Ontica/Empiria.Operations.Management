/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Ouptut DTO                              *
*  Type     : SupplierDto, SupplierDescriptor            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return suppliers.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

using Empiria.Financial.Adapters;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Holds full information for a supplier.</summary>
  public class SupplierHolderDto {

    public SupplierDescriptor Supplier {
      get; internal set;
    }

    public FixedList<PaymentAccountDto> PaymentAccounts {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

  }  // class SupplierHolderDto



  /// <summary>Output DTO used to return minimal suppliers data to be used in lists.</summary>
  public class SupplierDescriptor {

    public string UID {
      get; internal set;
    }

    public string TypeUID {
      get; internal set;
    }

    public string TypeName {
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
