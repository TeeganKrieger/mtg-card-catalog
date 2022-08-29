import React, { Component } from 'react';
import './Catalog.css'
import Card from './Card';

export default function Catalog(props) {

    const mode = props.mode;
    const rawCards = props.cards;

    let cards = [];
    let count = 0;
    if (rawCards)
    rawCards.forEach(x => cards.push(<Card key={count++} card={x} mode={mode} />));

    return (
        <div className="catalog">
            {cards}
        </div>
    );
}