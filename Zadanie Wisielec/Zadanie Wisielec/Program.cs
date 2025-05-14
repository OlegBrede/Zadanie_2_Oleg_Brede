using System;
using System.Collections.Generic;
using System.Linq;

namespace Zadanie_Wisielec
{
    enum GameState
    {
        Playing,
        Won,
        Lost
    }

    class WordBank
    {
        private static readonly string[] ListaSłów =
        {
            "słońce","onomatopeja","auto","eufemizm","aluzja",
            "oksymoron","pleonazm","wiatrak","rower","kredka",
            "kot","pies","kurczak","lampa","źdźbło","czapka"
        };

        public static string RandomWordGet()
        {
            var rng = new Random();
            int index = rng.Next(ListaSłów.Length);
            return ListaSłów[index];
        }
    }

    class Game
    {
        private readonly string secret;
        private readonly HashSet<char> correct = new HashSet<char>();
        private readonly HashSet<char> wrong = new HashSet<char>();
        private readonly int maxErrors = 10;

        private char[,] canvas;
        private int rows = 7, cols = 7;
        private List<Action> drawSteps;

        public GameState State
        {
            get
            {
                if (Errors >= maxErrors) return GameState.Lost;
                if (secret.All(c => correct.Contains(c))) return GameState.Won;
                return GameState.Playing;
            }
        }

        public int Errors { get { return wrong.Count; } }

        public Game(string word)
        {
            secret = word;
            // przygotuj canvas i kroki rysowania
            canvas = new char[rows, cols];
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

        public void Draw(int errors)
        {
            ClearCanvas();
            for (int i = 0; i < errors && i < drawSteps.Count; i++)
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
            canvas[1, 4] = '│';
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

        public string GetMaskedWord()
        {
            return string.Concat(secret.Select(c => correct.Contains(c) ? c : '_'));
        }

        public void Guess(char c)
        {
            if (State != GameState.Playing) return;
            if (c == '\0') { wrong.Add(c); return; }
            if (secret.Contains(c))
                correct.Add(c);
            else
                wrong.Add(c);
        }

        public IEnumerable<char> WrongGuesses() { return wrong; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string word = WordBank.RandomWordGet();
            Game game = new Game(word);

            while (game.State == GameState.Playing)
            {
                Console.Clear();
                Console.WriteLine("Pozostało prób: " + (10 - game.Errors));
                Console.WriteLine("Błędne litery: " + string.Join(", ", game.WrongGuesses()));
                game.Draw(game.Errors);

                Console.Write("Hasło: ");
                Console.WriteLine(game.GetMaskedWord());

                Console.Write("Podaj literę: ");
                string input = Console.ReadLine();
                char guess = '\0';
                if (!string.IsNullOrEmpty(input) && input.Length == 1)
                    guess = char.ToLower(input[0]);
                game.Guess(guess);
            }

            Console.Clear();
            game.Draw(game.Errors);
            if (game.State == GameState.Won)
                Console.WriteLine("Gratulacje! Odgadłeś: " + word);
            else
                Console.WriteLine("Przegrałeś. Hasło to: " + word);
        }
    }
}

