/* Empiria Operations ****************************************************************************************
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
using Empiria.Projects;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Financial;

using Empiria.Orders.Data;

namespace Empiria.Orders {

  /// <summary>Represents an abstract order.</summary>
  [PartitionedType(typeof(OrderType))]
  abstract public class Order : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<OrderItem>> _items = new Lazy<List<OrderItem>>();

    #endregion Fields

    #region Constructors and parsers

    protected Order(OrderType orderType) : base(orderType) {
      // Required by Empiria Framework for all partitioned types.

      OrderNo = "Por asignar";
    }

    public Order() {
      //no-op
    }

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
        return (OrderType) GetEmpiriaType();
      }
    }


    [DataField("ORDER_CATEGORY_ID")]
    public OrderCategory Category {
      get; private set;
    }


    [DataField("ORDER_NO")]
    public string OrderNo {
      get; protected set;
    }


    [DataField("ORDER_DESCRIPTION")]
    public string Description {
      get; private set;
    }

    string INamedEntity.Name {
      get {
        return Description;
      }
    }

    [DataField("ORDER_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("ORDER_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("ORDER_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
      }
    }

    [DataField("ORDER_REQUISITION_ID")]
    private int _requisitionId;
    public Requisition Requisition {
      get {
        if (this.IsEmptyInstance && this is Requisition) {
          return (Requisition) this;
        }
        return Requisition.Parse(_requisitionId);
      }
      private set {
        _requisitionId = value.Id;
      }
    }


    [DataField("ORDER_PARENT_ID")]
    private int _parentId;
    public Order ParentOrder {
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


    [DataField("ORDER_CONTRACT_ID")]
    public int ContractId {
      get; private set;
    }


    [DataField("ORDER_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    public BudgetType BaseBudgetType {
      get {
        return BaseBudget.BudgetType;
      }
    }


    [DataField("ORDER_BASE_BUDGET_ID")]
    public Budget BaseBudget {
      get; protected set;
    }


    [DataField("ORDER_CURRENCY_ID")]
    public Currency Currency {
      get; protected set;
    }


    [DataField("ORDER_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("ORDER_RESPONSIBLE_ID")]
    public Party Responsible {
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


    [DataField("ORDER_SOURCE_ID")]
    public OperationSource Source {
      get; private set;
    }


    [DataField("ORDER_PRIORITY", Default = Priority.Normal)]
    public Priority Priority {
      get; private set;
    }

    [DataField("ORDER_CONDITIONS_EXT_DATA")]
    protected internal JsonObject Conditions {
      get; private set;
    }

    [DataField("ORDER_SPECIFICATION_EXT_DATA")]
    protected internal JsonObject Specifications {
      get; private set;
    }

    [DataField("ORDER_DELIVERY_EXT_DATA")]
    protected internal JsonObject Delivery {
      get; private set;
    }


    [DataField("ORDER_EXT_DATA")]
    protected internal JsonObject ExtData {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(OrderNo, Description, Beneficiary.Keywords,
                                           Provider.Keywords, Project.Keywords,
                                           Responsible.Keywords);
      }
    }


    [DataField("ORDER_RECORDING_TIME")]
    public DateTime RecordingTime {
      get; private set;
    }


    [DataField("ORDER_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }


    [DataField("ORDER_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get; private set;
    }

    [DataField("ORDER_APPLIED_BY_ID")]
    public Party AppliedBy {
      get; private set;
    }

    [DataField("ORDER_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get; private set;
    }

    [DataField("ORDER_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("ORDER_CLOSING_TIME")]
    public DateTime ClosingTime {
      get; private set;
    }


    [DataField("ORDER_CLOSED_BY_ID")]
    public Party ClosedBy {
      get; private set;
    }


    [DataField("ORDER_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }

    [DataField("ORDER_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    public bool IsForMultipleBeneficiaries {
      get {
        return ExtData.Get("multipleBeneficiaries", false);
      }
      set {
        ExtData.SetIf("multipleBeneficiaries", value, value);
      }
    }

    #endregion Properties

    #region Methods

    internal protected virtual void Activate() {
      Assertion.Require(this.Status == EntityStatus.Suspended,
                  $"No se puede activar una orden que no está suspendida.");

      this.Status = EntityStatus.Active;
    }


    public virtual void AddItem(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));
      Assertion.Require(orderItem.Order.Equals(this), "OrderItem.Order instance mismatch.");

      _items.Value.Add(orderItem);
    }


    public virtual void Close() {
      Assertion.Require(this.Status == EntityStatus.Pending,
                  $"No se puede cerrar una orden que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Closed;
      this.ClosedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.ClosingTime = DateTime.Now;
    }


    public virtual void Delete() {
      Assertion.Require(this.Status == EntityStatus.Pending,
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


    public decimal GetTotal() {
      return _items.Value.Sum(x => x.Subtotal);
    }

    protected override void OnSave() {
      if (base.IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      OrdersData.WriteOrder(this);
    }


    public virtual void RemoveItem(OrderItem orderItem) {
      Assertion.Require(orderItem, nameof(orderItem));

      orderItem.Delete();

      _items.Value.Remove(orderItem);
    }


    internal protected virtual void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender una orden que no está activa.");

      this.Status = EntityStatus.Suspended;
    }


    internal protected virtual void Update(OrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Category = Patcher.Patch(fields.CategoryUID, Category);
      Description = Patcher.PatchClean(fields.Description, Description);
      Justification = Patcher.PatchClean(fields.Justification, string.Empty);
      Responsible = Patcher.Patch(fields.ResponsibleUID, Responsible);
      Beneficiary = Patcher.Patch(fields.BeneficiaryUID, Beneficiary);
      IsForMultipleBeneficiaries = fields.IsForMultipleBeneficiaries;
      Provider = Patcher.Patch(fields.ProviderUID, Provider);
      RequestedBy = Patcher.Patch(fields.RequestedByUID, RequestedBy);
      Project = Patcher.Patch(fields.ProjectUID, Project);
      Priority = fields.Priority.Value;
      ParentOrder = Patcher.Patch(fields.ParentOrderUID, Empty);

      _tags = EmpiriaString.Tagging(fields.Tags);
      _identificators = EmpiriaString.Tagging(fields.Identificators);
    }

    #endregion Methods

  }  // class Order

}  // namespace Empiria.Orders
