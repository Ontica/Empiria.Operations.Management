/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractOrderFields                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contract supply orders.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Procurement.Contracts.Adapters {

  /// <summary>Input fields DTO used for update contract supply orders.</summary>
  public class ContractOrderFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string ContractUID {
      get; set;
    }

    public string Name {
      get; set;
    }

    public string Description {
      get; set;
    }

    public string SupplierUID {
      get; set;
    }

    public string PaymentExtData {
      get; set;
    }


    public string ManagedByOrgUnitUID {
      get; set;
    }

    public string Status {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Assertion.Require(Name, "Se requiere el nombre del entregable.");
      Assertion.Require(Description, "Se requiere la descripción del entregable.");
      Assertion.Require(SupplierUID, "Se requiere el proveedor del entregable.");

    }

  }  // class ContractOrderFields

}  // namespace Empiria.Procurement.Contracts.Adapters
