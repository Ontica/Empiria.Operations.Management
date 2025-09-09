/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : InventoryType                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes inventory type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Orders;

namespace Empiria.Orders {

  /// <summary>Describes inventory type.</summary>
  public class SalesOrderType : OrderCategory {

    #region Constructors and parsers

    static public new SalesOrderType Parse(int id) => ParseId<SalesOrderType>(id);

    static public new SalesOrderType Parse(string uid) => ParseKey<SalesOrderType>(uid);

    static public new SalesOrderType Empty => ParseEmpty<SalesOrderType>();

    static public new FixedList<SalesOrderType> GetList() {
      return GetStorageObjects<SalesOrderType>();
    }

    #endregion Constructors and parsers

    #region Properties

    public Boolean EntriesRequired {
      get {
        return base.ExtData.Get("entriesRequired", false);
      }
      private set {
        base.ExtData.SetIfValue("entriesRequired", value);
      }
    }

    public Boolean ItemsRequired {
      get {
        return base.ExtData.Get("itemsRequired", false);
      }
      private set {
        base.ExtData.SetIfValue("itemsRequired", value);
      }
    }

    #endregion Properties

  } // class SalesOrderType

} // namespace Empiria.Orders
