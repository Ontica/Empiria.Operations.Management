﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryQuery                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve inventory orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Locations;
using Empiria.StateEnums;

namespace Empiria.Inventory.Adapters {

  /// <summary>Input query DTO used to retrieve inventory orders.</summary>
  public class InventoryOrderQuery {

    public string WarehouseUID {
      get; set;
    } = string.Empty;


    public string InvetoryTypeUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


  }


  static internal class InventoryOrderQueryExtensions {

    static internal string MapToFilterString(this InventoryOrderQuery query) {

      string keywords = BuildKeywordsFilter(query.Keywords);
      string warehouse = BuildWarehouseFilter(query.WarehouseUID);
      string inventoryType = BuildInventoryTypeFilter(query.InvetoryTypeUID);
      string status = BuildStatusFilter(query.Status);

      var filter = new Filter(status);

      filter.AppendAnd(keywords);
      filter.AppendAnd("Order_Type_Id = 4010");
      filter.AppendAnd(warehouse);
      filter.AppendAnd(inventoryType);

      return filter.ToString();
    }


    static internal string MapToSortString(this InventoryOrderQuery query) {

      return string.Empty;
    }

    #region Private methods

    private static string BuildInventoryTypeFilter(string invetoryTypeUID) {
      if (invetoryTypeUID.Length == 0) {
        return string.Empty;
      }

      var invetoryType = InventoryType.Parse(invetoryTypeUID);

      return $" Order_Category_Id = {invetoryType.Id}";
    }


    private static string BuildKeywordsFilter(string keywords) {

      if (keywords != string.Empty) {
        return $"{SearchExpression.ParseAndLikeKeywords("Order_Keywords", keywords)} ";
      }

      return string.Empty;
    }


    private static string BuildStatusFilter(EntityStatus status) {

      if (status == EntityStatus.All) {
        return $"Order_Status <> 'X'";
      }

      return $"(Order_Status = '{(char) status}' AND Order_Id <> -1)";
    }


    private static string BuildWarehouseFilter(string warehouseUID) {
      if (warehouseUID.Length == 0) {
        return string.Empty;
      }

      var warehouse = Location.Parse(warehouseUID);

      return $"Order_Location_Id = {warehouse.Id}";
    }

    #endregion Private methods
  }

} // namespace Empiria.Inventory.Adapters
