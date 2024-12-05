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

    public string ContractTypeUID {
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


    public string ManagedByOrgUnitUID {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime SignDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string SupplierUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;

    public decimal Total {
      get; set;
    } = 0.00m;


    internal void EnsureValid() {
      Assertion.Require(ContractTypeUID, "Necesito el tipo de contrato.");
      Assertion.Require(Name, "Necesito el nombre del contrato.");
      Assertion.Require(CurrencyUID, "Necesito la moneda del contrato.");
      Assertion.Require(Total > 0, "Necesito el importe del contrato.");
      Assertion.Require(BudgetTypeUID, "Necesito tipo de presupuesto del contrato.");
    }

  }  // class ContractFields

}  // namespace Empiria.Procurement.Contracts
