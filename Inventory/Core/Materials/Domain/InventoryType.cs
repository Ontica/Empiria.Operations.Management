/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : InventoryType                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes inventory type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Inventory {

  /// <summary>Describes inventory type.</summary>
  public class InventoryType : OrderCategory {

    #region Constructors and parsers

    static public new InventoryType Parse(int id) => ParseId<InventoryType>(id);

    static public new InventoryType Parse(string uid) => ParseKey<InventoryType>(uid);

    static public new InventoryType Empty => ParseEmpty<InventoryType>();

    static public new FixedList<InventoryType> GetList() {
      return GetStorageObjects<InventoryType>();
    }

    #endregion Constructors and parsers

  } // class InventoryType

} // namespace Empiria.Inventory
