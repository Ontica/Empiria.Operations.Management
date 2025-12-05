/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Suppliers Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : SuppliersController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update suppliers.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Parties;

using Empiria.Procurement.Suppliers.Adapters;
using Empiria.Procurement.Suppliers.UseCases;

namespace Empiria.Procurement.Suppliers.WebApi {

  /// <summary>Web API used to retrive and update suppliers.</summary>
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
    [Route("v8/procurement/suppliers/kinds")]
    [Route("v8/procurement/suppliers/types")]
    public CollectionModel GetSupplierKinds() {

      FixedList<NamedEntityDto> kinds = TaxData.GetTaxEntityKinds()
                                               .MapToNamedEntityList(false);

      return new CollectionModel(base.Request, kinds);
    }


    [HttpPost]
    [Route("v8/procurement/suppliers/search")]
    public CollectionModel SearchSuppliers([FromBody] SuppliersQuery query) {

      base.RequireBody(query);

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        FixedList<SupplierDescriptor> suppliers = usecases.SearchSuppliers(query);

        return new CollectionModel(base.Request, suppliers);
      }
    }

    #endregion Query web apis

  }  // class SuppliersController

}  // namespace Empiria.Procurement.Suppliers.WebApi
