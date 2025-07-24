/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : SearchInventoryQuery                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve inventory orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;
using Empiria.Locations;
using Empiria.Products;

namespace Empiria.Inventory.Reporting.Adapters {

  /// <summary>Input query DTO used to retrieve inventory orders.</summary>
  public class SearchInventoryQuery {

    public string Keywords {
      get; set;
    } = string.Empty;


    public List<string> Products {
      get; set;
    } = new List<string>();


    public List<string> Locations {
      get; set;
    } = new List<string>();
    

    public string Product {
      get; set;
    } = string.Empty;


    public string Location {
      get; set;
    } = string.Empty;


    public string WarehouseUID {
      get; set;
    } = string.Empty;
     
    
    public string RackUID {
      get; set;
    } = string.Empty;


    public string LevelUID {
      get; set;
    } = string.Empty;


    public string Position {
      get; set;
    } = string.Empty;

  } // class SearchInventoryQuery


  /// <summary>Extension methods for query DTO used to retrieve inventory orders.</summary>

  static internal class SearchInventoryQueryExtensions {

    static internal string MapToFilterString(this SearchInventoryQuery query) {

      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string locationFilter = BuildLocationFilter(query.Location);
      string productFilter = BuildProudctFilter(query.Product);
     
      string warehouseFilter = BuildWarehouseFilter(query.WarehouseUID);
      string rackFilter = BuildRackFilter(query.RackUID);
      string levelFilter = BuildLevelFilter(query.LevelUID);
      string positionFilter = BuildPositionFilter(query.Position);

      var filter = new Filter(keywordsFilter);
      filter.AppendAnd(locationFilter);
      filter.AppendOr(productFilter);

      filter.AppendAnd(warehouseFilter);
      filter.AppendAnd(positionFilter);
      filter.AppendOr(rackFilter);
      filter.AppendAnd(levelFilter);

      filter.AppendAnd("Inv_Entry_Status != 'X' ");

      return filter.ToString();
    }


    static internal string MapToSortString(this SearchInventoryQuery query) {

      return string.Empty;
    }

    #region Helpers 

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
        case 2 : return $"Rack = {loc.Id}"; //return BuildRackFilter(loc.UID);
        case 3 : return $"Leve = {loc.Id}"; // return BuildLevelFilter(loc.UID); 
        case 4 : return $"Position = {loc.Id}"; // BuildPositionFilter(loc.UID); 
        default : return string.Empty; 
      }
    }


    private static string BuildPositionFilter(string locatition) {
      if (locatition.Length == 0) {
        return string.Empty;
      }

     var position = Location.Parse(locatition);

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

      if (warehouse.Name == "VIRTUAL") {
        return "Warehouse = -1";
      }

      return $"Warehouse = {warehouse.Id}";
    }

    #endregion Helpers

  } // class SearchInventoryQueryExtensions {

} // namespace Empiria.Inventory.Reporting.Adapters
