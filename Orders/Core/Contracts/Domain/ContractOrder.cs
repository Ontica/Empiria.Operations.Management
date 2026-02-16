/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : ContractOrder                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract's supply order.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

using Empiria.Orders.Contracts.Data;

namespace Empiria.Orders.Contracts {

  /// <summary>Represents a contract's supply order.</summary>
  public class ContractOrder : PayableOrder {

    #region Constructors and parsers

    protected ContractOrder(OrderType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal ContractOrder(Contract contract) : base(contract.Requisition, OrderType.ContractOrder) {
      Assertion.Require(contract, nameof(contract));
      Assertion.Require(!contract.IsEmptyInstance, nameof(contract));

      Contract = contract;
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

    public new Contract Contract {
      get {
        return (Contract) base.Contract;
      }
      set {
        Assertion.Require(value, nameof(value));

        base.Contract = value;
      }
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
      if (IsNew) {
        OrderNo = ContractOrdersData.GetNextContractOrderNo(Contract);
      }
      base.OnSave();
    }


    internal void RemoveItem(ContractOrderItem contractOrderItem) {
      Assertion.Require(contractOrderItem, nameof(contractOrderItem));

      base.Items.Remove(contractOrderItem);
    }


    internal protected void Update(ContractOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.CurrencyUID = Contract.Currency.UID;
      fields.ExchangeRate = Contract.ExchangeRate;

      fields.EnsureValid();

      fields.ProviderUID = Contract.Provider.UID;

      fields.Budgets = new string[] { fields.BudgetUID };

      base.Update(fields);
    }

    #endregion Methods

  }  // class ContractOrder

}  // namespace Empiria.Orders.Contracts
