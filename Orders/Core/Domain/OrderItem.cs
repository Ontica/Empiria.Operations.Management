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
using Empiria.StateEnums;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents an abstract order item.</summary>
  [PartitionedType(typeof(OrderItemType))]
  abstract public class OrderItem : BaseObject, INamedEntity {

    #region Constructors and parsers

    static private OrderItem Parse(int id) => ParseId<OrderItem>(id);

    static private OrderItem Parse(string uid) => ParseKey<OrderItem>(uid);

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


    string INamedEntity.Name {
      get {
        if (Description.Length != 0) {
          return Description;
        }
        return Product.Name;
      }
    }


    [DataField("ORDER_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("ORDER_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("ORDER_ITEM_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("ORDER_ITEM_QTY_UNIT_ID")]
    public ProductUnit QuantityUnit {
      get; private set;
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Description, Product.Name);
      }
    }

    [DataField("ORDER_ITEM_EXT_DATA")]
    protected JsonObject ExtData {
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

    #endregion Properties

    #region Methods

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

      OrdersData.WriteOrderItem(this, this.ExtData.ToString());
    }

    #endregion Methods

  }  // class OrderItem

}  // namespace Empiria.Orders
