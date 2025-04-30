using System;
using System.Collections.Generic;

namespace Zadanie_Wisielec
{
    
    class WordBank
    {
        private static string[] ListaSłów = {"kot","pies","kurczak","słońce"}; // hasła muszą być małymi np ,"onomatopeja","auto","eufemizm","aluzja","oksymoron", "pleonazm","wiatrak","rower","kredka"
        public static string RandomWordGet()
        {
            var RNG = new Random();
            int index = RNG.Next(ListaSłów.Length);
            return ListaSłów[index];
        }
    }
    class Game
    {
        private char[,] canvas;
        private int rows = 7, cols = 7;
        private List<Action> drawSteps;

        public Game()
        {
            canvas = new char[rows, cols];
            // definiujemy kolejne kroki rysowania
            drawSteps = new List<Action>
            {
                DrawMainPost,
                DrawLeftSupport,
                DrawRightSupport,
                DrawCrane,
                DrawRope,
                DrawHead,
                DrawBody,
                DrawLeftArm,
                DrawRightArm,
                DrawLegs
            };
        }

        // metoda wywoływana przy każdej zmianie liczby błędów
        public void Draw(int Błędy)
        {
            ClearCanvas();
            // wykonaj kolejne kroki rysowania aż do liczby błędów
            for (int i = 0; i < Błędy && i < drawSteps.Count; i++)
                drawSteps[i]();
            Render();
        }

        private void ClearCanvas()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    canvas[r, c] = ' ';
        }

        private void Render()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                    Console.Write(canvas[r, c]);
                Console.WriteLine();
            }
        }
        private void DrawMainPost()
        {
            for (int r = 0; r < 6; r++) canvas[r, 1] = '│';
        }

        private void DrawLeftSupport()
        {
            canvas[6, 0] = '/';
        }

        private void DrawRightSupport()
        {
            canvas[6, 2] = '\\';
        }

        private void DrawCrane()
        {
            for (int c = 1; c < 5; c++) canvas[0, c] = '─';
            canvas[0, 1] = '┌'; canvas[0, 4] = '┐';
        }

        private void DrawRope()
        {
            for (int r = 1; r < 2; r++) canvas[r, 4] = '│';
        }

        private void DrawHead()
        {
            canvas[2, 4] = '0';
        }

        private void DrawBody()
        {
            canvas[3, 4] = '│';
        }

        private void DrawLeftArm()
        {
            canvas[3, 3] = '/';
        }

        private void DrawRightArm()
        {
            canvas[3, 5] = '\\';
        }

        private void DrawLegs()
        {
            canvas[4, 3] = '/'; canvas[4, 5] = '\\';
        }
        //odsłanianie liter
        //limit błędów
        //stan gry
    }

    class Player
    {
        public int Wygrane = 0;
        public int Przegrane = 0;
    }
    class Program
    {
    static int Błędy = 0;
    static bool exitBool = false;
        static void Main(string[] args)
        {
            var Gra = new Game();
            string WybraneSłowo = WordBank.RandomWordGet();
            List<char> OdgadnięteSłowa = new List<char>();
            string UsrInputString;      
            while (exitBool == false)
            {
                Console.Clear();

                int Wincheckint = 0;
                Console.WriteLine(""); // informacje dotyczące gry ogólnie
                Console.WriteLine($"Liczba prób .: {10 - Błędy}"); // informacje dotyczące tej rozgrywki
                Gra.Draw(Błędy);
                Console.Write("Litery hasła .: ");
                for (int i = 0; i < WybraneSłowo.Length; i++)// per każda litera hasła 
                {
                    char DanaLitera = WybraneSłowo[i]; //zamiana litery słowa na char
                    if (OdgadnięteSłowa.Contains(DanaLitera))// jeśli lista posiada dany char
                    {
                        Console.Write(DanaLitera);// zapisz te litere
                        Wincheckint++;// sprawdzamy czy gracz nie wygrał
                    }
                    else
                    {
                        Console.Write("_");// litera dalej ukryta
                    }      
                }
                if(Wincheckint == WybraneSłowo.Length)
                {
                    Console.Write("\nGRATULACJE !!! , ODGADŁEŚ HASŁO !!!");
                    WinCall(); //skoro break w tym wypadku działa to po co tu ta funkcja ? 
                    break;
                }
                Console.WriteLine("\nWpisz literę .:");
                UsrInputString = Console.ReadLine();
                UsrInputString = UsrInputString.ToLower(); // zamiana na małe
                if (UsrInputString.Length == 1)
                {
                    int Miss = 0;
                    char Lettr = UsrInputString[0]; // trzeba zamienić literę ze string na char 
                    for (int i = 0;i < WybraneSłowo.Length; i++) //per każda litera w słowie
                    {
                        if (Lettr == WybraneSłowo[i])//jeśli litera jest w wybranym słowie 
                        {
                            OdgadnięteSłowa.Add(Lettr);// jest taka litera w słowie
                            break;
                        }
                        else
                        {
                            Miss++; //dodaje sie miss
                        }
                        
                    }
                    if (Miss == WybraneSłowo.Length) //jeśli litera którą gracz wpisał missneła wszystkie litery danego słowa
                    {
                        ZaliczBłąd();
                    }
                }
                else
                {
                    ZaliczBłąd();
                }
            }   

        }
        static void WinCall()
        {
            exitBool = true;
        }
        static void ZaliczBłąd()
        {
            if (Błędy <= 9)
            {
                Błędy++;
            }
        }
    }
}
