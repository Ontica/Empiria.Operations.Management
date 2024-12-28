﻿/* Empiria Operations ****************************************************************************************
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

using Empiria.Procurement.Contracts.Adapters;

using Empiria.Procurement.Suppliers.UseCases;
using Empiria.Procurement.Suppliers.Adapters;

namespace Empiria.Procurement.Suppliers.WebApi {

  /// <summary>Web API used to retrive and update suppliers.</summary>
  public class SuppliersController : WebApiController {

    #region Query web apis


    [HttpPost]
    [Route("v8/procurement/suppliers/search")]
    public CollectionModel SearchSuppliers([FromBody] SuppliersQuery query) {

      base.RequireBody(query);

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        FixedList<SupplierDescriptor> suppliers = usecases.SearchSuppliers(query);

        return new CollectionModel(base.Request, suppliers);
      }
    }


    [HttpGet]
    [Route("v8/procurement/suppliers/{supplierUID:guid}/contracts/to-order")]
    public CollectionModel GetSupplierContractsToOrder([FromUri] string supplierUID) {

      using (var usecases = SupplierUseCases.UseCaseInteractor()) {
        FixedList<ContractDto> contracts = usecases.GetSupplierContractsToOrder(supplierUID);

        return new CollectionModel(base.Request, contracts);
      }
    }

    #endregion Query web apis

  }  // class SuppliersController

}  // namespace Empiria.Procurement.Suppliers.WebApi
