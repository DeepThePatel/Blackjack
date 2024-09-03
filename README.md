> NOTICE: Attempting to login/signup may not be functional at times. This is due to the deallocation of the database server to avoid incurring cloud service charges as the purpose of this project was to gain experience using different technologies and the cloud. If you would still like to play the game without logging in/signing up, you may use the link below to download the v1.3.0 release. If you would like to login/register and the credential functionalities are not working, please contact me directly via email: deepkalpana1@aol.com. Thank you for your interest!

Blackjack console game created using the .NET Framework programmed in C#.

*All code is written and owned by me.*

> NOTE: To play the game without creating an account, download the [Blackjack v1.3.0 Release](https://github.com/DeepThePatel/Blackjack/releases/tag/v1.3.0) (Docker image unavailable)

<h2>Installation</h2>

*You may choose either of the three methods that best suits you*

<br>

**Download the .ZIP**
1. Download the [latest release](https://github.com/DeepThePatel/Blackjack/releases) .ZIP folder for your respective OS
2. Unzip the folder in your desired location
3. Run the Blackjack application

<br>

**Download the Docker Image**
1. Run the command below in your terminal
```
docker pull ghcr.io/deepthepatel/blackjack:latest
```
2. Run the command below. The ```-it``` flag ensures the image runs in interactive mode. You may rename ```blackjack-container```
```
docker run -it --name blackjack-container ghcr.io/deepthepatel/blackjack:latest
```
   
*To run the app again with the existing container:*
```
docker start blackjack-container
```
```
docker attach blackjack-container
```

> NOTE: After running the attach command, the terminal may appear to do nothing, press ```Enter``` to see the application running

<br>

**Clone the Repository**
1. Clone the project into a folder on your computer
```
git clone https://github.com/DeepThePatel/Blackjack.git
```
2. Ensure you have the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed
3. Open the project folder in a new terminal in Visual Studio
4. Download the [necessary packages](https://github.com/DeepThePatel/Blackjack/wiki/Software-Documentation#libraries-and-packages)
5. Run
```
dotnet build
```
6. Then
```
dotnet run
```

<br>

<h2>Main Menu</h2>

Upon successful login/account creation, player will be notified of their initial balance.

The player will have the choice to pick from three (3) different casinos:

1. &nbsp;&nbsp;Las Vegas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**(Min. $50)**
2. &nbsp;&nbsp;Monte Carlo&nbsp;&nbsp;&nbsp;&nbsp;**(Min. $200)**
3. &nbsp;&nbsp;Dubai&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**(Min. $500)**

Player will also have the option to quit (```Q```) the game.
<br>
<br>

<h2>Inside the Casino</h2>

Once the preferred casino is selected, player is prompted to enter a bet within the accepted value range or to exit (```Q```) the casino.

Once a valid bet is placed, the program begins to deal the player and dealer their hands in an alternating fashion.

<br>

*<h3>Insurance</h3>*

Once the cards are dealt and in the event that the dealer's first card is an Ace (where the player does **NOT** have a blackjack), the player has the ability to bet ```Insurance```.
- The player is able to bet up to half of their original bet or ```0``` if they do not wish to take insurance.
- If insurance is taken and the player wins, they will receive their original bet and 2x their insurance.
- If insurance is taken and the player loses, they will lose their original bet and their insurance.
- If insurance is not taken, the game will continue.

<br>

*<h3>Blackjack</h3>*

Before the program continues, it checks to see whether the player or the dealer have a blackjack. 
- If either the player or the dealer have a blackjack, the player will be notified and the round will end.
- In the event that both the player and the dealer have a blackjack, the program will notify the player that there is a ```Push```.

<br>

*<h3>The Player's Turn</h3>*

Once the cards are dealt, player is shown their hand, the total of their hand, and the dealer's hand. 

They are then given the option to ```Hit``` or ```Double``` or ```Stay```. (Players may only double down once on their first turn. Players will **NOT** be able to double down after hitting a card)

```Hit```&nbsp;: &nbsp;A random card is drawn into the player's hand

If the player does not bust:
- The sum of the player's hand is updated
- Player is given the option to ```Hit``` or ```Stay```
- This process is repeated until the player busts or decides to stay
  
If the player busts:
- Player is notified they busted along with the total of their hand
- Player's balance is updated
- Player is then redirected to beginning where they can choose to bet again or exit the casino

```Split```&nbsp;: &nbsp; Player has the ability to split their hand should their initial drawn cards equal in value.

> **NOTE:** In this version of blackjack, the player is NOT able to double down on split hands.

```Double```&nbsp;: &nbsp; Player will be given the opportunity to double their bet and draw only one (1) card.

> **NOTE:** In this version of blackjack, the player is able to double down on any sum of their hand, not only 9, 10, and 11.

```Stay```&nbsp;: &nbsp;Player does not draw a card and the program proceeds to play the dealer's hand

<br>

*<h3>The Dealer's Turn</h3>*

Once the player decides to stand with their hand or doubles down without busting, the dealer begins to draw cards.

The dealer will continue to draw cards until they either land on at least a soft 17 or bust, which then the program will determine the winner.

<br>

<h2>Rules of Blackjack</h2>
This program follows Bicycle Cards official blackjack rules:
https://bicyclecards.com/how-to-play/blackjack

<br>
<br>

**Important Notes:**
- The Shuffle and Cut section is ignored as the cards dealt are randomly generated by the program.
- In this version of blackjack, player is able to double down on any sum of their hand, not only 9, 10, and 11.
- In this version of blackjack, player is NOT able to double down on split hands.
- Dealer must stand on soft 17
- Blackjack pays 3:2
