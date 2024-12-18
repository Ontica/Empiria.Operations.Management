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
using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Orders.Adapters {

  /// <summary>Abstract data transfer objects used to return orders information.</summary>
  abstract public class OrderHolderDto {

    public OrderDto Order {
      get; set;
    }

    public FixedList<OrderItem> Items {
      get; set;
    }

    public FixedList<DocumentDto> Documents {
      get; set;
    }

    public FixedList<HistoryDto> History {
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

  } // class OrderActions



  /// <summary>Data transfer object used to return orders information.</summary>
  public abstract class OrderDto {

    protected OrderDto(Order order) {
      UID = order.UID;
      Type = order.OrderType.MapToNamedEntity();
      Category = order.Category.MapToNamedEntity();
      OrderNo = order.OrderNo;
      Description = order.Description;
      Identificators = order.Identificators;
      Tags = order.Tags;
      Responsible = order.Responsible.MapToNamedEntity();
      Beneficiary = order.Beneficiary.MapToNamedEntity();
      IsForMultipleBeneficiaries = order.IsForMultipleBeneficiaries;
      Provider = order.Provider.MapToNamedEntity();
      if (order.Provider is Parties.Group group) {
        ProvidersGroup = group.Members.MapToNamedEntityList();
      } else {
        ProvidersGroup = new FixedList<NamedEntityDto>();
      }
      RequestedBy = order.RequestedBy.MapToNamedEntity();
      Project = order.Project.MapToNamedEntity();
      Priority = order.Priority.MapToDto();
      AuthorizationTime = order.AuthorizationTime;
      AuthorizedBy = order.AuthorizedBy.MapToNamedEntity();
      ClosingTime = order.ClosingTime;
      ClosedBy = order.ClosedBy.MapToNamedEntity();
      Status = order.Status.MapToDto();
    }

    public string UID {
      get; private set;
    }

    public NamedEntityDto Type {
      get; private set;
    }

    public NamedEntityDto Category {
      get; private set;
    }

    public string OrderNo {
      get; private set;
    }

    public string Description {
      get; private set;
    }

    public FixedList<string> Identificators {
      get; private set;
    }

    public FixedList<string> Tags {
      get; private set;
    }

    public NamedEntityDto Responsible {
      get; private set;
    }

    public NamedEntityDto Beneficiary {
      get; private set;
    }

    public bool IsForMultipleBeneficiaries {
      get; private set;
    }

    public NamedEntityDto Provider {
      get; private set;
    }

    public FixedList<NamedEntityDto> ProvidersGroup {
      get; private set;
    }

    public NamedEntityDto RequestedBy {
      get; private set;
    }

    public NamedEntityDto Project {
      get; private set;
    }

    public NamedEntityDto Priority {
      get; private set;
    }

    public DateTime AuthorizationTime {
      get; private set;
    }

    public NamedEntityDto AuthorizedBy {
      get; private set;
    }

    public DateTime ClosingTime {
      get; private set;
    }

    public NamedEntityDto ClosedBy {
      get; private set;
    }

    public NamedEntityDto Status {
      get; private set;
    }

  }  // class OrderDto



  /// <summary>Output Dto used to return minimal order data.</summary>
  abstract public class OrderDescriptor {

    protected OrderDescriptor(Order order) {
      UID = order.UID;
      TypeName = order.OrderType.Name;
      CategoryName = order.Category.Name;
      OrderNo = order.OrderNo;
      Description = order.Description;
      ResponsibleName = order.Responsible.Name;
      BeneficiaryName = order.Beneficiary.Name;
      ProviderName = order.Provider.Name;
      RequestedByName = order.RequestedBy.Name;
      ProjectName = order.Project.Name;
      PriorityName = order.Priority.GetName();
      AuthorizationTime = order.AuthorizationTime;
      AuthorizedByName = order.AuthorizedBy.Name;
      ClosingTime = order.ClosingTime;
      ClosedByName = order.ClosedBy.Name;
      StatusName = order.Status.GetName();
    }

    public string UID {
      get; private set;
    }

    public string TypeName {
      get; private set;
    }

    public string CategoryName {
      get; private set;
    }

    public string OrderNo {
      get; private set;
    }

    public string Description {
      get; private set;
    }

    public string ResponsibleName {
      get; private set;
    }

    public string BeneficiaryName {
      get; private set;
    }

    public string ProviderName {
      get; private set;
    }

    public string RequestedByName {
      get; private set;
    }

    public string ProjectName {
      get; private set;
    }

    public string PriorityName {
      get; private set;
    }

    public DateTime AuthorizationTime {
      get; private set;
    }

    public string AuthorizedByName {
      get; private set;
    }

    public DateTime ClosingTime {
      get; private set;
    }

    public string ClosedByName {
      get; private set;
    }

    public string StatusName {
      get; private set;
    }

  } // class OrderDescriptor

}  // namespace Empiria.Orders.Adapters
