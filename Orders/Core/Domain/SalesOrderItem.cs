/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Orders Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Orders.Core.dll                    Pattern   : Information Holder                      *
*  Type     : SalesOrderItem                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an abstract sales order item.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Orders.Data;
namespace Empiria.Orders {


  /// <summary>Represents an abstract sales order item.</summary>
  internal class SalesOrderItem : OrderItem {

      #region Constructors and parsers
    
      protected SalesOrderItem(OrderItemType powertype) : base(powertype) {
        // Required by Empiria Framework for all partitioned types.
      }

      internal SalesOrderItem() {
        //no-op
      }

      static public new SalesOrderItem Parse(int id) => ParseId<SalesOrderItem>(id);

      static public new SalesOrderItem Parse(string uid) => ParseKey<SalesOrderItem>(uid);

      static public new SalesOrderItem Empty => ParseEmpty<SalesOrderItem>();

      #endregion Constructors and parsers

      #region Properties
    
      internal FixedList<Order> Orders {
        get; set;
      }
    
      #endregion Properties


    }// class SalesOrderItem
  }// namespace Empiria.Orders.Domain 
