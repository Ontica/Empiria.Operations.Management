/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Partitioned Type / Information Holder   *
*  Type     : ProductEntry                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a product entry.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Inventory {

  /// <summary>Represents a product entry.</summary>
  public class ProductEntry {

    [DataField("Product_Id")]
    internal int ProductId {
      get; set;
    }


    [DataField("Product_UID")]
    internal string ProductUID {
      get; set;
    }


    [DataField("Product_Type_Id")]
    internal int ProductTypeId {
      get; set;
    }


    [DataField("Product_Name")]
    internal string Name {
      get; set;
    }


    [DataField("Product_Description")]
    internal string ProductDescription {
      get; set;
    }

  } // class ProductEntry

} // namespace Empiria.Inventory
