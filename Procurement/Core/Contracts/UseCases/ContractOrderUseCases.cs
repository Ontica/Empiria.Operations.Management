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
  public class ContractOrderUseCases : UseCase {

    #region Constructors and parsers

    protected ContractOrderUseCases() {
      // no-op
    }

    static public ContractOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ContractOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractOrderHolderDto CreateContractOrder(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = Contract.Parse(fields.ContractUID);

      var order = new ContractOrder(contract);

      order.Update(fields);

      order.Save();

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderHolderDto GetContractOrder(string contractOrderUID) {
      Assertion.Require(contractOrderUID, nameof(contractOrderUID));

      var order = ContractOrder.Parse(contractOrderUID);

      return ContractOrderMapper.Map(order);
    }


    public ContractOrderHolderDto RemoveContractOrder(string contractOrderUID) {
      Assertion.Require(contractOrderUID, nameof(contractOrderUID));

      var order = ContractOrder.Parse(contractOrderUID);

      order.Delete();

      order.Save();

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

    #endregion Use cases

  }  // class ContractOrderUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
