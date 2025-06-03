namespace Uno.controllers;

using System.Security.Cryptography;
using System.Text.Json;
using Uno.Enum;
using Uno.Interfaces;
using Uno.models;
// using Uno.services;
// using Uno.utils;

public class GameController
{
    List<IPlayer> _players = new List<IPlayer>();
    Deck _drawPile;
    List<ICard> _discardPile = new List<ICard>();
    IPlayer _currentPlayer;
    Direction _currentDirection = Direction.Clockwise;
    CardColor _currentColor = CardColor.Wild;
    Action<IPlayer, ICard>? _onCardPlayed;
    Action<IPlayer>? _onGameOver;
    int _addCount;

    public GameController(List<IPlayer> players)
    {
        _players = players;
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
    public void SetupInitialDiscard()
    {
        _discardPile.Insert(0, _drawPile.Cards[0]);
        SetCurrentColor(_discardPile[0].Color);
    }
    public void ReverseDirection()
    {
        _currentDirection = _currentDirection == Direction.Clockwise ? Direction.CounterClockwise : Direction.Clockwise;
    }
    public void SkipPlayer()
    {

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
                DrawCardToPlayer((Player)_currentPlayer, _addCount);
                break;
        }
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
    public bool CheckValidMove(Card cardToPlay)
    {
        if (cardToPlay.Value == CardValue.WildDrawFour) return true;
        if (cardToPlay.Value == CardValue.DrawTwo && (_discardPile[0].Value == CardValue.DrawTwo || _currentColor == cardToPlay.Color)) return true;
        if (_currentColor == cardToPlay.Color || _discardPile[0].Value == cardToPlay.Value) return true;
        return false;
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
    }
}