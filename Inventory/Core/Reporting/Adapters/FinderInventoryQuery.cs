/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryQuery                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve inventory orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;
using Empiria.Locations;
using Empiria.Products;

namespace Empiria.Inventory.Reporting.Adapters {

  /// <summary>Input query DTO used to retrieve inventory orders.</summary>
  public class FinderInventoryQuery {

    public string Keywords {
      get; set;
    } = string.Empty;


    public string[] Products {
      get;
      internal set;
    } = new string[0];


    public List<string> Locations {
      get;
      set;
    } = new List<string>();
    

    public string Product {
      get;
      internal set;
    } = string.Empty;


    public string Location {
      get;
      internal set;
    } = string.Empty;


    public string WarehouseUID {
      get; set;
    } = string.Empty;
     
    
    public string RackUID {
      get;
      internal set;
    } = string.Empty;


    public string LevelUID {
      get;
      internal set;
    } = string.Empty;


    public string Position {
      get;
      internal set;
    } = string.Empty;

  }


  static internal class FinderInventoryQueryExtensions {

    static internal string MapToFilterString(this FinderInventoryQuery query) {

      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string locationFilter = BuildLocationFilter(query.Location);
      string warehouseFilter = BuildWarehouseFilter(query.WarehouseUID);
      string rackFilter = BuildRackFilter(query.RackUID); 
      string levelFilter = BuildLevelFilter(query.LevelUID);
      string positionFilter = BuildPositionFilter(query.Position);
      string productFilter = BuildProudctFilter(query.Product);

      var filter = new Filter(keywordsFilter);
      filter.AppendAnd(locationFilter);
      filter.AppendAnd(positionFilter);
      filter.AppendAnd(warehouseFilter);
      filter.AppendOr(rackFilter);
      filter.AppendAnd(levelFilter);
      filter.AppendAnd(productFilter);
      filter.AppendAnd("Inv_Entry_Status != 'X' ");

      return filter.ToString();
    }


    static internal string MapToSortString(this FinderInventoryQuery query) {

      return string.Empty;
    }

    #region Private methods  

    private static string BuildKeywordsFilter(string keywords) {

      if (keywords != string.Empty) {
        return $"{SearchExpression.ParseAndLikeKeywords("Inv_Entry_Keywords", keywords)} ";
      }

      return string.Empty;
    }


    private static string BuildProudctFilter(string productCode) {
      if (productCode.Length == 0) {
        return string.Empty;
      }

      var product = Product.TryParseWithCode(productCode);

      return $"Inv_Entry_Product_Id = {product.Id}";
    }


    private static string BuildLocationFilter(string location) {
      if (location.Length == 0) {
        return string.Empty;
      }

      var loc = CommonStorage.TryParseNamedKey<Location>(location);
      int levelFilter = loc.Level;

      switch (levelFilter) {
        case 1 : return BuildWarehouseFilter(loc.UID); 
        case 2 : return BuildRackFilter(loc.UID);
        case 3 : return BuildLevelFilter(loc.UID); 
        case 4 : return BuildPositionFilter(loc.UID); 
        default : return string.Empty; 
      }
    }


    private static string BuildPositionFilter(string locatition) {
      if (locatition.Length == 0) {
        return string.Empty;
      }

      var position = CommonStorage.TryParseNamedKey<Location>(locatition);

      return $"Position = {position.Id}";
    }


    private static string BuildLevelFilter(string levelUID) {
      if (levelUID.Length == 0) {
        return string.Empty;
      }

      var level = Location.Parse(levelUID);

      return $"Leve = {level.Id}";
    }


    private static string BuildRackFilter(string rackUID) {
      if (rackUID.Length == 0) {
        return string.Empty;
      }

      var rack = Location.Parse(rackUID);

      return $"Rack = {rack.Id}";
    }


    private static string BuildWarehouseFilter(string warehouseUID) {
      if (warehouseUID.Length == 0) {
        return string.Empty;
      }

      var warehouse = Location.Parse(warehouseUID);

      return $"Warehouse = {warehouse.Id}";
    }

    #endregion Private methods

  } // class FinderInventoryQuery

} // namespace Empiria.Inventory.Reporting.Adapters
