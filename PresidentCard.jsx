import React from "react";


function PresidentCard(props) {
    
    const { aPrez } = props;
    console.log("aPrezObj:", aPrez);
    return (
        // id: 1,
        // president: 1,
        // nm: "George Washington",
        // pp: "None, Federalist",
        // tm: "1789-1797"
        < div className="col-md-4 mb-4" key={aPrez.id} >
            <div className="card p-3 shadow">
                <div className="card-body d-flex flex-column align-items-center">
                <img src="president portrait" className="card-img-top" alt="Portrait Here"></img>
                <div className="card-body">
                    <h5 className="cardId">President: {aPrez.president}</h5>
                    <h3 className="card-title" >{aPrez.nm}</h3>
                    <p className="card-text" > Party: {aPrez.pp}</p>
                </div>
                </div>
            </div>
        </div>   
    )
};

export default PresidentCard;
