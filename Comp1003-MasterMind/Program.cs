using System;

namespace Comp1003_MasterMind {

    /// <summary>
    /// Data structure that contains all its required functionality. 
    /// Simply create an object of type CustomQueue. Values are added to the top / back
    /// and removed from the bottom / front. if the queue is full, it dynamically removes from the bottom, to make space.
    /// The size is set on creation.
    /// </summary>
    class CustomQueue {

        public int QueueLength;
        public int QPos;
        public int[] QData;

        /// <summary>
        /// Constructor, ensures that all variables are set appropraitely when the object is created. ( Sets queue size etc )
        /// </summary>
        public CustomQueue(int QLength) { QData = new int[QLength]; QueueLength = QLength; QPos = -1; }

        /// <summary>
        /// Takes in a value and tries to add it to the top of the Queue. If the queue is full however,
        /// the value at the bottom of the queue is removed first, making the queue dynamic.
        /// </summary>
        public void Add(int val) {
            // First check if the queue is already full. If it is remove one from the bottom before adding to the top.
            if (QPos == QueueLength - 1) {Remove();}   // END OF QUEUE  

            // Simply add to the end of the queue.
            QPos++;  QData[QPos] = val;   // Increment by 1 and set the next value. 
        }

        /// <summary>
        /// Used if a value is needed to be removed from the queue. All values are moved down the queue by one,
        /// this naturally removes the value at the bottom / front. Creating roomn for more values to be added to the top.
        /// </summary>
        public void Remove() {
            if (QPos <= -1) { return; } // At the begining of q, cant delete.  Simply end func.

            // Shuffle - Moving everything back one, Also removing the first item in the queue.
            // Iterate through each value in the queue soo far. Shifting each back one.
            for (int i = 0; i < QPos; i++) {QData[i] = QData[i + 1];    } // Shifts back 1.
            QPos--;
        }
        
        /// <summary>
        /// Simple function used to return all the integer values within the queue in a single string. ( In order, from bottom to top )  
        /// </summary>
        public string Output() {
            string output = "";

            // Loop up to the current queue length and add each element to the string 
            for (int i = 0; i <= QPos; i++) { output += QData[i]; }
            return (output);
        }
    }
    
    class Program {

        private bool _isDebugging = false; // This is for testing, shows code at the start.
        private bool _isPlaying = true;   // If true the game loop will run, otherwise the program ends.

        private int _numColours = 8;     // Num Colours  -> (THIS IS M)
        private int _numPositions = 6;  // Num Positions -> (THIS IS N) 

        private CustomQueue _history; // Data stucture used to store the history of gueses in order.
        private int[] _code;         // Array of integers used to store the randomised code.
        private int[] _playerGuess; // Array of integers  used to hold the current user guess, for comparison with the actual code.

        private int _historyQSize = 10; // This is the size of the queue, if the queue goes beyond, it will start to remove values from the bottom of the queue. ( Dynamic )

        private int _numBlack = 0; // The guesses that are both in the correct position and colour.
        private int _numWhite = 0; // Wrong position but the actual code does contain that colour.

        /// <summary>
        /// This is the first function to be executed. It simply creates an object of type program, that contains all the functionality required,
        /// Then welcomes the user, initialises the progam and runs a loop of functions that make the game actually play. If the loops ends, the program ends.
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
        /// This function prompts the user to input the values for the number of colours and positions. It then verifies the inputs are valid,
        /// sets up all the data structures that are required through out the program, with their appropriate sizes, and generates a random code 
        /// to the user's request. Finally if debugging it will print out the actuall code. 
        /// </summary>
        private void Intro() {

            Console.WriteLine("Please enter the number of colours (1-9) :");
            bool isIntFlag = int.TryParse(Console.ReadLine(), out _numColours);
            if (!isIntFlag || _numColours > 9 || _numColours <= 0) { Console.WriteLine("Input Invalid"); Intro(); }

            Console.WriteLine("Please enter the number of positions:");
            isIntFlag = int.TryParse(Console.ReadLine(), out _numPositions);
            if (!isIntFlag) { Console.WriteLine("Input Invalid"); Intro(); }

            Random rnd = new Random(); int rndValu;

            _history = new CustomQueue(_historyQSize);

            _code = new int[_numPositions]; _playerGuess = new int[_numPositions];

            for (int i = 0; i < _numPositions; i++) { rndValu = rnd.Next(0, _numColours); _code[i] = rndValu; }
            Console.WriteLine();

            if (!_isDebugging) { return; }

            // This only occurs if debugging.
            Console.Write("[DEBUGGING ON] The code is : ");
            for (int i = 0; i < _numPositions; i++) { Console.Write(_code[i]); }
            Console.WriteLine();
        }

        /// <summary>
        /// This function loops for every value in the code, allows the user to enter a guess. Validates their guess is appropriate, if so adds the guess
        /// to the guess array to be checked later.
        /// </summary>
        private void MakeAGuess() {
            Console.WriteLine("Please now enter your code, each entry must be an integer between 0 and " + _numColours);
            for (int codePos = 0; codePos < _numPositions; codePos++) {
                bool flag = false; bool isInt;
                while (!flag) {
                    Console.WriteLine("[ENTERING YOUR CODE] - Pos [ " + codePos + " ]:");
                    isInt = int.TryParse(Console.ReadLine(), out _playerGuess[codePos]);

                    // Makes sure that the values for the code entered are within the colours available. For example if the colours are 5, they cant enter 6 as their guess.
                    if(isInt && _playerGuess[codePos] <= _numColours && _playerGuess[codePos] > 0) { flag = true; }

                    if (!flag) { Console.WriteLine("Input Invalid"); }
                }
            }

            Console.WriteLine(); Console.Write("Your entered code is : ");
            for (int i = 0; i < _numPositions; i++) { Console.Write(_playerGuess[i]); }
            Console.WriteLine();
        }

        /// <summary>
        /// Iterates through the values in the code and guesscode arrays, comparing the two and calculating how many black and white pins were recieved
        /// from this guess.
        /// </summary>
        private void CheckGuess() {

            _numBlack = 0; _numWhite = 0;
            for (int i = 0; i < _numPositions; i++) {

                // Same pos and colour (num)
                if (_code[i] == _playerGuess[i]) { _numBlack++; }

                // Not same pos but the code contains the same colour 
                else if (myContains(_code, _playerGuess[i], _numPositions)) { _numWhite++; }

                // Not in code 
                else { }
            }
        }

        /// <summary>
        /// Simply iterates through the player's guess and adds them to a queue data structure called _history.
        /// </summary>
        public void AddHistory() { for (int i = 0; i < _numPositions; i++) { _history.Add(_playerGuess[i]); } }

        /// <summary>
        /// Simply prints the history, that is stored within a queue with spaces inbetween each guess. 
        /// If the queue is empty, it will instead output a message stating that the history is null. 
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
