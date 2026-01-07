/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Partitioned type                        *
*  Type     : OrderItem                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract partitioned type that represents an order item.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Locations;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Abstract partitioned type that represents an order item.</summary>
  [PartitionedType(typeof(OrderItemType))]
  abstract public class OrderItem : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected OrderItem(OrderItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    protected OrderItem(OrderItemType powertype, Order order) : base(powertype) {
      Assertion.Require(order, nameof(order));
      Assertion.Require(!order.IsEmptyInstance, nameof(order));

      this.Order = order;
    }

    static public OrderItem Parse(int id) => ParseId<OrderItem>(id);

    static public OrderItem Parse(string uid) => ParseKey<OrderItem>(uid);

    static internal OrderItem Empty => ParseEmpty<OrderItem>();

    #endregion Constructors and parsers

    #region Properties

    public OrderItemType OrderItemType {
      get {
        return (OrderItemType) base.GetEmpiriaType();
      }
    }


    [DataField("ORDER_ITEM_ORDER_ID")]
    public Order Order {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUISITION_ID")]
    public Requisition Requisition {
      get; private set;
    }


    [DataField("ORDER_ITEM_CONTRACT_ID")]
    protected internal Order Contract {
      get; set;
    }


    [DataField("ORDER_ITEM_REQUISITION_ITEM_ID")]
    private int _requisitionItemId;

    public OrderItem RequisitionItem {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        if (_requisitionItemId == -1) {
          return Empty;
        }

        return Parse(_requisitionItemId);
      }
      protected set {
        _requisitionItemId = value.Id;
      }
    }


    [DataField("ORDER_ITEM_CONTRACT_ITEM_ID")]
    private int _contractItemId;

    public OrderItem ContractItem {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        if (_contractItemId == -1) {
          return Empty;
        }

        return Parse(_contractItemId);
      }
      protected set {
        _contractItemId = value.Id;
      }
    }


    [DataField("ORDER_ITEM_RELATED_ITEM_ID")]
    private int _relatedItemId;
    public OrderItem RelatedItem {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return Parse(_relatedItemId);
      }
      protected set {
        _relatedItemId = value.Id;
      }
    }


    [DataField("ORDER_ITEM_SKU_ID")]
    public int SkuId {
      get; private set;
    } = -1;


    [DataField("ORDER_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("ORDER_ITEM_PRODUCT_CODE")]
    public string ProductCode {
      get; private set;
    }


    [DataField("ORDER_ITEM_PRODUCT_NAME")]
    public string ProductName {
      get; private set;
    }


    [DataField("ORDER_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("ORDER_ITEM_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }

    string INamedEntity.Name {
      get {
        if (ProductName.Length != 0) {
          return ProductName;
        }
        if (Description.Length != 0) {
          return Description;
        }
        return Product.Name;
      }
    }


    [DataField("ORDER_ITEM_PRODUCT_UNIT_ID")]
    public ProductUnit ProductUnit {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUESTED_QTY")]
    public decimal RequestedQuantity {
      get; private set;
    }


    [DataField("ORDER_ITEM_MIN_QTY")]
    protected internal decimal MinQuantity {
      get; set;
    }


    [DataField("ORDER_ITEM_MAX_QTY")]
    protected internal decimal MaxQuantity {
      get; set;
    }


    [DataField("ORDER_ITEM_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("ORDER_ITEM_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("ORDER_ITEM_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("ORDER_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("ORDER_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("ORDER_ITEM_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("ORDER_ITEM_PENALTY_DISCOUNT")]
    public decimal PenaltyDiscount {
      get; private set;
    }


    public decimal DiscountsTotal {
      get {
        return Discount + PenaltyDiscount;
      }
    }


    public decimal Subtotal {
      get {
        return Math.Round((Quantity * UnitPrice) - DiscountsTotal, 2);
      }
    }

    [DataField("ORDER_ITEM_PRICE_ID")]
    public int PriceId {
      get; protected set;
    } = -1;


    [DataField("ORDER_ITEM_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ACCOUNT_ID")]
    public BudgetAccount BudgetAccount {
      get; protected set;
    }


    [DataField("ORDER_ITEM_BUDGET_ENTRY_ID")]
    public BudgetEntry BudgetEntry {
      get; private set;
    }


    [DataField("ORDER_ITEM_GEO_ORIGIN_ID")]
    public Country OriginCountry {
      get; private set;
    }


    [DataField("ORDER_ITEM_LOCATION_ID")]
    public Location Location {
      get; protected set;
    }


    [DataField("ORDER_ITEM_CONFIG_EXT_DATA")]
    internal protected JsonObject ConfigData {
      get; private set;
    }


    [DataField("ORDER_ITEM_CONDITIONS_EXT_DATA")]
    internal protected JsonObject ConditionsData {
      get; private set;
    }


    [DataField("ORDER_ITEM_SPEC_EXT_DATA")]
    internal protected JsonObject SpecificationData {
      get; private set;
    }


    [DataField("ORDER_ITEM_EXT_DATA")]
    internal protected JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ProductName, Description, Product.Keywords, ProductCode,
                                           Requisition.Keywords, Order.Keywords, Contract.Keywords,
                                           Justification);
      }
    }

    [DataField("ORDER_ITEM_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUIRED_TIME")]
    public DateTime RequiredTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_RESPONSIBLE_ID")]
    public Party Responsible {
      get; private set;
    }


    [DataField("ORDER_ITEM_BENEFICIARY_ID")]
    public Party Beneficiary {
      get; private set;
    }


    [DataField("ORDER_ITEM_PROVIDER_ID")]
    public Party Provider {
      get; protected set;
    }


    [DataField("ORDER_ITEM_RECEIVED_BY_ID")]
    public Party ReceivedBy {
      get; protected set;
    }


    [DataField("ORDER_ITEM_CLOSING_TIME")]
    public DateTime ClosingTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_CLOSED_BY_ID")]
    public Party ClosedBy {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSITION")]
    public int Position {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ORDER_ITEM_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods


    internal protected virtual void Close() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede cerrar una orden item que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Closed;
    }


    internal protected virtual void Delete() {
      Assertion.Require(this.Status != EntityStatus.Deleted,
                  $"No se puede eliminar una orden que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
      OrdersData.WriteOrderItem(this);
    }


    internal void SetBudgetEntry(BudgetEntry budgetEntry) {

      Assertion.Require(budgetEntry, nameof(budgetEntry));
      Assertion.Require(budgetEntry.Budget.Equals(this.Budget), "Budget mismatch");
      Assertion.Require(budgetEntry.BudgetAccount.Equals(this.BudgetAccount), "BudgetAccount mismatch");

      this.BudgetEntry = budgetEntry;

      MarkAsDirty();
    }


    internal protected virtual void Update(OrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Product = Patcher.Patch(fields.ProductUID, Product.Empty);
      ProductCode = Patcher.PatchClean(fields.ProductCode, Product.InternalCode);

      Requisition = Patcher.Patch(fields.RequisitionUID, Order.Requisition);
      RequisitionItem = Patcher.Patch(fields.RequisitionItemUID, Empty);

      RelatedItem = Patcher.Patch(fields.RelatedItemUID, Empty);

      Justification = EmpiriaString.Clean(fields.Justification);

      Description = EmpiriaString.Clean(fields.Description);
      if (Description.Length == 0 && !Product.IsEmptyInstance) {
        Description = Product.Description;

      } else if (Description.Length == 0 && !BudgetAccount.IsEmptyInstance) {
        Description = BudgetAccount.Name;

      } else if (Description.Length == 0) {
        Description = "Sin descripción";
      }

      ProductName = Patcher.Patch(fields.ProductName, Description);

      ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit);

      Quantity = fields.Quantity;

      if (RequestedQuantity != 0) {
        RequestedQuantity = fields.RequestedQty;
      } else {
        RequestedQuantity = Quantity;
      }

      StartDate = Patcher.Patch(fields.StartDate, Order.StartDate);
      EndDate = Patcher.Patch(fields.EndDate, Order.EndDate);

      Currency = Patcher.Patch(fields.CurrencyUID, Order.Currency);
      UnitPrice = fields.UnitPrice;

      Discount = fields.Discount;
      PenaltyDiscount = fields.PenaltyDiscount;

      Project = Patcher.Patch(fields.ProjectUID, Order.Project);

      RequestedBy = Patcher.Patch(fields.RequestedByUID, Order.RequestedBy);
      RequiredTime = Patcher.Patch(fields.RequiredTime, ExecutionServer.DateMaxValue);
      Responsible = Patcher.Patch(fields.ResponsibleUID, Order.Responsible);
      Beneficiary = Patcher.Patch(fields.BeneficiaryUID, Order.Beneficiary);
      ReceivedBy = Patcher.Patch(fields.ReceivedByUID, Party.Empty);

      OriginCountry = Patcher.Patch(fields.OriginCountryUID, Country.Default);
      Location = Patcher.Patch(fields.LocationUID, Location.Empty);

      UpdateBudgetData(fields);

      MarkAsDirty();
    }


    private void UpdateBudgetData(OrderItemFields fields) {

      Budget = Patcher.Patch(fields.BudgetUID, Order.BaseBudget);

      if (!Order.IsForMultipleBeneficiaries) {
        if (!ContractItem.IsEmptyInstance) {
          BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, ContractItem.BudgetAccount);
        } else if (!RequisitionItem.IsEmptyInstance) {
          BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, RequisitionItem.BudgetAccount);
        } else {
          BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);
        }
        return;
      }

      if (!ContractItem.IsEmptyInstance) {

        var budgetAccounts = BudgetAccount.GetList(ContractItem.Budget.BudgetType,
                                                   (OrganizationalUnit) Beneficiary);
        var account = budgetAccounts.Find(x => x.StandardAccount.Equals(ContractItem.BudgetAccount.StandardAccount));
        Assertion.Require(account, $"No budget account found for organizational unit '{Beneficiary.Name}' " +
                                   $"and standard account '{ContractItem.BudgetAccount.StandardAccount.Name}'.");
        BudgetAccount = account;
        return;
      }

      if (!RequisitionItem.IsEmptyInstance) {

        var budgetAccounts = BudgetAccount.GetList(RequisitionItem.Budget.BudgetType,
                                                   (OrganizationalUnit) Beneficiary);
        var account = budgetAccounts.Find(x => x.StandardAccount.Equals(RequisitionItem.BudgetAccount.StandardAccount));
        Assertion.Require(account, $"No budget account found for organizational unit '{Beneficiary.Name}' " +
                                   $"and standard account '{RequisitionItem.BudgetAccount.StandardAccount.Name}'.");
        BudgetAccount = account;
        return;
      }

      if (fields.BudgetAccountUID.Length != 0) {
        BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);
        return;
      }

      Assertion.EnsureNoReachThisCode($"Unreachable code reached in {nameof(UpdateBudgetData)}");
    }


    internal protected virtual void UpdateQuantity(decimal quantity) {
      Assertion.Require(quantity > 0, $"La cantidad debe de ser mayor que 0.");

      this.Quantity = quantity;
    }

    #endregion Methods

  }  // class OrderItem

}  // namespace Empiria.Orders
