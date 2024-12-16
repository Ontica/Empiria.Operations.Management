/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractOrderDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contract supply orders.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contract supply orders.</summary>
  public class ContractOrderDto {

    public string UID {
      get; internal set;
    }


    public string OrderNo {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }

    public NamedEntityDto Contract {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto Supplier {
      get; internal set;
    }


    public NamedEntityDto ManagedByOrgUnit {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }


    public FixedList<ContractOrderItemDto> Items {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class ContractOrderDto



  /// <summary>Output Dto used to return minimal contract order data.</summary>
  public class ContractOrderDescriptor {

    public string UID {
      get; internal set;
    }

    public string OrderNo {
      get; internal set;
    }

    public string ContractUID {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public string Responsible {
      get; internal set;
    }


    public string Beneficiary {
      get; internal set;
    }


    public string Provider {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class ContractOrderDescriptor

}  // namespace Empiria.Procurement.Contracts.Adapters
