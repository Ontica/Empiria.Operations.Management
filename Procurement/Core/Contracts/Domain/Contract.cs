/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Partitioned Type                        *
*  Type     : Contract                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Financial;
using Empiria.StateEnums;

using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  public class Contract : Order {

    #region Constructors and parsers

    public Contract(OrderType contractType) : base(contractType) {
      // Required by Empiria Framework.
    }

    static internal new Contract Parse(int contractId) => ParseId<Contract>(contractId);

    static public new Contract Parse(string contractUID) => ParseKey<Contract>(contractUID);

    static public new Contract Empty => ParseEmpty<Contract>();

    static public FixedList<Contract> GetList() {
      return Order.GetList<Contract>();
    }

    #endregion Constructors and parsers

    #region Properties


    public string ContractNo {
      get {
        return base.OrderNo;
      }
    }


    public DateTime SignDate {
      get {
        return base.ExtData.Get("signDate", ExecutionServer.DateMaxValue);
      }
      private set {
        base.ExtData.SetIf("signDate", value, !ExecutionServer.IsMinOrMaxDate(value));
      }
    }


    public Contract Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        if (base.ParentOrder.IsEmptyInstance) {
          return Empty;
        }
        return (Contract) base.ParentOrder;
      }
      private set {
        base.ParentOrder = value;
      }
    }


    public bool HasParent {
      get {
        return !Parent.IsEmptyInstance;
      }
    }

    public decimal MinTotal {
      get {
        return GetItems().Sum(x => x.MinTotal);
      }
    }


    public decimal MaxTotal {
      get {
        return GetItems().Sum(x => x.MaxTotal);
      }
    }

    #endregion Properties

    #region Methods

    internal protected virtual void AddItem(ContractItem contractItem) {
      Assertion.Require(contractItem, nameof(contractItem));

      base.Items.Add(contractItem);
    }


    internal bool CanActivate() {
      if (Status == EntityStatus.Active) {
        return false;
      }
      if (Status == EntityStatus.Suspended) {
        return true;
      }
      if (ContractNo.Length != 0 && !Provider.IsEmptyInstance &&
          StartDate != ExecutionServer.DateMaxValue &&
          EndDate != ExecutionServer.DateMaxValue &&
          SignDate != ExecutionServer.DateMaxValue) {
        return true;
      }
      return false;
    }


    internal bool CanDelete() {
      if (CanUpdate()) {
        return true;
      }
      return false;
    }


    internal bool CanSuspend() {
      if (Status == EntityStatus.Active) {
        return true;
      }
      return false;
    }


    internal bool CanUpdate() {
      return (Status == EntityStatus.Pending);
    }


    internal ContractItem GetItem(string contractItemUID) {
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      ContractItem contractItem = base.GetItem<ContractItem>(contractItemUID);

      Assertion.Require(contractItem, $"Contract item {contractItemUID} not found.");

      return contractItem;
    }


    internal FixedList<ContractItem> GetItems() {
      return base.GetItems<ContractItem>();
    }


    public override FixedList<IPayableEntity> GetPayableEntities() {
      return ContractOrder.GetListFor(this)
                          .Select(x => (IPayableEntity) x)
                          .ToFixedList();
    }


    internal ContractItem RemoveItem(string contractItemUID) {
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      ContractItem contractItem = GetItem(contractItemUID);

      base.Items.Remove(contractItem);

      return contractItem;
    }


    internal void Update(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      OrderNo = Patcher.Patch(fields.ContractNo, "Sin número asignado");
      SignDate = fields.SignDate.HasValue ? fields.SignDate.Value : ExecutionServer.DateMaxValue;

      base.Update(fields);
    }


    internal ContractItem UpdateItem(ContractItem contractItem, ContractItemFields fields) {
      Assertion.Require(contractItem, nameof(contractItem));
      Assertion.Require(contractItem.Contract.Equals(this), "Wrong ContractItem.Contract instance.");
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      contractItem.Update(fields);

      return contractItem;
    }

    #endregion Methods

  }  // class Contract

}  // namespace Empiria.Procurement.Contracts
