import axios from 'axios';
import React, { Component } from 'react';
import './SearchResults.css'
import Catalog from '../Shared/Catalog';
import {BuildSearchBodyFromQuery} from '../../Scripts/SearchBuilder';

export default class SearchResults extends Component {

    constructor(props) {
        super(props);
        this.state = { complete: false, results: null };
    }

    componentDidMount() {
        document.title = "MTGCC - Search Results";
        this.performSearch();
    }

    render() {
        var display = <div>Searching...</div>;

        if (this.state.complete) {
            if (this.state.results == null)
                display = <div>No Results Found.</div>;
            else
                display = <Catalog cards={this.state.results} />;
        }

        return (
            <div className="container-fluid">
                {display}
            </div>
        );
    }

    async performSearch() {
        let json = null;

        let searchResults = this;

        axios.post('/api/search', json).then(function (response) {
            if (response.status == 200) {
                console.log("Search success");
                const data = response.data
                const cards = data.cards;
                searchResults.setState({ complete: true, results: cards });
            }
            else {
                searchResults.setState({ complete: true, results: null });
            }
        });
    }
}