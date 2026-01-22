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

using Empiria.Services;

using Empiria.Procurement.Suppliers.Adapters;
using Empiria.Procurement.Suppliers.Data;

namespace Empiria.Procurement.Suppliers.UseCases {

  /// <summary>Use cases for suppliers management.</summary>
  public class SupplierUseCases : UseCase {

    #region Constructors and parsers

    protected SupplierUseCases() {

    }

    static public SupplierUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<SupplierUseCases>();
    }

    #endregion Constructors and parsers

    #region Query use cases

    public SupplierHolderDto GetSupplier(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Supplier.Parse(supplierUID);

      return SupplierMapper.Map(supplier);
    }


    public async Task MatchSupplierSubledgerAccount(SubledgerAccountQuery query) {
      Assertion.Require(query, nameof(query));

      await Task.CompletedTask;

      //var fields = new NamedEntityFields {
      //  UID = query.UID,
      //  Name = query.Name,
      //};

      //var financialAccountingServices = new AccountsServices();

      //FixedList<NamedEntityDto> subledgerAccounts = await
      //                              financialAccountingServices.SearchSuppliersSubledgerAccounts(fields);

      //subledgerAccounts = subledgerAccounts.FindAll(x => x.UID == query.UID);

      //if (subledgerAccounts.Count != 1) {
      //  Assertion.RequireFail("No encontré ningún auxiliar de proveedores en SICOFIN " +
      //                        "que coincida con datos similares a los de este proveedor. " +
      //                        "Es necesario registrar o revisar el auxiliar asociado a este proveedor en SICOFIN, " +
      //                        "y posteriormente volver a intentar ejecutar esta operación.");
      //}
    }


    public FixedList<SupplierDescriptor> SearchSuppliers(SuppliersQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      FixedList<Supplier> suppliers = SuppliersData.GetSuppliers(filter, sortBy);

      return SupplierMapper.Map(suppliers);
    }

    #endregion Query use cases

    #region Command use cases

    public async Task<SupplierHolderDto> CreateSupplier(SupplierFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      await MatchSupplierSubledgerAccount(new SubledgerAccountQuery {
        UID = fields.SubledgerAccount,
        Name = fields.Name,
        TaxCode = fields.TaxCode
      });

      FixedList<Supplier> sameTaxCode = Supplier.TryGetWithTaxCode(fields.TaxCode);

      if (sameTaxCode.Exists(x => x.SubledgerAccount == fields.SubledgerAccount)) {
        Assertion.RequireFail("Ya existe otro beneficiario con el mismo RFC y auxiliar contable.");
      }

      var supplier = new Supplier(fields);

      supplier.Save();

      return SupplierMapper.Map(supplier);
    }


    internal SupplierHolderDto DeleteSupplier(string supplierUID) {
      Assertion.Require(supplierUID, nameof(supplierUID));

      var supplier = Supplier.Parse(supplierUID);

      supplier.Delete();

      supplier.Save();

      return SupplierMapper.Map(supplier);
    }


    internal async Task<SupplierHolderDto> UpdateSupplier(SupplierFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      await MatchSupplierSubledgerAccount(new SubledgerAccountQuery {
        UID = fields.SubledgerAccount,
        Name = fields.Name,
        TaxCode = fields.TaxCode
      });

      var supplier = Supplier.Parse(fields.UID);

      FixedList<Supplier> sameTaxCode = Supplier.TryGetWithTaxCode(fields.TaxCode);

      if (sameTaxCode.Exists(x => x.UID != supplier.UID &&
                                  x.SubledgerAccount == fields.SubledgerAccount)) {
        Assertion.RequireFail("Ya existe otro beneficiario con el mismo RFC y auxiliar contable.");
      }

      supplier.Update(fields);

      supplier.Save();

      return SupplierMapper.Map(supplier);
    }

    #endregion Command use cases

  }  // class SupplierUseCases

}  // namespace Empiria.Procurement.Contracts.UseCases
