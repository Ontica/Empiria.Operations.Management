/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : FixedAssetLocationUseCases                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for fixed assets location management.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Locations;

namespace Empiria.Inventory.FixedAssets.UseCases {

  /// <summary>Use cases for fixed assets location management.</summary>
  public class FixedAssetLocationUseCases : UseCase {

    #region Constructors and parsers

    protected FixedAssetLocationUseCases() {
      // no-op
    }

    static public FixedAssetLocationUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FixedAssetLocationUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetRootLocations() {
      return LocationType.Building.GetLocations()
                                  .MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetLocationChildren(string locationUID) {
      Assertion.Require(locationUID, nameof(locationUID));

      var location = Location.Parse(locationUID);

      return location.GetChildren()
                     .MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class FixedAssetLocationUseCases

}  // namespace Empiria.Inventory.FixedAssets.UseCases
