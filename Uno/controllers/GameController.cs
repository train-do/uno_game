namespace Uno.controllers;

using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Uno.Enum;
using Uno.Interfaces;
using Uno.models;
using Uno.views;

// using Uno.services;
// using Uno.utils;

public class GameController
{
    List<IPlayer> _players = new List<IPlayer>();
    Deck _drawPile;
    List<ICard> _discardPile = new List<ICard>();
    IPlayer _currentPlayer;
    Direction _currentDirection = Direction.Clockwise;
    CardColor _currentColor;
    Action<IPlayer, ICard>? _onCardPlayed;
    Action<IPlayer>? _onGameOver;
    int _cardToDraw;
    bool _specialEffect;

    public GameController(List<IPlayer> players)
    {
        _players = players;
    }

    public void DrawCardToPlayer(Player player, int count)
    {
        for (int i = 0; i < count; i++)
        {
            player.Hand.Add(_drawPile.Cards[0]);
            _drawPile.Cards.RemoveAt(0);
        }
    }
    public void RemoveCardFromPlayer(Card card)
    {
        foreach (ICard item in _currentPlayer.Hand)
        {
            if (item.Color == card.Color && item.Value == card.Value)
            {
                _currentPlayer.Hand.Remove(item);
                break;
            }
        }
    }
    public void ReverseDirection()
    {
        _currentDirection = _currentDirection == Direction.Clockwise ? Direction.CounterClockwise : Direction.Clockwise;
    }
    public void SkipPlayer()
    {
        GetNextPlayer();
    }
    public CardColor PromptForColorChoice()
    {
        Console.Write("Choose a color(Red, Green, Blue, Yellow) : ");
        string? input = Console.ReadLine();
        if (System.Enum.TryParse<CardColor>(input, true, out CardColor color))
        {
            return color;
        }
        else
        {
            Console.WriteLine("Invalid color choice");
            return PromptForColorChoice();
        }
    }
    public void SetCurrentColor(CardColor color)
    {
        _currentColor = color;
    }
    public bool CanPlayerPlay(Player player)
    {
        foreach (Card card in player.Hand)
        {
            if (CheckValidMove(card)) return true;
        }
        return false;
    }
    public void ApplyEffect()
    {
        switch (_discardPile[0].Value)
        {
            case CardValue.Skip:
                SkipPlayer();
                break;
            case CardValue.Reverse:
                ReverseDirection();
                break;
            case CardValue.DrawTwo:
            case CardValue.WildDrawFour:
                DrawCardToPlayer((Player)_currentPlayer, _cardToDraw);
                break;
        }
    }
    public bool CheckValidMove(Card cardToPlay)
    {
        if (_specialEffect)
        {
            
        }
        if (cardToPlay.Value == CardValue.WildDrawFour) return true;
        if (cardToPlay.Value == CardValue.DrawTwo && (_discardPile[0].Value == CardValue.DrawTwo || _currentColor == cardToPlay.Color)) return true;
        if (_discardPile[0].Value != CardValue.DrawTwo && _discardPile[0].Value != CardValue.WildDrawFour && cardToPlay.Value == CardValue.Wild) return true;
        if (_currentColor == cardToPlay.Color || _discardPile[0].Value == cardToPlay.Value) return true;
        return false;
    }
    public void GetNextPlayer()
    {
        int currentIndex = _players.IndexOf(_currentPlayer);
        if (_currentDirection == Direction.Clockwise)
        {
            currentIndex = (currentIndex + 1) % _players.Count;
        }
        else
        {
            currentIndex = (currentIndex - 1 + _players.Count) % _players.Count;
        }
        _currentPlayer = _players[currentIndex];
    }
    public List<int> GetPlayableCards()
    {
        List<int> playableCards = new List<int>();
        for (int i = 0; i < _currentPlayer.Hand.Count; i++)
        {
            Card card = (Card)_currentPlayer.Hand[i];
            if (CheckValidMove(card))
            {
                playableCards.Add(i);
            }
        }
        return playableCards;
    }
    public void HandleTurn()
    {
        // Console.Clear();
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };
        Console.WriteLine(JsonSerializer.Serialize(_currentPlayer, options));
        Console.WriteLine($"Current Color: {_currentColor}");
        Console.WriteLine($"Current Card: {_discardPile[0]}");
        // Console.WriteLine("Your Hand:");
        Display.DrawPileCard((Card)_discardPile[0]);
        Display.DrawHandCard(_currentPlayer.Hand, GetPlayableCards());
        Console.Write("Choose a card (1, 2, ..., n) or type 'draw' to draw a card: ");

