using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image fillImage;

    public void SetMaxHealth(float maxHealth)
    {
        fillImage.fillAmount = 1f;
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}
