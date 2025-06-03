using Uno.Enum;
using Uno.models;
using Uno.utils;

namespace Uno.views;

public class Display
{
    private static int width = Console.WindowWidth;
    private static int height = Console.WindowHeight;
    public static void SerializeHandCard(CardColor color, CardValue value, int row)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        switch (color)
        {
            case CardColor.Wild:
                Console.BackgroundColor = ConsoleColor.White;
                break;
            case CardColor.Green:
                Console.BackgroundColor = ConsoleColor.Green;
                break;
            case CardColor.Blue:
                Console.BackgroundColor = ConsoleColor.Blue;
                break;
            case CardColor.Yellow:
                Console.BackgroundColor = ConsoleColor.Yellow;
                break;
            case CardColor.Red:
                Console.BackgroundColor = ConsoleColor.Red;
                break;
            default:
                Console.BackgroundColor = ConsoleColor.Black;
                break;
        }
        switch (row)
        {
            case 0:
                Console.Write($"┌{string.Concat(Enumerable.Repeat("-", 4))}┐");
                Console.ResetColor();
                break;
            case 1:
                if ((int)value < 10)
                {
                    Console.Write($"|{(int)value}{string.Concat(Enumerable.Repeat(" ", 3))}|");
                }
                else
                {
                    Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
                }
                Console.ResetColor();
                break;
            case 2:
                if ((int)value < 10)
                {
                    Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
                }
                else
                {
                    string symbol = value switch
                    {
                        CardValue.Skip => "🛇",
                        CardValue.Reverse => "⟳",
                        CardValue.DrawTwo => "+2",
                        CardValue.Wild => "🏳️‍🌈",
                        CardValue.WildDrawFour => "+4",
                        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid card value")
                    };
                    if (value == CardValue.Wild || value == CardValue.WildDrawFour || value == CardValue.DrawTwo)
                    {
                        Console.Write($"| {symbol} |");
                    }
                    else
                    {
                        Console.Write($"| {symbol}  |");
                    }
                }
                Console.ResetColor();
                break;
            case 3:
                if ((int)value < 10)
                {
                    Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 3))}{(int)value}|");
                }
                else
                {
                    Console.Write($"|{string.Concat(Enumerable.Repeat(" ", 4))}|");
                }
                Console.ResetColor();
                break;
            case 4:
                Console.Write($"└{string.Concat(Enumerable.Repeat("-", 4))}┘");
                Console.ResetColor();
                break;
            default:
                Console.ResetColor();
                break;
        }
    }
    public static void DrawHandCard(List<Card> cards)
    {
        Console.WriteLine("\nHand Cards:");
        for (int i = 0; i < 5; i++)
        {
            foreach (var item in cards)
            {
                SerializeHandCard(item.Color, item.Value, i);
            }
            Console.WriteLine();
        }
    }
    public static void DrawPileCard(Card card)
    {
        Console.WriteLine("\nPile Cards:");
        for (int i = 0; i < 5; i++)
        {
            SerializeHandCard(card.Color, card.Value, i);
            Console.WriteLine();
        }
    }
    public static void DrawLoading()
    {
        Console.CursorVisible = false;


        string[] loadings = new string[]
        {
            "Loading",
            "Loading.",
            "Loading..",
            "Loading..."
        };
        foreach (var item in loadings)
        {
            Console.Clear();
            for (int i = 0; i < loadings.Length; i++)
            {
                Console.SetCursorPosition(width / 2, 0);
                Console.Write(loadings[i]);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
        }
    }
    public static void DrawWelcome()
    {
        Console.Clear();
        Console.CursorVisible = false;

        string[] welcomeMessage = new string[]
        {
            Frames.play3,
            Frames.play4,
            Frames.play5,
            Frames.play6
        };
        List<List<Card>> unoCards = Frames.unoCards;
        int i = 0;
        int j = 0;

        // Console.Clear();
        // if (i >= welcomeMessage.Length) i = 0;
        // if (j >= unoCards.Count) j = 0;
        // Console.SetCursorPosition(0, height / 4 - 5);
        // Console.Write(welcomeMessage[i] + "\n\n\n");
        // for (int k = 0; k < 5; k++)
        // {
        //     foreach (var card in unoCards[j])
        //     {
        //         SerializeHandCard(card.Color, card.Value, k);
        //     }
        //     Console.WriteLine();
        // }
        // Console.SetCursorPosition(width / 2, height - 5);
        // Console.Write($"Press any {Frames.key} to start...");
        // i++;
        // j++;
        // Thread.Sleep(400);
        // if (Console.KeyAvailable)
        // {
        //     Console.ReadKey(true);
        // }
        // Console.WriteLine($"{width} --- {height}");
        // Thread.Sleep(100);


        while (true)
        {
            Console.Clear();
            if (i >= welcomeMessage.Length) i = 0;
            if (j >= unoCards.Count) j = 0;
            Console.SetCursorPosition(0, height / 4 - 5);
            Console.Write(welcomeMessage[i] + "\n\n\n");
            for (int k = 0; k < 5; k++)
            {
                int currentY = Console.CursorTop;
                Console.SetCursorPosition(30, currentY);
                foreach (var card in unoCards[j])
                {
                    SerializeHandCard(card.Color, card.Value, k);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(width / 2, height - 7);
            Console.Write($"Press any {Frames.key} to start...");
            i++;
            j++;
            Thread.Sleep(300);
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                break;
            }
            // Console.SetCursorPosition(0, height / 4 - 5);
            // for (int s = 0; s < height; s++)
            // {
            //     Console.Write(new string(' ', width));
            // }
            Thread.Sleep(100);
        }
    }
}