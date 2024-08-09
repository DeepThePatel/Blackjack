/* Developed by Deep Patel (2024) */

using System;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Text;

namespace Blackjack
{
    public class Program
    {
        private static double balance = 5000;
        private static string? readResult;
        private static string db_query = "";
        private static string casino = "";
        private static bool validEntry = false;
        private static string? username;
        private static string? connectionString;
        private static bool isLoggedIn = false;
        private static bool isPlaying = true;   // Is playing game
        private static bool isPlayingCasino = true; // Is playing a casino
        private static bool reset = false;
        private static bool hasInsurance = false;
        private static bool split = false;
        private static int playerCardSum = 0;
        private static int playerFirstHandSum = 0;  // Split - First hand
        private static int playerSecondHandSum = 0; // Split - Second hand
        private static int dealerCardSum = 0;
        private static Dictionary<string, List<int>> cards = new Dictionary<string, List<int>> {
            {"2 of Spades", new List <int> {2}}, {"3 of Spades", new List <int> {3}}, {"4 of Spades", new List <int> {4}},
            {"5 of Spades", new List <int> {5}}, {"6 of Spades", new List <int> {6}}, {"7 of Spades", new List <int> {7}}, {"8 of Spades", new List <int> {8}},
            {"9 of Spades", new List <int> {9}}, {"10 of Spades", new List <int> {10}}, {"Jack of Spades", new List <int> {10}}, {"Queen of Spades", new List <int> {10}},
            {"King of Spades", new List <int> {10}}, {"Ace of Spades", new List <int> {11,1}},

            {"2 of Clubs", new List <int> {2}}, {"3 of Clubs", new List <int> {3}}, {"4 of Clubs", new List <int> {4}},
            {"5 of Clubs", new List <int> {5}}, {"6 of Clubs", new List <int> {6}}, {"7 of Clubs", new List <int> {7}}, {"8 of Clubs", new List <int> {8}},
            {"9 of Clubs", new List <int> {9}}, {"10 of Clubs", new List <int> {10}}, {"Jack of Clubs", new List <int> {10}}, {"Queen of Clubs", new List <int> {10}},
            {"King of Clubs", new List <int> {10}}, {"Ace of Clubs", new List <int> {11,1}},

            {"2 of Hearts", new List <int> {2}}, {"3 of Hearts", new List <int> {3}}, {"4 of Hearts", new List <int> {4}},
            {"5 of Hearts", new List <int> {5}}, {"6 of Hearts", new List <int> {6}}, {"7 of Hearts", new List <int> {7}}, {"8 of Hearts", new List <int> {8}},
            {"9 of Hearts", new List <int> {9}}, {"10 of Hearts", new List <int> {10}}, {"Jack of Hearts", new List <int> {10}}, {"Queen of Hearts", new List <int> {10}},
            {"King of Hearts", new List <int> {10}}, {"Ace of Hearts", new List <int> {11,1}},

            {"2 of Diamonds", new List <int> {2}}, {"3 of Diamonds", new List <int> {3}}, {"4 of Diamonds", new List <int> {4}},
            {"5 of Diamonds", new List <int> {5}}, {"6 of Diamonds", new List <int> {6}}, {"7 of Diamonds", new List <int> {7}}, {"8 of Diamonds", new List <int> {8}},
            {"9 of Diamonds", new List <int> {9}}, {"10 of Diamonds", new List <int> {10}}, {"Jack of Diamonds", new List <int> {10}}, {"Queen of Diamonds", new List <int> {10}},
            {"King of Diamonds", new List <int> {10}}, {"Ace of Diamonds", new List <int> {11,1}}
        };

        public static void Main()
        {
            //Establishing SQL database connection
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            // Get the connection string
            connectionString = config.GetConnectionString("DefaultConnection");

            // Loop main menu until user logs in successfully
            while (isLoggedIn == false)
            {
                Console.Clear();
                Home(connectionString);
            }

            isPlaying = true;
            while (isPlaying)
            {
                validEntry = false;
                // Check if casino choice is valid or if player exits
                do
                {
                    Console.WriteLine("Welcome to Blackjack!");
                    Console.WriteLine($"Your current balance: ${balance}.\n");
                    Console.WriteLine("To begin, choose your casino:\n  1. Las Vegas\t\t(Min Bet: $50)\n  2. Monte Carlo\t(Min Bet: $200)\n  3. Dubai\t\t(Min Bet: $500)\n");
                    Console.WriteLine("Enter Q to logout.");
                    readResult = Console.ReadLine()?.ToLower().Trim();
                    if (readResult != null)
                    {
                        if (readResult == "1" || readResult == "2" || readResult == "3")
                        {
                            casino = readResult;
                            validEntry = true;
                            isPlayingCasino = true;
                        }
                        else if (readResult == "q")
                        {
                            isLoggedIn = false;
                            isPlaying = false;
                            validEntry = true;
                            Console.Clear();
                            Main();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Error: Invalid entry. Choose either 1, 2, or 3.\n");
                        }
                    }
                } while (validEntry == false);

                // Necessary to validate player quitting the game
                if (isPlaying == false)
                {
                    break;
                }

                Console.Clear();

                switch (casino)
                {
                    // Las Vegas
                    case "1":
                        PlayCasino("Las Vegas", 50);
                        break;

                    // Monte Carlo
                    case "2":
                        PlayCasino("Monte Carlo", 200);
                        break;

                    // Dubai    
                    case "3":
                        PlayCasino("Dubai", 500);
                        break;
                }
            }
        }
        /* Home screen
            PARAMETERS:
                * string connectionString - Connection string
            RETURN VALUES:
                * bool - (True) if registering a new account (False) otherwise
         */
        static void Home(string connectionString)
        {
            validEntry = false;
            while (validEntry == false)
            {
                Console.WriteLine("--Blackjack v1.4.0--\n");
                Console.WriteLine("(1) Login\n(2) Register");
                readResult = Console.ReadLine()?.Trim();
                if (readResult.Equals("1"))
                {
                    Console.Clear();
                    Login(connectionString);
                    validEntry = true;
                }
                else if (readResult.Equals("2"))
                {
                    Console.Clear();
                    Register(connectionString);
                    validEntry = true;
                }
                else if (readResult.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid entry. Please choose either (1) to Login or (2) to Register");
                }
            }
        }

