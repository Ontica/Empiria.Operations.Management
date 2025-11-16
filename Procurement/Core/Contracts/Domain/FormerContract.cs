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

using Empiria.Orders;

using Empiria.Procurement.Contracts.Data;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract.</summary>
  [PartitionedType(typeof(FormerContractType))]
  public class FormerContract : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<FormerContractItem>> _items = new Lazy<List<FormerContractItem>>();

    #endregion Fields

    #region Constructors and parsers

    public FormerContract(FormerContractType contractType) : base(contractType) {
      // Required by Empiria Framework.
    }

    static internal FormerContract Parse(int contractId) => ParseId<FormerContract>(contractId);

    static public FormerContract Parse(string contractUID) => ParseKey<FormerContract>(contractUID);

    static public FormerContract Empty => ParseEmpty<FormerContract>();

    protected override void OnLoad() {
      _items = new Lazy<List<FormerContractItem>>(() => ContractItemData.GetContractItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    public FormerContractType ContractType {
      get {
        return (FormerContractType) base.GetEmpiriaType();
      }
    }


    [DataField("CONTRACT_CATEGORY_ID")]
    public FormerContractCategory ContractCategory {
      get; private set;
    }


    [DataField("CONTRACT_REQUISITION_ID")]
    public Requisition Requisition {
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


    [DataField("CONTRACT_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("CONTRACT_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("CONTRACT_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("CONTRACT_PROJECT_ID")]
    public Project Project {
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
    } = ExecutionServer.DateMaxValue;


    [DataField("CONTRACT_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("CONTRACT_RESPONSIBLE_ID")]
    public Party Responsible {
      get; private set;
    }


    [DataField("CONTRACT_BENEFICIARY_ID")]
    public Party Beneficiary {
      get; private set;
    }


    public bool IsForMultipleBeneficiaries {
      get {
        return ExtData.Get("multipleBeneficiaries", false);
      }
      set {
        ExtData.SetIf("multipleBeneficiaries", value, true);
      }
    }


    [DataField("CONTRACT_PROVIDER_ID")]
    public Party Provider {
      get; private set;
    }


    [DataField("CONTRACT_NOTES")]
    public string Notes {
      get; private set;
    }


    [DataField("CONTRACT_EXT_DATA")]
    public JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ContractNo, Name, Description, Justification, Notes,
                                           RequestedBy.Keywords, Beneficiary.Keywords, Provider.Keywords,
                                           BudgetType.Name);
      }
    }


    [DataField("CONTRACT_PARENT_ID", Default = -1)]
    private int _parentId;

    public FormerContract Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
      private set {
        _parentId = value.Id;
      }
    }


    public bool HasParent {
      get {
        return !Parent.IsEmptyInstance;
      }
    }


    [DataField("CONTRACT_CLOSING_TIME")]
    public DateTime ClosingTime {
      get; private set;
    }


    [DataField("CONTRACT_CLOSED_BY_ID")]
    public Party ClosedBy {
      get; private set;
    }


    [DataField("CONTRACT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CONTRACT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CONTRACT_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
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

      ContractData.WriteContract(this);

      foreach (FormerContractItem item in GetItems()) {
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


    internal FormerContractItem AddItem(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      var contractItem = new FormerContractItem(FormerContractItemType.Payable, this, fields);

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
      if (ContractNo.Length != 0 && !Provider.IsEmptyInstance &&
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


    internal FormerContractItem GetItem(string contractItemUID) {
      FormerContractItem contractItem = _items.Value.Find(x => x.UID == contractItemUID);

      Assertion.Require(contractItem, $"Contract item {contractItemUID} not found.");

      return contractItem;
    }


    internal FixedList<FormerContractItem> GetItems() {
      return _items.Value.ToFixedList();
    }


    internal FormerContractItem RemoveItem(string contractItemUID) {
      Assertion.Require(contractItemUID, nameof(contractItemUID));

      FormerContractItem contractItem = GetItem(contractItemUID);

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

      Party lastProvider = Provider;

      ContractCategory = Patcher.Patch(fields.ContractCategoryUID, ContractCategory);
      Requisition = Patcher.Patch(fields.RequisitionUID, Requisition);

      ContractNo = EmpiriaString.Clean(fields.ContractNo);
      Name = Patcher.PatchClean(fields.Name, Name);
      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      Notes = EmpiriaString.Clean(fields.Notes);

      FromDate = fields.FromDate;
      ToDate = fields.ToDate;
      SignDate = fields.SignDate;

      BudgetType = BudgetType.Parse(fields.BudgetTypeUID);
      Budgets = fields.BudgetsUIDs.Select(x => Budget.Parse(x)).ToFixedList();
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      Project = Patcher.Patch(fields.ProjectUID, Project.Empty);

      RequestedBy = Patcher.Patch(fields.RequestedByUID, Requisition.RequestedBy);
      Responsible = Patcher.Patch(fields.ResponsibleUID, Requisition.Responsible);
      Beneficiary = Patcher.Patch(fields.BeneficiaryUID, Requisition.Beneficiary);
      IsForMultipleBeneficiaries = fields.IsForMultipleBeneficiaries;

      Provider = Patcher.Patch(fields.ProviderUID, Provider);

      if (Provider.Distinct(lastProvider)) {
        UpdateSupplierForAllItems();
      }
    }

    internal FormerContractItem UpdateItem(FormerContractItem contractItem, ContractItemFields fields) {
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
      foreach (FormerContractItem item in GetItems()) {
        item.SetProvider(Provider);
      }
    }


    private void UpdateTotals() {
      MinTotal = _items.Value.Sum(x => x.MinTotal);
      MaxTotal = _items.Value.Sum(x => x.MaxTotal);
    }

    #endregion Helpers

  }  // class Contract

}  // namespace Empiria.Procurement.Contracts
