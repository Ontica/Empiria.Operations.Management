﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Information Holder                      *
*  Type     : ContractMilestone                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;
using Empiria.Financial;

using Empiria.Procurement.Contracts.Data;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Procurement.Contracts {

  /// <summary>Represents a contract milestone.</summary>
  public class ContractMilestone : BaseObject, IPayableEntity, INamedEntity {

    #region Fields

    private Lazy<List<ContractMilestoneItem>> _items = new Lazy<List<ContractMilestoneItem>>();

    #endregion Fields

    #region Constructors and parsers

    static internal ContractMilestone Parse(int id) => ParseId<ContractMilestone>(id);

    static internal ContractMilestone Parse(string uid) => ParseKey<ContractMilestone>(uid);

    static internal FixedList<ContractMilestone> GetList() {
      return BaseObject.GetFullList<ContractMilestone>()
                       .ToFixedList()
                       .FindAll(x => x.Status != EntityStatus.Deleted);
    }


    static internal FixedList<ContractMilestone> GetListFor(Contract contract) {
      return BaseObject.GetFullList<ContractMilestone>()
                       .ToFixedList()
                       .FindAll(x => x.Contract.Equals(contract) && x.Status != EntityStatus.Deleted);
    }


    static internal ContractMilestone Empty => ParseEmpty<ContractMilestone>();

    protected override void OnLoad() {
      _items = new Lazy<List<ContractMilestoneItem>>(() => ContractMilestoneItemData.GetContractMilestoneItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }


    [DataField("MILESTONE_NO")]
    public string MilestoneNo {
      get; private set;
    } = string.Empty;


    [DataField("MILESTONE_NAME")]
    public string Name {
      get; private set;
    } = string.Empty;


    [DataField("MILESTONE_DESCRIPTION")]
    public string Description {
      get; private set;
    } = string.Empty;


    [DataField("MILESTONE_SUPPLIER_ID")]
    public Party Supplier {
      get; private set;
    } = Party.Empty;


    //[DataField("MILESTONE_PAYMENT_EXT_DATA")]
    public PaymentData PaymentData {
      get; private set;
    } = new PaymentData(JsonObject.Empty);


    [DataField("MILESTONE_MGMT_ORG_UNIT_ID")]
    public OrganizationalUnit ManagedByOrgUnit {
      get; private set;
    }


    [DataField("MILESTONE_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ManagedByOrgUnit.Name, ManagedByOrgUnit.Code);
      }
    }


    [DataField("MILESTONE_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("MILESTONE_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("MILESTONE_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region IPayableEntity interface

    INamedEntity IPayableEntity.Type {
      get {
        return base.GetEmpiriaType();
      }
    }


    string IPayableEntity.EntityNo {
      get {
        return this.MilestoneNo;
      }
    }

    INamedEntity IPayableEntity.PayTo {
      get {
        return this.Supplier;
      }
    }

    IEnumerable<IPayableEntityItem> IPayableEntity.Items {
      get {
        return ContractMilestoneItemData.GetContractMilestoneItems(this)
                                        .ToFixedList();
      }
    }

    #endregion IPayableEntity interface

    #region Methods

    internal void Activate() {
      Assertion.Require(this.Status == EntityStatus.Suspended,
                  $"No se puede activar un entregable que no está suspendido.");

      this.Status = EntityStatus.Active;
    }


    internal void Delete() {
      Assertion.Require(this.Status == EntityStatus.Active || this.Status == EntityStatus.Suspended,
                  $"No se puede eliminar un entregable que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    internal ContractMilestoneItem GetItem(string milestoneItemUID) {
      var item = _items.Value.Find(x => x.UID == milestoneItemUID);

      Assertion.Require(item, $"Contract milestone item {milestoneItemUID} not found.");

      return item;
    }


    internal FixedList<ContractMilestoneItem> GetItems() {
      return _items.Value.ToFixedList();
    }


    internal decimal GetTotal() {
      return GetItems().Sum(x => x.Total);
    }


    internal void Load(ContractMilestoneFields fields) {
      this.Contract = Contract.Parse(fields.ContractUID);
      this.MilestoneNo = fields.MilestoneNo;
      this.Name = fields.Name;
      this.Description = fields.Description;
      this.Supplier = Party.Parse(fields.SupplierUID);
      this.ManagedByOrgUnit = OrganizationalUnit.Parse(fields.ManagedByOrgUnitUID);
      //this.PaymentData = PaymentData();
      ExtData = new JsonObject();
    }


    internal void AddItem(ContractMilestoneItem milestoneItem) {
      Assertion.Require(milestoneItem, nameof(milestoneItem));
      Assertion.Require(milestoneItem.ContractMilestone.Equals(this), "Wrong ContractMilestoneItem.Contract instance");

      _items.Value.Add(milestoneItem);
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      ContractMilestoneData.WriteContractMilestone(this, this.ExtData.ToString());
    }


    internal void RemoveItem(ContractMilestoneItem milestoneItem) {
      milestoneItem.Delete();

      _items.Value.Remove(milestoneItem);
    }


    internal void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender un contrato que no está activo.");

      this.Status = EntityStatus.Suspended;
    }

    #endregion Methods

  }  // class ContractMilestone

}  // namespace Empiria.Procurement.Contracts
