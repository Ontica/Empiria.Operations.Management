/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Query DTO                               *
*  Type     : ContractItemQuery                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search contracts item.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Query data transfer object used to search contracts item.</summary>
  public class ContractItemQuery {

    public string ProductUID {
      get; set;
    }

    public string Keywords {
      get; set;
    }

  }  // class ContractItemQuery

} // namespace Empiria.Procurement.ContractsItem.Adapters
