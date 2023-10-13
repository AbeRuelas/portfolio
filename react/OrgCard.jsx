import React from "react";
import { useNavigate } from "react-router-dom";
import { Card, Row, Col, Image } from "react-bootstrap";
import PropTypes from "prop-types";
import debug from "wepair-debug";
import "./OrganizationCard.css";

const _logger = debug.extend("organization");

function OrganizationCard({ newOrg }) {
  _logger("props are passing", newOrg);

  const navigate = useNavigate();
  const handleImageClick = () => {
    navigate(`/organization/single/${newOrg.id}`);
  };

  return (
    <Card className="mb-4 shadow-lg" key={newOrg.id}>
      {newOrg.logo && (
        <div className="d-flex justify-content-center overflow-hidden fixed-height-200">
          <Image
            variant="top"
            src={newOrg?.logo}
            className="img-fluid mx-auto d-block"
            onClick={handleImageClick}
          />
        </div>
      )}
      <Card.Body className="d-flex flex-column justify-content-between">
        <span className="fs-5 fw-semi-bold d-block mb-3 text-success">
          Organizations
        </span>
        <h3>
          <span className="text-inherit">{newOrg?.headline}</span>
        </h3>
        <hr></hr>
        <h5
          className="text-secondary mb-1"
          dangerouslySetInnerHTML={{
            __html: newOrg?.description.slice(0, 60) + "...",
          }}
        ></h5>
        <Row className="align-items-center g-0 mt-4">
          <Col className="col lh-8">
            <h5 className="mb-1">{newOrg?.name}</h5>
            <p className="fs-6 mb-0">{newOrg?.phone}</p>
            <p className="fs-6 mb-0">{newOrg?.siteUrl}</p>
          </Col>
        </Row>
      </Card.Body>
    </Card>
  );
}

OrganizationCard.propTypes = {
  newOrg: PropTypes.shape({
    id: PropTypes.number.isRequired,
    logo: PropTypes.string,
    headline: PropTypes.string,
    description: PropTypes.string,
    name: PropTypes.string.isRequired,
    phone: PropTypes.string,
    siteUrl: PropTypes.string,
  }),
};

export default OrganizationCard;
