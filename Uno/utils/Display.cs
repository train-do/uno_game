using Uno.Interfaces;

namespace Uno.utils;

public class DisplayUtils
{
    public static void PrintDeck(IDeck deck)
    {
        foreach (var card in deck.Cards)
        {
            Console.WriteLine($"{card.Color} {card.Value}");
        }
    }

    public static void PrintCard(ICard card)
    {
        Console.WriteLine($"{card.Color} {card.Value}");
    }

    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
}
public class @Utils
{
//     public static void SerializeHandCard(CardColor color, CardValue value, int row)
//     {
//         switch (color)
//         {
//             case CardColor.Wild:
//                 Console.ForegroundColor = ConsoleColor.Black;
//                 Console.BackgroundColor = ConsoleColor.White;
//                 // Console.WriteLine("Wild");
//                 break;
//             case CardColor.Green:
//                 Console.BackgroundColor = ConsoleColor.Green;
//                 // Console.WriteLine("Green");
//                 break;
//             case CardColor.Blue:
//                 Console.BackgroundColor = ConsoleColor.Blue;
//                 // Console.WriteLine("Blue");
//                 break;
//             case CardColor.Yellow:
//                 Console.BackgroundColor = ConsoleColor.Yellow;
//                 // Console.WriteLine("Yellow");
//                 break;
//             case CardColor.Red:
//                 Console.BackgroundColor = ConsoleColor.Red;
//                 // Console.WriteLine("Red");
//                 break;
//             default:
//                 Console.BackgroundColor = ConsoleColor.Black;
//                 // Console.WriteLine("Black");
//                 break;
//         }
//         switch (row)
//         {
//             case 0:
//                 Console.Write($"┌{string.Concat(Enumerable.Repeat("-", 4))}┐");
//                 Console.ResetColor();
//                 break;
//             case 1:
//                 if ((int)value < 10)
//                 {
//                     Console.Write($"|{(int)value}{string.Concat(Enumerable.Repeat(" ", 3))}|");
//                 }
//                 else
//                 {
//                     Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//                 }
//                 Console.ResetColor();
//                 break;
//             case 2:
//                 if ((int)value < 10)
//                 {
//                     Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//                 }
//                 else
//                 {
//                     string symbol = value switch
//                     {
//                         CardValue.Skip => "🛇",
//                         CardValue.Reverse => "⟳",
//                         CardValue.DrawTwo => "+2",
//                         CardValue.Wild => "🏳️‍🌈",
//                         CardValue.WildDrawFour => "+4",
//                         _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid card value")
//                     };
//                     if (value == CardValue.Wild || value == CardValue.WildDrawFour || value == CardValue.DrawTwo)
//                     {
//                         Console.Write($"| {symbol} |");
//                     }
//                     else
//                     {
//                         Console.Write($"| {symbol}  |");
//                     }
//                 }
//                 Console.ResetColor();
//                 break;
//             case 3:
//                 if ((int)value < 10)
//                 {
//                     Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 3))}{(int)value}|");
//                 }
//                 else
//                 {
//                     Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//                 }
//                 Console.ResetColor();
//                 break;
//             case 4:
//                 Console.Write($"└{string.Concat(Enumerable.Repeat("-", 4))}┘");
//                 Console.ResetColor();
//                 break;
//             default:
//                 Console.ResetColor();
//                 break;
//         }
//         // if ((int)value < 9)
//         // {
//         //     Console.WriteLine($"┌{string.Concat(Enumerable.Repeat("-", 4))}┐");
//         //     Console.WriteLine($"|{(int)value}{string.Concat(Enumerable.Repeat(" ", 3))}|");
//         //     Console.WriteLine($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//         //     Console.WriteLine($"|{string.Concat(Enumerable.Repeat(" ", 3))}{(int)value}|");
//         //     Console.ResetColor();
//         // }
//         // else
//         // {
//         //     string symbol = value switch
//         //     {
//         //         CardValue.Skip => "🛇",
//         //         CardValue.Reverse => "⟳",
//         //         CardValue.DrawTwo => "+2",
//         //         CardValue.Wild => "🏳️‍🌈",
//         //         CardValue.WildDrawFour => "+4",
//         //         _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid card value")
//         //     };
//         //     Console.WriteLine($"┌{string.Concat(Enumerable.Repeat("-", 4))}┐");
//         //     Console.WriteLine($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//         //     if (value == CardValue.Wild || value == CardValue.WildDrawFour || value == CardValue.DrawTwo)
//         //     {
//         //         Console.WriteLine($"| {symbol} |");
//         //     }
//         //     else
//         //     {
//         //         Console.WriteLine($"| {symbol}  |");
//         //     }
//         //     Console.WriteLine($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
//         //     Console.WriteLine($"└{string.Concat(Enumerable.Repeat("-", 4))}┘");
//         //     Console.ResetColor();
//         // }
//     }
//     public static void DrawHandCard(List<Card> cards)
//     {
//         Console.WriteLine("\nHand Cards:");
//         for (int i = 0; i < 5; i++)
//         {
//             foreach (var item in cards)
//             {
//                 SerializeHandCard(item.Color, item.Value, i);
//             }
//             Console.WriteLine();
//         }
//     }
//     public static void DrawPileCard(Card card)
//     {
//         Console.WriteLine("\nPile Cards:");
//         for (int i = 0; i < 5; i++)
//         {
//             SerializeHandCard(card.Color, card.Value, i);
//             Console.WriteLine();
//         }
//     }
}