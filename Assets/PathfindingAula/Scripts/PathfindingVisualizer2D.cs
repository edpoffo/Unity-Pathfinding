using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingVisualizer2D : MonoBehaviour
{
    public enum SearchMode { AStar, Dijkstras, BFS }
    public SearchMode searchMode = SearchMode.AStar;

    public Node2D nodePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float nodeSpacing = 1.1f;

    public float stepDelay = 0.2f;
    public float heuristicWeight = 1f;

    private Node2D[,] grid;
    private Node2D startNode;
    private Node2D goalNode;

    private bool isRunning = false;

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRunning)
            StartCoroutine(RunSelectedAlgorithm());
        if (Input.GetKeyDown(KeyCode.Delete))
            ResetColors();
    }

    void GenerateGrid()
    {
        grid = new Node2D[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Node2D node = Instantiate(nodePrefab, new Vector3(x * nodeSpacing, y * nodeSpacing, 0), Quaternion.identity, transform);
                node.Initialize(x, y, this);
                grid[x, y] = node;
            }
        }

        // Cria conexões N, S, L, O
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Node2D node = grid[x, y];
                node.neighbors = new List<Node2D>();

                if (x > 0) node.neighbors.Add(grid[x - 1, y]);
                if (x < gridWidth - 1) node.neighbors.Add(grid[x + 1, y]);
                if (y > 0) node.neighbors.Add(grid[x, y - 1]);
                if (y < gridHeight - 1) node.neighbors.Add(grid[x, y + 1]);
            }
        }
    }

    public void SetStart(Node2D node)
    {
        if (isRunning) return;
        if (startNode != null) startNode.SetNormal();
        startNode = node;
        node.SetStart();
    }

    public void SetGoal(Node2D node)
    {
        if (isRunning) return;
        if (goalNode != null) goalNode.SetNormal();
        goalNode = node;
        node.SetGoal();
    }

    IEnumerator RunSelectedAlgorithm()
    {
        if (startNode == null || goalNode == null)
            yield break;

        isRunning = true;

        switch (searchMode)
        {
            case SearchMode.AStar:
                yield return StartCoroutine(RunAStar());
                break;
            case SearchMode.Dijkstras:
                yield return StartCoroutine(RunDijkstra());
                break;
            case SearchMode.BFS:
                yield return StartCoroutine(RunBFS());
                break;
        }

        isRunning = false;
    }

    // -------------------------------------------------------------------------
    // A* ALGORITHM
    // -------------------------------------------------------------------------
    IEnumerator RunAStar()
    {
        List<Node2D> openSet = new List<Node2D>();
        List<Node2D> closedSet = new List<Node2D>();

        foreach (var node in grid)
        {
            node.gCost = Mathf.Infinity;
            node.hCost = 0;
            node.parent = null;
        }

        startNode.gCost = 0;
        startNode.hCost = Heuristic(startNode, goalNode);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node2D current = GetLowestFCost(openSet);
            if (current == goalNode)
            {
                HighlightPath(goalNode);
                yield break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node2D neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeG = current.gCost + neighbor.cost;

                if (!openSet.Contains(neighbor) || tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor, goalNode) * heuristicWeight;
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            UpdateNodeColors(openSet, closedSet);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    // -------------------------------------------------------------------------
    // 🧭 DIJKSTRA ALGORITHM
    // -------------------------------------------------------------------------
    IEnumerator RunDijkstra()
    {
        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();

        foreach (var node in grid)
        {
            node.gCost = Mathf.Infinity;
            node.parent = null;
        }

        startNode.gCost = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node2D current = GetLowestGCost(openSet);
            if (current == goalNode)
            {
                HighlightPath(goalNode);
                yield break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node2D neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeG = current.gCost + neighbor.cost;

                if (!openSet.Contains(neighbor) || tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            UpdateNodeColors(openSet, closedSet);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    // -------------------------------------------------------------------------
    // 🔍 BFS ALGORITHM
    // -------------------------------------------------------------------------
    IEnumerator RunBFS()
    {
        Queue<Node2D> queue = new Queue<Node2D>();
        HashSet<Node2D> visited = new HashSet<Node2D>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node2D current = queue.Dequeue();

            if (current == goalNode)
            {
                HighlightPath(goalNode);
                yield break;
            }

            foreach (Node2D neighbor in current.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    neighbor.parent = current;
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            UpdateNodeColors(queue, visited);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    // -------------------------------------------------------------------------
    void HighlightPath(Node2D end)
    {
        Node2D current = end;
        while (current != null)
        {
            if (current != startNode && current != goalNode) current.SetPath();
            
            current = current.parent;
        }
    }

    float Heuristic(Node2D a, Node2D b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    Node2D GetLowestFCost(List<Node2D> list)
    {
        Node2D lowest = list[0];
        foreach (var node in list)
            if (node.FCost < lowest.FCost)
                lowest = node;
        return lowest;
    }

    Node2D GetLowestGCost(List<Node2D> list)
    {
        Node2D lowest = list[0];
        foreach (var node in list)
            if (node.gCost < lowest.gCost)
                lowest = node;
        return lowest;
    }

    void UpdateNodeColors(IEnumerable openSet, IEnumerable closedSet)
    {
        foreach (var node in grid)
            if (node == startNode) node.SetStart();
            else if (node == goalNode) node.SetGoal();
            else (node).UpdateColorByCost();
            

        foreach (Node2D node in openSet)
            if (node == startNode) node.SetStart();
            else if (node == goalNode) node.SetGoal();
            else node.SetOpenColor();

        foreach (Node2D node in closedSet)
            if (node == startNode) node.SetStart();
            else if (node == goalNode) node.SetGoal();
            else node.SetClosedColor();
    }

    public void ResetColors()
    {
        foreach (var node in grid)
            if(node != startNode && node != goalNode)
                node.SetNormal();
    }
}
