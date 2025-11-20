using UnityEngine;
using TMPro; // si usas TextMeshPro

public class FragmentUI : MonoBehaviour
{
    public TextMeshProUGUI countText;

    public void UpdateCount(int amount)
    {
        countText.text = amount.ToString();
    }
}
