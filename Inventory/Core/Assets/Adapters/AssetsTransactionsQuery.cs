/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Query DTO                               *
*  Type     : AssetsTransactionsQuery                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search assets transactions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Query data transfer object used to search assets transactions.</summary>
  public class AssetsTransactionsQuery {

    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public string ManagerUID {
      get; set;
    } = string.Empty;


    public string ManagerOrgUnitUID {
      get; set;
    } = string.Empty;


    public string AssignedToUID {
      get; set;
    } = string.Empty;


    public string AssignedToOrgUnitUID {
      get; set;
    } = string.Empty;


    public string BuildingUID {
      get; set;
    } = string.Empty;


    public string FloorUID {
      get; set;
    } = string.Empty;


    public string PlaceUID {
      get; set;
    } = string.Empty;


    public string OperationSourceUID {
      get; set;
    } = string.Empty;


    public string[] TransactionsNo {
      get; set;
    } = new string[0];


    public string Keywords {
      get; set;
    } = string.Empty;


    public string EntriesKeywords {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public TransactionDateType DateType {
      get; set;
    } = TransactionDateType.None;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public TransactionPartyRole PartyType {
      get; set;
    } = TransactionPartyRole.None;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public TransactionStatus Status {
      get; set;
    } = TransactionStatus.All;


    public TransactionStage Stage {
      get; set;
    } = TransactionStage.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class AssetsTransactionsQuery

}  // namespace Empiria.Inventory.Assets.Adapters
