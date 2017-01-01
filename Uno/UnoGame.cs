using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno
{
    class UnoGame
    {
        public int numberOfPlayers { get; private set; }
        private List<List<Card>> hand;
        private List<Card> deck;
        public List<Card> discard { get; private set; }
        public int currentPlayer { get; private set; }
        public int prevPlayer { get; private set; }
        public string lastColor { get; private set; }

        public int drawCount { get; private set; }

        public bool isReversed { get; private set; }
        public bool isSkipped { get; private set; }
        public bool canPlay { get; private set; }
        public bool canDraw { get; private set; }
        public bool canChallenge { get; private set; }
        public bool canPass { get; private set; }

        public bool gameOver { get; private set; }
        public int winner { get; private set; }

        public UnoGame(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;

            hand = new List<List<Card>>();
            for (int i = 0; i < numberOfPlayers; i += 1)
            {
                hand.Add(new List<Card>());
            }

            deck = new List<Card>();
            discard = new List<Card>();
            string[] colors = { "Red", "Green", "Blue", "Yellow" };
            foreach (string color in colors)
            {
                deck.Add(new Card(color, "0"));
                for (int num = 1; num < 10; num += 1)
                {
                    deck.Add(new Card(color, num.ToString()));
                    deck.Add(new Card(color, num.ToString()));
                }
                deck.Add(new Card(color, "Skip"));
                deck.Add(new Card(color, "Skip"));
                deck.Add(new Card(color, "Reverse"));
                deck.Add(new Card(color, "Reverse"));
                deck.Add(new Card(color, "Draw2"));
                deck.Add(new Card(color, "Draw2"));
            }
            deck.Add(new Card("Wild", "Draw4"));
            deck.Add(new Card("Wild", "Draw4"));
            deck.Add(new Card("Wild", "Draw4"));
            deck.Add(new Card("Wild", "Draw4"));
            deck.Add(new Card("Wild", "Wild"));
            deck.Add(new Card("Wild", "Wild"));
            deck.Add(new Card("Wild", "Wild"));
            deck.Add(new Card("Wild", "Wild"));
            deck.Shuffle();

            for (currentPlayer = 0; currentPlayer < numberOfPlayers; currentPlayer += 1)
            {
                drawCount = 7;
                canDraw = true;
                Draw(currentPlayer);
            }

            int initialCardIdx = 0;
            while (!char.IsDigit(deck[initialCardIdx].symbol[0]))
                initialCardIdx += 1;
            discard.Add(deck[initialCardIdx]);
            deck.RemoveAt(initialCardIdx);

            isReversed = false;
            currentPlayer = 0;
            prevPlayer = 0;
            lastColor = discard.Last().color;
            canDraw = true;
            canPlay = true;
            canChallenge = false;
            canPass = false;
            drawCount = 0;

            gameOver = false;
        }    

        public List<Card> GetHand(int player)
        {
            return hand[player];
        }

        public int GetHandCount(int player)
        {
            return hand[player].Count;
        }

        public List<Card> Draw(int player)
        {
            if (player != currentPlayer || canDraw == false)
            {
                return null;
            }
            int cardCount = (drawCount > 0) ? drawCount : 1;
            if (cardCount == 1)
                canPass = true;
            if (deck.Count < cardCount)
            {
                Card last = discard.Last();
                discard.RemoveAt(discard.Count - 1);
                deck.AddRange(discard);
                deck.Shuffle();
                discard.Add(last);
            }

            List<Card> cards = new List<Card>();
            for (int cardIdx = 0; cardIdx < cardCount; cardIdx += 1)
            {
                cards.Add(deck[0]);
                deck.RemoveAt(0);
            }
            hand[player].AddRange(cards);
            canDraw = false;
            drawCount = 0;
            return cards;
        }

        public bool Play(int player, int cardIdx)
        {
            if (player != currentPlayer || canPlay == false)
            {
                return false;
            }
            Card card = hand[player][cardIdx];
            if (card.color.Equals(lastColor) || card.color.Equals("Wild") || card.symbol.Equals(discard.Last().symbol))
            {
                if (card.symbol.Equals("Skip"))
                {
                    isSkipped = true;
                }
                if (card.symbol.Equals("Reverse"))
                {
                    isReversed = !isReversed;
                }
                if (card.symbol.Equals("Draw2"))
                {
                    drawCount = 2;
                }
                if (card.color.Equals("Wild"))
                {
                    if (card.symbol.Equals("Draw4"))
                    {
                        canChallenge = true;
                        drawCount = 4;
                    }
                    if (card.color.Equals("Wild"))
                    {
                        //DO NOTHING
                    }
                }
                lastColor = card.color;
                discard.Add(card);
                hand[player].RemoveAt(cardIdx);
                canPlay = false;
                canDraw = false;
                if (hand[player].Count == 0)
                {
                    gameOver = true;
                    winner = player;
                }
                return true;
            }
            return false;
        }

        public bool Challenge(int player)
        {
            if (player != currentPlayer || canChallenge == false)
            {
                return false;
            }
            return true;
        }

        public void setWildColor(int player, string color)
        {
            if (player == currentPlayer && lastColor.Equals("Wild"))
            {
                lastColor = color;
            }       
        }

        public Card AutoPlay()
        {
            if (gameOver)
                return null;
            if (canPlay)
            {
                for (int idx = 0; idx < GetHand(currentPlayer).Count; idx++)
                    if (Play(currentPlayer, idx) && !gameOver)
                    {
                        if (lastColor.Equals("Wild"))
                        {
                            string wcolor = "Blue";
                            foreach (Card ccard in GetHand(currentPlayer))
                            {
                                if (!ccard.Equals("Wild"))
                                    wcolor = ccard.color;
                            }
                            setWildColor(currentPlayer, wcolor);
                        }
                        Next();
                        return discard.Last();
                    }
                Draw(currentPlayer);
                if (Play(currentPlayer, GetHand(currentPlayer).Count - 1))
                {
                    if (lastColor.Equals("Wild"))
                    {
                        string wcolor = "Blue";
                        foreach (Card ccard in GetHand(currentPlayer))
                        {
                            if (!ccard.Equals("Wild"))
                                wcolor = ccard.color;
                        }
                        setWildColor(currentPlayer, wcolor);
                    }
                    Next();
                    return discard.Last();
                }
                Pass(currentPlayer);
                Next();
                return null;
            }
            if (canDraw)
            {
                Draw(currentPlayer);
            }         
            Next();
            return null;
        }

        public bool Next()
        {
            if (gameOver)
                return false;
            if (!canDraw && !canPlay && !lastColor.Equals("Wild")) 
            {
                prevPlayer = currentPlayer;
                currentPlayer += isReversed ? -1 : 1;
                if (isSkipped)
                {
                    currentPlayer += isReversed ? -1 : 1;
                    isSkipped = false;
                }
                currentPlayer += numberOfPlayers;
                currentPlayer %= numberOfPlayers;

                canPlay = true;
                canDraw = true;
                canChallenge = false;
                canPass = false;
                if (drawCount != 0)
                {
                    canPlay = false;
                }
                return true;
            }
            return false;
        }

        public bool Pass(int player)
        {
            if (player != currentPlayer || canPass == false)
            {
                return false;
            }
            canPass = false;
            canPlay = false;
            return true;
        }
    }

    static class Utils
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
