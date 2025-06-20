/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : InventoryType                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes inventory type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory {

  /// <summary>Describes inventory type.</summary>
  public class InventoryType : CommonStorage {

    #region Constructors and parsers

    static public InventoryType Parse(int id) => ParseId<InventoryType>(id);

    static public InventoryType Parse(string uid) => ParseKey<InventoryType>(uid);

    static public InventoryType Empty => ParseEmpty<InventoryType>();

    static public FixedList<InventoryType> GetList() {
      return GetStorageObjects<InventoryType>();
    }

    #endregion Constructors and parsers

  } // class InventoryType

} // namespace Empiria.Inventory
