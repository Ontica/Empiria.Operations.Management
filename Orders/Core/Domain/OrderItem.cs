/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : OrderItem                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an abstract order item.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;
using Empiria.StateEnums;

namespace Empiria.Orders {

  /// <summary>Represents an abstract order item.</summary>
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


    public OrderItem() {
      //no-op
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
      get;
      private set;
    }


    [DataField("ORDER_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("ORDER_ITEM_DESCRIPTION")]
    public string Description {
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


    [DataField("ORDER_ITEM_PRODUCT_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("ORDER_ITEM_RELATED_ITEM_ID")]
    protected internal int RelatedItemId {
      get; protected set;
    } = -1;


    [DataField("ORDER_ITEM_REQUISITION_ITEM_ID")]
    internal int RequisitionItemId {
      get; private set;
    } = -1;


    [DataField("ORDER_ITEM_REQUESTED_BY_ID")]
    public Party RequestedBy {
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


    [DataField("ORDER_ITEM_PER_EACH_ITEM_ID")]
    internal int PerEachItemId {
      get; private set;
    } = -1;


    [DataField("ORDER_ITEM_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSITION")]
    public int Position {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ORDER_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Description, Product.Name);
      }
    }

    #endregion Properties

    #region Methods

    internal protected virtual void Delete() {
      Assertion.Require(this.Status != EntityStatus.Deleted,
                  $"No se puede eliminar una orden que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnBeforeSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
    }


    internal protected virtual void Update(OrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Product = Patcher.Patch(fields.ProductUID, Product);
      Description = EmpiriaString.Clean(fields.Description);
      ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit);
      Quantity = fields.Quantity;
      RequestedBy = Patcher.Patch(fields.RequestedByUID, Order.RequestedBy);
      Project = Patcher.Patch(fields.ProjectUID, Order.Project);
      Provider = Patcher.Patch(fields.ProviderUID, Order.Provider);

      MarkAsDirty();
    }

    #endregion Methods

  }  // class OrderItem

}  // namespace Empiria.Orders
