using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int _balance = 1000;
    [SerializeField] private TMP_Text _balanceText;

    private void Start()
    {
        _balanceText.text = _balance.ToString();
    }

    public bool TryTakeBet(int bet)
    {
        if (bet > _balance)
            return false;

        _balance -= bet;
        _balanceText.text = _balance.ToString();
        return true;
    }

    public void GiveBet(int bet)
    {
        _balance += bet;
        _balanceText.text = _balance.ToString();
    }

    public bool BalanceIsEmpty()
    {
        return _balance == 0;
    }
}
