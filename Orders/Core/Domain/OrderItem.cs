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


    [DataField("ORDER_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("ORDER_ITEM_SKU_ID")]
    internal int SkuId {
      get; private set;
    } = -1;


    [DataField("ORDER_ITEM_PRODUCT_CODE")]
    public string ProductCode {
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
    public decimal RequestedQty {
      get; private set;
    }


    [DataField("ORDER_ITEM_QTY")]
    public decimal Quantity {
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

    public decimal Subtotal {
      get {
        return Math.Round((Quantity * UnitPrice) - Discount, 2);
      }
    }

    [DataField("ORDER_ITEM_PRICE_ID")]
    public int PriceId {
      get; protected set;
    } = -1;


    [DataField("ORDER_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }


    [DataField("ORDER_ITEM_BUDGET_ACCOUNT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }

    [DataField("ORDER_ITEM_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("ORDER_ITEM_PROVIDER_ID")]
    public Party Provider {
      get; private set;
    }

    [DataField("ORDER_ITEM_REQUISITION_ID")]
    public Requisition Requisition {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUISITION_ITEM_ID")]
    private int _requisitionItemId;

    public OrderItem RequisitionItem {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return Parse(_requisitionItemId);
      }
      private set {
        _requisitionItemId = value.Id;
      }
    }


    [DataField("ORDER_ITEM_CONTRACT_ITEM_ID")]
    protected internal int ContractItemId {
      get; protected set;
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


    [DataField("ORDER_ITEM_ORIGIN_COUNTRY_ID")]
    public Location OriginCountry {
      get; private set;
    }


    [DataField("ORDER_ITEM_SUPPLY_START_DATE")]
    public DateTime SupplyStartDate {
      get; private set;
    }


    [DataField("ORDER_ITEM_SUPPLY_END_DATE")]
    public DateTime SupplyEndDate {
      get; private set;
    }


    [DataField("ORDER_ITEM_LOCATION_ID")]
    public Location Location {
      get; protected set;
    }


    [DataField("ORDER_ITEM_DELIVERY_PLACE_ID")]
    public Location DeliveryPlace {
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
        return EmpiriaString.BuildKeywords(Description, Product.Keywords, ProductCode,
                                           Requisition.Keywords, Order.Keywords, Justification);
      }
    }


    [DataField("ORDER_ITEM_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("ORDER_ITEM_REQUIRED_TIME")]
    public DateTime RequiredTime {
      get; private set;
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

      this.Status = StateEnums.EntityStatus.Closed;
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


    internal protected virtual void Update(OrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Product = Patcher.Patch(fields.ProductUID, Product.Empty);
      ProductCode = EmpiriaString.Clean(fields.ProductCode);

      Description = EmpiriaString.Clean(fields.Description);
      if (Description.Length == 0 && !Product.IsEmptyInstance) {
        Description = Product.Description;
      } else {
        Description = "Sin descripción";
      }

      Justification = EmpiriaString.Clean(fields.Justification);

      ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit);

      Quantity = fields.Quantity;

      if (RequestedQty != 0) {
        RequestedQty = fields.RequestedQty;
      } else {
        RequestedQty = Quantity;
      }

      UnitPrice = fields.UnitPrice;
      Discount = fields.Discount;
      Currency = Patcher.Patch(fields.CurrencyUID, Order.Currency);

      Budget = Patcher.Patch(fields.BudgetUID, Order.BaseBudget);
      BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, BudgetAccount.Empty);

      Project = Patcher.Patch(fields.ProjectUID, Order.Project);
      Provider = Patcher.Patch(fields.ProviderUID, Order.Provider);
      Requisition = Patcher.Patch(fields.RequisitionUID, Order.Requisition);
      RequisitionItem = Patcher.Patch(fields.RequisitionItemUID, Empty);
      RelatedItem = Patcher.Patch(fields.RelatedItemUID, Empty);

      OriginCountry = Patcher.Patch(fields.OriginCountryUID, Location.Empty);

      SupplyStartDate = Patcher.Patch(fields.SupplyStartDate, ExecutionServer.DateMaxValue);
      SupplyEndDate = Patcher.Patch(fields.SupplyEndDate, ExecutionServer.DateMaxValue);

      RequestedBy = Patcher.Patch(fields.RequestedByUID, Order.RequestedBy);
      RequiredTime = Patcher.Patch(fields.RequiredTime, ExecutionServer.DateMaxValue);

      MarkAsDirty();
    }


    internal protected virtual void UpdateQuantity(decimal quantity) {
      Assertion.Require(quantity > 0,
                  $"La cantidad debe de ser mayor que 0.");

      this.Quantity = quantity;
    }

    #endregion Methods

  }  // class OrderItem

}  // namespace Empiria.Orders
