/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Material Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : InventoryQuery                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve inventory orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Orders.Adapters;
using Empiria.StateEnums;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Empiria.Inventory.Adapters {

  /// <summary>Input query DTO used to retrieve inventory orders.</summary>
  public class InventoryOrderQuery {


    //public string InventoryOrderTypeUID {
    //  get; set;
    //} = string.Empty;


    //public string AssignedToUID {
    //  get; set;
    //} = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


  }


  static internal class InventoryOrderQueryExtensions {

    static internal string MapToFilterString(this InventoryOrderQuery query) {

      string orderType = "Order_Type_Id = 4008";
      string keywords = BuildKeywordsFilter(query.Keywords);
      string status = BuildStatusFilter(query.Status);

      var filter = new Filter(orderType);
      filter.AppendAnd(keywords);
      filter.AppendAnd(status);

      return filter.ToString();
    }

    
    static internal string MapToSortString(this InventoryOrderQuery query) {

      return string.Empty;
    }

    #region Private methods


    private static string BuildKeywordsFilter(string keywords) {

      if (keywords != string.Empty) {
        return $"{SearchExpression.ParseAndLikeKeywords("Order_Keywords", keywords)} ";
      }

      return string.Empty;
    }


    private static string BuildStatusFilter(EntityStatus status) {

      if (status == EntityStatus.All) {
        return $"Order_Status != '{(char) EntityStatus.Discontinued}' AND " +
               $"Order_Status != '{(char) EntityStatus.Suspended}' AND " +
               $"Order_Status != '{(char) EntityStatus.Deleted}' ";
      }

      return $"Order_Status = '{(char) status}'";
    }


    #endregion Private methods
  }

} // namespace Empiria.Inventory.Adapters
