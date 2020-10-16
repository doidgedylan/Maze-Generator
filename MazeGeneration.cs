using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    public GameObject XCube;
    private const int MAZE_SIZE = 400;
    private Node[] nodes = new Node[MAZE_SIZE];
    private int numRowAndColumn;
    private Node start;
    private int startIndex;
    private Node current;
    private Node temp = new Node();

    // Start is called before the first frame update
    void Start()
    {
        PopulateNodeArray();
        //Debug.Log(nodes[0].adjacentNodes[0].x + " " + nodes[0].adjacentNodes[0].y);
        //Debug.Log(nodes[4].adjacentNodes[1].x + " " + nodes[4].adjacentNodes[1].y);
        //Debug.Log(nodes[4].adjacentNodes[2].x + " " + nodes[4].adjacentNodes[2].y);
        //Debug.Log(nodes[4].adjacentNodes[3].x + " " + nodes[4].adjacentNodes[3].y);
        ChooseRandomStartingNode();
        //Debug.Log(start.index);
        ChooseNextNode();
        //Debug.Log(current.index);
        while (current.index != startIndex)
        {
            ChooseNextNode();
            //Debug.Log(current.index);
        }
        DrawMaze();
    }

    void DrawMaze()
    {
        for (int i = 0; i < MAZE_SIZE; i++)
        {
            for (int j = 0; j < 4; j++) 
            {
                if (nodes[i].edges[j] == 0)
                {
                    Debug.Log("NODE: " + i);
                    if (j == 0)
                    {
                        Instantiate(XCube, new Vector3(nodes[i].x - 0.5f, 0.5f, nodes[i].y), Quaternion.identity);
                        Debug.Log("Upper Wall: " + nodes[i].x + "," + nodes[i].y);
                    }
                    if (j == 1)
                    {
                        Instantiate(XCube, new Vector3(nodes[i].x, 0.5f, nodes[i].y - 0.5f), Quaternion.Euler(0, 90, 0));
                        Debug.Log("Left Wall: " + nodes[i].x+ "," + nodes[i].y);
                    }
                    if (j == 2)
                    {
                        Instantiate(XCube, new Vector3(nodes[i].x + 0.5f, 0.5f, nodes[i].y), Quaternion.identity);
                        Debug.Log("Bottom Wall: " + nodes[i].x + "," + nodes[i].y);
                    }
                    if (j == 3)
                    {
                        Instantiate(XCube, new Vector3(nodes[i].x, 0.5f, nodes[i].y + 0.5f), Quaternion.Euler(0, 90, 0));
                        Debug.Log("Right Wall: " + nodes[i].x + "," + nodes[i].y);
                    }
                }
            }
        }
    }

    void PopulateNodeArray()
    {
        numRowAndColumn = (int)Mathf.Sqrt(MAZE_SIZE);
        int i = 0;
        for (int j = 0; j < numRowAndColumn; j++)
        {
            for (int k = 0; k < numRowAndColumn; k++)
            {
                Node node = new Node();
                node.x = j;
                node.y = k;
                node.index = i;
                nodes[i] = node;
                i++;
            }

        }
        for (i = 0; i < MAZE_SIZE; i++)
        {
            if (i >= numRowAndColumn)
            {
                nodes[i].adjacentNodes[0] = nodes[i - numRowAndColumn];
            }
            if (nodes[i].y != 0)
            {
                nodes[i].adjacentNodes[1] = nodes[i - 1];
            }
            if (i <= MAZE_SIZE - numRowAndColumn - 1)
            {
                nodes[i].adjacentNodes[2] = nodes[i + numRowAndColumn];
            }
            if (nodes[i].y != numRowAndColumn - 1)
            {
                nodes[i].adjacentNodes[3] = nodes[i + 1];
            }
        }
    }

    void ChooseRandomStartingNode()
    {
        int num = (int)UnityEngine.Random.Range(0, MAZE_SIZE - 1);
        start = nodes[num];
        start.visited = true;
        current = start;
        startIndex = start.index;
    }

    void ChooseNextNode()
    {
        List<Node> neighbors = new List<Node>();
        for (int i = 0; i < 4; i++) {
            if (current.adjacentNodes[i] != null && current.adjacentNodes[i].visited == false)
            {
                if (current.edges[i] == 0)
                {
                    neighbors.Add(current.adjacentNodes[i]);
                }
            }
        }
        if (neighbors.Count == 0)
        {
            current = current.last;
        }
        else
        {
            int num = (int)UnityEngine.Random.Range(0, neighbors.Count);
            temp = current;
            current = neighbors[num];
            current.last = temp;
            current.visited = true;
            if (current.index - current.last.index == numRowAndColumn)
            {
                current.edges[0] = 1;
                current.last.edges[2] = 1;
            }
            else if (current.last.index - current.index == numRowAndColumn)
            {
                current.edges[2] = 1;
                current.last.edges[0] = 1;
            }
            else if (current.last.index - current.index == 1)
            {
                current.edges[3] = 1;
                current.last.edges[1] = 1;
            }
            else
            {
                current.edges[1] = 1;
                current.last.edges[3] = 1;
            }
        }
    }

    public class Node
    {
        public bool visited = false;
        public Node last = null;
        public int index = 0;
        public int x;
        public int y;
        public int[] edges = { 0, 0, 0, 0 };
        public Node[] adjacentNodes = new Node[4];
    }
}
