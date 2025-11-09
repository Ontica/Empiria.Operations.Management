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
using System.Collections.Generic;
using System.Linq;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Projects;
using Empiria.StateEnums;
using Empiria.Financial;

using Empiria.Budgeting;

using Empiria.Procurement.Contracts.Data;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract.</summary>
  [PartitionedType(typeof(ContractType))]
  public class Contract : BaseObject, IPayableEntity, IBudgetingEntity {

    #region Fields

    private Lazy<List<ContractItem>> _items = new Lazy<List<ContractItem>>();

    #endregion Fields

    #region Constructors and parsers

    public Contract(ContractType contractType) : base(contractType) {
      // Required by Empiria Framework.
    }

    static internal Contract Parse(int contractId) => ParseId<Contract>(contractId);

    static public Contract Parse(string contractUID) => ParseKey<Contract>(contractUID);

    static public Contract Empty => ParseEmpty<Contract>();

    protected override void OnLoad() {
      _items = new Lazy<List<ContractItem>>(() => ContractItemData.GetContractItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    public ContractType ContractType {
      get {
        return (ContractType) base.GetEmpiriaType();
      }
    }


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


    [DataField("CONTRACT_MIN_TOTAL")]
    public decimal MinTotal {
      get; private set;
    }


    [DataField("CONTRACT_MAX_TOTAL")]
    public decimal MaxTotal {
      get; private set;
    }


    [DataField("CONTRACT_MGMT_ORG_UNIT_ID")]
    public OrganizationalUnit ManagedByOrgUnit {
      get; private set;
    }


    public bool IsForMultipleOrgUnits {
      get {
        return ExtData.Get("isForMultipleOrgUnits", false);
      }
      set {
        ExtData.SetIf("isForMultipleOrgUnits", value, true);
      }
    }


    [DataField("CONTRACT_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("CONTRACT_CUSTOMER_ID")]
    public Party Customer {
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


    public FixedList<Budget> Budgets {
      get {
        return ExtData.GetFixedList<Budget>("budgets", false);
      }
      private set {
        ExtData.Set("budgets", value.Select(x => x.Id));
        ExtData = JsonObject.Parse(ExtData.ToString());
      }
    }

    #endregion Properties

    #region IPayableEntity implementation

    INamedEntity IPayableEntity.Type {
      get {
        return this.ContractType;
      }
    }


    string IPayableEntity.EntityNo {
      get {
        return this.ContractNo;
      }
    }


    INamedEntity IPayableEntity.PayTo {
      get {
        return this.Supplier;
      }
    }


    INamedEntity IPayableEntity.OrganizationalUnit {
      get {
        return this.ManagedByOrgUnit;
      }
    }


    INamedEntity IPayableEntity.Currency {
      get {
        return this.Currency;
      }
    }


    decimal IPayableEntity.Total {
      get {
        return this.MaxTotal;
      }
    }

    INamedEntity IPayableEntity.Budget {
      get {
        return Budgets.Count != 0 ? Budgets[0] : Budget.Empty;
      }
    }


    INamedEntity IPayableEntity.Project {
      get {
        return Project.Empty;
      }
    }

    IEnumerable<IPayableEntityItem> IPayableEntity.Items {
      get {
        return this.GetItems();
      }
    }

    #endregion IPayableEntity implementation

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

      foreach (ContractItem item in GetItems()) {
        item.Save();
      }
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


    internal ContractItem AddItem(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      var contractItem = new ContractItem(ContractItemType.Payable, this, fields);

      _items.Value.Add(contractItem);

      contractItem.SetPosition(_items.Value.Count - 1);

      UpdateTotals();

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
      if (CanUpdate()) {
        return true;
      }
      return false;
    }


    internal bool CanRequestBudget() {
      return CanUpdate();
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
      ContractItem contractItem = _items.Value.Find(x => x.UID == contractItemUID);

      Assertion.Require(contractItem, $"Contract item {contractItemUID} not found.");

      return contractItem;
    }


    internal FixedList<ContractItem> GetItems() {
      return _items.Value.ToFixedList();
    }


    internal ContractItem RemoveItem(string contractItemUID) {
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      ContractItem contractItem = GetItem(contractItemUID);

      int removedItemPosition = contractItem.Position;

      contractItem.Delete();

      _items.Value.Remove(contractItem);

      for (int index = removedItemPosition; index < _items.Value.Count; index++) {
        _items.Value[index].SetPosition(index);
      }

      UpdateTotals();

      return contractItem;
    }


    internal void Update(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Party lastSupplier = Supplier;
      bool lastIsForMultipleOrgUnits = IsForMultipleOrgUnits;

      ContractCategory = Patcher.Patch(fields.ContractCategoryUID, ContractCategory);
      ContractNo = EmpiriaString.Clean(fields.ContractNo);
      Name = Patcher.PatchClean(fields.Name, Name);
      Description = EmpiriaString.Clean(fields.Description);
      ManagedByOrgUnit = Patcher.Patch(fields.ManagedByOrgUnitUID, ManagedByOrgUnit);
      IsForMultipleOrgUnits = fields.IsForMultipleOrgUnits;
      Customer = fields.CustomerUID.Length != 0 ? Party.Parse(fields.CustomerUID) : Organization.Primary;
      Supplier = fields.SupplierUID.Length != 0 ? Party.Parse(fields.SupplierUID) : Party.Empty;
      FromDate = fields.FromDate;
      ToDate = fields.ToDate;
      SignDate = fields.SignDate;
      BudgetType = BudgetType.Parse(fields.BudgetTypeUID);
      Budgets = fields.BudgetsUIDs.Select(x => Budget.Parse(x)).ToFixedList();
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);

      if (Supplier.Distinct(lastSupplier)) {
        UpdateSupplierForAllItems();
      }
    }

    internal ContractItem UpdateItem(ContractItem contractItem, ContractItemFields fields) {
      Assertion.Require(contractItem, nameof(contractItem));
      Assertion.Require(contractItem.Contract.Equals(this), "Wrong ContractItem.Contract instance.");
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      contractItem.Update(fields);

      UpdateTotals();

      return contractItem;
    }

    #endregion Methods

    #region Helpers

    private void UpdateSupplierForAllItems() {
      foreach (ContractItem item in GetItems()) {
        item.SetSupplier(Supplier);
      }
    }


    private void UpdateTotals() {
      MinTotal = _items.Value.Sum(x => x.MinTotal);
      MaxTotal = _items.Value.Sum(x => x.MaxTotal);
    }

    #endregion Helpers

  }  // class Contract

}  // namespace Empiria.Procurement.Contracts
