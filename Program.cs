﻿/* Developed by Deep Patel (2024) */

double balance = 5000;
string? readResult;
string casino = "";
bool validEntry = false;
bool isPlaying = true;
bool isPlayingCasino = true;
bool reset = false;
bool hasInsurance = false;
int playerCardSum = 0;
int dealerCardSum = 0;
var cards = new Dictionary<string, List<int>> {
    {"2 of Spades", new List <int> {2}}, {"3 of Spades", new List <int> {3}}, {"4 of Spades", new List <int> {4}},
    {"5 of Spades", new List <int> {5}}, {"6 of Spades", new List <int> {6}}, {"7 of Spades", new List <int> {7}}, {"8 of Spades", new List <int> {8}},
    {"9 of Spades", new List <int> {9}}, {"10 of Spades", new List <int> {10}}, {"Jack of Spades", new List <int> {10}}, {"Queen of Spades", new List <int> {10}},
    {"King of Spades", new List <int> {10}}, {"Ace of Spades", new List <int> {1,11}},

    {"2 of Clubs", new List <int> {2}}, {"3 of Clubs", new List <int> {3}}, {"4 of Clubs", new List <int> {4}},
    {"5 of Clubs", new List <int> {5}}, {"6 of Clubs", new List <int> {6}}, {"7 of Clubs", new List <int> {7}}, {"8 of Clubs", new List <int> {8}},
    {"9 of Clubs", new List <int> {9}}, {"10 of Clubs", new List <int> {10}}, {"Jack of Clubs", new List <int> {10}}, {"Queen of Clubs", new List <int> {10}},
    {"King of Clubs", new List <int> {10}}, {"Ace of Clubs", new List <int> {1,11}},

    {"2 of Hearts", new List <int> {2}}, {"3 of Hearts", new List <int> {3}}, {"4 of Hearts", new List <int> {4}},
    {"5 of Hearts", new List <int> {5}}, {"6 of Hearts", new List <int> {6}}, {"7 of Hearts", new List <int> {7}}, {"8 of Hearts", new List <int> {8}},
    {"9 of Hearts", new List <int> {9}}, {"10 of Hearts", new List <int> {10}}, {"Jack of Hearts", new List <int> {10}}, {"Queen of Hearts", new List <int> {10}},
    {"King of Hearts", new List <int> {10}}, {"Ace of Hearts", new List <int> {1,11}},

    {"2 of Diamonds", new List <int> {2}}, {"3 of Diamonds", new List <int> {3}}, {"4 of Diamonds", new List <int> {4}},
    {"5 of Diamonds", new List <int> {5}}, {"6 of Diamonds", new List <int> {6}}, {"7 of Diamonds", new List <int> {7}}, {"8 of Diamonds", new List <int> {8}},
    {"9 of Diamonds", new List <int> {9}}, {"10 of Diamonds", new List <int> {10}}, {"Jack of Diamonds", new List <int> {10}}, {"Queen of Diamonds", new List <int> {10}},
    {"King of Diamonds", new List <int> {10}}, {"Ace of Diamonds", new List <int> {1,11}}
};

