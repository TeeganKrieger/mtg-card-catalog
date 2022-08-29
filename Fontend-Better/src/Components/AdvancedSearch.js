import React, { useEffect, useState } from "react";
import "./AdvancedSearch.css";

export default function AdvancedSearch(props) {

    let id = props.id;

    return (
        <div id={id} className="as-modal as-hidden">
            <div className="as-header">
                <span className="as-header-text">Advanced Search Parameters</span>
            </div>
            <div className="as-body container-fluid">
                <div className="row">
                    
                </div>
            </div>
        </div>
    );
}