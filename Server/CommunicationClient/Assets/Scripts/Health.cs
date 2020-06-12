using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthBar;
    public int health;

    private int currentHealth;

    bool isClient;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        healthBar.maxValue = health;

        SetBar();
    }

    public void TakeDamage(int damage)
    {
        if (!isClient)
        {
            return;
        }

        currentHealth -= damage;

        Client.Instance.SendNumber(currentHealth);

        SetBar();

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        //Destroy(gameObject);
    }

    public void SetNetworkedHealth (int _h)
    {
        currentHealth = _h;
        SetBar();
    }

    void SetBar()
    {
        healthBar.value = currentHealth;
    }

    public void SetHealthStatus()
    {
        isClient = true;
    }

    public bool GetHealthStatus()
    {
        return isClient;
    }
}
