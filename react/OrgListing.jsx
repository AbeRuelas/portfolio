import React, { Fragment, useState, useEffect } from "react";
import { Col, Row, Form, Container } from "react-bootstrap";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import "./OrganizationForm.css";
import lookUpService from "services/lookUpService";
import * as helper from "../../helper/utils";
import debug from "wepair-debug";
import OrganizationCard from "./OrganizationCard";
import OrganizationCardFullWidth from "./OrganizationCardFullWidth";
import organizationService from "../../services/organizationService";
import { ReactComponent as CircleX } from "../../assets/images/icons/circlex.svg";
const _logger = debug.extend("organization");
const OrganizationListing = () => {
  const [organizationData, setOrganizationData] = useState({
    arrayOfOrganizations: [],
    organizationComponents: [],
    totalResults: 0,
    pageIndex: 1,
    pageSize: 9,
    selectByCategory: 0,
    currentPage: 1,
    searchListing: "",
  });
  const [lookUpData, setLookUpType] = useState({
    organizationTypes: [],
    mappedOrganizationTypes: [],
  });
  useEffect(() => {
    if (!lookUpData.organizationTypes.length > 0) {
      lookUpService.lookUp(["OrganizationTypes"]).then(onLookSuccess).catch(onLookError);
    }
    if (organizationData.selectByCategory > 0) {
      organizationService
        .selectByCategory(
          organizationData.selectByCategory,
          organizationData.pageIndex - 1,
          organizationData.pageSize
        )
        .then(onPageSuccess)
        .catch(onPageError);
    } else {
      organizationService
        .selectAllPaginate(organizationData.pageIndex - 1, organizationData.pageSize)
        .then(onPageSuccess)
        .catch(onPageError);
    }
  }, [organizationData.pageIndex, organizationData.selectByCategory]);
  const onLookSuccess = (response) => {
    const { organizationTypes } = response.item;
    setLookUpType((prevState) => {
      let newState = { ...prevState, organizationTypes };
      newState.mappedOrganizationTypes = newState.organizationTypes.map(
        helper.mapLookUpItem
      );
      return newState;
    });
  };
  const onLookError = (error) => {
    _logger("onLookError", error);
  };
  const onChangePage = (page) => {
    _logger("settingCurrentPage", page);
    setOrganizationData((prevState) => {
      let newState = { ...prevState };
      newState.currentPage = page;
      newState.pageIndex = page;
      return newState;
    });
  };
  const onPageSuccess = (response) => {
    let { pagedItems, totalCount } = response.item;
    setOrganizationData((prevState) => {
      let newState = { ...prevState };
      newState.arrayOfOrganizations = pagedItems;
      newState.organizationComponents = pagedItems.map(cardMap);
      newState.totalResults = totalCount;
      return newState;
    });
  };
  const onPageError = (error) => {
    _logger("onPageError", error);
  };
  const cardMap = (newOrg) => {
    return (
      <Col xl={4} lg={4} md={6} sm={12} className="d-flex" key={newOrg}>
        <OrganizationCard newOrg={newOrg} />
      </Col>
    );
  };
  const mainOrganization =
    organizationData.arrayOfOrganizations.length > 0
      ? organizationData.arrayOfOrganizations[0]
      : null;
  const handleSelectChange = (e) => {
    e.preventDefault();
    let { value } = e.target;
    setOrganizationData((prevState) => {
      let newState = { ...prevState };
      newState.pageIndex = 1;
      newState.selectByCategory = value;
      return newState;
    });
  };
  const handleSearchChange = (e) => {
    e.preventDefault();
    let { value } = e.target;
    setOrganizationData((prevState) => {
      let newState = { ...prevState };
      newState.searchListing = value;
      return newState;
    });
  };
  const handleSearchSubmit = (e) => {
    if (e.key === "Enter") {
      organizationService
        .searchPaginated(
          organizationData.pageIndex - 1,
          organizationData.pageSize,
          organizationData.searchListing
        )
        .then(onPageSuccess)
        .catch(onPageError);
    }
  };
  const handleResetSearch = () => {
    setOrganizationData((prevState) => {
      let newState = { ...prevState };
      newState.searchListing = "";
      return newState;
    });
  };
  return (
    <Fragment>
      <Container fluid>
        <Row className="justify-content-center mb-4 pt-4">
          <Col xl={10} lg={10} md={12} sm={10}>
            <Row className="justify-content-center">
              <Col xs={8} sm={7} md={7} lg={7} className="mb-2">
                <div className="input-group position-relative">
                  <Form.Control
                    type="text"
                    placeholder="Search organizations by name or description..."
                    value={organizationData.searchListing}
                    onChange={handleSearchChange}
                    onKeyDown={handleSearchSubmit}
                    className="py-3"
                  />
                  <span className="reset-icon" onClick={handleResetSearch}>
                    {<CircleX />}
                  </span>
                </div>
              </Col>
              <Col xs={2} sm={3} md={3} lg={3} className="mb-2">
                <Form.Control as="select" onChange={handleSelectChange} className="py-3">
                  <option>Select All</option>
                  {lookUpData.mappedOrganizationTypes}
                </Form.Control>
              </Col>
            </Row>
          </Col>
        </Row>
        <Row className="mx-auto justify-content-center">
          <Col xl={10} lg={10} md={8} sm={10}>
            {mainOrganization && <OrganizationCardFullWidth newOrg={mainOrganization} />}
          </Col>
          <Col xl={10} lg={10} md={8} sm={10} className="mt-4">
            <Row>{organizationData.organizationComponents}</Row>
          </Col>
        </Row>
        <div className="justify-content-center d-flex">
          <Pagination
            className="pagination-item pagination-item-active"
            onChange={onChangePage}
            current={organizationData.currentPage}
            total={organizationData.totalResults}></Pagination>
        </div>
      </Container>
    </Fragment>
  );
};
export default OrganizationListing;
