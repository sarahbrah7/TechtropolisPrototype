using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [Header("ROUTES")]
    public Route commonRoute;
    public Route finalRoute;

    public List<Node> fullRoute = new List<Node>();

    [Header("NODES")]
    public Node startNode;

    public Node baseNode;
    public Node currentNode;
    public Node goalNode;

    int routePosition;
    //for the start position//
    int startNodeIndex;

    //rolled dice amount//
    int steps;
    int doneSteps;
    

    [Header("BOOLS")]

    public bool isOut;
    public bool isMoving;
    //for the human player//
    public bool hasTurn;

    void Start()
    {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);

        CreateFullRoute();
    }

    //This is where i should consider the infinite loop//
    void CreateFullRoute()
    {

        for (int i = 0; i < commonRoute.childNodeList.Count; i++)
        {
            int tempPos = startNodeIndex + i;
            //avoids overflow of the list//
            tempPos %= commonRoute.childNodeList.Count;

            fullRoute.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodeList.Count; i++)
        {
            fullRoute.Add(finalRoute.childNodeList[i].GetComponent<Node>());
        }
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 7);

            Debug.Log("Dice number " + steps);
            StartCoroutine(Move());
            if(doneSteps + steps < fullRoute.Count)
            {
                CreateFullRoute();
            }
            
        }
    }

    IEnumerator Move()
    {
       

        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        while (steps > 0)
        {
            routePosition++;
            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            while (MoveToNextNode(nextPos)){yield return null;}
            yield return new WaitForSeconds(0.1f);
            steps--;
            doneSteps++;

        }

        GameManager.instance.state = GameManager.States.ROLL_DICE;
         

        isMoving = false;

        
    }

    bool MoveToNextNode(Vector3 goalPos)
    {
        return goalPos != (transform.position = Vector3.MoveTowards(transform.position,goalPos,2f * Time.deltaTime));
        
    }

    //DID NOT PUT IN WRITE UP YET!!
    public bool CheckPossible(int diceNumber)
    {
        int tempPos = routePosition + diceNumber;
        if(tempPos >= fullRoute.Count)
        {
            return false;
        }

        return !fullRoute[tempPos].isTaken;

    }

    public void StartTheMove(int diceNumber)
    {
        steps = diceNumber;
        StartCoroutine(Move());
    }

}
