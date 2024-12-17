/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : ContractOrderDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contract supply orders.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders.Adapters;

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Data transfer object used to return contract supply orders.</summary>
  public class ContractOrderDto : PayableOrderDto {

    public NamedEntityDto Contract {
      get; internal set;
    }

  }  // class ContractOrderDto


  /// <summary>Output Dto used to return minimal contract order data.</summary>
  public class ContractOrderDescriptor : PayableOrderDescriptor {

    public string ContractNo {
      get; internal set;
    }

  } // class ContractOrderDescriptor

}  // namespace Empiria.Procurement.Contracts.Adapters