while (isPlaying)
{
    Console.Clear();
    Console.WriteLine("Welcome to Blackjack!");
    if (reset == true)
        balance = 500;
    Console.WriteLine($"Your current balance: ${balance}.\n");
    Console.WriteLine("To begin, choose your casino:\n  1. Las Vegas\t\t(Min Bet: $50)\n  2. Monte Carlo\t(Min Bet: $200)\n  3. Dubai\t\t(Min Bet: $500)\n");
    Console.WriteLine("Enter Q to exit.");

    // Check if casino choice is valid or if player exits
    do
    {
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
                isPlaying = false;
                validEntry = true;
            }
            else
            {
                Console.WriteLine("Error: Invalid entry.");
                Console.WriteLine("\rChoose either 1, 2, or 3.");
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

/* Main method, playing the casino
    PARAMETERS:
        * string casino - Name of the casino
        * int minBet - Casino's minimum bet
*/
void PlayCasino(string casino, int minBet) {
    while (isPlayingCasino)
    {
        bool playingGames = true;
        // Checking termination of round
        while (playingGames)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to {casino}!");
            if (reset == true) {    // Check if player ran out of money, reset with balance of $500
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

            (playingGames, playerCardSum, bet) = PlayerTurn(playerCards, dealerCards, bet);
            if (playingGames == false)
                break;
            DealerTurn(playerCards, dealerCards, bet);
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
(int, bool) EnterBet(int minBet)
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
(List<Tuple<string, List<int>>>, List<Tuple<string, List<int>>>) DealCards()
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

        if (i % 2 == 0)
        {
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

    //DEBUGGING - PRINT CARDS RECEIVED BY PLAYER AND DEALER
    /*  foreach (var playerCard in playerCards) {
        Console.Write($"{playerCard.Item1} - ");
        foreach (int value in playerCard.Item2) {
            Console.Write($"{value} ");
        }
        Console.WriteLine();
    }

    foreach (var dealerCard in dealerCards) {
        Console.Write($"{dealerCard.Item1} - ");
        foreach (int value in dealerCard.Item2) {
            Console.Write($"{value} ");
        }
        Console.WriteLine();
    } */

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
int CheckForInsurance(List<Tuple<string, List<int>>> dealerCards, List<Tuple<string, List<int>>> playerCards,int bet) {
    var dealerFirstCard = dealerCards[0].Item1;
    int maxInsuranceBet = bet/2;
    int insuranceBet = 0;
    playerCardSum = CalculateBestHand(playerCards);

    if (playerCardSum != 21) {
        if (dealerFirstCard == "Ace of Spades" ||
        dealerFirstCard == "Ace of Clubs" ||
        dealerFirstCard == "Ace of Hearts" ||
        dealerFirstCard == "Ace of Diamonds") 
        {
            if (balance > 0) {
                DisplayPlayersHand(playerCards, playerCardSum);
                Console.WriteLine();
                Console.WriteLine("--Dealer's Hand--");
                Console.WriteLine($"{dealerFirstCard}\nHidden Card");
                Console.WriteLine();
                Console.WriteLine($"\nDealer has an Ace, would you like to purchase insurance?\nYou may bet up to ${(balance > maxInsuranceBet ? maxInsuranceBet : balance)}. If you do not want insurance, enter 0.");
                do
                {
                    readResult = Console.ReadLine()?.ToLower().Trim();
                    bool validNumber = int.TryParse(readResult, out insuranceBet);
                    if (validNumber == false || insuranceBet > maxInsuranceBet)
                    {
                        Console.WriteLine($"Invalid entry. Insurance bet must be between 1 and {(balance > maxInsuranceBet ? maxInsuranceBet : balance)} or 0 for no insurance.");
                    }
                    if(insuranceBet == 0) {
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
(bool, int, int) CheckBlackJack(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet) {
    playerCardSum = CalculateBestHand(playerCards);
    dealerCardSum = CalculateBestHand(dealerCards);
    int insurance = CheckForInsurance(dealerCards, playerCards, bet);

    if (playerCardSum == 21)
    {
        if (dealerCardSum == 21)
        {
            Console.WriteLine("\nPush");
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
            CheckBalance(balance);
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
        if (insurance > 0) {
            Console.WriteLine("You won insurance!");
            balance += insurance * 2;
        }
        CheckBalance(balance);
        
        return (true, playerCardSum, dealerCardSum);
    }
    else {
        balance -= insurance;
    }

    Thread.Sleep(1000);

    return (false, playerCardSum, dealerCardSum);
}

/*  Calculates best value of hand (handling aces)
    PARAMETERS:
        * List<Tuple<string, List<int>>> cards - Hand of cards (player or dealer)
    RETURN VALUES:
        * int sum - Returns best calculated sum
*/
int CalculateBestHand(List<Tuple<string, List<int>>> cards)
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
(bool, int, int) PlayerTurn(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet)
{
    bool stand = false;
    bool canDouble = true;
    bool doubled = false;
    var (blackjack, playerCardSum, dealerCardSum) = CheckBlackJack(playerCards, dealerCards, bet);
    if (blackjack)
        return (false, 21, bet);

    while (playerCardSum <= 21 && stand == false)
    {
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
            if (doubled == true)
                break;
            if ((bet * 2) > balance)
                canDouble = false;
            if (hasInsurance == true)
                canDouble = false;
            // Loop that asks player to hit, double or stand, will continue until player busts or stands
            if (canDouble == true)
                Console.WriteLine("Would you like to (Hit) or (Double) or (Stand)?");
            else    
                Console.WriteLine("Would you like to (Hit) or (Stand)?");
            readResult = Console.ReadLine()?.ToLower().Trim();
            if (readResult == "hit" || readResult == "double")
            {
                if (readResult == "double" && canDouble == false) {
                    Console.Clear();
                    Console.WriteLine("**Invalid input, please enter (Hit) or (Stand)**\n");
                    break;
                }
                if (readResult == "double") {
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
                    CheckBalance(balance);
                    return (false, playerCardSum, bet);
                }

                Console.Clear();
                canDouble = false;
                validEntry = true;
            }
            else if (readResult == "stand")
            {
                stand = true;
                validEntry = true;
            }
            else
            {
                Console.Clear();
                if (canDouble == true)
                    Console.WriteLine("**Invalid input, please enter (Hit) or (Double) or (Stand)**\n");
                else {
                    Console.WriteLine("**Invalid input, please enter (Hit) or (Stand)**\n");
                }
            }
        } while (validEntry == false);
        if (reset == true)
            break;
    }

    return (true, playerCardSum, bet);
}

/*  Handling the dealer's turn
    PARAMETERS:
        * List<Tuple<string, List<int>>> playerCards - The player's hand
        * List<Tuple<string, List<int>>> dealerCards - The dealer's hand
        * int bet - The player's bet
*/
void DealerTurn(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet)
{
    dealerCardSum = CalculateBestHand(dealerCards); // Calculate dealer's sum before entering loop
    if (dealerCardSum >= 17 && dealerCardSum <= 20)
    {
        if (dealerCardSum > playerCardSum)
        {
            Console.Clear();
            DisplayPlayersHand(playerCards, playerCardSum);
            Console.WriteLine();
            DisplayDealersHand(dealerCards, dealerCardSum);

            Console.WriteLine("\nDealer wins.");
            CheckBalance(balance);
        }
        else if (playerCardSum > dealerCardSum)
        {
            Console.Clear();
            DisplayPlayersHand(playerCards, playerCardSum);
            Console.WriteLine();
            DisplayDealersHand(dealerCards, dealerCardSum);

            Console.WriteLine("\nYou win!");
            balance += bet * 2;
            CheckBalance(balance);
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

            if (dealerCardSum > 16 && dealerCardSum <= 21)
            {
                Console.Clear();
                DisplayPlayersHand(playerCards, playerCardSum);
                Console.WriteLine();
                DisplayDealersHand(dealerCards, dealerCardSum);

                if (dealerCardSum > playerCardSum)
                {
                    Console.WriteLine("\nDealer wins.");
                    CheckBalance(balance);
                    
                }
                else if (playerCardSum > dealerCardSum)
                {
                    Console.WriteLine("\nYou win!");
                    balance += bet * 2;
                    CheckBalance(balance);
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
                CheckBalance(balance);
            }
        } while (dealerCardSum < 17);
    }
}

/* Displays player's and dealer's hands
    PARAMETERS:
        * List<Tuple<string, List<int>>> playerCards - Player's hand
        * int playerCardSum - Sum of player's hand
        * List<Tuple<string, List<int>>> dealerCards - Dealer's hand
        * int dealerCardSum - Sum of dealer's hand
*/
void DisplayPlayersHand(List<Tuple<string, List<int>>> playerCards, int playerCardSum) {
    Console.WriteLine($"--Player's Hand ({playerCardSum})--");
    foreach (var playerCard in playerCards)
    {
        Console.WriteLine(playerCard.Item1);
    }
}
void DisplayDealersHand(List<Tuple<string, List<int>>> dealerCards, int dealerCardSum) {
    Console.WriteLine($"--Dealer's Hand ({dealerCardSum})--");
    foreach (var dealerCard in dealerCards)
    {
        Console.WriteLine(dealerCard.Item1);
    }
}

// Resets player's balance to $500 if their balance is 0
void ResetMoney() {
    Console.WriteLine("\nYou are out of money!");
    Thread.Sleep(1500);
    Console.WriteLine("You will be reset with $500.");
    Thread.Sleep(1500);
    Console.WriteLine("\nPress the Enter key to return to the menu");
    Console.ReadLine();
}

/* Checks if player's balance is < 50
    PARAMETERS:
        * double balance - Player's current balance
*/
void CheckBalance(double balance) {
    if (balance < 50) {
        reset = true;
        ResetMoney();
    }
    else {
        Console.WriteLine($"Your balance is ${balance}.");
        Console.WriteLine("\nPress the Enter key to continue");
        Console.ReadLine();
    }
}