/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Query DTO                               *
*  Type     : AssetsQuery                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search assets.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Inventory.Assets.Adapters {

  /// <summary>Query data transfer object used to search assets.</summary>
  public class AssetsQuery {

    public string AssetNo {
      get; set;
    } = string.Empty;


    public string AssetTypeUID {
      get; set;
    } = string.Empty;


    public string AssignedToUID {
      get; set;
    } = string.Empty;


    public string AssignedToOrgUnitUID {
      get; set;
    } = string.Empty;


    public string ManagerUID {
      get; set;
    } = string.Empty;


    public string ManagerOrgUnitUID {
      get; set;
    } = string.Empty;


    public string BuildingUID {
      get; set;
    } = string.Empty;


    public string FloorUID {
      get; set;
    } = string.Empty;


    public string PlaceUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class AssetsQuery

} // namespace Empiria.Inventory.Assets.Adapters
