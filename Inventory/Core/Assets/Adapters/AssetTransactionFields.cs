/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Input fields DTO                        *
*  Type     : AssetTransactionFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to update and create asset transactions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Input fields used to update and create asset transactions.</summary>
  public class AssetTransactionFields {

    public string TransactionTypeUID {
      get; set;
    }

  }  // class AssetTransactionFields

}  // namespace Empiria.Inventory.Assets.Adapters
