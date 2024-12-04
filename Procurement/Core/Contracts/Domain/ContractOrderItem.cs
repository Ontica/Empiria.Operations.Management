/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractOrderItem                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract supply order item.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract supply order item.</summary>
  public class ContractOrderItem : PayableOrderItem {

    #region Constructors and parsers

    static private ContractOrderItem Parse(int id) => ParseId<ContractOrderItem>(id);

    static private ContractOrderItem Parse(string uid) => ParseKey<ContractOrderItem>(uid);

    static internal ContractOrderItem Empty => ParseEmpty<ContractOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("ORDER_ITEM_CONTRACT_ITEM_ID")]
    public ContractItem ContractItem {
      get; private set;
    }

    #endregion Properties

  }  // class ContractOrderItem

}  // namespace Empiria.Procurement.Contracts
