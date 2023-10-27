
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
    }
}
