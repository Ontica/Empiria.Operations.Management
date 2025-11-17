/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : ContractsController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update contracts.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Orders.Adapters;

using Empiria.Procurement.Contracts.Adapters;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Procurement.Contracts.WebApi {

  /// <summary>Web API used to retrive and update contracts</summary>
  public class ContractsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/procurement/contracts/{contractUID:guid}")]
    public SingleObjectModel GetContract([FromUri] string contractUID) {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        ContractHolderDto contract = usecases.GetContract(contractUID);

        return new SingleObjectModel(base.Request, contract);
      }
    }


    [HttpGet]
    [Route("v8/procurement/contracts/contract-types")]    // ToDo: Remove
    [Route("v8/procurement/contracts/categories")]
    public CollectionModel GetContractCategories() {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> categories = usecases.GetContractCategories();

        return new CollectionModel(base.Request, categories);
      }
    }


    [HttpPost]
    [Route("v8/procurement/contracts/search")]
    public CollectionModel SearchContracts([FromBody] OrdersQuery query) {

      base.RequireBody(query);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<ContractDescriptor> contracts = usecases.SearchContracts(query);

        return new CollectionModel(base.Request, contracts);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/procurement/contracts")]
    public SingleObjectModel CreateContract([FromBody] ContractFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        ContractHolderDto contract = usecases.CreateContract(fields);

        return new SingleObjectModel(base.Request, contract);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/contracts/{contractUID:guid}")]
    public NoDataModel DeleteContract([FromUri] string contractUID) {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteContract(contractUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/contracts/{contractUID:guid}")]
    public SingleObjectModel UpdateContract([FromUri] string contractUID,
                                            [FromBody] ContractFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        ContractHolderDto contract = usecases.UpdateContract(contractUID, fields);

        return new SingleObjectModel(base.Request, contract);
      }
    }

    #endregion Command web apis

  }  // class ContractsController

}  // namespace Empiria.Procurement.Contracts.WebApi
