// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Uno.controllers;
using Uno.Enum;
using Uno.Interfaces;
using Uno.models;
using Uno.utils;
using Uno.views;

// Console.WriteLine("Hello, World!");
// Console.BackgroundColor = ConsoleColor.Red;
// string reverse = @"
// ⠀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⠀
// ⢸⣿⡿⠋⢹⣿⣿⣿⣿⡿⠿⠿⠿⠿⣿⡇
// ⢸⡿⠗⣩⣿⣿⡿⢋⣥⣶⣿⣿⣿⣷⣌⠃
// ⢸⣷⣶⣿⡿⢫⣶⣿⣿⣿⣿⣿⣿⣿⣿⡄
// ⢸⣿⣿⠏⣴⣿⣿⣿⠿⠿⠿⣿⣿⣿⣿⡇
// ⢸⣿⢃⣾⣿⣿⣿⠿⠃⠀⠀⣿⣿⣿⣿⠁
// ⢸⢃⣾⣿⣿⣿⡇⢀⠠⢾⣾⣿⣿⣿⡏⡄
// ⠘⣸⣿⣿⣿⠿⡷⠊⠀⣸⣿⣿⣿⡿⢡⡇
// ⢀⣿⣿⣿⣿⠀⠀⢠⣾⣿⣿⣿⡿⣡⣿⡇
// ⢸⣿⣿⣿⣿⣶⣶⣾⣿⣿⣿⠟⣵⣿⣿⡇
// ⠘⣿⣿⣿⣿⣿⣿⣿⣿⡿⣣⣾⣿⣿⣿⡇
// ⢠⡙⢿⣿⣿⣿⠿⣛⣵⣾⣿⣿⡟⢡⣸⡇
// ⢸⣿⣶⣶⣶⣶⣿⣿⣿⣿⣿⡏⠡⣴⣿⡇
// ⠀⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠁";
// Console.WriteLine(reverse);
// Console.ResetColor();
// Console.WriteLine("🛇");


// Card card1 = new Card(CardColor.Red, CardValue.Zero);
// Card card2 = new Card(CardColor.Green, CardValue.One);
// Card card3 = new Card(CardColor.Blue, CardValue.Two);
// Card card4 = new Card(CardColor.Yellow, CardValue.Three);
// Card card5 = new Card(CardColor.Wild, CardValue.Wild);
// Card card6 = new Card(CardColor.Wild, CardValue.WildDrawFour);
// Card card7 = new Card(CardColor.Red, CardValue.Reverse);
// Card card8 = new Card(CardColor.Green, CardValue.DrawTwo);
// Card card9 = new Card(CardColor.Blue, CardValue.Skip);
// List<Card> cards = new List<Card>
// {
//     card1,
//     card2,
//     card3,
//     card4,
//     card5,
//     card6,
//     card7,
//     card8
// };
// Console.WriteLine(cards.Find((card) => card.Value == CardValue.WildDrawFour).Value);
// Console.WriteLine(cards[2].Value.GetType());
// Display.DrawWelcome();
// Display.DrawLoading();
// Display.DrawPileCard(card9);
// Display.DrawHandCard(cards);
// int setCards = 1;
// int cards = 108;

// Console.WriteLine(Frames.play99.Length);
// Console.WriteLine(Enum.GetValues(typeof(CardColor)).Length);
// Console.WriteLine(Enum.GetValues(typeof(CardValue)).Length);

// Console.Write("\u001b[1;33m");
// Console.WriteLine("Welcome to the Uno Game!");
// Console.WriteLine("Press any key to start...");
// string? key = Console.ReadLine();
// Console.ResetColor();
// Console.Write("\u001b[48;2;255;0;0m");
// Console.Write("Starting the game...");
// Console.ResetColor();
// Console.WriteLine($"Game started!{key}");
// + GameController(List<IPlayer> player)
// + StartGame()
// + InitializeDeck()
// + ShuffleDeck(Deck drawPile)
// + DrawCardToPlayer(Player player, int count)
// + RemoveCardFromPlayer(Card card)
// + SetupInitialDiscard()
// + HandleTurn()
// + CanPlayerCallUno(Player player)
// + HandleUnoCall(Player caller)
// + ApplyEffect()
// + IsDrawPileEmpty()
// + RestockDrawPile()
// + SkipPlayer()
// + ReverseDirection()
// + SetCurrentColor(CardColor color)
// + CheckGameOver()
// + GetNextPlayer()
// + PromptForColorChoice()
// + CheckValidMove(Card cardToPlay)
// + CanPlayerPlay(Player player)



