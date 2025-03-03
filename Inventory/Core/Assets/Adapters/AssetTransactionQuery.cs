/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Query DTO                               *
*  Type     : AssetTransactionQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search assets transactions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Query data transfer object used to search assets transactions.</summary>
  public class AssetTransactionQuery {

    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public string BasePartyUID {
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
    } = ExecutionServer.DateMinValue;


    public TransactionPartyType PartyType {
      get; set;
    } = TransactionPartyType.None;


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

  }  // class AssetTransactionQuery

}  // namespace Empiria.Inventory.Assets.Adapters
