
function OrganizationForm() {
  const [lookUpData, setLookUpType] = useState({
    organizationTypes: [],
    mappedOrganizationTypes: [],
  });
  const initialValues = {
    organizationTypeId: 0,
    name: "",
    headline: "",
    description: "",
    logo: "",
    locationId: 0,
    phone: "",
    siteUrl: "",
  };
  const [orgData, setOrgData] = useState({
    initialValues,
  });

  const [editor, setEditor] = useState(null);
  const [orgCharCount, setOrgCharCount] = useState(0);

  const updateCharCount = (data) => {
    const tempDiv = document.createElement("div");
    tempDiv.innerHTML = data;
    const textDescription = tempDiv.textDescription || tempDiv.innerText || "";
    _logger("text removal", textDescription);
    setOrgCharCount(textDescription.length);
    tempDiv.innerHTML = "";
  };

  const renderOrgCharCount = () => {
    return orgCharCount > 3000 ? (
      <p className="red-text">Story must be at less than 3000 characters!</p>
    ) : null;
  };

  const navigate = useNavigate();

  useEffect(() => {
    lookUpService
      .lookUp(["OrganizationTypes"])
      .then(onLookSuccess)
      .catch(onLookError);
  }, []);

  const onLookSuccess = (response) => {
    const { organizationTypes } = response.item;

    setLookUpType((prevState) => {
      let newState = { ...prevState, organizationTypes };
      newState.mappedOrganizationTypes = newState.organizationTypes.map(
        helper.mapLookUpItem
      );
      return newState;
    });
    _logger(lookUpData);
  };

  const onLookError = (error) => {
    _logger("onLookError", error);
  };

  function handleSubmit(values, { setSubmitting, resetForm }) {
    organizationService
      .add(values)
      .then((response) => {
        onSubmitSuccess(response, resetForm);
      })
      .catch((error) => {
        onSubmitError(error, { setSubmitting });
      });
  }

  const onSubmitSuccess = (response, resetForm) => {
    Swal.fire({
      title: "Organization has been created!",
      showDenyButton: true,
      showCancelButton: false,
      confirmButtonText: "View Organizations",
      denyButtonText: `Add Another`,
    }).then((result) => {
      if (result.isConfirmed) {
        navigate("/organization/listing");
      }
    });
    resetForm();
    if (editor) {
      editor.setData("");
    }
  };

  function onSubmitError(error) {
    _logger(error);
    Swal.fire({
      title: "Something went wrong!",
      text: "Please try Again.",
    });
    return error;
  }

  const handleLocationSuccess = (response) => {
    setOrgData((prevState) => {
      let newState = { ...prevState };
      newState.initialValues.locationId = response;
      return newState;
    });
  };

  return (
    <React.Fragment>
      <Container>
        <Row>
          <Col lg={12} md={12} sm={12}>
            <div className="border-bottom pb-4 mb-4 d-md-flex align-items-center justify-content-between">
              <div className="mb-3 mb-md-0">
                <h1 className="mb-1 h2 fw-bold">Add New Organization</h1>
                <Breadcrumb>
                  <Breadcrumb.Item href="#">Dashboard</Breadcrumb.Item>
                  <Breadcrumb.Item active>Add New Organization</Breadcrumb.Item>
                </Breadcrumb>
              </div>
            </div>
          </Col>
        </Row>
        <div className="justify-content-center mb-6 pt-6">
          <Col xl={12} lg={12} md={12} sm={12} className="py-5 py-xl-5">
            {orgData.initialValues.locationId === 0 && (
              <>
                <h2>First add a location:</h2>

                <LocationForm
                  className="form-control form-control-lg"
                  name="locationId"
                  placeholder="location"
                  handleLocationSuccess={handleLocationSuccess}
                />
              </>
            )}
          </Col>
        </div>
        {orgData.initialValues.locationId > 0 && (
          <Row>
            <Formik
              enableReinitialize={true}
              initialValues={orgData.initialValues}
              onSubmit={handleSubmit}
              validationSchema={organizationSchema}
            >
              {({ values, setFieldValue }) => (
                <>
                  <Col lg={6} md={8} className="py-5 py-xl-5">
                    <Form>
                      <Card>
                        <Card.Body className="mb-3">
                          <div className="form-group organization flex-container">
                            <label
                              className="fw-bold"
                              htmlFor="organizationTypeId"
                            >
                              Add Organization
                            </label>
                            <Field
                              as="select"
                              className="form-control"
                              name="organizationTypeId"
                              placeholder="Type of Organization"
                            >
                              <option value="">Select a type</option>
                              {lookUpData.mappedOrganizationTypes}
                            </Field>
                            <ErrorMessage
                              name="organizationTypeId"
                              component="div"
                              className="has-error"
                            />
                          </div>
                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="name">
                              Name
                            </label>
                            <Field
                              className="form-control"
                              name="name"
                              type="name"
                              placeholder="Name of organization"
                            />
                            <ErrorMessage
                              name="name"
                              component="div"
                              className="has-error"
                            />
                          </div>
                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="headline">
                              Headline
                            </label>
                            <Field
                              className="form-control"
                              name="headline"
                              type="headline"
                              placeholder="Headline"
                            />
                            <ErrorMessage
                              name="headline"
                              component="div"
                              className="has-error"
                            />
                          </div>
                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="description">
                              Description
                            </label>
                            <CKEditor
                              name="description"
                              editor={ClassicEditor}
                              data={values?.description}
                              onReady={(editor) => {
                                setEditor(editor);
                              }}
                              onChange={(event, editor) => {
                                const data = editor.getData();
                                setFieldValue("description", data);
                                updateCharCount(data);
                              }}
                            />
                            <div className="org-content-error">
                              <p className="right-align-text">
                                <span
                                  className={`${
                                    orgCharCount > 3000 ? "red-text" : ""
                                  }`}
                                >
                                  {orgCharCount}
                                </span>
                                /3000{" "}
                              </p>
                              {renderOrgCharCount()}
                            </div>
                          </div>
                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="logo">
                              Logo
                            </label>
                            <Field
                              className="form-control"
                              name="logo"
                              type="logo"
                              placeholder="Logo"
                            />
                            <ErrorMessage
                              name="logo"
                              component="div"
                              className="has-error"
                            />
                          </div>

                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="phone">
                              Phone
                            </label>
                            <Field
                              className="form-control"
                              name="phone"
                              type="tel"
                              placeholder="Phone"
                            />
                            <ErrorMessage
                              name="phone"
                              component="div"
                              className="has-error"
                            />
                          </div>
                          <div className="form-group organization flex-container mb-3">
                            <label className="fw-bold" htmlFor="siteUrl">
                              SiteUrl
                            </label>
                            <Field
                              className="form-control"
                              name="siteUrl"
                              type="url"
                              placeholder="SiteUrl"
                            />
                            <ErrorMessage
                              name="siteUrl"
                              component="div"
                              className="has-error"
                            />
                          </div>
                          <button type="submit" className="btn btn-primary">
                            Submit
                          </button>
                        </Card.Body>
                      </Card>
                    </Form>
                  </Col>
                  <Col lg={6} md={12} className="py-5 py-xl-5">
                    <Card className="mb-4 shadow-lg">
                      <Image
                        variant="top"
                        src={values?.logo}
                        className="rounded-top-md img-fluid"
                      />
                      <Card.Body>
                        <a className="fs-5 fw-semi-bold d-block mb-3 text-success no-underline">
                          Organizations
                        </a>
                        <h3>
                          <a className="text-inherit no-underline">
                            {values?.headline}
                          </a>
                        </h3>
                        <hr></hr>
                        <h5
                          className="text-secondary mb-1"
                          dangerouslySetInnerHTML={{
                            __html: values?.description.slice(0, 100) + "...",
                          }}
                        ></h5>
                        <Row className="align-items-center g-0 mt-4">
                          <Col className="col lh-8">
                            <h5 className="mb-1">{values?.name}</h5>
                            <p className="fs-6 mb-0">{values?.phone}</p>
                            <p className="fs-6 mb-0">{values?.siteUrl}</p>
                          </Col>
                        </Row>
                      </Card.Body>
                    </Card>
                  </Col>
                </>
              )}
            </Formik>
          </Row>
        )}
      </Container>
    </React.Fragment>
  );
}

export default OrganizationForm;
