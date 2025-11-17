/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Fields DTO                              *
*  Type     : ContractItemFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update contracts information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Orders;

namespace Empiria.Procurement.Contracts {

  /// <summary>DTO fields structure used for update contracts item information.</summary>
  public class ContractItemFields : OrderItemFields {

    public string ContractUID {
      get; set;
    } = string.Empty;


    public decimal MinQuantity {
      get; set;
    }

    public decimal MaxQuantity {
      get; set;
    }


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public override void EnsureValid() {
      Assertion.Require(ContractUID, "Necesito se proporcione el contrato.");
      Assertion.Require(MinQuantity > 0, "Necesito se proporcione la cantidad mínima.");
      Assertion.Require(MaxQuantity > 0, "Necesito se proporcione la cantidad máxima.");
      Assertion.Require(MinQuantity <= MaxQuantity,
                       "La cantidad máxima no puede ser menor a la cantidad mínima.");

      base.Quantity = MaxQuantity;

      var contract = Contract.Parse(ContractUID);

      if (RequestedByUID.Length == 0) {
        RequestedByUID = contract.RequestedBy.UID;
      }

      if (ProviderUID.Length == 0) {
        ProviderUID = contract.Provider.UID;
      }

      base.EnsureValid();
    }

  }  // class ContractItemFields

}  // namespace Empiria.Procurement.Contracts
