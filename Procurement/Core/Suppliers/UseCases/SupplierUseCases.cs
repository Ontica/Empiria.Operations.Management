/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : SupplierUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for suppliers management.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Procurement.Contracts;
using Empiria.Procurement.Contracts.Data;
using Empiria.Procurement.Contracts.Adapters;

namespace Empiria.Procurement.Suppliers.UseCases {

  /// <summary>Use cases for suppliers management.</summary>
  public class SupplierUseCases : UseCase {

    #region Constructors and parsers

    protected SupplierUseCases() {
      // no-op
    }

    static public SupplierUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<SupplierUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

     internal FixedList<ContractDto> GetSupplierContractsToOrder(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var query = new ContractQuery {
         SupplierUID = supplierUID
      };

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();


      FixedList<Contract> contracts = ContractData.GetContracts(filter, sortBy);

      return ContractMapper.MapContracts(contracts);
    }

    #endregion Use cases

  }  // class SupplierUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
