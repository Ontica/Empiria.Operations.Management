﻿/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Orders Management Integration                 Component : Use cases Layer                      *
*  Assembly : Empiria.Operations.Integration.Core.dll       Pattern   : Use case interactor class            *
*  Type     : OrderTypeUseCases                             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to return order types information.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Orders;

namespace Empiria.Operations.Integration.Orders.UseCases {

  /// <summary>Use cases used return order types information.</summary>
  public class OrderTypeUseCases : UseCase {

    #region Constructors and parsers

    protected OrderTypeUseCases() {
      // no-op
    }

    static public OrderTypeUseCases UseCaseInteractor() {
      return CreateInstance<OrderTypeUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetOrderTypeCategories(string orderTypeUID) {
      Assertion.Require(orderTypeUID, nameof(orderTypeUID));

      var orderType = OrderType.Parse(orderTypeUID);

      FixedList<OrderCategory> categories = OrderCategory.GetListFor(orderType);

      return categories.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class OrderTypeUseCases

}  // namespace Empiria.Operations.Integration.Orders.UseCases
