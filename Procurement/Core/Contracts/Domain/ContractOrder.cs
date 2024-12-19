/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractOrder                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract's supply order.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

using Empiria.Procurement.Contracts.Data;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract's supply order.</summary>
  public class ContractOrder : PayableOrder {

    #region Constructors and parsers

    protected ContractOrder(OrderType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal ContractOrder(Contract contract) : base(OrderType.ContractOrder) {
      Assertion.Require(contract, nameof(contract));
      Assertion.Require(!contract.IsEmptyInstance, nameof(contract));

      Contract = contract;
      OrderNo = EmpiriaString.BuildRandomString(16);
    }

    static internal new ContractOrder Parse(int id) => ParseId<ContractOrder>(id);

    static internal new ContractOrder Parse(string uid) => ParseKey<ContractOrder>(uid);

    static internal FixedList<ContractOrder> GetListFor(Contract contract) {
      Assertion.Require(contract, nameof(contract));

      return ContractOrdersData.GetContractOrders(contract);
    }

    static internal new ContractOrder Empty => ParseEmpty<ContractOrder>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("ORDER_CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, Contract.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    internal void AddItem(ContractOrderItem contractOrderItem) {
      Assertion.Require(contractOrderItem, nameof(contractOrderItem));

      base.AddItem(contractOrderItem);
    }


    protected override void OnSave() {
      ContractOrdersData.WriteOrder(this, this.ExtData.ToString());
    }


    internal void RemoveItem(ContractOrderItem contractOrderItem) {
      Assertion.Require(contractOrderItem, nameof(contractOrderItem));

      base.RemoveItem(contractOrderItem);
    }

    #endregion Methods

  }  // class ContractOrder

}  // namespace Empiria.Procurement.Contracts
