using System;
using System.Collections.Generic;

namespace Card_Game {
    class Program {
        static void Main (string[] args) {
            Console.SetWindowSize (80, 20);

            Console.WriteLine("================================================================================" +
                              "||                     Welcome to the game of War!                            ||" +
                              "||                                                                            ||" +
                              "|| HOW TO PLAY:                                                               ||" +
                              "|| + Each of the two players is dealt one half of a shuffled deck of cards.   ||" +
                              "|| + Each turn, each player draws one card from their decks.                  ||" +
                              "|| + The player that drew the card with higher value gets both cards.         ||" +
                              "|| + Winning cards return to the winner's deck and get reshuffled.            ||" +
                              "|| + If there is a draw, the cards are thrown away.                           ||" +
                              "||                                                                            ||" +
                              "|| HOW TO WIN:                                                                ||" + 
                              "|| + The player that is runs out of cards first, wins.                        ||" +
                              "||                                                                            ||" +
                              "|| CONTROLS:                                                                  ||" +
                              "|| + Press enter in order to draw a new card.                                 ||" +
                              "||                                                                            ||" +
                              "||                              Have fun!                                     ||" +
                              "================================================================================");

            // Game loop
            while (true) {
                int totalMoves = 0;

                // Generate deck
                WarDeck mainDeck = new WarDeck ();
                mainDeck.generateDeck ();
                //mainDeck.shuffleDeck ();

                // Split deck into 2 (1 for each player)
                WarDeck playerDeck = new WarDeck ();
                WarDeck computerDeck = new WarDeck ();

                bool toggle = false;

                foreach (WarCard card in mainDeck.stack) {
                    if (toggle) {
                        playerDeck.stack.Add (card);
                    } else {
                        computerDeck.stack.Add (card);
                    }
                    toggle = !toggle;
                }

                // Draw loop
                while (!playerDeck.isEmpty () && !computerDeck.isEmpty ()) {
                    Console.ReadLine ();

                    // Each player draws a card
                    WarCard playerDraw = (WarCard) playerDeck.drawCard ();
                    WarCard computerDraw = (WarCard) computerDeck.drawCard ();
                    totalMoves++;

                    Console.WriteLine ("Player has drawn: {0} of {1}.\n", playerDraw.face, playerDraw.suite);
                    Console.WriteLine ("Computer has drawn: {0} of {1}.\n\n", computerDraw.face, computerDraw.suite);

                    if ((int) playerDraw.face > (int) computerDraw.face) {
                        Console.WriteLine ("The Player has won the cards.\nThe cards have been placed in your deck.\n\n");
                        playerDeck.placeInDeck (playerDraw, computerDraw);
                    } else if ((int) playerDraw.face < (int) computerDraw.face) {
                        Console.WriteLine ("The Computer has won the cards.\nThe cards have been placed in the computer's deck.\n\n");
                        computerDeck.placeInDeck (playerDraw, computerDraw);
                    } else {
                        Console.WriteLine ("It's a draw!\n\n");
                    }

                    Console.WriteLine ("================================================================================" +
                                       "================================================================================");
                }

                if (playerDeck.isEmpty ()) {
                    Console.WriteLine ("After a total of {0} moves, the player has won!\n\n", totalMoves);
                } else {
                    Console.WriteLine ("After a total of {0} moves, the computer has won!\n\n", totalMoves);
                }

                string line;
                do {
                    Console.WriteLine ("Would you like to play again?\nIf yes, type 'y'. If not, type 'n'.\n");
                    line = Console.ReadLine ();
                } while (line != "n" && line != "y");

                if (line == "n") {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// A standard playing card for the game of War.
    /// </summary>
    public class WarCard : Card {
        public WarCard (Suite s, Face f) : base (s, f) {
        }

        public override void printCard () {
            throw new NotImplementedException ();
        }
        public override int getFaceValue () {
            return ((int) suite);
        }
    }

    /// <summary>
    /// A standard deck for the game of War.
    /// </summary>
    public class WarDeck : Deck {
        public override void generateDeck () {
            // Creation of each card suite deck chunk
            for (int k = 0; k < 4; k++) {
                // Creation of the individual card
                for (int i = 1; i < 14; i++) {
                    stack.Add (new WarCard ((Card.Suite) k, (Card.Face) i));
                }
            }
        }
        public override void shuffleDeck () {
            Random rng = new Random ();

            for (int i = stack.Count - 1; i > 1; i--) {
                int k = rng.Next (i);

                WarCard v = (WarCard) stack [k];
                stack [k] = stack [i];
                stack [i] = v;

            }
        }
        public override Card drawCard () {
            WarCard popCard = (WarCard) stack [0];
            stack.Remove (popCard);

            return (popCard);
        }
        public override void placeInDeck (Card c1, Card c2) {
            stack.Add (c1);
            stack.Add (c2);
            shuffleDeck ();
        }
        public override bool isEmpty () {
            return (stack.Count <= 0);
        }
    }

    /// <summary>
    /// An abstract playing card.
    /// </summary>
    public abstract class Card {
        public enum Suite {Spades=0, Hearts, Clubs, Diamonds};
        public enum Face {Ace=1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King}

        public Suite suite;
        public Face face;

        public Card (Suite s, Face f) {
            this.suite = s;
            this.face = f;
        }

        public abstract void printCard ();
        public abstract int getFaceValue ();
    }

    /// <summary>
    /// An abstract card deck.
    /// </summary>
    public abstract class Deck {
        public List <Card> stack;

        public Deck () {
            this.stack = new List <Card> ();
        }

        public abstract void generateDeck ();
        public abstract void shuffleDeck ();
        public abstract Card drawCard ();
        public abstract void placeInDeck (Card c1, Card c2);
        public abstract bool isEmpty ();
    }
}