using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Ñard / Create new Card", order = 51)]
public class Card : ScriptableObject
{
    [SerializeField] private string _suit;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _spriteFront;
    [SerializeField] private Sprite _spriteBack;
    [SerializeField] private int _value;
    [SerializeField] private int _elseValue;

    public string Suit => _suit;
    public string Name => _name;
    public Sprite SpriteFront => _spriteFront;
    public Sprite SpriteBack => _spriteBack;
    public int Value => _value;
    public int ElseValue => _elseValue;
}
