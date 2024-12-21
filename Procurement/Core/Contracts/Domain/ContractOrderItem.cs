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

    protected ContractOrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal ContractOrderItem(ContractOrder order,
                               ContractItem contractItem) : base(OrderItemType.ContractOrderItemType, order) {

      Assertion.Require(contractItem, nameof(contractItem));

      _ = order.GetItem<ContractOrderItem>(contractItem.UID);

      this.ContractItem = contractItem;
    }

    static private ContractOrderItem Parse(int id) => ParseId<ContractOrderItem>(id);

    static private ContractOrderItem Parse(string uid) => ParseKey<ContractOrderItem>(uid);

    static internal new ContractOrderItem Empty => ParseEmpty<ContractOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    public new ContractOrder Order {
      get {
        return (ContractOrder) base.Order;
      }
    }

    public ContractItem ContractItem {
      get {
        return ContractItem.Parse(base.RelatedItemId);
      } set {
        base.RelatedItemId = value.Id;
      }
    }

    #endregion Properties

  }  // class ContractOrderItem

}  // namespace Empiria.Procurement.Contracts
