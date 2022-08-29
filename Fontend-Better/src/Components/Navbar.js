import React, { useEffect, useState, useContext } from "react";
import "./Navbar.css";
import HamburgerIcon from "../Assets/Hamburger.png"
import RegisterLogin from "./RegisterLogin";
import { performLogout } from '../Scripts/LoginHelper';
import { SessionContext } from "../App";

export default function Navbar() {

    const sessionContext = useContext(SessionContext);

    const [hideLoginModal, setHideLoginModal] = useState(true);

    const loginButton = <a className="nav-link nav-only-mobile" onClick={() => setHideLoginModal(false)}>Login</a>;

    const logoutButton = <a className="nav-link nav-only-mobile" onClick={() => performLogout()}>Logout</a>;

    const [loginLogoutJSX, setLoginLogoutJSX] = useState(loginButton);

    useEffect(() => {
        if (sessionContext)
            setLoginLogoutJSX(logoutButton);
        else
            setLoginLogoutJSX(loginButton);
    }, [sessionContext]);

    useEffect(() => {

        function hideLoginModal(event) {
            if (event.target.id === "register-login") {
                setHideLoginModal(true);
            }
        };

        window.addEventListener("click", hideLoginModal);

        return () => {
            window.removeEventListener("click", hideLoginModal);
        };

    }, []);

    return (
        <nav id="navbar" className="navbar">
            <a className="no-select nav-link" onClick={() => handlePageChange("/")}>Home</a>
            <a className="no-select nav-link" onClick={() => handlePageChange("/Collection")}>Collection</a>
            <a className="no-select nav-link" onClick={() => handlePageChange("/Decks")}>Decks</a>
            {/* <form className="nav-search" onSubmit={onSubmitSearch.bind(this)} autoComplete="off">
                <input id="nav-search-input" className="nav-search-input" placeholder="Search for a card..." />
                <a className="nav-search-link" onClick={handleSubmitSearch.bind(this)}>Search</a>
                <a className="nav-advanced-link"></a>
            </form> */}
            <a className="nav-burger" onClick={() => clickHamburger()}>
                <img src={HamburgerIcon} />
            </a>
            {loginLogoutJSX}
            <RegisterLogin hidden={hideLoginModal} setHidden={setHideLoginModal} />
        </nav>
    );
}

function clickHamburger() {
    var nav = document.getElementById("navbar");
    nav.classList.toggle("nav-responsive");
}

// function onSubmitSearch(e) {
//     e.preventDefault();
//     handleSubmitSearch();
// }

// function handleSubmitSearch() {
//     let search = document.getElementById("nav-search-input");
//     let nameText = search.value;
//     let searchBody = GenerateDefaultSearchBody();
//     searchBody.name = nameText;
//     let query = BuildSearchQuery(searchBody);
//     window.location.href = "/search?" + query;
// }

function handlePageChange(page) {
    window.location.href = page;
}