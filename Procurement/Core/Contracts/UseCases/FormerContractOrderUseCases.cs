/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : ContractOrderUseCases                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract supply orders.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Procurement.Contracts.UseCases {

  /// <summary>Use cases for contract supply orders.</summary>
  public class FormerContractOrderUseCases : UseCase {

    #region Constructors and parsers

    protected FormerContractOrderUseCases() {
      // no-op
    }

    static public FormerContractOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FormerContractOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractOrderHolderDto CreateContractOrder(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = FormerContract.Parse(fields.ContractUID);

      var order = new ContractOrder(contract);

      order.Update(fields);

      order.Save();

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderItemDto CreateContractOrderItem(string orderUID, ContractOrderItemFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var contractOrder = ContractOrder.Parse(orderUID);
      var contractItem = FormerContractItem.Parse(fields.ContractItemUID);

      var item = new ContractOrderItem(contractOrder, contractItem);

      item.Update(fields);

      contractOrder.AddItem(item);

      item.Save();

      return ContractOrderMapper.Map(item);
    }


    public ContractOrderHolderDto DeleteContractOrder(string contractOrderUID) {
      Assertion.Require(contractOrderUID, nameof(contractOrderUID));

      var order = ContractOrder.Parse(contractOrderUID);

      order.Delete();

      order.Save();

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderItemDto DeleteContractOrderItem(string orderUID, string orderItemUID) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(orderItemUID, nameof(orderItemUID));

      var order = ContractOrder.Parse(orderUID);

      var item = order.GetItem<ContractOrderItem>(orderItemUID);

      order.RemoveItem(item);

      item.Save();

      return ContractOrderMapper.Map(item);
    }


    public ContractOrderHolderDto GetContractOrder(string contractOrderUID) {
      Assertion.Require(contractOrderUID, nameof(contractOrderUID));

      var order = ContractOrder.Parse(contractOrderUID);

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderHolderDto UpdateContractOrder(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = ContractOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderItemDto UpdateContractOrderItem(string orderUID,
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

  }  // class ContractOrderUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