        /* Register screen
            PARAMETERS:
                * string connectionString - Connection string
         */
        static void Register(string connectionString)
        {
            while (validEntry == false)
            {
                Console.WriteLine("--Register--\n");
                Console.WriteLine("Enter (C) to clear fields or (Q) to go back to main menu");
                Console.Write("Username: ");
                readResult = Console.ReadLine();
                username = readResult.ToLower();
                // Username only allows characters, numbers, and underscores
                if (!string.IsNullOrEmpty(readResult) && Regex.IsMatch(readResult, @"^[a-zA-Z0-9_]+$") && readResult.Length >= 6 && readResult.Length <= 20)
                {
                    if (UsernameExists(username, connectionString))
                    {
                        Console.Clear();
                        Console.WriteLine("Error: Username is already taken");
                    }
                    else
                    {
                        Console.Clear();
                        RegisterPassword(readResult, connectionString); // Pass readResult to display case insensitive username
                        validEntry = true;
                    }
                }
                else if (readResult.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Home(connectionString);
                    validEntry = true;
                }
                else if (readResult.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid username. Username must be between 6-20 alphanumeric characters or underscores");
                }
            }
        }

        /* Create password for new account
            PARAMETERS:
                * string username - User's username
                * string connectionString - Connection string
         */
        static void RegisterPassword(string username, string connectionString)
        {
            validEntry = false;
            bool passwordEntered = false;
            string? password = null;
            var hashedPassword = "";
            do
            {
                Console.WriteLine("--Register--\n");
                Console.WriteLine("Enter (C) to clear fields or (Q) to go back to main menu");
                Console.WriteLine($"Username: {username}");
                if (passwordEntered == false)
                {
                    Console.Write("Password: ");
                    password = ReadPassword();
                    hashedPassword = HashPassword(password);
                    passwordEntered = true;
                }
                else
                {
                    Console.WriteLine($"Password: {MaskPassword(password)}");
                }

                // Password may contain characters, numbers, or special characters (!,@,#,$,%,^,&,*,(,))
                if (!string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()]+$") && password.Length >= 8 && password.Length <= 50)
                {
                    string? reenterPassword;
                    Console.Write("Reenter Password: ");
                    reenterPassword = ReadPassword();
                    if (password.Equals(reenterPassword))
                    {

                        // Add username and password to database
                        db_query = "INSERT INTO Blackjack_db (username,password,balance) VALUES (@username, @passwordHash, @balance)";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                using (SqlCommand command = new SqlCommand(db_query, connection))
                                {
                                    // Add parameters to the command
                                    command.Parameters.AddWithValue("@username", username);
                                    command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                                    command.Parameters.AddWithValue("@balance", balance);

                                    // Execute query
                                    int rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        Console.Clear();
                                        isLoggedIn = true;
                                        Console.WriteLine("Account successfully created!");
                                        Thread.Sleep(1500);
                                        return;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Error: Failed to create account. Please try again");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Clear();
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    else if (reenterPassword.Equals("q", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        Home(connectionString);
                        validEntry = true;
                    }
                    else if (reenterPassword.Equals("c", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        Register(connectionString);
                        validEntry = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Error: Passwords do not match");
                    }
                }
                else if (password.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Home(connectionString);
                    validEntry = true;
                }
                else if (password.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Register(connectionString);
                    validEntry = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid password. Password must contain at least 8 alphanumeric or special (!,@,#,$,%,^,&,*,(,)) characters.");
                }
            } while (validEntry == false);
        }

        /* Login screen
            PARAMETERS:
                * string connectionString - Connection String
         */
        static void Login(string connectionString)
        {
            bool validUsername = false;
            do
            {
                Console.WriteLine("--Login--\n");
                Console.WriteLine("Enter (C) to clear fields or (Q) to go back to main menu");
                Console.Write("Username: ");
                readResult = Console.ReadLine();
                username = readResult.ToLower();
                if (!string.IsNullOrEmpty(readResult) && Regex.IsMatch(readResult, @"^[a-zA-Z0-9_]+$") && readResult.Length >= 6 && readResult.Length <= 20)
                {
                    try
                    {
                        if (UsernameExists(username, connectionString))
                        {
                            Console.Clear();
                            LoginPassword(readResult, connectionString);
                            validUsername = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Error: Invalid username");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                else if (readResult.Equals("q"))
                {
                    Console.Clear();
                    Home(connectionString);
                    break;
                }
                else if (readResult.Equals("c"))
                {
                    Console.Clear();
                    Login(connectionString);
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid username");
                }
            } while (validUsername == false);

        }

        /* Get password with existing username
            PARAMETERS:
                * string username - User's username
                * string connectionString - Connection string
            RETURN VALUES:
                * bool - (True) if username exists (False) otherwise
         */
        static void LoginPassword(string username, string connectionString)
        {
            bool validPassword = false;
            do
            {
                string username_db = username.ToLower();
                Console.WriteLine("--Login--\n");
                Console.WriteLine("Enter (C) to clear fields or (Q) to go back to main menu");
                Console.WriteLine($"Username: {username}");
                Console.Write("Password: ");
                string? password = ReadPassword();

                // Quitting and clearing fields
                if (password.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Home(connectionString);
                    break;
                }
                else if (password.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Login(connectionString);
                    break;
                }

                // Check if password is correct and retrieve balance if so
                db_query = "SELECT password, balance FROM Blackjack_db WHERE username = @username";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(db_query, connection))
                        {
                            // Add parameter to the command
                            command.Parameters.AddWithValue("@username", username);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string? password_db = reader["password"] as string;

                                    if (password_db != null && VerifyPassword(password, password_db))
                                    {
                                        // Retrieve and set the balance
                                        decimal balance_db = reader.GetDecimal(reader.GetOrdinal("balance"));
                                        balance = (double)balance_db;
                                        isLoggedIn = true;
                                        Console.WriteLine("Login successful, welcome back!");
                                        Thread.Sleep(1500);
                                        Console.Clear();
                                        validPassword = true;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Incorrect password.");
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Username not found.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            } while (validPassword == false);
        }

        /* Checks if username exists in database
            PARAMETERS:
                * string username - User entered username
                * string connectionString - Connecting string for database
            RETURN VALUES:
                * bool - Whether username exists in database or not
         */
        static bool UsernameExists(string username, string connectionString)
        {
            // Find if entered username exists in database
            db_query = "SELECT 1 FROM Blackjack_db WHERE username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(db_query, connection))
                    {
                        // Add parameter to the command
                        command.Parameters.AddWithValue("@username", username);

                        // Execute query
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /* Method to hide user's typed password
            RETURN VALUES:
                * string - User's password
         */
        static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        /* Method to return masked password for UX purposes
         *  PARAMETERS:
                * string password - User's password
            RETURN VALUES:
                * string - User's hidden password
         */
        static string MaskPassword(string password)
        {
            return new string('*', password.Length);
        }

        /* Encrypting password
            PARAMETERS:
                * string password - User's password
            RETURN VALUES:
                * string - Hashed password
         */
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /* Verifying the password
            PARAMETERS:
                * string password - User entered password
                * string storedHash - Stored hash in db
            RETURN VALUES
                * bool - (True) if password matches (False) otherwise
         */
        private static bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        /* Playing the casino
            PARAMETERS:
                * string casino - Name of the casino
                * int minBet - Casino's minimum bet
        */
        static void PlayCasino(string casino, int minBet)
        {
            while (isPlayingCasino)
            {
                bool playingGames = true;
                // Checking termination of round
                while (playingGames)
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome to {casino}!");
                    if (reset == true)
                    {    // Check if player was reset
                        balance = 500;
                        reset = false;
                    }
                    Console.WriteLine($"Your balance is ${balance}");
                    var (bet, quit) = EnterBet(minBet);
                    if (quit == true)
                    {
                        isPlayingCasino = false;
                        break;
                    }

                    var (playerCards, dealerCards) = DealCards();
                    List<Tuple<string, List<int>>>? playerFirstHand = new List<Tuple<string, List<int>>>();     // List to hold player's first split hand
                    List<Tuple<string, List<int>>>? playerSecondHand = new List<Tuple<string, List<int>>>();    // List to hold player's second split hand

                    (playingGames, playerCardSum, playerFirstHand, playerSecondHand, bet) = PlayerTurn(playerCards, dealerCards, bet);
                    if (playingGames == false)
                        break;
                    DealerTurn(playerCards, dealerCards, playerFirstHand, playerSecondHand, bet);   // Null reference is OK since not all hands are split
                }
            }
        }

        /*  Validate user entered bet amount
            PARAMETERS:
                * int minBet - The minimum required bet by the casino
            RETURN VALUES:
                * int intBet - Returns the bet entered by the user
                * bool quit - Returns true if player wishes to exit the casino
        */
        static (int, bool) EnterBet(int minBet)
        {
            string? bet;
            int intBet;
            bool validEntry = false;
            bool quit = false;

            Console.WriteLine($"Enter your bet (Min: {minBet}) or Q to exit casino.");
            do
            {
                bet = Console.ReadLine()?.ToLower().Trim();
                if (bet == "q")
                {
                    Console.Clear();
                    quit = true;
                    return (0, quit);
                }
                bool validNumber = int.TryParse(bet, out intBet);   // Check if entry is a valid integer
                if (validNumber == false || intBet > balance || intBet < minBet)
                {
                    Console.WriteLine($"Invalid entry. Bet must be between ${minBet} and ${balance}");
                }
                else
                {
                    balance -= intBet;
                    validEntry = true;
                }
            } while (validEntry == false);

            return (intBet, quit);
        }

        /*  Deals initial cards to the player and dealer
            RETURN VALUES:
                * List<Tuple<string, List<int>>> playerCards - Returns the player's initial hand
                * List<Tuple<string, List<int>>> dealerCards - Returns the dealer's initial hand
        */
        static (List<Tuple<string, List<int>>>, List<Tuple<string, List<int>>>) DealCards()
        {
            List<string> deck = new List<string>(cards.Keys);   // Deck of cards
            Random dealCard = new Random();
            List<Tuple<string, List<int>>> playerCards = new List<Tuple<string, List<int>>>();  // Tuple to hold player's cards
            List<Tuple<string, List<int>>> dealerCards = new List<Tuple<string, List<int>>>();  // Tuple to hold dealer's cards

            Console.WriteLine();

            // Deal cards to player and dealer in an alternating fashion where the dealer's second card is kept hidden
            for (int i = 0; i < 4; i++)
            {
                int index = dealCard.Next(0, deck.Count);
                string randomCard = deck[index];

                if (i % 2 == 0)     // BREAKPOINT: Set player and dealer cards
                {                   // BREAKPOINT: Set player cards
                    Console.WriteLine($"You are dealt the {randomCard}.");
                    playerCards.Add(Tuple.Create(randomCard, cards[randomCard]));
                    Thread.Sleep(1000);
                }
                else if (i == 3)
                {
                    Console.WriteLine($"Dealer is dealt a hidden card.");
                    dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine($"Dealer is dealt the {randomCard}.");
                    dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));
                    Thread.Sleep(1000);
                }
                /*  Uncomment to play with SINGLE DECK
                cards.Remove(randomCard);
                deck.RemoveAt(index);
                */
            }

            Console.Clear();

            return (playerCards, dealerCards);
        }

        /* Checks if insurance is available
            PARAMETERS:
                * List<Tuple<string, List<int>>> dealerCards - Dealer's cards
                * List<Tuple<string, List<int>>> playerCards - Player's cards
                * int bet - Player's original bet
            RETURN VALUES:
                * insuranceBet - Player's insurance bet
        */
        static int CheckForInsurance(List<Tuple<string, List<int>>> dealerCards, List<Tuple<string, List<int>>> playerCards, int bet)
        {
            var dealerFirstCard = dealerCards[0].Item1;
            int maxInsuranceBet = bet / 2;
            int insuranceBet = 0;
            bool validInsuranceNumber = false;
            playerCardSum = CalculateBestHand(playerCards);

            if (playerCardSum != 21)
            {
                if (dealerFirstCard == "Ace of Spades" ||
                dealerFirstCard == "Ace of Clubs" ||
                dealerFirstCard == "Ace of Hearts" ||
                dealerFirstCard == "Ace of Diamonds")
                {
                    if (balance > 0)
                    {
                        DisplayPlayersHand(playerCards, playerCardSum);
                        Console.WriteLine();
                        Console.WriteLine("--Dealer's Hand--");
                        Console.WriteLine($"{dealerFirstCard}\nHidden Card");
                        Console.WriteLine();
                        Console.WriteLine($"Dealer has an Ace, would you like to purchase insurance?\nYou may bet up to ${(balance > maxInsuranceBet ? maxInsuranceBet : balance)}. If you do not want insurance, enter 0.");
                        do
                        {
                            while (validInsuranceNumber == false)
                            {
                                readResult = Console.ReadLine()?.ToLower().Trim();
                                validInsuranceNumber = int.TryParse(readResult, out insuranceBet);
                                if (validInsuranceNumber == false || insuranceBet > maxInsuranceBet)
                                {
                                    validInsuranceNumber = false;
                                    Console.WriteLine($"\nInvalid entry. Insurance bet must be between 1 and {(balance > maxInsuranceBet ? maxInsuranceBet : balance)} or 0 for no insurance.");
                                }
                                else
                                {
                                    validInsuranceNumber = true;
                                }
                            }
                            if (insuranceBet == 0)
                            {
                                insuranceBet = 0;
                            }
                            else
                            {
                                hasInsurance = true;
                                validEntry = true;
                            }
                        } while (validEntry == false);
                    }
                }
            }

            Console.Clear();
            return insuranceBet;
        }

        /*  Checks if either the player or the dealer has a blackjack
            PARAMETERS:
                * List<Tuple<string, List<int>>> playerCards - Player's hand
                * List<Tuple<string, List<int>>> dealerCards - Dealer's hand
                * int bet - Player's bet
            RETURN VALUES:
                * bool - Boolean value that returns (true) if there is a blackjack and (false) if there is not
                * int playerCardSum - The sum of the player's hand
                * int dealerCardSum - The sum of the dealer's hand
        */
        static (bool, int, int) CheckBlackJack(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet)
        {
            playerCardSum = CalculateBestHand(playerCards);
            dealerCardSum = CalculateBestHand(dealerCards);
            int insurance = CheckForInsurance(dealerCards, playerCards, bet);

            if (playerCardSum == 21)
            {
                if (dealerCardSum == 21)
                {
                    DisplayPlayersHand(playerCards, playerCardSum);
                    Console.WriteLine();
                    DisplayDealersHand(dealerCards, dealerCardSum);
                    Console.WriteLine("\nPush.");
                    balance += bet;
                    Console.WriteLine("\nPress Enter to continue");
                    Console.ReadLine();
                    return (true, playerCardSum, dealerCardSum);
                }
                else
                {
                    DisplayPlayersHand(playerCards, playerCardSum);
                    Console.WriteLine();
                    DisplayDealersHand(dealerCards, dealerCardSum);
                    Console.WriteLine("\nBLACKJACK!");
                    balance += bet * 2.5;
                    CheckBalance(balance,username,connectionString);
                    return (true, playerCardSum, dealerCardSum);
                }
            }
            else if (dealerCardSum == 21)
            {
                DisplayPlayersHand(playerCards, playerCardSum);
                Console.WriteLine();
                DisplayDealersHand(dealerCards, dealerCardSum);

                Console.WriteLine();

                Console.WriteLine("Dealer has a blackjack.");
                if (insurance > 0)
                {
                    Console.WriteLine("You won insurance!");
                    balance += bet + (insurance * 2);
                }
                CheckBalance(balance,username,connectionString);

                return (true, playerCardSum, dealerCardSum);
            }
            else
            {
                balance -= insurance;
            }

            Thread.Sleep(1000);

            return (false, playerCardSum, dealerCardSum);
        }

        /*  Method that handles player splitting their hand
            PARAMETERS: 
                * List<Tuple<string, List<int>>> playerCards - Player's cards
            RETURN VALUES:
                * List<Tuple<string, List<int>>> - Player's first hand
                * List<Tuple<string, List<int>>> - Player's second hand
        */
        static (List<Tuple<string, List<int>>>, List<Tuple<string, List<int>>>) Split(List<Tuple<string, List<int>>> playerCards)
        {
            Console.Clear();
            List<Tuple<string, List<int>>> playerFirstHand = new List<Tuple<string, List<int>>>();
            List<Tuple<string, List<int>>> playerSecondHand = new List<Tuple<string, List<int>>>();
            int playerFirstCard = playerCards[0].Item2[0];
            int playerSecondCard = playerCards[1].Item2[0];
            bool invalidEntry = false;

            playerFirstHand.Add(Tuple.Create(playerCards[0].Item1, new List<int> { playerFirstCard }));   // Add first card from player's hand to its own hand
            playerSecondHand.Add(Tuple.Create(playerCards[1].Item1, new List<int> { playerSecondCard }));  // Add second card from player's hand to its own hand

            playerFirstHandSum = CalculateBestHand(playerFirstHand);
            playerSecondHandSum = CalculateBestHand(playerSecondHand);

            DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
            Console.WriteLine();

            // Deal second card to each players' hand
            for (int i = 0; i < 2; i++)
            {
                List<string> deck = new List<string>(cards.Keys);
                Random dealCard = new Random();
                int index = dealCard.Next(0, deck.Count);
                string randomCard = deck[index];

                if (i == 0)
                {
                    Thread.Sleep(1500);
                    Console.Clear();
                    playerFirstHand.Add(Tuple.Create(randomCard, cards[randomCard]));
                    playerFirstHandSum = CalculateBestHand(playerFirstHand);
                    DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                    Console.WriteLine($"\nYou are dealt the {randomCard} for your first hand");
                    Thread.Sleep(2500);
                }
                else
                {
                    Console.Clear();
                    playerSecondHand.Add(Tuple.Create(randomCard, cards[randomCard]));
                    playerSecondHandSum = CalculateBestHand(playerSecondHand);
                    DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                    Console.WriteLine($"\nYou are dealt the {randomCard} for your second hand");
                    Thread.Sleep(2500);
                }
            }

            // Playing the split hands
            for (int i = 0; i < 2; i++)
            {
                while (i == 0 ? playerFirstHandSum < 21 : playerSecondHandSum < 21)
                {
                    Console.Clear();
                    if (invalidEntry == true)
                        Console.WriteLine("**Invalid input, please enter (Hit) or (Stand)**\n");
                    DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                    Console.WriteLine($"{(i == 0 ? "\nFIRST Hand: Would you like to (Hit) or (Stand)?" : "\nSECOND Hand: Would you like to (Hit) or (Stand)?")}");
                    readResult = Console.ReadLine()?.ToLower().Trim();
                    if (readResult == "hit")
                    {
                        invalidEntry = false;
                        // Draw a random card
                        Random dealCard = new Random();
                        List<string> deck = new List<string>(cards.Keys);
                        int index = dealCard.Next(0, deck.Count);
                        string randomCard = deck[index];

                        if (i == 0)
                        {
                            playerFirstHand.Add(Tuple.Create(randomCard, cards[randomCard]));   // Add newly drawn card to player's hand
                            playerFirstHandSum = CalculateBestHand(playerFirstHand);
                        }
                        else
                        {
                            playerSecondHand.Add(Tuple.Create(randomCard, cards[randomCard]));
                            playerSecondHandSum = CalculateBestHand(playerSecondHand);
                        }

                        if (i == 0)
                        {
                            if (playerFirstHandSum > 21)
                            {
                                Console.Clear();
                                DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                                Console.WriteLine("\nYou busted.");
                                Thread.Sleep(2500);
                            }
                        }
                        else
                        {
                            if (playerSecondHandSum > 21)
                            {
                                Console.Clear();
                                DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                                Console.WriteLine("\nYou busted.");
                                Thread.Sleep(2500);
                            }
                        }
                    }
                    else if (readResult == "stand")
                    {
                        invalidEntry = false;
                        break;
                    }
                    else
                    {
                        invalidEntry = true;
                    }
                }
            }

            return (playerFirstHand, playerSecondHand);
        }

        /*  Calculates best value of hand (handling aces)
            PARAMETERS:
                * List<Tuple<string, List<int>>> cards - Hand of cards (player or dealer)
            RETURN VALUES:
                * int sum - Returns best calculated sum
        */
        static int CalculateBestHand(List<Tuple<string, List<int>>> cards)
        {
            int sum = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                // Check if the card is an Ace
                if (card.Item2.Contains(11))
                {
                    aceCount++;
                    sum += 11; // Add Ace as value of 11
                }
                else
                {
                    sum += card.Item2[0];
                }
            }

            // Adjust for Aces if the sum exceeds 21
            while (sum > 21 && aceCount > 0)
            {
                sum -= 10;
                aceCount--;
            }

            return sum;
        }

        /*  Handling the player's turn
            PARAMETERS:
                * List<Tuple<string, List<int>>> playerCards - The player's hand
                * List<Tuple<string, List<int>>> dealerCards - The dealer's hand
                * int bet - The player's bet
            RETURN VALUES:
                * bool - Boolean value that returns (true) if round should continue and (false) if not
                * int playerCardSum - Returns the sum of the player's hand
        */
        static (bool, int, List<Tuple<string, List<int>>>, List<Tuple<string, List<int>>>, int) PlayerTurn(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet)
        {
            bool stand = false;
            bool canDouble = true;
            bool canSplit = false;
            bool doubled = false;
            split = false;
            bool splitCompleted = false;
            List<Tuple<string, List<int>>> playerFirstHand = new List<Tuple<string, List<int>>>();
            List<Tuple<string, List<int>>> playerSecondHand = new List<Tuple<string, List<int>>>();
            int playerFirstHandSum = 0;
            int playerSecondHandSum = 0;
            var playerFirstCard = playerCards[0].Item2;
            var playerSecondCard = playerCards[1].Item2;
            var (blackjack, playerCardSum, dealerCardSum) = CheckBlackJack(playerCards, dealerCards, bet);

            if (blackjack)
                return (false, 21, new List<Tuple<string, List<int>>>(), new List<Tuple<string, List<int>>>(), bet);
            if (playerFirstCard.SequenceEqual(playerSecondCard))    // Compare if integer values of the two cards are equal
                canSplit = true;
            while (playerCardSum < 21 && stand == false)
            {
                if (splitCompleted == true)
                    break;
                if (doubled == true)
                    break;
                playerCardSum = CalculateBestHand(playerCards);
                DisplayPlayersHand(playerCards, playerCardSum);
                Console.WriteLine();
                Console.WriteLine("--Dealer's Hand--");
                var dealerFirstCard = dealerCards[0].Item1;
                Console.WriteLine($"{dealerFirstCard}\nHidden Card");
                Console.WriteLine();

                do
                {
                    if (splitCompleted == true)
                        break;
                    if (doubled == true)
                        break;
                    if ((bet * 2) > balance)
                        canDouble = false;
                    if (hasInsurance == true)
                        canDouble = false;
                    // Menu iteration logic
                    if (canDouble == true && canSplit == true)
                        Console.WriteLine("Would you like to (Hit) or (Split) or (Double) or (Stand)?");
                    else if (canDouble == true && canSplit == false)
                        Console.WriteLine("Would you like to (Hit) or (Double) or (Stand)?");
                    else if (canDouble == false && canSplit == true)
                    {
                        Console.WriteLine("Would you like to (Hit) or (Split) or (Stand)?");
                    }
                    else
                        Console.WriteLine("Would you like to (Hit) or (Stand)?");

                    readResult = Console.ReadLine()?.ToLower().Trim();
                    if (readResult == "hit" || readResult == "double")
                    {
                        if (readResult == "double" && canDouble == false)
                        {
                            Console.Clear();
                            Console.WriteLine("**Invalid input. You cannot double**\n");
                            break;
                        }
                        if (readResult == "double")
                        {
                            balance -= bet;
                            bet *= 2;
                            doubled = true;
                        }
                        // Draw a random card
                        Random dealCard = new Random();
                        List<string> deck = new List<string>(cards.Keys);
                        int index = dealCard.Next(0, deck.Count);
                        string randomCard = deck[index];

                        playerCards.Add(Tuple.Create(randomCard, cards[randomCard]));   // Add newly drawn card to player's hand

                        playerCardSum = CalculateBestHand(playerCards);

                        if (playerCardSum > 21)
                        {
                            Console.Clear();
                            DisplayPlayersHand(playerCards, playerCardSum);
                            Console.WriteLine();
                            DisplayDealersHand(dealerCards, dealerCardSum);
                            Console.WriteLine($"\nYou busted.");
                            CheckBalance(balance,username,connectionString);
                            return (false, playerCardSum, new List<Tuple<string, List<int>>>(), new List<Tuple<string, List<int>>>(), bet);
                        }

                        Console.Clear();
                        canDouble = false;
                        validEntry = true;
                    }
                    else if (readResult == "split")
                    {
                        if (canSplit == false)
                        {
                            Console.Clear();
                            Console.WriteLine("**Invalid input. You cannot split**\n");
                            break;
                        }
                        else
                        {
                            split = true;
                            balance -= bet;
                            bet *= 2;
                            (playerFirstHand, playerSecondHand) = Split(playerCards);
                            playerFirstHandSum = CalculateBestHand(playerFirstHand);
                            playerSecondHandSum = CalculateBestHand(playerSecondHand);

                            if (playerFirstHandSum > 21 && playerSecondHandSum > 21)
                            {
                                Console.WriteLine("Dealer wins.");
                                CheckBalance(balance,username,connectionString);
                                return (false, 0, playerFirstHand, playerSecondHand, bet);
                            }
                            splitCompleted = true;
                            break;
                        }
                    }
                    else if (readResult == "stand")
                    {
                        stand = true;
                        validEntry = true;
                    }
                    else
                    {
                        Console.Clear();
                        if (canDouble == true && canSplit == true)
                            Console.WriteLine("**Invalid input, please enter (Hit) or (Split) or (Double) or (Stand)**\n");
                        else if (canDouble == true && canSplit == false)
                            Console.WriteLine("**Invalid input, please enter (Hit) or (Double) or (Stand)**\n");
                        else if (canDouble == false && canSplit == true)
                            Console.WriteLine("**Invalid input, please enter (Hit) or (Split) or (Stand)?**\n");
                        else
                            Console.WriteLine("**Invalid input, please enter (Hit) or (Stand)**\n");
                    }
                } while (validEntry == false);
                if (reset == true)
                    break;
            }

            return (true, playerCardSum, playerFirstHand, playerSecondHand, bet);
        }

        /*  Handling the dealer's turn
            PARAMETERS:
                * List<Tuple<string, List<int>>> playerCards - The player's hand
                * List<Tuple<string, List<int>>> dealerCards - The dealer's hand
                * int bet - The player's bet
        */
        static void DealerTurn(List<Tuple<string, List<int>>> playerCards,
                        List<Tuple<string, List<int>>> dealerCards,
                        List<Tuple<string, List<int>>> playerFirstHand,
                        List<Tuple<string, List<int>>> playerSecondHand,
                        int bet)
        {
            dealerCardSum = CalculateBestHand(dealerCards); // Calculate dealer's sum before entering loop
            playerFirstHandSum = CalculateBestHand(playerFirstHand);
            playerSecondHandSum = CalculateBestHand(playerSecondHand);
            if (split == true)
            {
                if (dealerCardSum >= 17 && dealerCardSum <= 20)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            if (dealerCardSum > playerFirstHandSum)
                            {
                                Console.Clear();
                                DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                                Console.WriteLine();
                                DisplayDealersHand(dealerCards, dealerCardSum);
                                Console.WriteLine("\nDealer wins the first hand.");
                                Thread.Sleep(2500);
                            }
                            else if (playerFirstHandSum > dealerCardSum)
                            {
                                Console.Clear();
                                DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                                Console.WriteLine();
                                DisplayDealersHand(dealerCards, dealerCardSum);
                                balance += bet;
                                Console.WriteLine("\nYou win the first hand!");
                                Thread.Sleep(2500);
                            }
                            else if (playerFirstHandSum == dealerCardSum)
                            {
                                Console.Clear();
                                DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                                Console.WriteLine();
                                DisplayDealersHand(dealerCards, dealerCardSum);
                                balance += bet / 2;
                                Console.WriteLine("\nThe first hand is a Push.");
                                Thread.Sleep(2500);
                            }
                        }
                        else
                        {
                            if (dealerCardSum > playerSecondHandSum)
                            {
                                Console.WriteLine("Dealer wins the second hand.\n");
                                CheckBalance(balance,username,connectionString);
                            }
                            else if (playerSecondHandSum > dealerCardSum)
                            {
                                balance += bet;
                                Console.WriteLine("You win the second hand!\n");
                                CheckBalance(balance,username,connectionString);
                            }
                            else if (playerSecondHandSum == dealerCardSum)
                            {
                                balance += bet / 2;
                                Console.WriteLine("The second hand is a Push.\n");
                                CheckBalance(balance,username,connectionString);
                            }
                        }
                    }
                }
                else
                {
                    do
                    {
                        Console.Clear();
                        DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                        Console.WriteLine();
                        DisplayDealersHand(dealerCards, dealerCardSum);

                        Console.WriteLine("\nDealer will now draw a card.");
                        Thread.Sleep(2000);

                        // Draw a random card
                        Random dealCard = new Random();
                        List<string> deck = new List<string>(cards.Keys);
                        int index = dealCard.Next(0, deck.Count);
                        string randomCard = deck[index];

                        dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));   // Add newly drawn card to dealer's hand

                        dealerCardSum = CalculateBestHand(dealerCards);

                        if (dealerCardSum >= 17 && dealerCardSum <= 21)
                        {
                            Console.Clear();
                            DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                            Console.WriteLine();
                            DisplayDealersHand(dealerCards, dealerCardSum);
                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    if (dealerCardSum > playerFirstHandSum)
                                    {
                                        Console.WriteLine("\nDealer wins the first hand.");
                                        Thread.Sleep(2500);

                                    }
                                    else if (playerFirstHandSum > dealerCardSum)
                                    {
                                        Console.WriteLine("\nYou win the first hand!");
                                        balance += bet;
                                        Thread.Sleep(2500);
                                    }
                                    else if (playerFirstHandSum == dealerCardSum)
                                    {
                                        Console.WriteLine("\nPush.");
                                        balance += bet / 2;
                                        Console.WriteLine("\nPress Enter to continue");
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    if (dealerCardSum > playerSecondHandSum)
                                    {
                                        Console.WriteLine("Dealer wins the second hand.");
                                        CheckBalance(balance,username,connectionString);

                                    }
                                    else if (playerSecondHandSum > dealerCardSum)
                                    {
                                        Console.WriteLine("\nYou win the second hand!");
                                        balance += bet;
                                        CheckBalance(balance,username,connectionString);
                                    }
                                    else if (playerSecondHandSum == dealerCardSum)
                                    {
                                        Console.WriteLine("\nPush.");
                                        balance += bet / 2;
                                        CheckBalance(balance,username,connectionString);
                                    }
                                }
                            }
                        }
                        else if (dealerCardSum > 21)
                        {
                            Console.Clear();
                            DisplayPlayersSplitHands(playerFirstHand, playerSecondHand, playerFirstHandSum, playerSecondHandSum);
                            Console.WriteLine();
                            DisplayDealersHand(dealerCards, dealerCardSum);
                            Console.WriteLine("\nDealer busted!");
                            balance += bet * 2;
                            CheckBalance(balance,username,connectionString);
                        }
                    } while (dealerCardSum < 17);
                }
            }
            else if (split == false)
            {
                if (dealerCardSum >= 17 && dealerCardSum <= 20)
                {
                    if (dealerCardSum > playerCardSum)
                    {
                        Console.Clear();
                        DisplayPlayersHand(playerCards, playerCardSum);
                        Console.WriteLine();
                        DisplayDealersHand(dealerCards, dealerCardSum);

                        Console.WriteLine("\nDealer wins.");
                        CheckBalance(balance,username,connectionString);
                    }
                    else if (playerCardSum > dealerCardSum)
                    {
                        Console.Clear();
                        DisplayPlayersHand(playerCards, playerCardSum);
                        Console.WriteLine();
                        DisplayDealersHand(dealerCards, dealerCardSum);

                        Console.WriteLine("\nYou win!");
                        balance += bet * 2;
                        CheckBalance(balance,username,connectionString);
                    }
                    else if (playerCardSum == dealerCardSum)
                    {
                        Console.Clear();
                        DisplayPlayersHand(playerCards, playerCardSum);
                        Console.WriteLine();
                        DisplayDealersHand(dealerCards, dealerCardSum);

                        Console.WriteLine("\nPush.");
                        balance += bet;
                        Console.WriteLine("\nPress Enter to continue");
                        Console.ReadLine();
                    }
                }
                else
                {
                    do
                    {
                        Console.Clear();
                        DisplayPlayersHand(playerCards, playerCardSum);
                        Console.WriteLine();
                        DisplayDealersHand(dealerCards, dealerCardSum);

                        Console.WriteLine("\nDealer will now draw a card.");
                        Thread.Sleep(2000);

                        // Draw a random card
                        Random dealCard = new Random();
                        List<string> deck = new List<string>(cards.Keys);
                        int index = dealCard.Next(0, deck.Count);
                        string randomCard = deck[index];

                        dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));   // Add newly drawn card to dealer's hand

                        dealerCardSum = CalculateBestHand(dealerCards);

                        if (dealerCardSum >= 17 && dealerCardSum <= 21)
                        {
                            Console.Clear();
                            DisplayPlayersHand(playerCards, playerCardSum);
                            Console.WriteLine();
                            DisplayDealersHand(dealerCards, dealerCardSum);

                            if (dealerCardSum > playerCardSum)
                            {
                                Console.WriteLine("\nDealer wins.");
                                CheckBalance(balance,username,connectionString);

                            }
                            else if (playerCardSum > dealerCardSum)
                            {
                                Console.WriteLine("\nYou win!");
                                balance += bet * 2;
                                CheckBalance(balance,username,connectionString);
                            }
                            else if (playerCardSum == dealerCardSum)
                            {
                                Console.WriteLine("\nPush.");
                                balance += bet;
                                Console.WriteLine("\nPress Enter to continue");
                                Console.ReadLine();
                            }
                        }
                        else if (dealerCardSum > 21)
                        {
                            Console.Clear();
                            DisplayPlayersHand(playerCards, playerCardSum);
                            Console.WriteLine();
                            DisplayDealersHand(dealerCards, dealerCardSum);
                            Console.WriteLine("\nDealer busted!");
                            balance += bet * 2;
                            CheckBalance(balance,username,connectionString);
                        }
                    } while (dealerCardSum < 17);
                }
            }
        }

