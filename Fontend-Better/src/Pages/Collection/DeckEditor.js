import React, { useContext } from 'react';
import DeckObject from '../../Scripts/DeckObject';
import { DeckContext } from './Collection';
import axios from 'axios';

export default function DeckEditor() {

    const deckObj = useContext(DeckContext);

    let cards = [];

    

    return (
        <div className="deck-editor">
            <Dropdown />
        </div>
    );
}

function Dropdown(props) {

    const decks = props.decks;

    let optionsJSX = [];

    decks.foreach(x => {
        optionsJSX.push(
            <a onClick={() => setActiveDeck(x.id)}>x.name</a>
        );
    });

    return (
        <div id="deck-editor-dropdown">
            {optionsJSX}
        </div>
    );
}

function setActiveDeck(deckID) {

}

function getLocalDecks() {
    axios.get("/api/deck/list").then((response) => {
        return response.data;
    }).catch((response) => {

    });
}