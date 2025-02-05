/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractFields                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contracts information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Procurement.Contracts {

  /// <summary>Input fields DTO used for update contracts information.</summary>
  public class ContractFields {

    [Newtonsoft.Json.JsonProperty(PropertyName = "ContractTypeUID")]
    public string ContractCategoryUID {
      get; set;
    } = string.Empty;


    public string ContractNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string[] BudgetsUIDs {
      get; set;
    } = new string[0];


    public string ManagedByOrgUnitUID {
      get; set;
    } = string.Empty;


    public bool IsForMultipleOrgUnits {
      get; set;
    } = false;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime SignDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string CustomerUID {
      get; set;
    } = string.Empty;


    public string SupplierUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Assertion.Require(ContractCategoryUID, "Necesito la clasificación o categoría del contrato.");
      Assertion.Require(Name, "Necesito el nombre del contrato.");
      Assertion.Require(CurrencyUID, "Necesito la moneda del contrato.");
      Assertion.Require(BudgetTypeUID, "Necesito se seleccione el tipo de presupuesto.");
      Assertion.Require(BudgetsUIDs.Length > 0, "Necesito se seleccione el presupuesto " +
                                                "que se utilizará para el contrato.");
    }

  }  // class ContractFields

}  // namespace Empiria.Procurement.Contracts
