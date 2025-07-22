/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapter Layer                           *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Output DTO                              *
*  Type     : AssetTransactionEntryDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output data transfer object with an asset transaction entry.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Output data transfer object with an asset transaction entry.</summary>
  public class AssetTransactionEntryDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto EntryType {
      get; internal set;
    }

    public NamedEntityDto Transaction {
      get; internal set;
    }

    public AssetDto Asset {
      get; internal set;
    }

    public string PreviousCondition {
      get; internal set;
    }

    public string ReleasedCondition {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

  }  // class AssetTransactionEntryDto

}  // namespace Empiria.Inventory.Assets.Adapters
