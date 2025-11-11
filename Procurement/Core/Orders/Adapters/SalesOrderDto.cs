/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : SalesOrderDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract data transfer objects used to return sales orders information.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Documents;
using Empiria.History;
using Empiria.Procurement.Orders;
using Empiria.StateEnums;


namespace Empiria.Orders.Adapters {


  /// <summary>Data transfer object used to return complete Sales orders information.</summary>
  public class SalesOrderHolderDto {

    public SalesOrderDto Order {
      get; internal set;
    }


    public FixedList<SalesOrderItemDto> Items {
      get; internal set;
    }


    public SalesOrderActions Actions {
      get; internal set;
    }

  }  // class SalesOrderHolderDto

  public class SalesOrderActions {

    public bool CanEdit {
      get; set;
    } = false;


    public bool CanEditItems {
      get; set;
    } = false;


    public bool CanEditEntries {
      get; set;
    } = false;


    public bool CanDelete {
      get; set;
    } = false;


    public bool CanClose {
      get; set;
    } = false;


    public bool CanOpen {
      get; set;
    } = false;


  } // class SalesOrderHolderDto


  /// <summary>Data transfer object used to return Sales orders information.</summary>
  /// 
  public class SalesOrderDto : OrderDto {

    public SalesOrderDto() : base(null) {
    }

    protected internal SalesOrderDto(SalesOrder order) : base(order) {
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

    public new string UID {
      get; private set;
    }

    public new NamedEntityDto Type {
      get; internal set;
    }

    public new NamedEntityDto Category {
      get; internal set;
    }

    public new string OrderNo {
      get; internal set;
    }

    public new string Description {
      get; internal set;
    }

    public new FixedList<string> Identificators {
      get; internal set;
    }

    public new FixedList<string> Tags {
      get; internal set;
    }

    public new NamedEntityDto Responsible {
      get; internal set;
    }

    public new NamedEntityDto Beneficiary {
      get; internal set;
    }

    public new bool IsForMultipleBeneficiaries {
      get; internal set;
    }

    public new NamedEntityDto Provider {
      get; internal set;
    }

    public new FixedList<NamedEntityDto> ProvidersGroup {
      get; internal set;
    }

    public new NamedEntityDto RequestedBy {
      get; internal set;
    }

    public new NamedEntityDto Project {
      get; internal set;
    }

    public new NamedEntityDto Priority {
      get; internal set;
    }

    public new DateTime AuthorizationTime {
      get; internal set;
    }

    public new NamedEntityDto AuthorizedBy {
      get; internal set;
    }

    public new DateTime ClosingTime {
      get; internal set;
    }

    public new NamedEntityDto ClosedBy {
      get; internal set;
    }

    public new NamedEntityDto Status {
      get; internal set;
    }

    public SalesOrderDto Order {
      get;
      internal set;
    }
    public FixedList<SalesOrderItemDto> Items {
      get;
      internal set;
    }
    public FixedList<DocumentDto> Documents {
      get;
      internal set;
    }
    public FixedList<HistoryEntryDto> History {
      get;
      internal set;
    }

  } // class SalesOrderDto

  public class SalesOrderDescriptor : OrderDescriptor {

    protected internal SalesOrderDescriptor(SalesOrder order) : base(order) {
      ResponsibleName = order.Responsible.Name;
      BeneficiaryName = order.Beneficiary.Name;
      RequestedByName = order.RequestedBy.Name;
    }

    public string ResponsibleName {
      get; private set;
    }

    public string BeneficiaryName {
      get; private set;
    }

    public string RequestedByName {
      get; private set;
    }

  } // class SalesOrderDescriptor

} // namespace Empiria.Orders.Adapters
