using System;

namespace Comp1003_MasterMind {
    class Program {

        private bool isDebugging = true; // This is for testing, shows code.

        private bool _isPlaying = true;
        private int _numColours = 8; // Num Colours -> M
        private int _numPositions = 6; // Num Positions -> N [ DO BEGORE SUBMIT ]

        private int[] _code;
        private int[] _playerGuess;

        private int _numBlack = 0;
        private int _numWhite = 0;

        static void Main(string[] args) {
            Program program = new Program();

            Console.WriteLine("Welcome to MasterMind!");

            program.Intro();

            while (program._isPlaying) {

                program.MakeAGuess();
                program.CheckGuess();
                program.DisplayResults();
            }

        }

        private void Intro() {

            Console.WriteLine("Please enter the number of colours:");
            bool success = int.TryParse(Console.ReadLine(), out _numColours);
            if (!success) { Console.WriteLine("Input Invalid"); Intro(); }

            Console.WriteLine("Please enter the number of positions:");
            success = int.TryParse(Console.ReadLine(), out _numPositions);
            if (!success) { Console.WriteLine("Input Invalid"); Intro(); }

            Random rnd = new Random(); int rndValu;
            _code = new int[_numPositions]; _playerGuess = new int[_numPositions]; 

            for (int i = 0; i < _numPositions; i++) {
                rndValu = rnd.Next(0, _numColours);
                _code[i] = rndValu;
            }
            Console.WriteLine();

            if (!isDebugging) { return; }

            Console.Write("The code is : ");
            foreach (int i in _code) {
                Console.Write(i);
            }
            Console.WriteLine();
        }

        private void MakeAGuess() {

            for (int codePos = 0; codePos < _numPositions; codePos++) {
                bool flag = false;
                while (!flag) {
                    Console.WriteLine("Code, Pos [ " + codePos + " ] :");
                    flag = int.TryParse(Console.ReadLine(), out _playerGuess[codePos]);
                    if (!flag) { Console.WriteLine("Input Invalid"); }
                }
            }

            Console.WriteLine(); Console.Write("Your entered code is : ");
            foreach (int i in _playerGuess) {
                Console.Write(i);
            }
            Console.WriteLine();

        }


        private void CheckGuess() {

            _numBlack = 0; _numWhite = 0;

            for (int i = 0; i < _numPositions; i++) {

                // Same pos and colour (num)
                if (_code[i] == _playerGuess[i]) { _numBlack++; }

                // Not same pos but the contains colour colour
                else if (myContains(_code, _playerGuess[i])) { _numWhite++; }

                // Not in code 
                else {  }
            }

        }

        private void DisplayResults() {
            
            Console.WriteLine("\n You recieved this result: \n");
            Console.Write("| Black : " + _numBlack);
            Console.Write(" | White : " + _numWhite + " | \n");

            Console.WriteLine();

            if (_numBlack == _numPositions) {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("CONGRATS YOU WON!");
                Console.ResetColor();
                _isPlaying = false;
            }
            else {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write("NOT QUITE, TRY AGAIN...");
                Console.ResetColor();
            }
            Console.WriteLine("\n");
        }


        public bool myContains(int[] array, int value) {
            foreach (int val in array) { if (val == value) { return true; } }
            return false;
        }

    }
}
