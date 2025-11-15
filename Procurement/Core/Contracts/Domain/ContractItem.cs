/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Partitioned Type                        *
*  Type     : ContractItem                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract item.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Commands;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;
using Empiria.StateEnums;
using Empiria.Time;

using Empiria.Budgeting;

using Empiria.Procurement.Contracts.Data;
using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract item.</summary>
  [PartitionedType(typeof(ContractItemType))]
  public class ContractItem : BaseObject, IPositionable, INamedEntity {

    #region Constructors and parsers

    private ContractItem(ContractItemType contractItemType) : base(contractItemType) {
      // Required by Empiria Framework.
    }


    public ContractItem(ContractItemType contractItemType,
                        Contract contract, ContractItemFields fields) : this(contractItemType) {
      Assertion.Require(contract, nameof(contract));
      Assertion.Require(fields, nameof(fields));

      Contract = contract;

      Update(fields);
    }


    static internal ContractItem Parse(string contractItemUID) => ParseKey<ContractItem>(contractItemUID);

    static internal ContractItem Parse(int id) => ParseId<ContractItem>(id);

    static public ContractItem Empty => ParseEmpty<ContractItem>();

    #endregion Constructors and parsers

    #region Properties

    public ContractItemType ContractItemType {
      get {
        return (ContractItemType) base.GetEmpiriaType();
      }
    }


    [DataField("CONTRACT_ITEM_CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    public string Name {
      get {
        if (Description.Length != 0) {
          return Description;
        } else {
          return Product.Name;
        }
      }
    }


    [DataField("CONTRACT_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PRODUCT_UNIT_ID")]
    public ProductUnit ProductUnit {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_MIN_QTY")]
    public decimal MinQuantity {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_MAX_QTY")]
    public decimal MaxQuantity {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_REQUISITION_ITEM_ID")]
    internal PayableOrderItem RequisitionItem {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_REQUESTER_ORG_UNIT_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_BUDGET_ACCOUNT_ID")]
    public FormerBudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_SUPPLIER_ID")]
    public Party Provider {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PERIODICITY_RULE")]
    internal JsonObject PeriodicityRule {
      get; private set;
    }


    public Periodicity PeriodicityType {
      get {
        return PeriodicityRule.Get("periodicityTypeId", Periodicity.Empty);
      }
      private set {
        PeriodicityRule.SetIf("periodicityTypeId", value.Id, !Periodicity.Empty.Equals(value));
      }
    }


    [DataField("CONTRACT_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("CONTRACT_ITEM_POSITION")]
    public int Position {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Description, this.Product.Keywords,
                                           this.Contract.Keywords, this.BudgetAccount.Keywords,
                                           this.Project.Keywords);
      }
    }


    [DataField("CONTRACT_ITEM_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;


    public decimal MinTotal {
      get {
        return MinQuantity * UnitPrice;
      }
    }


    public decimal MaxTotal {
      get {
        return MaxQuantity * UnitPrice;
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
      this.Position = -1;

      MarkAsDirty();
    }

    internal void SetPosition(int newPosition) {
      this.Position = newPosition;

      MarkAsDirty();
    }

    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      if (IsDirty) {
        ContractItemData.WriteContractItem(this, this.ExtData.ToString());
      }
    }


    internal void SetProvider(Party provider) {
      Assertion.Require(provider, nameof(provider));

      this.Provider = provider;

      MarkAsDirty();
    }


    internal void Update(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.Description = Patcher.PatchClean(fields.Description, Description);
      this.Product = Patcher.Patch(fields.ProductUID, Product);
      this.ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit);
      this.MinQuantity = fields.MinQuantity;
      this.MaxQuantity = fields.MaxQuantity;
      this.UnitPrice = fields.UnitPrice;
      this.Currency = Patcher.Patch(fields.CurrencyUID, Contract.Currency);
      this.RequisitionItem = Patcher.Patch(fields.RequisitionItemUID, RequisitionItem);
      this.RequestedBy = Patcher.Patch(fields.RequestedByUID, Contract.RequestedBy);
      this.Budget = Patcher.Patch(fields.BudgetUID, Budget);
      this.BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, BudgetAccount);
      this.Project = Patcher.Patch(fields.ProjectUID, Contract.Project);
      this.Provider = Patcher.Patch(fields.ProviderUID, Contract.Provider);
      this.PeriodicityType = Patcher.Patch(fields.PeriodicityTypeUID, PeriodicityType);

      MarkAsDirty();
    }

    #endregion Methods

  }  // class ContractItem

}  // namespace Empiria.Procurement.Contracts
