using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthImage;
    public Sprite[] healthSprites;

    private int maxHealth = 100;

    public void UpdateHealth(int currentHealth)
    {
        if (healthSprites == null || healthSprites.Length == 0) return;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        int index = currentHealth / (maxHealth / (healthSprites.Length - 1));

        index = Mathf.Clamp(index, 0, healthSprites.Length - 1);

        healthImage.sprite = healthSprites[index];
    }
}