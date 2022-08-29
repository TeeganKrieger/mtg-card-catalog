import React, { Component } from 'react';
import './Home.css'

export default class Home extends Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        document.title = "MTGCC - Home"
    }

    render() {
        return (
            <div>
                Home
            </div>
        );
    }
}