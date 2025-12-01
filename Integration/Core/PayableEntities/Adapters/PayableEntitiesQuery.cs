/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Input Query DTO                         *
*  Type     : PayableEntitiesQuery                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrive payable entities.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Input query DTO used to retrive payable entities.</summary>
  public class PayableEntitiesQuery {

    public string OrganizationalUnitUID {
      get; set;
    } = string.Empty;


    public string PayableEntityTypeUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class PayableEntitiesQuery

}  // namespace Empiria.Payments.Payables.Adapters
