/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : ContractOrderItemUseCases                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to create and update order item information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Operations.Integration.Orders.UseCases {

  /// <summary>Use cases used to create and update order item information.</summary>
  public class ContractOrderItemUseCases : UseCase {

    #region Constructors and parsers

    protected ContractOrderItemUseCases() {
      // no-op
    }

    static public ContractOrderItemUseCases UseCaseInteractor() {
      return CreateInstance<ContractOrderItemUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractOrderItemDto CreateOrderItem(string orderUID, ContractOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var contractOrder = ContractOrder.Parse(orderUID);
      var contractItem = ContractItem.Parse(fields.ContractItemUID);

      var item = new ContractOrderItem(contractOrder, contractItem);

      item.Update(fields);

      contractOrder.AddItem(item);

      item.Save();

      return ContractOrderMapper.Map(item);
    }


    public ContractOrderItemDto DeleteOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var order = ContractOrder.Parse(orderUID);

      var item = order.GetItem<ContractOrderItem>(orderItemUID);

      order.RemoveItem(item);

      item.Save();

      return ContractOrderMapper.Map(item);
    }


    public ContractOrderItemDto UpdateOrderItem(string orderUID,
                                                string orderItemUID,
                                                ContractOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = ContractOrder.Parse(orderUID);

      var item = order.GetItem<ContractOrderItem>(orderItemUID);

      item.Update(fields);

      item.Save();

      return ContractOrderMapper.Map(item);
    }

    #endregion Use cases

  }  // class OrderItemUseCases

}  // namespace Empiria.Operations.Integration.Orders.UseCases
