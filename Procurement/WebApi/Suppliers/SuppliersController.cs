/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : SuppliersController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update suppliers.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using System.Threading.Tasks;

using Empiria.WebApi;

using Empiria.Procurement.Suppliers.Adapters;
using Empiria.Procurement.Suppliers.UseCases;

namespace Empiria.Procurement.Suppliers.WebApi {

  /// <summary>Web API used to retrieve and update suppliers.</summary>
  public class SuppliersController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/procurement/suppliers/{supplierUID:guid}")]
    public SingleObjectModel GetSupplier([FromUri] string supplierUID) {

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        SupplierHolderDto supplier = usecases.GetSupplier(supplierUID);

        return new SingleObjectModel(base.Request, supplier);
      }
    }


    [HttpGet]
    [Route("v8/procurement/suppliers/types")]
    public CollectionModel GetSupplierTypes() {

      FixedList<NamedEntityDto> types = SupplierType.GetSupplierTypes()
                                                    .MapToNamedEntityList(false);

      return new CollectionModel(base.Request, types);
    }


    //[HttpPost]
    //[Route("v8/procurement/suppliers/match-subledger-account")]
    //public async Task<SingleObjectModel> MatchSupplierSubledgerAccount([FromBody] SubledgerAccountQuery fields) {

    //  using (var usecases = SupplierUseCases.UseCaseInteractor()) {
    //    NamedEntityDto subledgerAccount = await usecases.MatchSupplierSubledgerAccount(fields);

    //    var json = new {
    //      number = subledgerAccount.UID,
    //      name = subledgerAccount.Name
    //    };

    //    return new SingleObjectModel(base.Request, json);
    //  }
    //}


    [HttpPost]
    [Route("v8/procurement/suppliers/search")]
    public CollectionModel SearchSuppliers([FromBody] SuppliersQuery query) {

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        FixedList<SupplierDescriptor> suppliers = usecases.SearchSuppliers(query);

        return new CollectionModel(base.Request, suppliers);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/procurement/suppliers")]
    public async Task<SingleObjectModel> CreateSupplier([FromBody] SupplierFields fields) {

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        SupplierHolderDto supplier = await usecases.CreateSupplier(fields);

        return new SingleObjectModel(base.Request, supplier);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/suppliers/{supplierUID:guid}")]
    public NoDataModel DeleteSupplier([FromUri] string supplierUID) {

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteSupplier(supplierUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/suppliers/{supplierUID:guid}")]
    public async Task<SingleObjectModel> UpdateSupplier([FromUri] string supplierUID,
                                                        [FromBody] SupplierFields fields) {

      fields.UID = supplierUID;

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        SupplierHolderDto supplier = await usecases.UpdateSupplier(fields);

        return new SingleObjectModel(base.Request, supplier);
      }
    }

    #endregion Command web apis

  }  // class SuppliersController

}  // namespace Empiria.Procurement.Suppliers.WebApi
