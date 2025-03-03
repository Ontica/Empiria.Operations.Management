/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Use cases Layer                         *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : AssetLocationUseCases                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for assets location management.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Locations;

namespace Empiria.Inventory.Assets.UseCases {

  /// <summary>Use cases for assets location management.</summary>
  public class AssetLocationUseCases : UseCase {

    #region Constructors and parsers

    protected AssetLocationUseCases() {
      // no-op
    }

    static public AssetLocationUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AssetLocationUseCases>();
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

  }  // class AssetLocationUseCases

}  // namespace Empiria.Inventory.Assets.UseCases