        string? input = Console.ReadLine();
        if (input?.ToLower() == "draw")
        {
            DrawCardToPlayer((Player)_currentPlayer, 1);
            return;
        }
        int cardIndex;
        if (int.TryParse(input, out cardIndex))
        {
            cardIndex -= 1;
            if (cardIndex >= 0 && cardIndex < _currentPlayer.Hand.Count)
            {
                Card selectedCard = (Card)_currentPlayer.Hand[cardIndex];
                if (CheckValidMove(selectedCard))
                {
                    RemoveCardFromPlayer(selectedCard);
                    ApplyEffect();
                    InsertDiscardPile(selectedCard);
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Out Range.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Try again.");
        }
    }
    internal void StartGame()
    {
        Console.Write($"Masukkan jumlah pack kartu: ");
        string? packCard = Console.ReadLine();
        _drawPile = InitializeDeck(int.Parse(packCard ?? "1"));
        ShuffleDeck(_drawPile.Cards);
        foreach (IPlayer player in _players)
        {
            DrawCardToPlayer((Player)player, 7);
        }
        SetupInitialDiscard();
        IntializeTurn();
        // Console.WriteLine("Game started!");
        // Console.WriteLine($"{JsonSerializer.Serialize(_currentPlayer)}, {JsonSerializer.Serialize(_currentColor)}, {JsonSerializer.Serialize(_discardPile[0])}, {JsonSerializer.Serialize(_currentDirection)}, {JsonSerializer.Serialize(_players)}, {_drawPile.Cards.Count} cards in draw pile.");
    }
    public void IntializeTurn()
    {
        _currentPlayer = _players[RandomNumberGenerator.GetInt32(0, _players.Count)];
    }
    public Deck InitializeDeck(int packCard)
    {
        Deck deck = new Deck(new List<ICard>());
        foreach (CardValue item in System.Enum.GetValues(typeof(CardValue)))
        {
            switch (item)
            {
                case CardValue.Wild:
                case CardValue.WildDrawFour:
                    for (int i = 0; i < (4 * packCard); i++)
                    {
                        deck.Cards.Add(new Card(CardColor.Wild, item));
                    }
                    break;
                case CardValue.Zero:
                    for (int i = 0; i < (1 * packCard); i++)
                    {
                        deck.Cards.Add(new Card(CardColor.Red, item));
                        deck.Cards.Add(new Card(CardColor.Green, item));
                        deck.Cards.Add(new Card(CardColor.Blue, item));
                        deck.Cards.Add(new Card(CardColor.Yellow, item));
                    }
                    break;
                default:
                    for (int i = 0; i < (2 * packCard); i++)
                    {
                        deck.Cards.Add(new Card(CardColor.Red, item));
                        deck.Cards.Add(new Card(CardColor.Green, item));
                        deck.Cards.Add(new Card(CardColor.Blue, item));
                        deck.Cards.Add(new Card(CardColor.Yellow, item));
                    }
                    break;
            }
        }
        return deck;
    }
    public void ShuffleDeck(List<ICard> deck)
    {
        for (int i = 0; i < deck.Count
    ; i++)
        {
            int n = RandomNumberGenerator.GetInt32(0, 100);
            (deck[i], deck[n]) = (deck[n], deck[i]);
        }
        Console.WriteLine("Deck shuffled.");
    }
    public void SetupInitialDiscard()
    {
        InsertDiscardPile((Card)_drawPile.Cards[0]);
    }
    public void InsertDiscardPile(Card card)
    {
        _discardPile.Insert(0, card);
        if (_discardPile[0].Value == CardValue.Wild || _discardPile[0].Value == CardValue.WildDrawFour)
        {
            SetCurrentColor(PromptForColorChoice());
            return;
        }
        SetCurrentColor(_discardPile[0].Color);
    }
    public void RestockDrawPile()
    {
        if (_drawPile.Cards.Count <= 30)
        {
            List<ICard> tempDiscard = _discardPile.Skip(1).ToList();
            ShuffleDeck(tempDiscard);
            _drawPile.Cards.AddRange(tempDiscard);
            _discardPile.RemoveRange(1, tempDiscard.Count);
            Console.WriteLine("Draw pile restocked from discard pile.");
        }
    }
    // public void SetWildCard()
    // {
    //     // Console.Clear();
    //     Console.WriteLine(_drawPile.Cards.FindIndex((card) => card.Value == CardValue.Wild || card.Value == CardValue.WildDrawFour));
    //     Console.WriteLine(_drawPile.Cards.Find((card) => card.Value == CardValue.Wild || card.Value == CardValue.WildDrawFour));
    //     _drawPile.Cards.FindIndex((card) => card.Value == CardValue.Wild || card.Value == CardValue.WildDrawFour);
    //     _discardPile.Insert(0, _drawPile.Cards.Find((card) => card.Value == CardValue.Wild || card.Value == CardValue.WildDrawFour));
    // }
    public CardColor GetCurretColor()
    {
        return _currentColor;
    }
}