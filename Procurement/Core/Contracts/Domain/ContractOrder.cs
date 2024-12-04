/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractOrder                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract supply order.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.StateEnums;

using Empiria.Orders;

using Empiria.Procurement.Contracts.Data;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract supply order.</summary>
  public class ContractOrder : PayableOrder {

    #region Constructors and parsers

    static internal new ContractOrder Parse(int id) => ParseId<ContractOrder>(id);

    static internal new ContractOrder Parse(string uid) => ParseKey<ContractOrder>(uid);

    static internal FixedList<ContractOrder> GetListFor(Contract contract) {
      return BaseObject.GetFullList<ContractOrder>()
                       .ToFixedList()
                       .FindAll(x => x.Contract.Equals(contract) && x.Status != EntityStatus.Deleted);
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


    internal decimal GetTotal() {
      return base.GetItems<ContractOrderItem>()
                 .Sum(x => x.Total);
    }


    internal void Load(ContractOrderFields fields) {
      this.Contract = Contract.Parse(fields.ContractUID);
    }


    protected override void OnSave() {
      ContractOrdersData.WriteContractOrder(this, this.ExtData.ToString());
    }


    internal void RemoveItem(ContractOrderItem contractOrderItem) {
      Assertion.Require(contractOrderItem, nameof(contractOrderItem));

      base.RemoveItem(contractOrderItem);
    }

    #endregion Methods

  }  // class ContractOrder

}  // namespace Empiria.Procurement.Contracts
