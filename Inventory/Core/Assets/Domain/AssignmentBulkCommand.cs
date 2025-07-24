/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Input command DTO                       *
*  Type     : AssignmentBulkCommand                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command data structure used to create assets transactions from assets assignments.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Inventory.Assets {

  /// <summary>Command data structure used to create assets transactions from assets assignments.</summary>
  public class AssignmentBulkCommand {

    public string OperationID {
      get; set;
    }

  }  // class AssignmentBulkCommand

}  // namespace Empiria.Inventory.Assets
