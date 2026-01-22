/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Ouptut DTO                              *
*  Type     : SupplierDto, SupplierDescriptor            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return suppliers.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing.Adapters;

using Empiria.Financial.Adapters;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Holds full information for a supplier.</summary>
  public class SupplierHolderDto {

    public SupplierDto Supplier {
      get; internal set;
    }

    public FixedList<PaymentAccountDto> PaymentAccounts {
      get; internal set;
    }

    public BillsStructureDto Bills {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class SupplierHolderDto


  /// <summary>Output DTO with supplier data.</summary>
  public class SupplierDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string TaxCode {
      get; internal set;
    }

    public string EmployeeNo {
      get; internal set;
    }

    public string SubledgerAccount {
      get; internal set;
    }

    public FixedList<string> Tags {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class SupplierDescriptor


  /// <summary>Output DTO used to return minimal suppliers data to be used in lists.</summary>
  public class SupplierDescriptor {

    public string UID {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string TaxCode {
      get; internal set;
    }

    public string EmployeeNo {
      get; internal set;
    }

    public string SubledgerAccount {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class SupplierDescriptor

}  // namespace Empiria.Procurement.Suppliers.Adapters
