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
    [Route("v8/procurement/contracts/contract-types")]
    public CollectionModel GetContractTypes() {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> contractTypes = usecases.GetContractTypes();

        return new CollectionModel(base.Request, contractTypes);
      }
    }


    [HttpPost]
    [Route("v8/procurement/contracts/search")]
    public CollectionModel SearchContracts([FromBody] ContractQuery query) {

      base.RequireBody(query);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<ContractDescriptor> contracts = usecases.SearchContracts(query);

        return new CollectionModel(base.Request, contracts);
      }
    }


    [HttpGet]
    [Route("v8/procurement/contracts/contract-items/{contractUID:guid}")]
    public CollectionModel GetContractItems([FromUri] string contractUID) {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<ContractItemDto> contractItems = usecases.GetContractItems(contractUID);

        return new CollectionModel(base.Request, contractItems);
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


    [HttpPost]
    [Route("v8/procurement/contracts/add-contract-items/{contractUID:guid}")]
    public SingleObjectModel CreateContractItems([FromUri] string contractUID,
                                                 [FromBody] ContractItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        ContractItemDto contractItem = usecases.CreateContractItem(contractUID, fields);

        return new SingleObjectModel(base.Request, contractItem);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/contracts/contract-items/{contractItemUID:guid}")]
    public NoDataModel DeleteContractItems([FromUri] string contractItemUID) {

      base.RequireResource(contractItemUID, nameof(contractItemUID));

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        usecases.DeleteContractItem(contractItemUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v8/procurement/contracts/contract-items/{contractItemUID:guid}")]
    public SingleObjectModel UpdateContractItem([FromUri] string contractItemUID,
                                                [FromBody] ContractItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        ContractItemDto contractItem = usecases.UpdateContractItem(contractItemUID, fields);

        return new SingleObjectModel(base.Request, contractItem);
      }
    }

    #endregion Command web apis

  }  // class ContractsController

}  // namespace Empiria.Procurement.Contracts.WebApi
