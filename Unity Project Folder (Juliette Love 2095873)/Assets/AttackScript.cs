using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackScript : MonoBehaviour
{
    public GameObject MeleeAttackButton;
    public GameObject FireballAttackButton;

    public Image enemyHealth;
    public Text ConsoleText;

    public int FireballBurn;

    public bool PlayerCanAttack = true;

    public GameObject WinUI;
    public GameObject LoseUI;

    //CODE DEALING WITH PLAYER COMBAT STAGE

    public void FireballButton()
    {
        Enemy enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        CombatSystem combatSystem = GameObject.FindWithTag("CombatSystem").GetComponent<CombatSystem>();

        FireballBurn = Random.Range(1, 6);

        if (FireballBurn >= 3 && PlayerCanAttack == true)
        {
            enemy.currentHealth -= 0.4f;
            enemyHealth.fillAmount = enemy.currentHealth / 1;

            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            DiceScript diceScript = GameObject.FindWithTag("Dice").GetComponent<DiceScript>();

            player.currentHealth -= 0.2f;
            diceScript.playerHealth.fillAmount = player.currentHealth / 1;
            ConsoleText.text = "You were burned";

            if (enemy.currentHealth < 0.1f || enemy.currentHealth > 1f) //Player Wins
            {
                Debug.Log("Has Won");
                WinUI.SetActive(true);
            }

            if (player.currentHealth < 0.1f || player.currentHealth > 1f) //Player Loses
            {
                Debug.Log("Has Lost");
                LoseUI.SetActive(true);
            }

            PlayerCanAttack = false;
        }
        
        if (FireballBurn < 3 && PlayerCanAttack == true)
        {
            enemy.currentHealth -= 0.4f;
            enemyHealth.fillAmount = enemy.currentHealth / 1;

            ConsoleText.text = "You were not burned";

            if (enemy.currentHealth < 0.1f || enemy.currentHealth > 1f) //Player Wins
            {
                Debug.Log("Has Won");
                WinUI.SetActive(true);
            }

            PlayerCanAttack = false;
        }

        Invoke("EnemyTurnChange", 3f);
    }

    public void EnemyTurnChange()
    {
        CombatSystem combatSystem = GameObject.FindWithTag("CombatSystem").GetComponent<CombatSystem>();

        combatSystem.state = CombatState.ENEMYTURN;
        ConsoleText.text = "Enemy Turn";
        combatSystem.EnemyCanRoll = true;
    }

    public void MeleeButton()
    {
        Enemy enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        enemy.currentHealth -= 0.2f;
        enemyHealth.fillAmount = enemy.currentHealth/1;

        if (enemy.currentHealth < 0.1f || enemy.currentHealth > 1f) //Player Wins
        {
            Debug.Log("Has Won");
            WinUI.SetActive(true);
        }

        CombatSystem combatSystem = GameObject.FindWithTag("CombatSystem").GetComponent<CombatSystem>();

        combatSystem.state = CombatState.ENEMYTURN;
        ConsoleText.text = "Enemy Turn";
        combatSystem.EnemyCanRoll = true;
    }
}
