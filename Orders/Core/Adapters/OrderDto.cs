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
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public PayableOrderActions Actions {
      get; internal set;
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

    public string UID {
      get; set;
    }

    public NamedEntityDto Type {
      get; set;
    }

    public string OrderNo {
      get; set;
    }

    public string Description {
      get; set;
    }

    public NamedEntityDto Beneficiary {
      get; set;
    }

    public NamedEntityDto Provider {
      get; set;
    }

    public FixedList<NamedEntityDto> ProvidersGroup {
      get; set;
    }

    public NamedEntityDto ManagedBy {
      get; set;
    }

    public bool IsForMultipleOrgUnits {
      get; set;
    }

    public DateTime Date {
      get; set;
    }

    public NamedEntityDto Status {
      get; set;
    }

  }  // class OrderDto



  /// <summary>Output Dto used to return minimal order data.</summary>
  abstract public class OrderDescriptor {

    public string UID {
      get; set;
    }

    public string TypeName {
      get; set;
    }

    public string OrderNo {
      get; set;
    }

    public string Description {
      get; set;
    }

    public string BeneficiaryName {
      get; set;
    }

    public string ProviderName {
      get; set;
    }

    public string ManagedByName {
      get; set;
    }

    public DateTime Date {
      get; set;
    }

    public string StatusName {
      get; set;
    }

  } // class OrderDescriptor

}  // namespace Empiria.Orders.Adapters
