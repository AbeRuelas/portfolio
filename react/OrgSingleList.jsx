
function OrganizationSingleList() {
  const [orgArticle, setOrgArticle] = useState();
  const { id } = useParams();

  useEffect(() => {
    organizationService.selectById(id).then(onPageSuccess).catch(onPageError);
  }, [id]);

  const onPageSuccess = (response) => {
    setOrgArticle((prevState) => {
      return {
        ...prevState,
        ...response.item,
      };
    });
  };

  const onPageError = (error) => {
    _logger("onPageError", error);
  };

  return (
    <Container className="d-flex justify-content-center align-items-center mt-5 mb-5">
      <Row>
        <div className="mb-2 mt-2 text-end">
          <Link to="/organization/listings" className="btn btn-outline-white">
            Back to All Organizations
          </Link>
        </div>
        <Row>
          <h1 className="text-inherit fw-bold mb-4 text-center centered-headline">
            {orgArticle?.headline}
          </h1>

          {orgArticle?.logo && (
            <div className="d-flex justify-content-center">
              <Image
                variant="top"
                src={orgArticle?.logo}
                className="img-fluid mx-auto d-block mt-4 mb-4"
              />
            </div>
          )}
          <span className="fs-5 fw-semi-bold d-block mb-3 text-success">
            Organizations
          </span>
          <hr></hr>
          <h5
            className="text-secondary mb-1"
            dangerouslySetInnerHTML={{
              __html: orgArticle?.description,
            }}
          ></h5>
          <hr></hr>
        </Row>
        <Row className="align-items-center g-0 mt-4">
          <Col className="col lh-8">
            <h5 className="mb-1">{orgArticle?.name}</h5>
            <p className="fs-6 mb-0">{orgArticle?.phone}</p>
            <p className="fs-6 mb-0">{orgArticle?.siteUrl}</p>
          </Col>
        </Row>
      </Row>
    </Container>
  );
}

OrganizationSingleList.propTypes = {
  orgArticle: PropTypes.shape({
    id: PropTypes.number.isRequired,
    logo: PropTypes.string,
    headline: PropTypes.string,
    description: PropTypes.string,
    name: PropTypes.string.isRequired,
    phone: PropTypes.string,
    siteUrl: PropTypes.string,
  }),
};

export default OrganizationSingleList;
