int balance = 5000;
string? readResult;
string casino = "";
int minBet = 0;
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

Console.WriteLine("Welcome to Blackjack!");
Console.WriteLine($"Your current balance is ${balance}\n");
Console.WriteLine("To begin, choose your casino:\n  1. Las Vegas\t\t(Min Bet: 50)\n  2. Monte Carlo\t(Min Bet: 500)\n  3. Dubai\t\t(Min Bet: 1000)\n");

// Check if input is valid
readResult = Console.ReadLine();
if (readResult != null) {
    casino = readResult.ToLower();
}

Console.Clear();

switch (casino)
{
    // Las Vegas
    case "1":
        minBet = 50;
        Console.WriteLine("Welcome to Las Vegas!");
        int bet = EnterBet(minBet);
        Play();
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
    
    // Default
    default:
        Console.WriteLine("Error: Invalid entry.");
        Console.WriteLine("\rPress the Enter key to continue.");
        Console.ReadLine();
        break;
}

int EnterBet(int minBet) {
    string? bet;
    int intBet;
    bool validEntry = false;

    Console.WriteLine($"Enter your bet (Min: {minBet})");
    do {
        bet = Console.ReadLine();
        bool validNumber = int.TryParse(bet, out intBet);
        if (validNumber == false || intBet > balance || intBet < minBet) {
            Console.WriteLine($"Invalid entry. Bet must be between ${minBet} and ${balance}");
        }
        else {
            validEntry = true;
        }
    } while (validEntry == false);
        
    return intBet;
}

void Play() {

    // Retrieve all cards
    List<string> deck = new List<string>(cards.Keys);
    Random dealCard = new Random();
    Dictionary<string, List <int>> playerCards = new Dictionary<string, List<int>>();
    Dictionary<string, List <int>> dealerCards = new Dictionary<string, List<int>>();

    // Deal cards to player and dealer in an alternating fashion where the dealer's second card is kept hidden
    for (int i = 0; i < 4; i++) {
        int index = dealCard.Next(0, deck.Count);
        string randomCard = deck[index];
        if (i % 2 == 0) {
            Console.WriteLine($"You are dealt the {randomCard}.");
            playerCards.Add(randomCard, cards[randomCard]);
            Thread.Sleep(1000);
        }
        else if (i == 3) {
            Console.WriteLine("Dealer is dealt a random card.");
            dealerCards.Add(randomCard, cards[randomCard]);
            Thread.Sleep(1000);
        }
        else {
            Console.WriteLine($"Dealer is dealt the {randomCard}.");
            playerCards.Add(randomCard, cards[randomCard]);
            Thread.Sleep(1000);
        }
        /*  Uncomment to play with single deck
        cards.Remove(randomCard);
        deck.RemoveAt(index);
        */
    }
}