import React, { useEffect, useState } from "react";
import "./RegisterLogin.css";
import { performLogin, performRegistration } from '../Scripts/LoginHelper';

export default function RegisterLogin(props) {

    const hidden = props.hidden;
    const setHidden = props.setHidden;

    const [mode, setMode] = useState("login");

    const [headerText, setHeaderText] = useState("Login");
    const [footerText, setFooterText] = useState("Not registered?");
    const [footerAnchor, setFooterAnchor] = useState("Sign up");

    useEffect(() => {

        if (hidden)
            closeModal();
        else
            openModal();

        return undefined;

    }, [hidden]);

    useEffect(() => {

        if (mode === "register") {
            setHeaderText("Register");
            setFooterText("Already registered?");
            setFooterAnchor("Sign in");
        } else {
            setHeaderText("Login");
            setFooterText("Not registered?");
            setFooterAnchor("Sign up");
        }

        return undefined;

    }, [mode]);

    return (
        <div id="register-login" className="rl-modal">
            <div className="rl-content">
                <span className="rl-title">{headerText}</span>
                <form className="rl-form" onSubmit={(event) => {onSubmitForm(event, mode, setHidden)}}>
                    <label>Username</label>
                    <input id="rl-username" type="text" />
                    <label>Password</label>
                    <input id="rl-password" type="password" />
                    <button>{headerText}</button>
                </form>
                <div className="rl-footer">
                    {footerText} <a href="#" onClick={swapMode.bind(this, mode, setMode)}>{footerAnchor}</a>
                </div>
            </div>
        </div>
    );
}

function swapMode(mode, setMode) {
    if (mode === "register")
        setMode("login");
    else
        setMode("register");
}

function onSubmitForm(e, mode, setHidden) {
    e.preventDefault();

    const username = document.getElementById('rl-username').value;
    const password = document.getElementById('rl-password').value;

    function success() {
        setHidden(true);
    }

    if (mode === "register")
        performRegistration(username, password, success);
    else
        performLogin(username, password, success);
}

function closeModal() {
    let modal = document.getElementById('register-login');
    modal.style.display = "none";
}

function openModal() {
    let modal = document.getElementById('register-login');
    modal.style.display = "block";
}