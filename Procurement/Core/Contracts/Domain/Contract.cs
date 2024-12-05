/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : Contract                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;
using Empiria.Financial;

using Empiria.Budgeting;

using Empiria.Procurement.Contracts.Data;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract.</summary>
  public class Contract : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<ContractItem>> _items = new Lazy<List<ContractItem>>();

    #endregion Fields

    #region Constructors and parsers

    public Contract() {
      // Required by Empiria Framework.
    }

    static internal Contract Parse(int contractId) {
      return BaseObject.ParseId<Contract>(contractId);
    }


    static public Contract Parse(string contractUID) {
      return BaseObject.ParseKey<Contract>(contractUID);
    }

    static public Contract Empty => BaseObject.ParseEmpty<Contract>();

    protected override void OnLoad() {
      _items = new Lazy<List<ContractItem>>(() => ContractItemData.GetContractItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("CONTRACT_CATEGORY_ID")]
    public ContractCategory ContractCategory {
      get; private set;
    }


    [DataField("CONTRACT_NO")]
    public string ContractNo {
      get; private set;
    }


    [DataField("CONTRACT_NAME")]
    public string Name {
      get; private set;
    }


    [DataField("CONTRACT_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("CONTRACT_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("CONTRACT_FROM_DATE")]
    public DateTime FromDate {
      get; private set;
    }


    [DataField("CONTRACT_TO_DATE")]
    public DateTime ToDate {
      get; private set;
    }


    [DataField("CONTRACT_SIGN_DATE")]
    public DateTime SignDate {
      get; private set;
    }


    [DataField("CONTRACT_MGMT_ORG_UNIT_ID")]
    public OrganizationalUnit ManagedByOrgUnit {
      get; private set;
    }


    [DataField("CONTRACT_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("CONTRACT_SUPPLIER_ID")]
    public Party Supplier {
      get; private set;
    }


    [DataField("CONTRACT_PARENT_ID", Default = -1)]
    private int ParentId {
      get; set;
    } = -1;


    public Contract Parent {
      get {
        if (this.IsEmptyInstance || this.ParentId == this.Id) {
          return this;
        }
        return Contract.Parse(this.ParentId);
      }
    }


    public bool HasParent {
      get {
        return !Parent.IsEmptyInstance && Parent.Distinct(this);
      }
    }


    [DataField("CONTRACT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.ContractNo, this.Name, this.Description,
                                           ManagedByOrgUnit.Name, ManagedByOrgUnit.Code);
      }
    }


    [DataField("CONTRACT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CONTRACT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CONTRACT_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    [DataField("CONTRACT_TOTAL", ConvertFrom = typeof(decimal))]
    public decimal Total {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal void Activate() {
      Assertion.Require(this.Status == EntityStatus.Suspended,
                  $"No se puede activar un contrato que no está suspendido.");

      this.Status = EntityStatus.Active;
    }


    internal void Delete() {
      Assertion.Require(this.Status == EntityStatus.Pending,
                  $"No se puede eliminar un contrato que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      ContractData.WriteContract(this, this.ExtData.ToString());
    }


    internal void SetDates(DateTime signDate, DateTime fromDate, DateTime toDate) {
      Assertion.Require(signDate <= fromDate,
                        "La fecha de firma del contrato no puede ser posterior a la fecha de inicio.");
      Assertion.Require(fromDate <= toDate,
                        "La fecha de inicio del contrato no puede ser posterior a la fecha de su terminación.");

      this.SignDate = signDate;
      this.FromDate = fromDate;
      this.ToDate = toDate;
    }


    internal void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender un contrato que no está activo.");

      this.Status = EntityStatus.Suspended;
    }

    #endregion Methods

    #region Helpers

    internal ContractItem AddItem(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      var contractItem = new ContractItem(this, fields);

      _items.Value.Add(contractItem);

      return contractItem;
    }


    internal bool CanActivate() {
      if (Status == EntityStatus.Active) {
        return false;
      }
      if (Status == EntityStatus.Suspended) {
        return true;
      }
      if (ContractNo.Length != 0 && !Supplier.IsEmptyInstance &&
          FromDate != ExecutionServer.DateMaxValue &&
          ToDate != ExecutionServer.DateMaxValue &&
          SignDate != ExecutionServer.DateMaxValue) {
        return true;
      }
      return false;
    }

    internal bool CanDelete() {
      if (Status == EntityStatus.Pending) {
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

    internal ContractItem GetItem(string contractItemUID) {
      ContractItem contractItem = _items.Value.Find(x => x.UID == contractItemUID);

      Assertion.Require(contractItem, $"Contract item {contractItemUID} not found.");

      return contractItem;
    }


    internal FixedList<ContractItem> GetItems() {
      return _items.Value.ToFixedList();
    }


    internal FixedList<ContractMilestone> GetMilestones() {
      return ContractMilestone.GetListFor(this);
    }

    internal ContractItem RemoveItem(string contractItemUID) {
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      ContractItem contractItem = GetItem(contractItemUID);

      contractItem.Delete();

      _items.Value.Remove(contractItem);

      return contractItem;
    }


    internal void Update(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      ContractCategory = PatchField(fields.ContractCategoryUID, ContractCategory);
      ContractNo = EmpiriaString.Clean(fields.ContractNo);
      Name = PatchCleanField(fields.Name, Name);
      Description = EmpiriaString.Clean(fields.Description);
      ManagedByOrgUnit = PatchField(fields.ManagedByOrgUnitUID, ManagedByOrgUnit);
      Supplier = fields.SupplierUID.Length != 0 ? Party.Parse(fields.SupplierUID) : Party.Empty;
      FromDate = fields.FromDate;
      ToDate = fields.ToDate;
      SignDate = fields.SignDate;
      BudgetType = BudgetType.Parse(fields.BudgetTypeUID);
      Currency = PatchField(fields.CurrencyUID, Currency);
      Total = fields.Total;
    }


    internal ContractItem UpdateItem(ContractItem contractItem, ContractItemFields fields) {
      Assertion.Require(contractItem, nameof(contractItem));
      Assertion.Require(contractItem.Contract.Equals(this), "Wrong ContractItem.Contract instance.");
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      contractItem.Update(fields);

      return contractItem;
    }

    #endregion Helpers

  }  // class Contract

}  // namespace Empiria.Procurement.Contracts
