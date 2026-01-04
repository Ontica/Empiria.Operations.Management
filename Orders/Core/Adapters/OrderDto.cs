/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : PayableOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract data transfer objects used to return orders information.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

using Empiria.Documents;
using Empiria.History;

namespace Empiria.Orders.Adapters {

  /// <summary>Abstract data transfer objects used to return orders information.</summary>
  abstract public class OrderHolderDto {

    public OrderDto Order {
      get; set;
    }

    public FixedList<DocumentDto> Documents {
      get; set;
    }

    public FixedList<HistoryEntryDto> History {
      get; set;
    }

    public OrderActions Actions {
      get; set;
    }

  }  // class OrderHolderDto



  public class OrderActions : BaseActions {

    public bool CanActivate {
      get; set;
    }

    public bool CanEditItems {
      get; set;
    }

    public bool CanSuspend {
      get; set;
    }

    public bool CanCommitBudget {
      get; set;
    }

    public bool CanRequestBudget {
      get; set;
    }

    public bool CanEditBills {
      get; set;
    }

    public bool CanRequestPayment {
      get; set;
    }

    public bool CanValidateBudget {
      get; set;
    }

  } // class OrderActions



  /// <summary>Data transfer object used to return orders information.</summary>
  public abstract class OrderDto {

    protected OrderDto(Order order) {
      UID = order.UID;
      Type = order.OrderType.MapToNamedEntity();
      Category = order.Category.MapToNamedEntity();
      if (!order.Requisition.IsEmptyInstance) {
        Requisition = RequisitionMapper.MapToDescriptor(order.Requisition);
      }
      OrderNo = order.OrderNo;
      Name = order.Name;
      Description = order.Description;
      Justification = order.Justification;
      Identificators = order.Identificators;
      Tags = order.Tags;
      StartDate = order.StartDate;
      EndDate = order.EndDate;
      EstimatedMonths = order.EstimatedMonths;
      Currency = order.Currency.MapToNamedEntity();
      Subtotal = order.Subtotal;
      Taxes = order.Taxes.Total;
      Total = order.GetTotal();

      RequestedBy = order.RequestedBy.MapToNamedEntity();
      Responsible = order.Responsible.MapToNamedEntity();
      Beneficiary = order.Beneficiary.MapToNamedEntity();
      IsForMultipleBeneficiaries = order.IsForMultipleBeneficiaries;
      Provider = order.Provider.MapToNamedEntity();

      if (order.Provider is Parties.Group group) {
        ProvidersGroup = group.Members.MapToNamedEntityList();
      } else {
        ProvidersGroup = new FixedList<NamedEntityDto>();
      }
      Project = order.Project.MapToNamedEntity();
      Priority = order.Priority.MapToDto();
      AuthorizationTime = order.AuthorizationTime;
      AuthorizedBy = order.AuthorizedBy.MapToNamedEntity();
      ClosingTime = order.ClosingTime;
      ClosedBy = order.ClosedBy.MapToNamedEntity();
      Status = order.Status.MapToDto();

      BaseOrgUnitName = order.RequestedBy.Name;

      BudgetType = order.BudgetType.MapToNamedEntity();
      Budgets = order.Budgets.MapToNamedEntityList();
      BudgetPeriodName = order.BudgetPeriodName;

      Observations = order.Observations;
      GuaranteeNotes = order.GuaranteeNotes;
      PenaltyNotes = order.PenaltyNotes;
      DeliveryNotes = order.DeliveryNotes;
    }


    public string UID {
      get;
    }

    public NamedEntityDto Type {
      get;
    }

    public NamedEntityDto Category {
      get;
    }

    public RequisitionDescriptor Requisition {
      get;
    }

    public string OrderNo {
      get;
    }

    public string Name {
      get;
    }

    public string Description {
      get;
    }

    public string Justification {
      get;
    }

    public FixedList<string> Identificators {
      get;
    }

    public FixedList<string> Tags {
      get;
    }

    public DateTime StartDate {
      get;
    }

    public DateTime EndDate {
      get;
    }

    public int EstimatedMonths {
      get;
    }

    public NamedEntityDto Currency {
      get;
    }

    public decimal Subtotal {
      get;
    }

    public decimal Taxes {
      get;
    }

    public decimal Total {
      get;
    }

    public string BaseOrgUnitName {
      get; protected set;
    }


    public NamedEntityDto BudgetType {
      get;
    }

    public FixedList<NamedEntityDto> Budgets {
      get;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "BaseBudgetName")]
    public string BudgetPeriodName {
      get;
    }

    public NamedEntityDto RequestedBy {
      get;
    }

    public NamedEntityDto Responsible {
      get;
    }

    public NamedEntityDto Beneficiary {
      get;
    }

    public bool IsForMultipleBeneficiaries {
      get;
    }

    public NamedEntityDto Provider {
      get;
    }

    public FixedList<NamedEntityDto> ProvidersGroup {
      get;
    }

    public NamedEntityDto Project {
      get;
    }

    public NamedEntityDto Priority {
      get; protected set;
    }

    public DateTime AuthorizationTime {
      get;
    }

    public NamedEntityDto AuthorizedBy {
      get;
    }

    public DateTime ClosingTime {
      get;
    }

    public NamedEntityDto ClosedBy {
      get;
    }

    public NamedEntityDto Status {
      get;
    }

    public string Observations {
      get;
    }

    public string GuaranteeNotes {
      get;
    }

    public string PenaltyNotes {
      get;
    }

    public string DeliveryNotes {
      get;
    }

  }  // class OrderDto


  /// <summary>Output Dto used to return minimal order data.</summary>
  public class OrderDescriptor {

    public OrderDescriptor(Order order) {
      UID = order.UID;
      TypeName = order.OrderType.DisplayName;
      CategoryName = order.Category.Name;
      RequisitionNo = order.Requisition.OrderNo;
      OrderNo = order.OrderNo;
      Name = order.Name;
      Description = order.Description;
      BaseOrgUnitName = order.RequestedBy.Name;
      Budgets = order.Budgets.MapToNamedEntityList();
      BudgetPeriodName = order.BudgetPeriodName;
      ProviderName = order.Provider.Name;
      ProjectName = order.Project.Name;
      CurrencyName = order.Currency.ISOCode;
      Subtotal = order.Subtotal;
      Taxes = order.Taxes.Total;
      Total = order.GetTotal();
      StartDate = order.StartDate;
      EndDate = order.EndDate;
      PriorityUID = order.Priority.ToString();
      PriorityName = order.Priority.GetName();
      StatusName = order.Status.GetName();
    }

    public string UID {
      get;
    }

    public string TypeName {
      get;
    }

    public string CategoryName {
      get;
    }

    public string RequisitionNo {
      get;
    }

    public string OrderNo {
      get;
    }

    public string Name {
      get;
    }

    public string Description {
      get;
    }

    public string BaseOrgUnitName {
      get;
    }

    public FixedList<NamedEntityDto> Budgets {
      get;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "BaseBudgetName")]
    public string BudgetPeriodName {
      get;
    }

    public string ProviderName {
      get;
    }

    public string ProjectName {
      get;
    }

    public string CurrencyName {
      get;
    }

    public decimal Subtotal {
      get;
    }

    public decimal Taxes {
      get;
    }

    public decimal Total {
      get;
    }

    public DateTime StartDate {
      get;
    }

    public DateTime EndDate {
      get;
    }

    public string PriorityUID {
      get;
    }

    public string PriorityName {
      get;
    }

    public string StatusName {
      get;
    }

  } // class OrderDescriptor

}  // namespace Empiria.Orders.Adapters
