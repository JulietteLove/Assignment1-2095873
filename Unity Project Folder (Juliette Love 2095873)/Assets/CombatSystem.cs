using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatState { START, PLAYERROLL, PLAYERCOMBAT, ENEMYTURN, WON, LOST }
public class CombatSystem : MonoBehaviour
{
    public bool CanRoll = false;
    public bool EnemyCanRoll = false;
    public CombatState state;

    public GameObject LoseUI;
    public GameObject WinUI;

    public GameObject MeleeAttackButton;
    public GameObject FireballAttackButton;

    public GameObject enemyMissExplanation;
    public bool FirstTimeEnemyMiss = true;
    void Start()
    {
        state = CombatState.PLAYERROLL;
    }

    void Update()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Enemy enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        DiceScript diceScript = GameObject.FindWithTag("Dice").GetComponent<DiceScript>();
        AttackScript attackScript = GameObject.FindWithTag("CombatSystem").GetComponent<AttackScript>();

        if (state == CombatState.PLAYERROLL)
        {
            PlayerRollDice();
        }
        
        if (state == CombatState.PLAYERCOMBAT)         
        {
            Debug.Log("I am in combat");
            MeleeAttackButton.SetActive(true);
            FireballAttackButton.SetActive(true);
        }
        else
        {
            MeleeAttackButton.SetActive(false);
            FireballAttackButton.SetActive(false);
        }

        if (state == CombatState.ENEMYTURN)
        {
            Debug.Log("Enemy is in combat");
            Invoke("EnemyRoll", 2f);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }


    public void EnemyRoll() //CODE DEALING WITH ENEMY TURN
    {
        if (EnemyCanRoll == true)
        {
            DiceScript diceScript = GameObject.FindWithTag("Dice").GetComponent<DiceScript>();

            diceScript.EnemyNumberRolled = Random.Range(1, 6);
            diceScript.EnemyDiceText.GetComponent<UnityEngine.UI.Text>().text = diceScript.EnemyNumberRolled.ToString("F0"); 

            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

            if (player.defenceNumber >= diceScript.EnemyNumberRolled) //Enemy defends itself.
            {
                diceScript.ConsoleText.text = "Enemy misses";
                Invoke("PlayerTurn", 2f);
                Debug.Log("Enemy has missed");

                if(FirstTimeEnemyMiss == true)
                {
                    enemyMissExplanation.SetActive(true);
                    Invoke("MissTextDisappear", 3f);
                    FirstTimeEnemyMiss = false;
                }

            }

            if (player.defenceNumber < diceScript.EnemyNumberRolled) //Deals damage to player.
            {
                player.currentHealth -= 0.2f;
                diceScript.playerHealth.fillAmount = player.currentHealth / 1;
                Invoke("PlayerTurn", 2f);
                Debug.Log("Enemy has hit");

                if (player.currentHealth < 0.1f || player.currentHealth > 1f) //Player Loses
                {
                    Debug.Log("Has Lost");
                    LoseUI.SetActive(true);
                }
            }

            EnemyCanRoll = false;
        }
    }

    void PlayerTurn()
    {
        DiceScript diceScript = GameObject.FindWithTag("Dice").GetComponent<DiceScript>();

        state = CombatState.PLAYERROLL;
        diceScript.ConsoleText.text = "Your turn";
    }

    void PlayerRollDice()
    {
        CanRoll = true;
    }

    void MissTextDisappear()
    {
        enemyMissExplanation.SetActive(false);
    }
}
