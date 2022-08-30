import React, { Component, useEffect, useState } from 'react';
import './App.css'
import Navbar from './Components/Navbar';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './Pages/Home/Home';
import {Collection} from './Pages/Collection/Collection';
import SearchResults from './Pages/SearchResults/SearchResults';
import { initializeLoginHelper, performCookieLogin } from './Scripts/LoginHelper';

const SessionContext = React.createContext(false);

function App() {

     let [loginState, setLoginState] = useState(performCookieLogin());

     useEffect(() => {
         initializeLoginHelper(setLoginState);
         return undefined;
     }, []);

    return (
        <div style={{ height: "100%" }}>
            <SessionContext.Provider value={loginState}>
                <Navbar/>
                <BrowserRouter>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/collection" element={<Collection />} />
                        <Route path="/search" element={<SearchResults />} />
                    </Routes>
                </BrowserRouter>
            </SessionContext.Provider>
        </div>
    );
}

export {SessionContext, App};