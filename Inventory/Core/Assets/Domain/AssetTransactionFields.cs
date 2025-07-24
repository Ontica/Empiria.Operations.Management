/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Input fields DTO                        *
*  Type     : AssetTransactionFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to update and create asset transactions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory.Assets {

  /// <summary>Input fields used to update and create asset transactions.</summary>
  public class AssetTransactionFields {

    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public DateTime ApplicationDate {
      get; set;
    } = DateTime.Today;


    public string AssignedToUID {
      get; set;
    } = string.Empty;


    public string AssignedToOrgUnitUID {
      get; set;
    } = string.Empty;


    public string ReleasedByUID {
      get; set;
    } = string.Empty;


    public string ReleasedByOrgUnitUID {
      get; set;
    } = string.Empty;


    public string LocationUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string[] Identificators {
      get; set;
    } = new string[0];


    public string[] Tags {
      get; set;
    } = new string[0];


    internal void EnsureValid() {
      Assertion.Require(TransactionTypeUID, nameof(TransactionTypeUID));
      Assertion.Require(Description, nameof(Description));
      Assertion.Require(AssignedToUID, nameof(AssignedToUID));
      Assertion.Require(AssignedToOrgUnitUID, nameof(AssignedToOrgUnitUID));
      Assertion.Require(ReleasedByUID, nameof(ReleasedByUID));
      Assertion.Require(ReleasedByOrgUnitUID, nameof(ReleasedByOrgUnitUID));
      Assertion.Require(LocationUID, nameof(LocationUID));
    }

  }  // class AssetTransactionFields

}  // namespace Empiria.Inventory.Assets
