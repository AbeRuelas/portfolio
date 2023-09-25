using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Organizations;
using Sabio.Models.Requests.Organizations;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationApiController : BaseApiController
    {
        private IOrganizationService _service = null;
        private IAuthenticationService<int> _authenticationService = null;
        public OrganizationApiController(IOrganizationService service
            , ILogger<OrganizationApiController> logger
            , IAuthenticationService<int> authenticationService) : base(logger)
        {
            _service = service;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(OrganizationAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int CreatedBy = _authenticationService.GetCurrentUserId();
                int organization = _service.Add(model, CreatedBy);
                ItemResponse<int> itemResponse = new ItemResponse<int> { Item = organization };
                result = Created201(itemResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Organization>> SelectById(int id)
        {
            int iCode = 200;
            BaseResponse itemResponse = null;
            try
            {
                Organization organization = _service.SelectById(id);
                if (organization == null)
                {
                    iCode = 404;
                    itemResponse = new ErrorResponse("Records not found.");
                }
                else
                {
                    itemResponse = new ItemResponse<Organization> { Item = organization };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                itemResponse = new ErrorResponse($"Generic Error:{ex.Message}");
            }
            return StatusCode(iCode, itemResponse);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Organization>>> SelectAll_Paginated(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Organization> paged = _service.SelectAll_Paginated(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Organization>> response = new ItemResponse<Paged<Organization>>{ Item = paged };
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }
