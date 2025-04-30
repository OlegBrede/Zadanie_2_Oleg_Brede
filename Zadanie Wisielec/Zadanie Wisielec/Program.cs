using System;

namespace Zadanie_Wisielec
{
    
    class WordBank
    {
        private static string[] ListaSłów = {"kot","pies","kurczak"};
        public static string RandomWordGet()
        {
            var RNG = new Random();
            int index = RNG.Next(ListaSłów.Length);
            return ListaSłów[index];
        }
    }
    class Game
    {
        //odsłanianie liter
        //limit błędów
        //stan gry
    }
    class Player
    {
        //punktacja ?
    }
    class Program
    {
        static void Main(string[] args)
        {
            string WybraneSłowo = WordBank.RandomWordGet();
            Console.WriteLine("Hello World!");
            Console.WriteLine("Wybrane słowo to .: " + WybraneSłowo);
        }
    }
}