        /* Displays player's and dealer's hands
            PARAMETERS:
                * List<Tuple<string, List<int>>> playerCards - Player's hand
                * int playerCardSum - Sum of player's hand
                * List<Tuple<string, List<int>>> dealerCards - Dealer's hand
                * int dealerCardSum - Sum of dealer's hand
        */
        static void DisplayPlayersHand(List<Tuple<string, List<int>>> playerCards, int playerCardSum)
        {
            Console.WriteLine($"--Player's Hand ({playerCardSum})--");
            foreach (var playerCard in playerCards)
            {
                Console.WriteLine(playerCard.Item1);
            }
        }
        static void DisplayDealersHand(List<Tuple<string, List<int>>> dealerCards, int dealerCardSum)
        {
            Console.WriteLine($"--Dealer's Hand ({dealerCardSum})--");
            foreach (var dealerCard in dealerCards)
            {
                Console.WriteLine(dealerCard.Item1);
            }
        }

        /* Displays player's split hands
            PARAMETERS:
                * List<Tuple<string, List<int>>> playerFirstHand - Player's first hand
                * List<Tuple<string, List<int>>> playerSecondHand - Player's second hand
                * int playerFirstHandSum - Sum of the player's first hand
                * int playerSecondHandSum - Sum of the player's second hand
        */
        static void DisplayPlayersSplitHands(List<Tuple<string, List<int>>> playerFirstHand, List<Tuple<string, List<int>>> playerSecondHand, int playerFirstHandSum, int playerSecondHandSum)
        {
            Console.WriteLine($"--Player's First Hand ({playerFirstHandSum})--\t\t--Player's Second Hand ({playerSecondHandSum})--");

            // Determine the maximum number of cards in either hand
            int numberOfCards = Math.Max(playerFirstHand.Count, playerSecondHand.Count);

            // Define a standard width for card names to ensure alignment
            int cardWidth = 17; // Queen of Diamonds being the longest card name

            // Print each card side by side
            for (int i = 0; i < numberOfCards; i++)
            {
                string firstHandCard = (i < playerFirstHand.Count) ? playerFirstHand[i].Item1 : "";
                string secondHandCard = (i < playerSecondHand.Count) ? playerSecondHand[i].Item1 : "";

                Console.WriteLine($"{firstHandCard.PadRight(cardWidth)}\t\t\t{secondHandCard.PadRight(cardWidth)}");
            }
        }

        /* Checks if player's balance is < 50
            PARAMETERS:
                * double balance - Player's current balance
        */
        static void CheckBalance(double balance, string username, string connectionString)
        {
            if (balance < 50)
            {
                reset = true;
                UpdateBalance(500,username,connectionString);   // Reset player with $500 balance
                Console.WriteLine("\nYou are out of money!");
                Thread.Sleep(1500);
                Console.WriteLine("You will be reset with $500.");
                Thread.Sleep(1500);
                Console.WriteLine("\nPress the Enter key to return to the menu");
                Console.ReadLine();
            }
            else
            {
                UpdateBalance(balance, username, connectionString);
                Console.WriteLine($"Your balance is ${balance}.");
                Console.WriteLine("\nPress the Enter key to continue");
                Console.ReadLine();
            }
        }

        /*  Update player's balance
             PARAMETERS:
                 * double balance - Player's balance
                 * string connectionString - Connection string
         */
        static void UpdateBalance(double balance, string username, string connectionString)
        {
            db_query = "UPDATE Blackjack_db SET balance = @balance WHERE username = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(db_query, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@balance", balance);

                        // Execute query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected < 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Error: Failed to update balance.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}