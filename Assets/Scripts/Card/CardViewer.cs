using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardViewer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Card _card;

    public void Initialize(Card card)
    {
        _card = card;
    }

    public void ViewFront() {
        _image.sprite = _card.SpriteFront;
    }

    public void ViewBack()
    {
        _image.sprite = _card.SpriteBack;
    }
}
