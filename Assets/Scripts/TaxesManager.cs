using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaxesManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _taxesValue;
    [SerializeField] private Image _fillArea;

    private int taxes;

    public int Taxes { get => taxes; set => taxes = value; }

    private void Start()
    {
        _slider.value = Taxes;
        _taxesValue.text = taxes + "%";
    }

    private void Update()
    {
        if (taxes != (int)_slider.value)
        {
            taxes = (int)_slider.value;
            _taxesValue.text = taxes + "%";
        }
    }
}
