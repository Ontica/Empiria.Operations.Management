/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Procurement.Core.dll               Pattern   : Use case interactor class               *
*  Type     : SupplierUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for suppliers management.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Parties;
using Empiria.Services;

using Empiria.Financial.Adapters;
using Empiria.Financial.Services;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.Procurement.Suppliers.Adapters;
using Empiria.Procurement.Suppliers.Data;

namespace Empiria.Procurement.Suppliers.UseCases {

  /// <summary>Use cases for suppliers management.</summary>
  public class SupplierUseCases : UseCase {

    private readonly AccountsServices _financialAccountingServices;

    #region Constructors and parsers

    protected SupplierUseCases() {
      _financialAccountingServices = new AccountsServices();
    }

    static public SupplierUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<SupplierUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public SupplierHolderDto GetSupplier(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Party.Parse(supplierUID);

      return SupplierMapper.Map(supplier);
    }


    public FixedList<PaymentAccountDto> GetSupplierPaymentAccounts(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Party.Parse(supplierUID);

      return PaymentAccountServices.GetPaymentAccounts(supplier.UID);
    }


    public async Task<NamedEntityDto> MatchSupplierSubledgerAccount(SubledgerAccountQuery query) {
      Assertion.Require(query, nameof(query));

      var fields = new NamedEntityFields {
        UID = query.UID,
        Name = query.Name,
      };


      FixedList<NamedEntityDto> subledgerAccounts = await
                                  _financialAccountingServices.SearchSuppliersSubledgerAccounts(fields);

      if (subledgerAccounts.Count == 1) {
        return subledgerAccounts[0];
      }

      if (subledgerAccounts.Count == 0) {
        Assertion.RequireFail("No encontré ningún auxiliar de proveedores en SICOFIN " +
                              "que coincida con datos similares a los de este proveedor. " +
                              "Es necesario registrar o revisar el auxiliar asociado a este proveedor en SICOFIN, " +
                              "y posteriormente volver a intentar ejecutar esta operación.");
      }

      if (subledgerAccounts.Count > 1) {
        Assertion.RequireFail($"Encontré {subledgerAccounts.Count} auxiliares de proveedores " +
                              $"en el sistema de contabilidad que se asemejan a los datos de " +
                              $"este proveedor. Es necesario limpiar los auxiliares de SICOFIN para " +
                              $"que no existan proveedores activos con datos similares.");
      }

      throw Assertion.EnsureNoReachThisCode();
    }


    public FixedList<SupplierDescriptor> SearchSuppliers(SuppliersQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      FixedList<Party> suppliers = SuppliersData.GetSuppliers(filter, sortBy);

      return SupplierMapper.Map(suppliers);
    }

    #endregion Use cases

  }  // class SupplierUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
