﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : Order                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an abstract order.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents an abstract order.</summary>
  [PartitionedType(typeof(OrderType))]
  abstract public class Order : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<OrderItem>> _items = new Lazy<List<OrderItem>>();

    #endregion Fields

    #region Constructors and parsers

    static public Order Parse(int id) => ParseId<Order>(id);

    static public Order Parse(string uid) => ParseKey<Order>(uid);

    static public Order Empty => ParseEmpty<Order>();

    protected override void OnLoad() {
      _items = new Lazy<List<OrderItem>>(() => OrdersData.GetOrderItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    public OrderType OrderType {
      get {
        return (OrderType) base.GetEmpiriaType();
      }
    }


    [DataField("ORDER_CATEGORY_ID")]
    public OrderCategory Category {
      get; private set;
    }


    [DataField("ORDER_NO")]
    public string OrderNo {
      get; private set;
    }


    string INamedEntity.Name {
      get {
        return Description;
      }
    }


    [DataField("ORDER_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("ORDER_BENEFICARY_ID")]
    public Party Beneficiary {
      get; private set;
    }


    [DataField("ORDER_PROVIDER_ID")]
    public Party Provider {
      get; private set;
    }


    [DataField("ORDER_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(OrderNo, Beneficiary.Name, Provider.Name, Description);
      }
    }


    [DataField("ORDER_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ORDER_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal protected virtual void Activate() {
      Assertion.Require(this.Status == EntityStatus.Suspended,
                  $"No se puede activar una orden que no está suspendida.");

      this.Status = EntityStatus.Active;
    }


    internal protected virtual void AddItem(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));
      Assertion.Require(orderItem.Order.Equals(this), "Wrong OrderItem.Order instance");

      _items.Value.Add(orderItem);
    }


    internal protected virtual void Delete() {
      Assertion.Require(this.Status == EntityStatus.Active || this.Status == EntityStatus.Suspended,
                  $"No se puede eliminar una orden que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    public T GetItem<T>(string orderItemUID) where T : OrderItem {
      var item = _items.Value.Find(x => x.UID == orderItemUID);

      Assertion.Require(item, $"Order item {orderItemUID} not found.");

      return (T) item;
    }


    public FixedList<T> GetItems<T>() where T : OrderItem {
      return _items.Value.Select(x => (T) x)
                         .ToFixedList();
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      OrdersData.WriteOrder(this, this.ExtData.ToString());
    }


    internal protected virtual void RemoveItem(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      orderItem.Delete();

      _items.Value.Remove(orderItem);
    }


    internal protected virtual void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender un contrato que no está activo.");

      this.Status = EntityStatus.Suspended;
    }

    #endregion Methods

  }  // class Order

}  // namespace Empiria.Orders
