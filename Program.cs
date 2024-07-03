﻿﻿double balance = 5000;
string? readResult;
string casino = "";
int minBet;
bool validEntry = false;
bool isPlaying = true;
int playerCardSum;
int dealerCardSum;
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
        readResult = Console.ReadLine()?.ToLower().Trim();
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
                    Console.Clear();
                    Console.WriteLine("Welcome to Las Vegas!");
                    Console.WriteLine($"Your balance is ${balance}");
                    var(bet, quit) = EnterBet(minBet);
                    if (quit == true) {
                        isPlayingCasino = false;
                        break;
                    }
                    var (playerCards, dealerCards) = DealCards();   // Get the initial dealt cards
                    
                    (playingGames,playerCardSum) = PlayerTurn(playerCards, dealerCards, bet);

                    DealerTurn(playerCards, dealerCards, bet);  
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

    Console.WriteLine($"Enter your bet (Min: {minBet}) or Q to exit casino.");
    do {
        bet = Console.ReadLine()?.ToLower().Trim();
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
(List<Tuple<string, List<int>>>, List<Tuple<string, List<int>>>) DealCards() {
    // Retrieve all cards
    List<string> deck = new List<string>(cards.Keys);
    Random dealCard = new Random();
    List<Tuple<string, List<int>>> playerCards = new List<Tuple<string, List<int>>>();
    List<Tuple<string, List<int>>> dealerCards = new List<Tuple<string, List<int>>>();

    // Deal cards to player and dealer in an alternating fashion where the dealer's second card is kept hidden
    for (int i = 0; i < 4; i++) {
        int index = dealCard.Next(0, deck.Count);
        string randomCard = deck[index];

        if (i % 2 == 0) {
            Console.WriteLine($"You are dealt the {randomCard}.");
            playerCards.Add(Tuple.Create(randomCard, cards[randomCard]));
            Thread.Sleep(1000);
        }
        else if (i == 3) {
            Console.WriteLine($"Dealer is dealt a random card.");
            dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));
            Thread.Sleep(1000);
        }
        else {
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

    return (playerCards, dealerCards);
}

(bool, int) PlayerTurn(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet) {
    var(blackjack, playerCardSum, dealerCardSum) = CheckBlackJack(playerCards, dealerCards, bet);
    if (blackjack)
        return (false,21);

    bool stay = false;
    while (playerCardSum <= 21 && stay == false) {
        Console.Clear();
        playerCardSum = CalculateBestHand(playerCards);

        Console.WriteLine($"--Player's Hand ({playerCardSum})--");
        foreach (var playerCard in playerCards) {
            Console.WriteLine(playerCard.Item1);
        }

        Console.WriteLine();
        Console.WriteLine("--Dealer's Hand--");
        var dealerFirstCard = dealerCards[0].Item1;
        Console.WriteLine($"{dealerFirstCard}\nHidden Card");
        Console.WriteLine();

        Console.WriteLine("Would you like to (Hit) or (Stay)?");
        readResult = Console.ReadLine()?.ToLower().Trim();
        if (readResult != null) {
            validEntry = false;
            do {
                if (readResult == "hit") {
                    Random dealCard = new Random();
                    List<string> deck = new List<string>(cards.Keys);
                    int index = dealCard.Next(0, deck.Count);
                    string randomCard = deck[index];

                    playerCards.Add(Tuple.Create(randomCard, cards[randomCard]));

                    playerCardSum = CalculateBestHand(playerCards);

                    if (playerCardSum > 21) {
                        Console.Clear();
                        Console.WriteLine($"--Player's Hand ({playerCardSum})--");
                        foreach (var playerCard in playerCards) {
                            Console.WriteLine(playerCard.Item1);
                        }

                        Console.WriteLine();
                        Console.WriteLine("--Dealer's Hand--");
                        Console.WriteLine($"{dealerFirstCard}\nHidden Card");
                        Console.WriteLine($"\nYou were dealt the {randomCard}.");
                        Console.WriteLine($"You busted with a total of {playerCardSum}.");
                        balance -= bet;
                        Console.WriteLine($"Your balance is ${balance}.");
                        Console.WriteLine("\nPress the Enter key to continue");
                        Console.ReadLine();
                        return (false,playerCardSum);
                    }

                    validEntry = true;
                } else if (readResult == "stay") {
                    stay = true;
                    validEntry = true;
                } else {
                    Console.WriteLine("Invalid input, please enter (Hit) or (Stay)");
                }
            } while (validEntry == false);
        }
    }

    return (true,playerCardSum);
}

void DealerTurn(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet) {
    Console.Clear();
    dealerCardSum = CalculateBestHand(dealerCards); // Calculate dealer's sum before entering loop

    do {
        Console.Clear();

        Console.WriteLine($"--Player's Hand ({playerCardSum})--");
        foreach (var playerCard in playerCards) {
            Console.WriteLine(playerCard.Item1);
        }

        Console.WriteLine();

        Console.WriteLine($"--Dealer's Hand ({dealerCardSum})--");
        foreach (var dealerCard in dealerCards) {
            Console.WriteLine(dealerCard.Item1);
        }

        Console.WriteLine("\nDealer will now draw a card.");
        Thread.Sleep(2000);

        Random dealCard = new Random();
        List<string> deck = new List<string>(cards.Keys);
        int index = dealCard.Next(0, deck.Count);
        string randomCard = deck[index];

        dealerCards.Add(Tuple.Create(randomCard, cards[randomCard]));

        dealerCardSum = CalculateBestHand(dealerCards);

        if (dealerCardSum > 16 && dealerCardSum <= 21) {
            Console.Clear();
            Console.WriteLine($"--Player's Hand ({playerCardSum})--");
            foreach (var playerCard in playerCards) {
                Console.WriteLine(playerCard.Item1);
            }

            Console.WriteLine();
            Console.WriteLine($"--Dealer's Hand ({dealerCardSum})--");
            foreach (var dealerCard in dealerCards) {
                Console.WriteLine(dealerCard.Item1);
            }

            if(dealerCardSum > playerCardSum) {
                Console.WriteLine("\nDealer wins.");
                balance -= bet;
                Console.WriteLine($"Your balance is ${balance}.");
                Console.WriteLine("\nPress the Enter key to continue");
                Console.ReadLine();
            }
            else if(playerCardSum > dealerCardSum) {
                Console.WriteLine("You win!");
                balance += bet;
                Console.WriteLine($"Your balance is ${balance}.");
                Console.WriteLine("\nPress the Enter key to continue");
                Console.ReadLine();
            }
            else if(playerCardSum == dealerCardSum) {
                Console.WriteLine("Push.");
                Console.WriteLine("\nPress the Enter key to continue");
                Console.ReadLine();
            }
        }
        else if (dealerCardSum > 21) {
            Console.Clear();
            Console.WriteLine($"--Player's Hand ({playerCardSum})--");
            foreach (var playerCard in playerCards) {
                Console.WriteLine(playerCard.Item1);
            }

            Console.WriteLine();
            Console.WriteLine($"--Dealer's Hand ({dealerCardSum})--");
            foreach (var dealerCard in dealerCards) {
                Console.WriteLine(dealerCard.Item1);
            }
            Console.WriteLine($"Dealer busted with a total of {dealerCardSum}.");
            balance += bet;
            Console.WriteLine($"Your balance is ${balance}.");
            Console.WriteLine("\nPress the Enter key to continue");
            Console.ReadLine();
        }
    } while (dealerCardSum < 17);
}

// Calculates best value of hand (handling aces)
int CalculateBestHand(List<Tuple<string, List<int>>> cards) {
    int sum = 0;
    int aceCount = 0;

    foreach (var card in cards) {
        // Check if the card is an Ace
        if (card.Item2.Contains(11)) {
            aceCount++;
            sum += 11; // Add Ace as value of 11
        } else {
            sum += card.Item2[0];
        }
    }

    // Adjust for Aces if the sum exceeds 21
    while (sum > 21 && aceCount > 0) {
        sum -= 10;
        aceCount--;
    }

    return sum;
}


// Checks if either the player or the dealer has a blackjack
(bool, int, int) CheckBlackJack(List<Tuple<string, List<int>>> playerCards, List<Tuple<string, List<int>>> dealerCards, int bet) {
    int playerCardSum = CalculateBestHand(playerCards);
    int dealerCardSum = CalculateBestHand(dealerCards);

    if(playerCardSum == 21) {
        if(dealerCardSum == 21) {
            Console.WriteLine("\nPush");
            Console.WriteLine("\nPress Enter to continue");
            Console.ReadLine();
            return (false, playerCardSum, dealerCardSum);
        } else {
            Console.WriteLine("\nYou have a blackjack!");
            balance += bet * 1.5;
            Console.WriteLine($"Your new balance is ${balance}.");
            Console.WriteLine("\nPress Enter to continue");
            Console.ReadLine();
            return (true, playerCardSum, dealerCardSum);
        }
    } else if(dealerCardSum == 21) {
        Console.WriteLine("\nDealer has a blackjack.");
        var hiddenCard = dealerCards[1].Item1;
        Console.WriteLine($"Dealer's hidden card is {hiddenCard}");
        balance -= bet;
        Console.WriteLine($"Your new balance is ${balance}.");
        Console.WriteLine("\nPress Enter to continue");
        Console.ReadLine();
        return (true, playerCardSum, dealerCardSum);
    }

    Thread.Sleep(1000);

    return (false, playerCardSum, dealerCardSum);
}