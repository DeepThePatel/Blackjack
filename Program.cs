﻿int balance = 5000;
string? readResult;
string casino = "";
int minBet;
bool validEntry = false;
bool isPlaying = true;
var cards = new Dictionary<string, List <int>> {
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

while(isPlaying) {
    Console.Clear();
    Console.WriteLine("Welcome to Blackjack!");
    Console.WriteLine($"Your current balance: ${balance}\n");
    Console.WriteLine("To begin, choose your casino:\n  1. Las Vegas\t\t(Min Bet: $50)\n  2. Monte Carlo\t(Min Bet: $500)\n  3. Dubai\t\t(Min Bet: $1000)\n");
    Console.WriteLine("Enter Q to exit.");

    // Check if casino choice is valid or if player exits
    do
    {
        readResult = Console.ReadLine()?.ToLower();
        if (readResult != null) {
            if(readResult == "1" || readResult == "2" || readResult == "3") {
                casino = readResult;
                validEntry = true;
            }
            else if (readResult == "q") {
                isPlaying = false;
                validEntry = true;
            }
            else {
                Console.WriteLine("Error: Invalid entry.");
                Console.WriteLine("\rChoose either 1, 2, or 3.");
            }
        }
    } while (validEntry == false);

    // Necessary to validate player quitting after exiting casino
    if(isPlaying == false) {
        break;
    }

    Console.Clear();

    switch (casino)
    {
        // Las Vegas
        case "1":
            minBet = 50;
            bool isPlayingCasino = true;

            // Playing the casino
            while(isPlayingCasino) {
                bool playingGames = true;
                // Checking termination of round
                while (playingGames) {
                    Console.WriteLine("Welcome to Las Vegas!");
                    var(bet, quit) = EnterBet(minBet);
                    if (quit == true) {
                        isPlayingCasino = false;
                        break;
                    }
                    var (playerCards, dealerCards) = DealCards();   // Get the initial dealt cards
                    
                    playingGames = Play(playerCards, dealerCards);  
                }
            }
            break;  

        // Monte Carlo
        case "2":
            minBet = 500;
            Console.WriteLine("Welcome to Monte Carlo!");
            break;

        // Dubai    
        case "3":
            minBet = 1000;
            Console.WriteLine("Welcome to Dubai!");
            break;
    }
}
    

// Validate user entered bet amount
(int, bool) EnterBet(int minBet) {
    string? bet;
    int intBet;
    bool validEntry = false;
    bool quit = false;

    Console.WriteLine($"Enter your bet (Min: {minBet}) or Q to quit");
    do {
        bet = Console.ReadLine()?.ToLower();
        if (bet == "q") {
            quit = true;
            return (0,quit);
        }
        bool validNumber = int.TryParse(bet, out intBet);
        if (validNumber == false || intBet > balance || intBet < minBet) {
            Console.WriteLine($"Invalid entry. Bet must be between ${minBet} and ${balance}");
        }
        else {
            validEntry = true;
        }
    } while (validEntry == false);
        
    return (intBet,quit);
}

// Dealing initial cards
(Dictionary<string, List<int>>, Dictionary<string, List<int>>) DealCards() {
    // Retrieve all cards
    List<string> deck = new List<string>(cards.Keys);
    Random dealCard = new Random();
    Dictionary<string, List<int>> playerCards = new Dictionary<string, List<int>>();
    Dictionary<string, List<int>> dealerCards = new Dictionary<string, List<int>>();

    // Deal cards to player and dealer in an alternating fashion where the dealer's second card is kept hidden
    for (int i = 0; i < 4; i++) {
        int index = dealCard.Next(0, deck.Count);
        string randomCard = deck[index];

        if (i % 2 == 0) {
            Console.WriteLine($"You are dealt the {randomCard}.");
            if (!playerCards.ContainsKey(randomCard))
            {
                playerCards.Add(randomCard, cards[randomCard]);
            }
            else
            {
                playerCards[randomCard].AddRange(cards[randomCard]);
            }
            Thread.Sleep(1000);
        }
        else if (i == 3) {
            Console.WriteLine("Dealer is dealt a random card.");
            if (!dealerCards.ContainsKey(randomCard))
            {
                dealerCards.Add(randomCard, cards[randomCard]);
            }
            else
            {
                dealerCards[randomCard].AddRange(cards[randomCard]);
            }
            Thread.Sleep(1000);
        }
        else {
            Console.WriteLine($"Dealer is dealt the {randomCard}.");
            if (!dealerCards.ContainsKey(randomCard))
            {
                dealerCards.Add(randomCard, cards[randomCard]);
            }
            else
            {
                dealerCards[randomCard].AddRange(cards[randomCard]);
            }
            Thread.Sleep(1000);
        }
        /*  Uncomment to play with SINGLE DECK
        cards.Remove(randomCard);
        deck.RemoveAt(index);
        */
    }

    // Print player and dealer cards
    foreach (KeyValuePair<string, List<int>> playerCard in playerCards) {
        Console.Write($"{playerCard.Key} - ");
        foreach (int value in playerCard.Value) {
            Console.Write($"{value} ");
        }
        Console.WriteLine();
    }

    foreach (KeyValuePair<string, List<int>> dealerCard in dealerCards) {
        Console.Write($"{dealerCard.Key} - ");
        foreach (int value in dealerCard.Value) {
            Console.Write($"{value} ");
        }
        Console.WriteLine();
    }

    return (playerCards, dealerCards);
}

bool Play(Dictionary<string, List<int>> playerCards, Dictionary<string, List<int>> dealerCards) {
    Thread.Sleep(2000);
    Console.Clear();
    bool blackjack = CheckBlackJack(playerCards, dealerCards);
    if (blackjack == true)
        return false;

    return true;
}

// Calculates best value of hand (handling aces)
int CalculateBestHand(Dictionary<string, List<int>> cards) {
    int sum = 0;
    int aceCount = 0;

    foreach (var card in cards) {
        foreach (int value in card.Value) {
            if (value == 11) {
                aceCount++;
            }
            sum += value;
            break;
        }
    }

    // Adjust for Aces
    while (sum > 21 && aceCount > 0) {
        sum -= 10;
        aceCount--;
    }

    return sum;
}

// Checks if either the player or the dealer has a blackjack
bool CheckBlackJack(Dictionary<string, List<int>> playerCards, Dictionary<string, List<int>> dealerCards) {
    int playerCardSum = CalculateBestHand(playerCards);
    int dealerCardSum = CalculateBestHand(dealerCards);

    if(playerCardSum == 21) {
        if(dealerCardSum == 21) {
            Console.WriteLine("Push");
            return false;
        }
        else {
            Console.WriteLine("You have a blackjack!");
            return true;
        }
    }
    else if(dealerCardSum == 21) {
        Console.WriteLine("Dealer has a blackjack.");
        return true;
    }

    return false;
}