// Deck deck = InitializeDeck();
// Console.WriteLine($"Deck initialized with {setCards} set : " + deck.Cards.Count + " cards.");
// int cardsCount = 1;
// foreach (var card in deck.Cards)
// {
//     Console.WriteLine($"{cardsCount}. {card.Color} - {card.Value}");
//     cardsCount++;
// }
// cardsCount = 1;
// ShuffleDeck(deck.Cards);
// foreach (var card in deck.Cards)
// {
//     Console.WriteLine($"{cardsCount}. {card.Color} - {card.Value}");
//     cardsCount++;
// }


// Deck InitializeDeck()
// {
//     Deck deck = new Deck(new List<ICard>());
//     foreach (CardValue item in CardValue.GetValues(typeof(CardValue)))
//     {
//         switch (item)
//         {
//             case CardValue.Wild:
//             case CardValue.WildDrawFour:
//                 for (int i = 0; i < (4 * setCards); i++)
//                 {
//                     deck.Cards.Add(new Card(CardColor.Wild, item));
//                 }
//                 break;
//             case CardValue.Zero:
//                 for (int i = 0; i < (1 * setCards); i++)
//                 {
//                     deck.Cards.Add(new Card(CardColor.Red, item));
//                     deck.Cards.Add(new Card(CardColor.Green, item));
//                     deck.Cards.Add(new Card(CardColor.Blue, item));
//                     deck.Cards.Add(new Card(CardColor.Yellow, item));
//                 }
//                 break;
//             default:
//                 for (int i = 0; i < (2 * setCards); i++)
//                 {
//                     deck.Cards.Add(new Card(CardColor.Red, item));
//                     deck.Cards.Add(new Card(CardColor.Green, item));
//                     deck.Cards.Add(new Card(CardColor.Blue, item));
//                     deck.Cards.Add(new Card(CardColor.Yellow, item));
//                 }
//                 break;
//         }
//     }
//     return deck;
// }
// void ShuffleDeck(List<ICard> deck)
// {
//     // int i = 15;
//     for (int i = 0; i < deck.Count
// ; i++)
//     {
//         int n = RandomNumberGenerator.GetInt32(0, 100);
//         (deck[i], deck[n]) = (deck[n], deck[i]);
//     }
//     Console.WriteLine("Deck shuffled.");
// }


Display.DrawWelcome();
Display.DrawLoading();
List<IPlayer> players = new List<IPlayer>();
Console.Clear();
Console.WriteLine("Welcome to the Uno Game!");
Console.WriteLine("1. 2-Player Game");
Console.WriteLine("2. 3-Player Game");
Console.WriteLine("3. 4-Player Game");
Console.Write("Pilih Jumlah Pemain (1-3): ");
string? manyPlayer = Console.ReadLine();
for (int i = 0; i <= int.Parse(manyPlayer); i++)
{
    Console.Write($"Masukkan nama Player{i + 1}: ");
    string? playerName = Console.ReadLine();
    players.Add(new Player(playerName));
}
GameController gameController = new GameController(players);
// Console.BackgroundColor = ConsoleColor.DarkMagenta;
gameController.StartGame();
while (true)
{
    gameController.HandleTurn();
    gameController.GetNextPlayer();
}
// Console.WriteLine($"Game Over! {gameController.GetCurretColor()} is the current color.");
// // gameController.SetWildCard();
// gameController.ApplyEffect();
// Console.WriteLine($"Game Over! {gameController.GetCurretColor()} is the current color.");