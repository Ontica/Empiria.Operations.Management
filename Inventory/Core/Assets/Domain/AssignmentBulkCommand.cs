/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Input command DTO                       *
*  Type     : AssignmentBulkCommand                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command data structure used to create assets transactions from assets assignments.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

namespace Empiria.Inventory.Assets {

  /// <summary>Command data structure used to create assets transactions from assets assignments.</summary>
  public class AssignmentBulkCommand {

    public AssetTransactionType TransactionType {
      get; internal set;
    } = AssetTransactionType.Empty;


    public string[] Assignments {
      get; set;
    } = new string[0];


    public string[] Assets {
      get; set;
    } = new string[0];


    public DateTime ApplicationDate {
      get; set;
    } = DateTime.Today;


    public string AssignedToUID {
      get; set;
    } = string.Empty;


    public string AssignedToOrgUnitUID {
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


    internal FixedList<AssetTransactionEntryFields> GetTransactionEntryFields(AssetAssignment assignment) {
      int size = Math.Max(Assets.Length, assignment.GetAssets().Count);

      var list = new List<AssetTransactionEntryFields>(size);

      if (Assets.Length > 0) {
        foreach (var assetEntryUID in Assets) {
          var txnEntry = AssetTransactionEntry.Parse(assetEntryUID);

          AssetTransactionEntryFields entryFields = GetTransactionEntryFields(txnEntry);

          list.Add(entryFields);
        }
        return list.ToFixedList();
      }

      foreach (var txnEntry in assignment.Transaction.Entries) {
        AssetTransactionEntryFields entryFields = GetTransactionEntryFields(txnEntry);

        list.Add(entryFields);
      }

      return list.ToFixedList();
    }


    internal AssetTransactionFields GetTransactionFields(AssetAssignment assignment) {
      return new AssetTransactionFields {
        ApplicationDate = ApplicationDate,
        AssignedToUID = AssignedToUID,
        AssignedToOrgUnitUID = AssignedToOrgUnitUID,
        ReleasedByUID = assignment.AssignedTo.UID,
        ReleasedByOrgUnitUID = assignment.AssignedToOrgUnit.UID,
        Description = Description,
        Identificators = Identificators,
        Tags = Tags,
        LocationUID = LocationUID,
        TransactionTypeUID = TransactionType.UID
      };
    }


    private AssetTransactionEntryFields GetTransactionEntryFields(AssetTransactionEntry entry) {
      return new AssetTransactionEntryFields {
         AssetUID = entry.Asset.UID,
         EntryTypeUID = TransactionType.DefaultAssetTransactionEntryType.UID,
         PreviousCondition = entry.Asset.CurrentCondition,
         ReleasedCondition = entry.Asset.CurrentCondition
      };
    }

  }  // class AssignmentBulkCommand

}  // namespace Empiria.Inventory.Assets
