using UnityEngine;
using System.Collections;
 
public class Health : MonoBehaviour
{

    public int attack = 20;
    public int heroHealth = 100;
    public int currentheroHealth;
    public bool isDamage = false;
 //   UIManager uiManager;

    void Awake()
    {
        currentheroHealth = heroHealth;
//        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    public void TakeDamage(int damage)
    {
        isDamage = true;
        currentheroHealth -= damage;
        if (currentheroHealth > 0)
        {
            GetComponent<Animator>().SetTrigger("Damage");
            GetComponent<AudioSource>().Play();
        }
        
//        uiManager.updateHealthBar(currentheroHelth);
    }
    private void Update()
    {
 //       Debug.Log(currentheroHealth);   
    }

}
