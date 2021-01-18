using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CardStack player;
    public CardStack dealer;
    public CardStack deck;

    public Button playAgainButton;
    public Button dealBtn;
    public Button betBtn;
    public Button shuffleBtn;
    public Button RestartGameBtn;
    public Button RestartStatic;
    public Button Bet5Btn;

    public Text cashText;
    public Text betsText;
    public Text mainText;
    public Text potText;
    public Text PlayerValue;
    public Text DealerValue;
    public Text ShuffleCount;
    public Text GameIntroText;

    //Player and dealer scripts
    public PlayerScript playerScript;
    public PlayerScript dealerScript;


    //set the pot amount
    int pot = 0;

    //Card delt to each player
    //Player with higher card wins

    // #region Unity messages

    void Start()
    {
        GameIntroText.gameObject.SetActive(true);
        ShuffleCount.text = "Cards left until shuffle: " + deck.CardCount;
        dealBtn.onClick.AddListener(() => DealClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        Bet5Btn.onClick.AddListener(() => Bet5Clicked());
        playAgainButton.gameObject.SetActive(false);
        shuffleBtn.gameObject.SetActive(false);
        RestartGameBtn.gameObject.SetActive(false);
        betBtn.gameObject.SetActive(true);
        dealBtn.gameObject.SetActive(true);
        RestartStatic.gameObject.SetActive(true);
        playAgainButton.onClick.AddListener(() => PlayAgain());
        shuffleBtn.onClick.AddListener(() => ShuffleDeck());
        RestartGameBtn.onClick.AddListener(() => RestartGame());
        RestartStatic.onClick.AddListener(() => RestartGame());
    }

    // #endregion

    private void DealClicked()
    {
            StartGame();
            CheckDeck();
            GameIntroText.gameObject.SetActive(false);
            ShuffleCount.text = "Cards left until shuffle: " + deck.CardCount;
            dealBtn.gameObject.SetActive(false);
            betBtn.gameObject.SetActive(false); 
            Bet5Btn.gameObject.SetActive(false);
            PlayerValue.text = "YOU: " + player.CardValue(); 
            DealerValue.text = "DEALER: " + dealer.CardValue();
    }

    void Bet5Clicked()
    {
        //ensure player can't go into negatives

        if(playerScript.GetMoney() <= 4)
        {
            mainText.text = "Not Enough Funds";
            Bet5Btn.gameObject.SetActive(false);
            // RestartGameBtn.gameObject.SetActive(true);
        }
        else
        {
            Text newBet = Bet5Btn.GetComponentInChildren(typeof(Text)) as Text;
            int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
            playerScript.AdjustMoney(-intBet);
            cashText.text = "$" + playerScript.GetMoney().ToString() + ".00";
            pot += (intBet * 2);
            betsText.text = "Set Bet: $" + (pot / 2).ToString();
            potText.text = "To Win: $" + pot.ToString();
        }
    }


    void BetClicked()
    {
        //ensure player can't go into negatives

        if(playerScript.GetMoney() <= 0)
        {
            mainText.text = "Not Enough Funds";
            betBtn.gameObject.SetActive(false);
            // RestartGameBtn.gameObject.SetActive(true);
        }
        else
        {
            Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
            int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
            playerScript.AdjustMoney(-intBet);
            cashText.text = "$" + playerScript.GetMoney().ToString() + ".00";
            pot += (intBet * 2);
            betsText.text = "Set Bet: $" + (pot / 2).ToString();
            potText.text = "To Win: $" + pot.ToString();
        }
    }

    void StartGame()
    {
        for(int i = 0; i < 1; i++)
        {
            player.Push(deck.Pop());
            dealer.Push(deck.Pop());
            checkWinner();
        }
    }

    public void checkWinner()
    {
       
        if(player.CardValue() > dealer.CardValue())
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
            cashText.text ="$" + playerScript.GetMoney().ToString() + ".00";
            // playAgainButton.gameObject.SetActive(true);
        }
        else if(player.CardValue() < dealer.CardValue())
        {
            mainText.text = "You lose!";
            // playAgainButton.gameObject.SetActive(true);
        }
        else
        {
            mainText.text = "TIE: Bets returned";
            playerScript.AdjustMoney(pot / 2);
            // playAgainButton.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        //clears hand & shuffles deck   
        ShuffleDeck();
        mainText.text = "";

        //resets scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayAgain()
    {        
        if(playerScript.GetMoney() <= 0)
        {
            mainText.text = "Not Enough Funds";
            dealBtn.gameObject.SetActive(false);
            RestartGameBtn.gameObject.SetActive(true);
            betBtn.gameObject.SetActive(false);
            Bet5Btn.gameObject.SetActive(false);
            playAgainButton.gameObject.SetActive(false);
            pot = 0; 
        }
        else
        {
            ShuffleCount.text = "Cards left until shuffle: " + deck.CardCount;
            PlayerValue.text = "";
            DealerValue.text = "";
            dealBtn.gameObject.SetActive(true);
            playAgainButton.gameObject.SetActive(false);
            betBtn.gameObject.SetActive(true);
            Bet5Btn.gameObject.SetActive(true);
            betsText.text = "Set Bet: $0";
            potText.text = "To Win: $0";
            mainText.text = "";
            cashText.text = "$" + playerScript.GetMoney().ToString() + ".00";
            pot = 0;   
        }

    }

    //method to check if there are >= 10 cards left in the deck
    public void CheckDeck()
    {
        // print(deck.CardCount);
        //if there are 10 cards or more, play again
        CardStackView view = dealer.GetComponent<CardStackView>();
        if(deck.CardCount >= 11)
        {
            playAgainButton.gameObject.SetActive(true);
        }
        else
        {
            playAgainButton.gameObject.SetActive(false);
            shuffleBtn.gameObject.SetActive(true);
        }

    }

    public void ShuffleDeck()
    {
        player.GetComponent<CardStackView>().ClearHand();
        dealer.GetComponent<CardStackView>().ClearHand();
        deck.GetComponent<CardStackView>().ClearHand();
        DealerValue.text = "";
        PlayerValue.text = "";
        mainText.text = "Cards Shuffled";
        // player.RemoveCards();
        // dealer.RemoveCards();
        deck.CreateDeck();
        playAgainButton.gameObject.SetActive(true);
        shuffleBtn.gameObject.SetActive(false);
    }
}
