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

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;
using Empiria.Time;

using Empiria.Budgeting;
using Empiria.Projects;

using Empiria.Procurement.Contracts.Data;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract item.</summary>
  [PartitionedType(typeof(ContractItemType))]
  public class ContractItem : BaseObject, INamedEntity {

    #region Constructors and parsers

    private ContractItem(ContractItemType contractItemType) : base (contractItemType) {
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


    [DataField("CONTRACT_ITEM_PRODUCT_ID")]
    public Product Product {
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


    [DataField("CONTRACT_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PRODUCT_UNIT_ID")]
    public ProductUnit ProductUnit {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
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


    [DataField("CONTRACT_ITEM_BUDGET_ACCOUNT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_SUPPLIER_ID")]
    public Party Supplier {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PERIODICITY_RULE")]
    public JsonObject PeriodicityRule {
      get; private set;
    }


    public Periodicity PeriodicityType {
      get {
        return PeriodicityRule.Get("periodicityType", Periodicity.Empty);
      }
      set {
        PeriodicityRule.SetIfValue("periodicityType", value);
      }
    }

    [DataField("CONTRACT_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Description);
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

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      ContractItemData.WriteContractItem(this, this.ExtData.ToString());
    }

    #endregion Methods

    #region Helpers

    internal void Update(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.Product = PatchField(fields.ProductUID, Product);
      this.Description = PatchCleanField(fields.Description, Description);
      this.ProductUnit = PatchField(fields.ProductUnitUID, ProductUnit);
      this.MinQuantity = fields.MinQuantity;
      this.MaxQuantity = fields.MaxQuantity;
      this.UnitPrice = fields.UnitPrice;
      this.Supplier = PatchField(fields.SupplierUID, Contract.Supplier);
      this.BudgetAccount = PatchField(fields.BudgetAccountUID, BudgetAccount);
      this.Project = PatchField(fields.ProjectUID, Project);
      this.PeriodicityType = PatchField(fields.PeriodicityTypeUID, PeriodicityType);
    }

    #endregion Helpers

  }  // class ContractItem

}  // namespace Empiria.Procurement.Contracts
