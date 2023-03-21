using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    protected Transform _container;
    protected int _capacity;

    private List<GameObject> _pool = new List<GameObject>();
    private List<CardViewer> _cardViewers = new List<CardViewer>();

    protected void Initialize(GameObject preafab, Card[] cards)
    {
        for(int i = 0; i < _capacity; i++)
        {
            GameObject spawned = Instantiate(preafab, _container);
            spawned.SetActive(false);
            _pool.Add(spawned);
        }

        for (int i = 0; i < _capacity; i++)
        {
            if (TryGetCardViewer(i, out CardViewer viewer))
            {
                viewer.Initialize(cards[i]);
                viewer.ViewBack();

                _cardViewers.Add(viewer);
            }
        }            
    }

    protected bool TryGetCardViewer(int index, out CardViewer viewer)
    {
        viewer = null;

        if (index < 0 || index > _capacity)
            return false;

        viewer = _pool[index].GetComponent<CardViewer>();
        return true;
    }

    protected GameObject GetGameObject(int index)
    {
        if (index < 0 || index > _capacity)
            throw new System.Exception("Индекс вне диапазона!");

        return _pool[index];
    }

    protected bool TryViewCard(int index, bool setActive, Vector3? position = null, EViewCard direction = EViewCard.Back)
    {
        if (index < 0 || index > _capacity)
            return false;

        _pool[index].SetActive(setActive);

        if(position != null)
            _pool[index].transform.position = position.Value;

        if (direction == EViewCard.Front)
            _cardViewers[index].ViewFront();
        else
            _cardViewers[index].ViewBack();

        return true;
    }
}
