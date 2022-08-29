import Cookies from 'js-cookie';
import axios from 'axios';

let setLoginState = null;

function performCookieLogin() {
    let session = Cookies.get('session');
    if (session !== undefined) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + session;
        return true;
    } else {
        return false
    }
}

function initializeLoginHelper(setter) {
    setLoginState = setter;
}

function performRegistration(username, password, successCallback, failureCallback) {

    let json = { username, password };

    axios.post("/api/session/register", json).then(function (response) {
        console.log("Registered and logged in!");
        let responseData = response.data;
        let sessionToken = responseData.token;

        Cookies.set('session', sessionToken);
        axios.defaults.headers.common['Authorization'] = "Bearer " + sessionToken;

        setLoginState(true);

        if (successCallback !== undefined)
            successCallback();
    }).catch(function (response) {
        if (failureCallback !== undefined)
            failureCallback();
    });
}

function performLogin(username, password, successCallback, failureCallback) {

    let json = { username, password };

    axios.post("/api/session/login", json).then(function (response) {
        console.log("Logged in!");
        let responseData = response.data;
        let sessionToken = responseData.token;

        Cookies.set('session', sessionToken);
        axios.defaults.headers.common['Authorization'] = "Bearer " + sessionToken;

        setLoginState(true);

        if (successCallback !== undefined)
            successCallback();
    }).catch(function (response) {
        if (failureCallback !== undefined)
            failureCallback();
    });
}

function performLogout(successCallback, failureCallback) {

    axios.post("/api/session/logout").then(function (response) {
        console.log("Logged out!");

        Cookies.remove('session');
        axios.defaults.headers.common['Authorization'] = undefined;

        setLoginState(false);

        if (successCallback !== undefined)
            successCallback();

    }).catch(function (response) {
        console.log("Logged out!");

        Cookies.remove('session');
        axios.defaults.headers.common['Authorization'] = undefined;

        setLoginState(false);

        if (failureCallback !== undefined)
            failureCallback();
    });
}

export { performCookieLogin, initializeLoginHelper, performRegistration, performLogin, performLogout };