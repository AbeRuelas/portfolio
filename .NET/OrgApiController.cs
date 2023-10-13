using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WePair.Models;
using WePair.Models.Domain.Organizations;
using WePair.Models.Requests.Email;
using WePair.Models.Requests.Organizations;
using WePair.Services;
using WePair.Services.Interfaces;
using WePair.Web.Controllers;
using WePair.Web.Models.Responses;
using System;

namespace WePair.Web.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationApiController : BaseApiController
    {
        private IOrganizationService _service = null;
        private IAuthenticationService<int> _authenticationService = null;
        private IEmailService _emailService = null;
        public OrganizationApiController(IOrganizationService service
            , ILogger<OrganizationApiController> logger
            , IAuthenticationService<int> authenticationService, IEmailService emailService) : base(logger)
        {
            _service = service;
            _authenticationService = authenticationService;
            _emailService = emailService;
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
        [AllowAnonymous]
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

        [HttpGet("currentUser/paginate")]
        public ActionResult<ItemResponse<Paged<Organization>>> SelectByCreatedBy_Paginated(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                int currentUser = _authenticationService.GetCurrentUserId();
                Paged<Organization> paged = _service.SelectByCreatedBy_Paginated(pageIndex, pageSize, currentUser);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Organization>> response = new ItemResponse<Paged<Organization>> { Item = paged };
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

        [HttpGet("category/{id:int}/paginate")]
        public ActionResult<ItemResponse<Paged<Organization>>> SelectByCategory(int id, int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Organization> paged = _service.SelectByCategory(id, pageIndex, pageSize);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Organization>> response = new ItemResponse<Paged<Organization>> { Item = paged };
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

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Organization>>> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;
            try
            {
                Paged<Organization> paged = _service.SearchPaginated(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Organization>> response = new ItemResponse<Paged<Organization>> { Item = paged };
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

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(OrganizationUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int currentUser = _authenticationService.GetCurrentUserId();
                _service.Update(model, currentUser);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpPost("members")]
        public ActionResult<ItemResponse<int>> Create(OrgMemberAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authenticationService.GetCurrentUserId();
                int organizationMember = _service.AddOrgMember(model, userId);
                ItemResponse<int> itemResponse = new ItemResponse<int> { Item = organizationMember };
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

        [HttpPut("members/{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateOM(OrgMemberUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdateOM(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpDelete("members/{id:int}")]
        public ActionResult<SuccessResponse> DeleteOM(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.DeleteOM(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("members/{id:int}")]
        public ActionResult<ItemResponse<OrgMember>> OMSelectById(int id)
        {
            int iCode = 200;
            BaseResponse itemResponse = null;
            try
            {
                OrgMember orgMember = _service.OMSelectById(id);
                if (orgMember == null)
                {
                    iCode = 404;
                    itemResponse = new ErrorResponse("Records not found.");
                }
                else
                {
                    itemResponse = new ItemResponse<OrgMember> { Item = orgMember };
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

        [HttpGet("members/{orgId:int}/paginate")]
        public ActionResult<ItemResponse<Paged<OrgMember>>> OMSelectByOrgId_Paginated(int orgId, int pageIndex, int pageSize )
        {
            ActionResult result = null;
            try
            {
                Paged<OrgMember> paged = _service.OMSelectByOrgId_Paginated(orgId, pageIndex, pageSize );
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<OrgMember>> response = new ItemResponse<Paged<OrgMember>> { Item = paged };
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
        [HttpGet("members/search")]
        public ActionResult<ItemResponse<Paged<OrgMember>>> SearchByPaginate(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;
            try
            {
                Paged<OrgMember> paged = _service.OMSearchByPagination(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<OrgMember>> response = new ItemResponse<Paged<OrgMember>>();
                    response.Item = paged;
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

        [HttpPost("invite")]
        public ActionResult<ItemResponse<int>> CreateInvite(OrgInviteMemberAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authenticationService.GetCurrentUserId();
                string token = Guid.NewGuid().ToString();
                int inviteMember = _service.InviteOrgMember(model, userId, token);
                ConfirmEmailRequest emailConModel = new ConfirmEmailRequest
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                _emailService.InviteEmail(emailConModel, token);
                ItemResponse<int> itemResponse = new ItemResponse<int> { Item = inviteMember };
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
        [HttpDelete("invite/{id:int}")]
        public ActionResult<SuccessResponse> DeleteInviteMem(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.DeleteInviteMem(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}/confirm")]
        public ActionResult<SuccessResponse> ConfirmEmail(string token, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.ConfirmInvite(token, id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
    }
}
