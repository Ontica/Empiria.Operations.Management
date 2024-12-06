﻿/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Procurement.WebApi.dll               Pattern   : Web api Controller                    *
*  Type     : ContractMilestonesController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update milestone and milestone items.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;
using Empiria.Procurement.Contracts.Adapters;
using Empiria.Procurement.Contracts.UseCases;

namespace Empiria.Procurement.Contracts.WebApi {

  /// <summary>Web API used to retrive and update milestone and milestone items</summary>
  public class ContractMilestonesController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/procurement/contracts/milestones/{milestoneUID:guid}")]
    public SingleObjectModel GetMilestone([FromUri] string milestoneUID) {

      using (var usecases = ContractMilestoneUseCases.UseCaseInteractor()) {
        ContractMilestoneDto milestone = usecases.ReadContractMilestone(milestoneUID);

        return new SingleObjectModel(base.Request, milestone);
      }
    }


    [HttpGet]
    [Route("v8/procurement/contracts/milestones/{milestoneUID:guid}/items")]
    public CollectionModel GetContractMilestoneItems([FromUri] string milestoneUID) {

      using (var usecases = ContractMilestoneUseCases.UseCaseInteractor()) {
        FixedList<ContractMilestoneItemDto> milestoneItems = usecases.GetContracMilestonetItems(milestoneUID);

        return new CollectionModel(base.Request, milestoneItems);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/procurement/contracts/milestones")]
    public SingleObjectModel CreateMilestone([FromBody] ContractMilestoneFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractMilestoneUseCases.UseCaseInteractor()) {
        ContractMilestoneDto milestone = usecases.CreateContractMilestone(fields);

        return new SingleObjectModel(base.Request, milestone);
      }
    }


    [HttpPost]
    [Route("v8/procurement/contracts/milestones/{milestoneUID:guid}/items")]
    public SingleObjectModel CreateContractItems([FromUri] string milestoneUID,
                                                 [FromBody] ContractMilestoneItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractMilestoneItemUseCases.UseCaseInteractor()) {
        ContractMilestoneItemDto milestoneItem = usecases.CreateContractMilestoneItem(fields);

        return new SingleObjectModel(base.Request, milestoneItem);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/contracts/milestones/{milestoneUID:guid}")]
    public NoDataModel DeleteContractMilestone([FromUri] string milestoneUID) {

      base.RequireResource(milestoneUID, nameof(milestoneUID));

      using (var usecases = ContractMilestoneUseCases.UseCaseInteractor()) {
        usecases.RemoveContractMilestone(milestoneUID);

        return new NoDataModel(this.Request);
      }
    }

    [HttpDelete]
    [Route("v8/procurement/contracts/milestones/{milestoneUID:guid}/items/{milestoneItemUID:guid}")]
    public NoDataModel DeleteContractItems([FromUri] string milestoneUID,
                                           [FromUri] string milestoneItemUID) {

      base.RequireResource(milestoneItemUID, nameof(milestoneItemUID));

      using (var usecases = ContractMilestoneItemUseCases.UseCaseInteractor()) {
        usecases.RemoveContractMilestoneItem(milestoneUID, milestoneItemUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v8/procurement/contracts/milestone-items/{milestoneItemUID:guid}")]
    public SingleObjectModel UpdateContractItem([FromUri] string milestoneItemUID,
                                        [FromBody] ContractMilestoneItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractMilestoneItemUseCases.UseCaseInteractor()) {
        ContractMilestoneItemDto milestoneItem = usecases.UpdateContractMilestoneItem(milestoneItemUID, fields);

        return new SingleObjectModel(base.Request, milestoneItem);
      }
    }


    [HttpPut]
    [Route("v8/procurement/contracts/milestone/{milestoneUID:guid}")]
    public SingleObjectModel UpdateContractItem([FromUri] string milestoneUID,
                                                [FromBody] ContractMilestoneFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractMilestoneUseCases.UseCaseInteractor()) {
        ContractMilestoneDto milestone = usecases.UpdateContractMilestone(milestoneUID, fields);

        return new SingleObjectModel(base.Request, milestone);
      }
    }

    #endregion Command web apis

  }  // class ContractMilestonesController

}  // namespace Empiria.Procurement.Contracts.WebApi
