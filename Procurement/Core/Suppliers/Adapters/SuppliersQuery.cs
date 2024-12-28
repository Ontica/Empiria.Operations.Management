/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Query DTO                               *
*  Type     : SuppliersQuery                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search suppliers.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Procurement.Suppliers.Adapters {

  /// <summary>Query DTO used to search suppliers.</summary>
  public class SuppliersQuery {

    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class SuppliersQuery

}  // namespace Empiria.Procurement.Suppliers.Adapters
