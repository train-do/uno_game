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
    Action _onCardPlayed;
    Action<IPlayer>? _onGameOver;
    int _cardToDraw;
    bool isGameOver;

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
                _cardToDraw += 2;
                break;
            case CardValue.WildDrawFour:
                _cardToDraw += 4;
                break;
        }
    }
    public bool CheckValidMove(Card cardToPlay)
    {
        if (cardToPlay.Value == CardValue.WildDrawFour) return true;
        if (_cardToDraw == 0 && (_currentColor == cardToPlay.Color || cardToPlay.Color == CardColor.Wild || _discardPile[0].Value == cardToPlay.Value)) return true;
        if (_cardToDraw != 0 && _discardPile[0].Value == cardToPlay.Value) return true;
        // if (_cardToDraw != 0 && (_currentColor == cardToPlay.Color || cardToPlay.Color == CardColor.Wild) && _discardPile[0].Value == cardToPlay.Value) return true;
        // if (cardToPlay.Value == CardValue.DrawTwo && (_discardPile[0].Value == CardValue.DrawTwo || _currentColor == cardToPlay.Color)) return true;
        // if (_discardPile[0].Value != CardValue.DrawTwo && _discardPile[0].Value != CardValue.WildDrawFour && cardToPlay.Value == CardValue.Wild) return true;
        return false;
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
    public void HandleTurn()
    {
        Console.Clear();
        // var options = new JsonSerializerOptions
        // {
        //     Converters = { new JsonStringEnumConverter() },
        //     WriteIndented = true
        // };
        // Console.WriteLine(JsonSerializer.Serialize(_currentPlayer, options));
        // Console.WriteLine($"Current Color: {_currentColor}");
        // Console.WriteLine($"Current Card: {_discardPile[0]}");
        // Console.WriteLine($"Current Card To Draw: {_cardToDraw}");
        // Dashboard();
        _onCardPlayed.Invoke();
        Console.WriteLine();
        Display.DrawPileCard((Card)_discardPile[0]);
        Console.WriteLine($"{(_discardPile[0].Color == CardColor.Wild ? $"Color : {_currentColor}" : "")}\n");
        Console.WriteLine($"Hand Card Player {_currentPlayer.Name}");
        Display.DrawHandCard(_currentPlayer.Hand, GetPlayableCards());
        bool canPlay = CanPlayerPlay((Player)_currentPlayer);
        if (!canPlay)
        {
            Console.Write("No card can be played, press any key to continue (will draw card)");
            Console.ReadKey();
            DrawCardToPlayer((Player)_currentPlayer, _cardToDraw != 0 ? _cardToDraw : 1);
            _cardToDraw = 0;
            GetNextPlayer();
            return;
        }
        Console.Write("Choose a card (1, 2, ..., n) or type 'draw' : ");
        string? input = Console.ReadLine();
        string[] inputWwithUno = input.Split(" ");
        if (inputWwithUno[0]?.ToLower() == "draw")
        {
            DrawCardToPlayer((Player)_currentPlayer, _cardToDraw != 0 ? _cardToDraw : 1);
            _cardToDraw = 0;
            GetNextPlayer();
            return;
        }
        int cardIndex;
        if (int.TryParse(inputWwithUno[0], out cardIndex))
        {
            cardIndex -= 1;
            if (cardIndex >= 0 && cardIndex < _currentPlayer.Hand.Count)
            {
                Card selectedCard = (Card)_currentPlayer.Hand[cardIndex];
                if (CheckValidMove(selectedCard))
                {
                    RemoveCardFromPlayer(selectedCard);
                    InsertDiscardPile(selectedCard);
                    ApplyEffect();
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again. Press any key..");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Out Range. Press any key..");
                Console.ReadKey();
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Try again. Press any key..");
            Console.ReadKey();
            return;
        }
        HandleUnoCall(inputWwithUno);

        if (_currentPlayer.Hand.Count == 0)
        {
            isGameOver = true;
            return;
        }
        GetNextPlayer();
    }
    internal void StartGame()
    {
        Console.Write($"Masukkan jumlah pack kartu: ");
        string? packCard = Console.ReadLine();
        _drawPile = InitializeDeck(int.Parse(packCard ?? "1"));
        ShuffleDeck(_drawPile.Cards);
        foreach (IPlayer player in _players)
        {
            DrawCardToPlayer((Player)player, 1);
        }
        SetupInitialDiscard();
        IntializeTurn();
        _onCardPlayed = Dashboard;
        while (!isGameOver)
        {
            HandleTurn();
            RestockDrawPile();
        }
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
    // public void Dashboard1()
    // {
    //     string s1 = "";
    //     string s2 = "";
    //     string s3 = "";
    //     for (int i = 0; i < _players.Count; i++)
    //     {
    //         if (_players[i].Name == _currentPlayer.Name)
    //         {
    //             Console.BackgroundColor = ConsoleColor.DarkCyan;
    //         }
    //         string name = $"Name : {_players[i].Name}";
    //         Console.Write($"{name}{string.Concat(Enumerable.Repeat(" ", 20 - name.Length))}");
    //         Console.ResetColor();
    //         Console.Write("  ");
    //     }
    // }
    public void Dashboard()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        Console.SetCursorPosition(width * 2 / 5, 0);
        for (int i = 0; i < _players.Count; i++)
        {
            // Console.WriteLine($"{_players[i].Name == _currentPlayer.Name}{_players[i].Name} == {_currentPlayer.Name}");
            if (_players[i].Name == _currentPlayer.Name)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }
            string name = $"Name : {_players[i].Name}";
            Console.Write($"{name}{string.Concat(Enumerable.Repeat(" ", 20 - name.Length))}");
            Console.ResetColor();
            Console.Write("  ");
        }
        Console.WriteLine();
        int currentY = Console.CursorTop;
        Console.SetCursorPosition(width * 2 / 5, currentY);
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].Name == _currentPlayer.Name)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }
            string qty = $"Cards : {_players[i].Hand.Count}";
            Console.Write($"{qty}{string.Concat(Enumerable.Repeat(" ", 20 - qty.Length))}");
            Console.ResetColor();
            Console.Write("  ");
        }
        Console.WriteLine();
        currentY = Console.CursorTop;
        Console.SetCursorPosition(width * 2 / 5, currentY);
        Console.Write($"Direction : {(_currentDirection == Direction.Clockwise ? ">>>>" : "<<<<")}\n");
        // currentY = Console.CursorTop;
        // Console.SetCursorPosition(width * 2 / 5, currentY);
        Console.Write($"Discard Pile : {_discardPile.Count}    Draw Pile : {_drawPile.Cards.Count}{(_cardToDraw != 0 ? $"    Card to drawn : {_cardToDraw}" : "")}");
    }
    public bool CanPlayerCallUno()
    {
        return _currentPlayer.Hand.Count == 1 ? true : false;
    }
    public void HandleUnoCall(string[] input)
    {
        bool isUno = CanPlayerCallUno();
        // Console.WriteLine($"{input.Length <= 1 && isUno}  {input.Length} {isUno}");
        if (input.Length <= 1 && isUno)
        {
            DrawCardToPlayer((Player)_currentPlayer, 2);
            return;
        }
        if (input.Length <= 1) return;
        if ((isUno && input[1].ToLower() != "uno") || (!isUno && input[1].ToLower() == "uno"))
        {
            DrawCardToPlayer((Player)_currentPlayer, 2);
            return;
        }
    }
}