using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Croupier : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _buttonsBet;
    [SerializeField] private Button _buttonHit;
    [SerializeField] private Button _buttonStand;

    [Header("Text info")]
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private GameObject _infoGameObject;

    [Header("Settings")]
    [SerializeField] private TMP_Text _betText;
    [SerializeField] private Deck _deck;
    [SerializeField] private TMP_Text _playerScoreText;
    [SerializeField] private Player _player;

    [Header("Parameters")]
    [SerializeField] private int _croupierScore = 0;
    [SerializeField] private int _playerScore = 0;

    private int _bet = 0;
    private bool _playerStand = false;
    private System.Random rand = new System.Random();

    private void Start()
    {
        _buttonHit.interactable = false;
        _buttonStand.interactable = false;
    }

    private void NewGame()
    {
        ViewInfo("Новая партия!");

        _deck.DiscardCards();

        _croupierScore = 0;
        _playerScore = 0;
        _playerScoreText.text = _playerScore.ToString();

        _deck.HandOutCard(EWhose.Player);
        _deck.HandOutCard(EWhose.Croupier);
        _deck.HandOutCard(EWhose.Player);
        _deck.HandOutCard(EWhose.Croupier, EViewCard.Back);

        _playerStand = false;

        SetInterectibleButtonsGame(true);
        SetInterectibleButtonsBet(false);

        Checking();
    }

    private void PlayerScoring()
    {
        _playerScore = 0;

        if (_deck.TryGetPlayerCards(out List<Card> cards))
        {
            if (cards.Count == 2)
                if (cards[0].Name == "Ace" && cards[1].Name == "Ace")
                {
                    _playerScore = 21;
                    _playerScoreText.text = _playerScore.ToString();
                    return;
                }


            foreach (Card d in cards)
            {
                if(cards.Count == 2)
                {
                    _playerScore += d.ElseValue;
                }
                else
                    _playerScore += d.Value;
            }
        }

        _playerScoreText.text = _playerScore.ToString();
    }

    private void CroupierScoring()
    {
        _croupierScore = 0;

        if (_deck.TryGetCroupierCards(out List<Card> cards))
        {
            if (cards.Count == 2)
                if (cards[0].Name == "Ace" && cards[1].Name == "Ace")
                {
                    _croupierScore = 21;
                    return;
                }


            foreach (Card d in cards)
            {
                if (cards.Count == 2)
                {
                    _croupierScore += d.ElseValue;
                }
                else
                    _croupierScore += d.Value;
            }
        }
    }

    private void Checking()
    {
        PlayerScoring();
        CroupierScoring();
       
        if (_playerScore == 21 &&_croupierScore == 21)
        {
            ViewInfo("Ничья!");
            _player.GiveBet(_bet);

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(_playerScore == 21)
        {
            ViewInfo("Ставка игрока!");
            _player.GiveBet(_bet * 2);

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if (_croupierScore == 21)
        {
            ViewInfo("Ставка крупье!");

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(_playerScore > 21 && _croupierScore > 21)
        {
            ViewInfo("Ничья!");
            _player.GiveBet(_bet);

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(_playerScore > 21)
        {
            ViewInfo("Ставка крупье!");

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if (_croupierScore > 21)
        {
            ViewInfo("Ставка игрока!");
            _player.GiveBet(_bet * 2);

            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }

        if (!_playerStand)
        {
            return;
        }

        int playerPointsUpTo21 = 21 - _playerScore; 
        int croupierPointsUpTo21 = 21 - _croupierScore;

        if(croupierPointsUpTo21 != 0 && 
            croupierPointsUpTo21 > playerPointsUpTo21)
        {
            if (croupierPointsUpTo21 > rand.Next(4, 5))
            {
                _deck.HandOutCard(EWhose.Croupier);
                Checking();
                return;
            }
        }

        if(playerPointsUpTo21 == croupierPointsUpTo21)
        {
            ViewInfo("Ничья!");
            _player.GiveBet(_bet);
            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(playerPointsUpTo21 < 0)
        {
            ViewInfo("Ставка крупье!");
            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(croupierPointsUpTo21 < 0)
        {
            ViewInfo("Ставка игрока!");
            _player.GiveBet(_bet * 2);
            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else if(playerPointsUpTo21 < croupierPointsUpTo21)
        {
            ViewInfo("Ставка игрока!");
            _player.GiveBet(_bet * 2);
            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
        else
        {
            ViewInfo("Ставка крупье!");
            SetInterectibleButtonsGame(false);
            SetInterectibleButtonsBet(true);
            _deck.OpenCroupierCards();
        }
    }

    private void ViewInfo(string mess)
    {
        _infoText.text = mess;

        _infoGameObject.SetActive(false);
        _infoGameObject.SetActive(true);
    }

    private void PlaceBet(int bet)
    {
        if (_player.TryTakeBet(bet))
        {
            _bet = bet;
            _betText.text = _bet.ToString();

            SetInterectibleButtonsBet(false);
            SetInterectibleButtonsGame(true);

            NewGame();
        }
        else
        {
            ViewInfo("Недостаточно средств!");
        }
    }

    private void SetInterectibleButtonsGame(bool mode)
    {
        _buttonHit.interactable = mode;
        _buttonStand.interactable = mode;
    }

    private void SetInterectibleButtonsBet(bool mode)
    {
        foreach (Button d in _buttonsBet)
        {
            d.interactable = mode;
        }
    }

    private IEnumerator DisactivateInfoText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _infoGameObject.SetActive(false);
    }

    public void Hit()
    {
        _deck.HandOutCard(EWhose.Player);
        Checking();
    }

    public void Stand()
    {
        SetInterectibleButtonsGame(false);

        _playerStand = true;
        _deck.OpenCroupierCards();
        Checking();        
    }

    public void Bet10()
    {
        PlaceBet(10);
    }

    public void Bet50()
    {
        PlaceBet(50);
    }

    public void Bet100()
    {
        PlaceBet(100);
    }
}
