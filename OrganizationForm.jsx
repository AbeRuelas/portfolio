import React, { useState, useEffect } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import {
  Col,
  Row,
  Card,
  Container,
  Button,
  Breadcrumb,
  Image,
} from "react-bootstrap";
import { Link } from "react-router-dom";
import organizationSchema from "../../schema/organizationSchema";
import organizationService from "../../services/organizationService";
import lookUpService from "services/lookUpService";
import * as helper from "../../helper/utils";
import debug from "sabio-debug";
import Swal from "sweetalert2";
import "./OrganizationForm.css";

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
    locationId: 2,
    phone: "",
    siteUrl: "",
  };
  useEffect(() => {
    lookUpService
      .lookUp(["OrganizationTypes"])
      .then(onLookSuccess)
      .catch(onLookError);
  }, []);

  const onLookSuccess = (response) => {
    _logger("onLookSuccess", response);
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

  const handleSubmit = (values) => {
    _logger("onHandleSubmit", values);

    organizationService.add(values).then(onSubmitSuccess).catch(onSubmitError);
  };

  const onSubmitSuccess = (response) => {
    _logger(response);
    Swal.fire({
      title: "Organization successfully submitted!",
    });
    return response;
  };

  function onSubmitError(error) {
    _logger(error);
    Swal.fire({
      title: "Something went wrong!",
      text: "Please try Again.",
    });
    return error;
  }

  return (
    <React.Fragment>
      <Formik
        enableReinitialize={true}
        initialValues={initialValues}
        onSubmit={handleSubmit}
        validationSchema={organizationSchema}
      >
        {({ values }) => (
          <Container>
            <Row>
              <Col lg={12} md={12} sm={12}>
                <div className="border-bottom pb-4 mb-4 d-md-flex align-items-center justify-content-between">
                  <div className="mb-3 mb-md-0">
                    <h1 className="mb-1 h2 fw-bold">Add New Organization</h1>
                    <Breadcrumb>
                      <Breadcrumb.Item href="#">Dashboard</Breadcrumb.Item>
                      <Breadcrumb.Item href="#">CMS</Breadcrumb.Item>
                      <Breadcrumb.Item active>
                        Add New Organization
                      </Breadcrumb.Item>
                    </Breadcrumb>
                  </div>
                  <div>
                    <Link
                      to="/cms/all-organizations"
                      className="btn btn-outline-white"
                    >
                      Back to All Organizations
                    </Link>
                  </div>
                </div>
              </Col>
            </Row>
            <Row>
              <Col lg={6} md={8} className="py-5 py-xl-5">
                <Form>
                  <Card>
                    <Card.Body className="mb-3">
                      <div className="form-group organization flex-container">
                        <label className="fw-bold" htmlFor="organizationTypeId">
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
                        <Field
                          component="textarea"
                          className="form-control"
                          name="description"
                          type="description"
                          placeholder="Enter description..."
                        />
                        <ErrorMessage
                          name="description"
                          component="div"
                          className="has-error"
                        />
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
                      <Button type="submit" className="btn btn-primary">
                        Submit
                      </Button>
                    </Card.Body>
                  </Card>
                </Form>
              </Col>
              <Col lg={6} md={12} className="py-5 py-xl-5">
                <Card className="mb-4 shadow-lg">
                  <Image
                    variant="top"
                    src={values.logo}
                    className="rounded-top-md img-fluid"
                  />
                  <Card.Body>
                    <a className="fs-5 fw-semi-bold d-block mb-3 text-success no-underline">
                      Organizations
                    </a>
                    <h3>
                      <a className="text-inherit no-underline">
                        {values.headline}
                      </a>
                    </h3>
                    <hr></hr>
                    <Row className="align-items-center g-0 mt-4">
                      <Col className="col lh-8">
                        <h5 className="mb-1">{values.name}</h5>
                        <p className="fs-6 mb-0">{values.phone}</p>
                        <p className="fs-6 mb-0">{values.siteUrl}</p>
                      </Col>
                    </Row>
                  </Card.Body>
                </Card>
              </Col>
            </Row>
          </Container>
        )}
      </Formik>
    </React.Fragment>
  );
}

export default OrganizationForm;
