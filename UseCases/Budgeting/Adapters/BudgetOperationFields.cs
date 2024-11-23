/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Budgeting Integration                         Component : Adapters Layer                       *
*  Assembly : Empiria.OperationsManagement.UseCases.dll     Pattern   : Input Fields DTO                     *
*  Type     : BudgetOperationFields                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input fields DTO used to invoke a budgeting operation.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Integration.Adapters {

  /// <summary>Input fields DTO used to invoke a budgeting operation.</summary>
  public class BudgetOperationFields {

    public string BaseObjectTypeUID {
      get; set;
    } = string.Empty;


    public string BaseObjectUID {
      get; set;
    } = string.Empty;


  } // class BudgetOperationFields

}  // namespace Empiria.Budgeting.Integration.Adapters
