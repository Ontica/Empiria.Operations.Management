/* Empiria Integrated Operations Management ******************************************************************
*                                                                                                            *
*  Module   : Operations-Budgeting Integration           Component : Adapters Layer                          *
*  Assembly : Empiria.Operations.Integration.Core.dll    Pattern   : Input Query                             *
*  Type     : RelatedDocumentsQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query used to retrieve budgeting transaction's related documents.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Input query used to retrieve budgeting transaction's related documents.</summary>
  public class RelatedDocumentsQuery {

    public string RelatedDocumentTypeUID {
      get; set;
    } = string.Empty;


    public string OrganizationalUnitUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class RelatedDocumentsQuery

}  // namespace Empiria.Budgeting.Transactions.Adapters
