import React, { Component } from 'react';
import './Card.css'
import axios from 'axios';

export default function Card(props) {

    const mode = props.mode;

    let card = null;

    if (mode === true)
        card = props.card.cardData;
    else
        card = props.card;

    const twoSided = card.faces != null;

    let jsx = null;

    if (twoSided) {
        const face1 = card.faces[0];
        const face2 = card.faces[1];

        jsx =
            <div className="flex-card card-two-sided" id={card.id} onClick={() => flipCard(card.id)} onContextMenu={(e) => onCardContextMenu(e, card.id)}>
                <div className="flip-icon"></div>
                <img className="front-side" src={face1.images.png} alt={face1.name} />
                <img className="back-side" src={face2.images.png} alt={face2.name} />
            </div>;
    } else {
        jsx =
            <div className="flex-card card-one-sided" id={card.id} onContextMenu={(e) => onCardContextMenu(e, card.id)}>
                <img className="one-side" src={card.images.png} alt={card.name} />
                <ContextMenu />
            </div>;
    }
    return jsx;

}

function ContextMenu(props) {
    return (
        <div className="card-context-menu">
            <button className="remove-from-collection"></button>
        </div>
    );
}

function flipCard(id) {
    const card = document.getElementById(id);
    if (card != null)
        card.classList.toggle("flipped");
}

function onCardContextMenu(e, id) {
    e.preventDefault();
    const card = document.getElementById(id);
    card.classList.toggle('show-context-menu');
}

function addCardToCollection(cardID) {
    axios.post()
}