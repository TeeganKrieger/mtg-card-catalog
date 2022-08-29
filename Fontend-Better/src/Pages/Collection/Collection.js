import React, { useState, useEffect, useRef, useContext } from 'react';
import './Collection.css'
import Catalog from '../Shared/Catalog';
import { GenerateDefaultSearchBody, IsEmptySearchBody } from '../../Scripts/SearchBuilder';
import axios from 'axios';
import { SessionContext } from '../../App';

const DeckContext = React.createContext();

export default function Collection() {

    const sessionContext = useContext(SessionContext);

    let [pageNumber, setPageNumber] = useState(0);
    let [pageCount, setPageCount] = useState(1);

    let searchBody = useRef(GenerateDefaultSearchBody());
    let [collection, setCollection] = useState([]);
    let [searchMode, setSearchMode] = useState(true);

    let searchState = { searchBody, searchMode, setSearchMode, setCollection, setPageNumber, setPageCount };

    useEffect(() => {
        if (sessionContext === false)
            window.location.href = "/";
        return undefined;
    }, [sessionContext]);

    useEffect(() => {
        performSearch(searchState);
        return undefined;
    }, [searchMode]);



    return (
        <div className="container-fluid">
            <div className="row">
                <form className="collection-search" onSubmit={(e) => onSubmitSearch(e, searchState)} autoComplete="off">
                    <input id="collection-search-input" onInput={(e) => { searchBody.current.nameQuery = e.target.value; }} className="collection-search-input" placeholder="Search for a card..." />
                    <a className="collection-search-link" onClick={() => performSearch(searchState)}>Search</a>
                    <a className="collection-advanced-link"></a>
                    <br />
                    <input id="collection-only-show-owned" onChange={(e) => onToggleSearchMode(e, searchState)} className="collection-only-show-owned" type="checkbox" defaultChecked={true} />
                    <label className="collection-search-label">Show Owned</label>
                </form>
            </div>
            <Catalog cards={collection} mode={searchMode} />
            <Pagination pageNumber={pageNumber} pageCount={pageCount} searchState={searchState} />
        </div>
    )
}

function Pagination(props) {

    const pageNumber = props.pageNumber;
    const pageCount = props.pageCount;
    const searchState = props.searchState;
    const searchBody = searchState.searchBody;

    function changePage(pageNum) {
        searchBody.current.page = pageNum;
        performSearch(searchState);
    }

    let pages = [];

    pages.push(<a key={-1} className="no-select" onClick={() => changePage(Math.max(0, pageNumber - 1))}>{"<<"}</a>);

    for (let i = Math.max(0, pageNumber - 5); i < Math.min(pageNumber + 5, pageCount); i++)
        if (i == pageNumber)
            pages.push(<a key={i} className="no-select active">{i + 1}</a>);
        else
            pages.push(<a key={i} className="no-select" onClick={() => changePage(i)}>{i + 1}</a>);

    pages.push(<a key={pageCount} className="no-select" onClick={() => changePage(Math.min(pageNumber + 1, pageCount - 1))}>{">>"}</a>);

    return (
        <div className="collection-pagination">
            {pages}
        </div>
    );
}

function onToggleSearchMode(e, searchState) {
    const setPageNumber = searchState.setPageNumber;
    const setCollection = searchState.setCollection;
    const setSearchMode = searchState.setSearchMode;
    const searchBody = searchState.searchBody;

    searchBody.current.page = 0;
    setPageNumber(0);
    setCollection([]);
    setSearchMode(e.target.checked);
}

function onSubmitSearch(e, searchState) {
    e.preventDefault();
    performSearch(searchState);
}

async function performSearch(searchState) {

    const searchBody = searchState.searchBody.current;
    const searchMode = searchState.searchMode;
    const setCollection = searchState.setCollection;
    const setPageNumber = searchState.setPageNumber;
    const setPageCount = searchState.setPageCount;

    let endpoint = "/api/collection/get?page=" + searchBody.page;
    let usePost = false;

    if (!IsEmptySearchBody(searchBody)) {
        endpoint = "/api/collection/search";
        usePost = true;
    }

    if (searchMode === false) {
        endpoint = "/api/search";
        usePost = true;
    }

    let request = null;

    if (usePost)
        request = axios.post(endpoint, searchBody);
    else
        request = axios.get(endpoint);

    request.then(function (response) {
        const data = response.data;
        setPageCount(() => data.pageCount);
        setPageNumber(() => data.page);
        setCollection(() => data.cards);
    }).catch(function (response) {

    });
}