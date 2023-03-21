using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : ObjectPool
{
    [Header("Positions card")]
    [SerializeField] private Transform _startPositionCroupierCard;
    [SerializeField] private Transform _startPositionPlayerCard;
    [Header("Cards")]
    [SerializeField] private float _offsetCardPosition;
    [SerializeField] private GameObject _prefabCard;
    [SerializeField] private Card[] _deck = new Card[52];

    private List<int> _playerCards = new List<int>();
    private List<int> _croupierCards = new List<int>();

    private int _currentCard = 0;
    private System.Random rand = new System.Random();

    private Vector2 _positionPlayerCard;
    private Vector2 _positionCroupierCard;

    private void Awake()
    {
        _capacity = _deck.Length;
        _container = this.transform;

        ShuffleDeck();
        Initialize(_prefabCard, _deck);

        _positionPlayerCard = _startPositionPlayerCard.position;
        _positionCroupierCard = _startPositionCroupierCard.position;
    }

    private void ViewCard(int indexCard, bool setActiv, Vector2 position, EViewCard direction)
    {
        TryViewCard(indexCard, setActiv, position, direction);
    }

    private void ShuffleDeck()
    {
        for (int i = _deck.Length - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);

            var temp = _deck[j];
            _deck[j] = _deck[i];
            _deck[i] = temp;
        }
    }

    public void HandOutCard(EWhose whose, EViewCard diraction = EViewCard.Front)
    {
        if(_currentCard > _deck.Length - 1)
        {
            ShuffleDeck();
            _currentCard = 0;
        }

        if (whose == EWhose.Player)
        {
            _positionPlayerCard.x += _offsetCardPosition;
            _playerCards.Add(_currentCard);
            ViewCard(_currentCard++, true, _positionPlayerCard, diraction);
        }
        else
        {
            _positionCroupierCard.x += _offsetCardPosition;
            _croupierCards.Add(_currentCard);
            ViewCard(_currentCard++, true, _positionCroupierCard, diraction);
        }
    }

    public void DiscardCards()
    {
        _positionPlayerCard = _startPositionPlayerCard.position;
        _positionCroupierCard = _startPositionCroupierCard.position;

        foreach (int index in _playerCards)
        {
            ViewCard(index, false, transform.position, EViewCard.Back);
        }
        _playerCards.Clear();

        foreach (int index in _croupierCards)
        {
            ViewCard(index, false, transform.position, EViewCard.Back);
        }
        _croupierCards.Clear();
    }

    public void OpenCroupierCards()
    {
        _positionCroupierCard = _startPositionCroupierCard.position;
        foreach (int i in _croupierCards)
        {
            _positionCroupierCard.x += _offsetCardPosition;
            ViewCard(i, true, _positionCroupierCard, EViewCard.Front);
        }
    }

    public bool TryGetPlayerCards(out List<Card> cards)
    {
        cards = new List<Card>();
        foreach (int i in _playerCards)
        {
            cards.Add(_deck[i]);
        }

        return _playerCards.Count > 0;
    }

    public bool TryGetCroupierCards(out List<Card> cards)
    {
        cards = new List<Card>();
        foreach (int i in _croupierCards)
        {
            cards.Add(_deck[i]);
        }

        return _croupierCards.Count > 0;
    }
}
