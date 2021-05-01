using System;

namespace Comp1003_MasterMind {

    class CustomQueue {

        public int QueueLength;
        public int QPos;
        public int[] Data;

        /// <summary>
        ///
        /// </summary>
        public CustomQueue(int QLength) { Data = new int[QLength]; QueueLength = QLength; QPos = -1; }

        /// <summary>
        ///
        /// </summary>
        public void Add(int val) {
            if (QPos == QueueLength - 1) { } // END OF QUEUE  }
          
            else { QPos++;  Data[QPos] = val;  }  // Increment by 1 and set the next value. 
        }

        /*
        public void Remove() {
            if (QPos <= -1) { } // At the begining of q, cant delete.   }

            else {
                // Shuffle - Moving everything back one, Also removing the first item in the queue.
                // Iterate through each value in the queue soo far. Shifting each back one.
                for (int i = 0; i < QPos; i++) {
                    Data[i] = Data[i + 1]; // Shifts back 1.
                }
            }
        }*/
        
        public string Output() {
            string output = "";

            // Loop up to the current queue length and add each element to the string 
            for (int i = 0; i <= QPos; i++) { output += Data[i]; }
            return (output);
        }
    }

    class Program {

        private bool isDebugging = true; // This is for testing, shows code.
        private bool _isPlaying = true; // If true the game loop will run, otherwise the program ends.

        private int _numColours = 8; // Num Colours -> M
        private int _numPositions = 6; // Num Positions -> N [ DO BEFORE SUBMIT ]

        private CustomQueue _history;
        private int[] _code;
        private int[] _playerGuess;

        private int _numBlack = 0;
        private int _numWhite = 0;

        /// <summary>
        ///
        /// </summary>
        static void Main(string[] args) {
            Program program = new Program();

            Console.WriteLine("Welcome to MasterMind!");
            program.Intro();

            // Main Program loop
            while (program._isPlaying) {
                program.PrintHistory();
                program.MakeAGuess();
                program.CheckGuess();
                program.AddHistory();
                program.DisplayResults();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void Intro() {

            Console.WriteLine("Please enter the number of colours:");
            bool success = int.TryParse(Console.ReadLine(), out _numColours);
            if (!success) { Console.WriteLine("Input Invalid"); Intro(); }

            Console.WriteLine("Please enter the number of positions:");
            success = int.TryParse(Console.ReadLine(), out _numPositions);
            if (!success) { Console.WriteLine("Input Invalid"); Intro(); }

            //
            // CHANGE MAKE OWN CLASS
            Random rnd = new Random(); int rndValu;

            // Create a queue to storet the history. The size is ...
            _history = new CustomQueue(100);
            // CHANGE MAKE OWN CLASS

            _code = new int[_numPositions]; _playerGuess = new int[_numPositions];

            for (int i = 0; i < _numPositions; i++) { rndValu = rnd.Next(0, _numColours); _code[i] = rndValu; }
            Console.WriteLine();

            if (!isDebugging) { return; }

            // This only occurs if debugging.
            Console.Write("The code is : ");
            for (int i = 0; i < _numPositions; i++) { Console.Write(_code[i]); }
            Console.WriteLine();
        }

        /// <summary>
        ///
        /// </summary>
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
            for (int i = 0; i < _numPositions; i++) { Console.Write(_playerGuess[i]); }
            Console.WriteLine();
        }

        /// <summary>
        ///
        /// </summary>
        private void CheckGuess() {

            _numBlack = 0; _numWhite = 0;
            for (int i = 0; i < _numPositions; i++) {

                // Same pos and colour (num)
                if (_code[i] == _playerGuess[i]) { _numBlack++; }

                // Not same pos but the contains colour colour
                else if (myContains(_code, _playerGuess[i], _numPositions)) { _numWhite++; }

                // Not in code 
                else { }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void AddHistory() { for (int i = 0; i < _numPositions; i++) { _history.Add(_playerGuess[i]); } }

        /// <summary>
        ///
        /// </summary>
        private void PrintHistory() {
            string history = _history.Output();
            // Increment through the history, adding a space between each guess.
            // Done by checking if the remainder after deviding the current character pos by the size of a guess.
            if (history.Length == 0) { Console.WriteLine("Your Current History: Null"); return; }
            Console.Write("Your Current History:");
            for (int i = 0; i < history.Length; i++) {
                if (i % _numPositions == 0) { Console.Write(" "); }
                Console.Write(history[i]);
            } Console.WriteLine();
        }

        /// <summary>
        /// This function is in charge of outputing the result after the user attempts to answer.
        /// It works out if the user has been successfull or not and outputes the appropriate message.
        /// </summary>
        private void DisplayResults() {

            Console.WriteLine("\nYou recieved this result: \n");
            Console.Write("| Black : " + _numBlack);
            Console.Write(" | White : " + _numWhite + " | \n");
            Console.WriteLine();

            if (_numBlack == _numPositions) {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("CONGRATS YOU WON!");
                Console.ResetColor(); Console.WriteLine("\n");

                PrintHistory();
                Console.WriteLine("Would you like to play again? Yes (Y) or No (N) : ");

                // Loop until an input is recieved to quit or restart.
                string input;   bool flag = true;
                while (flag) {
                    input = Console.ReadLine();
                    if (input == "y" || input == "Y") { Intro(); flag = false; } // Recall Intro. Basically restarting the program. No need to clear variables.
                    else if (input == "n" || input == "N") { _isPlaying = false; flag = false; } // End the game loop, the program will then finish.
                }
            }
            else {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write("NOT QUITE, TRY AGAIN...");
                Console.ResetColor();
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// A very basic function that will loop through a given array and verify if it contains a certain value.
        /// Requires an array is passed in, along with its length and the value to be searched for.
        /// </summary>
        /// <returns> True if contains the value being searched for. </returns>
        public bool myContains(int[] array, int value, int len) {
            for (int i = 0; i < len; i++) { if (array[i] == value) { return true;  } }
            return false;
        }

    }
}
