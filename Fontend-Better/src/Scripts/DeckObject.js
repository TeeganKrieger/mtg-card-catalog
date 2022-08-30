import axios from 'axios';

export default class DeckObject
{

    constructor(deck)
    {
        this.id = deck.id;
        this.name = deck.name;
        this.cards = deck.cards;
    }
    
    get name() {
        return this.name;
    }

    get cards() {
        return this.cards;
    }

    addCard(card) {

        let id = this.id;
        let isOwned = false;
        let cardID = "";

        if (card.instanceID) {
            isOwned = true;
            cardID = card.cardData.id;
        } else {
            isOwned = false;
            cardID = card.id;
        }

        let json = {id, isOwned, cardID};

        axios.post("/api/deck/card/add", json).then((response) => {
            let newCard = response.data;
            this.cards.push(newCard);
        }).catch((response) => {

        });
    }

    removeCard(cardID) {

        let id = this.id;

        let json = {id, cardID};

        axios.post("/api/deck/card/remove", json).then((response) => {
            let removeCard = response.data;
            this.cards = this.cards.filter(x => x.id != cardID);
        }).catch((response) => {

        });
    }
}