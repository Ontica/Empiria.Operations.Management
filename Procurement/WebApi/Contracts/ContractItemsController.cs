/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Command Controller                    *
*  Type     : ContractItemsController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update contract items.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Procurement.Contracts.Adapters;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Procurement.Contracts.WebApi {

  /// <summary>Web API used to retrive and update contract items.</summary>
  public class ContractItemsController : WebApiController {

    #region Command web apis

    [HttpPost]
    [Route("v8/procurement/contracts/{contractUID:guid}/items")]
    public SingleObjectModel AddContractItem([FromUri] string contractUID,
                                             [FromBody] ContractItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        ContractItemDto contractItem = usecases.AddContractItem(contractUID, fields);

        return new SingleObjectModel(base.Request, contractItem);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/contracts/{contractUID:guid}/items/{contractItemUID:guid}")]
    public NoDataModel RemoveContractItem([FromUri] string contractUID,
                                          [FromUri] string contractItemUID) {

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveContractItem(contractUID, contractItemUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/contracts/{contractUID:guid}/items/{contractItemUID:guid}")]
    public SingleObjectModel UpdateContractItem([FromUri] string contractUID,
                                                [FromUri] string contractItemUID,
                                                [FromBody] ContractItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractItemUseCases.UseCaseInteractor()) {
        ContractItemDto contractItem = usecases.UpdateContractItem(contractUID,
                                                                   contractItemUID,
                                                                   fields);

        return new SingleObjectModel(base.Request, contractItem);
      }
    }

    #endregion Command web apis

  }  // class ContractItemsController

}  // namespace Empiria.Procurement.Contracts.WebApi
