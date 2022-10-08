using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [System.Serializable]

    public class Entity
    {
        public string playerName;
        public Stone[] myStones;

        public bool hasTurn;
        public enum PlayerTypes
        {
            HUMAN,
            CPU,
            NO_PLAYER
        }

        public PlayerTypes playerType;
        public bool hasWon;


    }

    public List<Entity> playerList = new List<Entity>();

    //States Machine
    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    public States state;

    public int activePlayer;
    bool switchingPlayer;

    //Human inputs
    //GameObject for our button
    //int rolledHumanDice; 

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    {
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING:
                    {
                        ///Idle
                    }
                    break;
            }
        }
    }

    void RollDice()
    {
        int diceNumber = Random.Range(1, 7);
   
        
        MoveAStone(diceNumber);
        Debug.Log("Dice rolled number " + diceNumber);
    }

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        RollDice();
    }

    

    void MoveAStone(int diceNumber)
    {
        List<Stone> movableStones = new List<Stone>();

        for (int i = 0; i < playerList[activePlayer].myStones.Length; i++)
        {
            movableStones.Add(playerList[activePlayer].myStones[i]);
        }

        if (movableStones.Count > 0)
        {
            int num = Random.Range(0, movableStones.Count);
            movableStones[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }

        state = States.SWITCH_PLAYER;
        Debug.Log("should switch Player");
    }

    IEnumerator SwitchPlayer()
    {
        if (switchingPlayer)
        {
            yield break;
        }

        switchingPlayer = true;

        yield return new WaitForSeconds(2);
        //Set to next player
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;


        state = States.ROLL_DICE;
    }
}
