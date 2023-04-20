using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadesAmountView : MonoBehaviour
{
    [SerializeField] private Image _grenadePrefab;
    [SerializeField] private RectTransform _parent;

    private List<Image> _grenades = new List<Image>();

    public void SetMaxValue(int value)
    {
        foreach (var grenade in _grenades)
        {
            Destroy(grenade.gameObject);
        }

        _grenades.Clear();

        for (int i = 0; i < value; i++)
        {
            var grenade = Instantiate(_grenadePrefab, _parent);
            _grenades.Add(grenade);
        }
    }

    public void SetCurrentValue(int value)
    {
        var grenadesToRemove = new List<Image>();
        for (int i = 0; i < _grenades.Count; i++)
        {
            if (i >= value)
            {
                grenadesToRemove.Add(_grenades[i]);
            }
        }

        foreach (var image in grenadesToRemove)
        {
            _grenades.Remove(image);
            Destroy(image.gameObject);
        }
    }
}