/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Input fields DTO                        *
*  Type     : AssetTransactionEntryFields                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update and create assets transactions entries.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets {

  /// <summary>Input fields DTO used to update and create assets transactions entries.</summary>
  public class AssetTransactionEntryFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string TransactionUID {
      get; set;
    } = string.Empty;


    public string AssetUID {
      get; set;
    } = string.Empty;


    public string EntryTypeUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Assertion.Require(AssetUID, nameof(AssetUID));

      Description = EmpiriaString.Clean(Description);

      if (EntryTypeUID.Length == 0) {
        EntryTypeUID = AssetTransaction.Parse(TransactionUID)
                                       .AssetTransactionType
                                       .DefaultAssetTransactionEntryType
                                       .UID;
      }
    }

  }  // class AssetTransactionEntryFields

}  // namespace Empiria.Inventory.Assets
