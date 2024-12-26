/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Query DTO                               *
*  Type     : OrdersQuery                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search orders.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Orders.Adapters {

  /// <summary>Query data transfer object used to search orders.</summary>
  public class OrdersQuery {

    public string OrderTypeUID {
      get; set;
    } = string.Empty;


    public string OrderNo {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public string ResponsibleUID {
      get; set;
    } = string.Empty;


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public Priority Priority {
      get; set;
    } = Priority.All;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class OrdersQuery

} // namespace namespace Empiria.Orders.Adapters
