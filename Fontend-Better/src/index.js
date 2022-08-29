import React from 'react';
import { createRoot } from 'react-dom/client';
import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
import { App } from './App';
import reportWebVitals from './Scripts/reportWebVitals';

const container = document.getElementById('root');
const root = createRoot(container);

const backgroundNum = Math.floor(Math.random() * 4) + 1;

document.getElementById('html').classList.add('background-' + backgroundNum);

root.render(
    <App />
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
