using UnityEngine;
using TMPro;

public class Node2D : MonoBehaviour
{
    public int x, y;
    public float cost = 1;
    public float gCost, hCost;
    public float FCost => gCost + hCost;
    public Node2D parent;
    public SpriteRenderer spriteRenderer;
    public TextMeshPro label;
    public PathfindingVisualizer2D grid;
    public System.Collections.Generic.List<Node2D> neighbors;

    private Color baseColor = Color.white;

    public void Initialize(int x, int y, PathfindingVisualizer2D grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        spriteRenderer = GetComponent<SpriteRenderer>();
        label = GetComponentInChildren<TextMeshPro>();
        UpdateLabel();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            grid.SetStart(this);
        else if (Input.GetMouseButtonDown(1))
            grid.SetGoal(this);
        else if (Input.mouseScrollDelta.y != 0)
        {
            cost = Mathf.Clamp(cost + Input.mouseScrollDelta.y, 0, 10);
            UpdateColorByCost();
            UpdateLabel();
        }
    }

    public void SetStart() => spriteRenderer.color = Color.green;
    public void SetGoal() => spriteRenderer.color = Color.red;
    public void SetPath() => spriteRenderer.color = Color.yellow;
    public void SetOpenColor() => spriteRenderer.color = Color.Lerp(Color.cyan, Color.black, cost / 10f);
    public void SetClosedColor() => spriteRenderer.color = Color.Lerp(Color.magenta, Color.black, cost / 10f);

    public void SetNormal()
    {
        UpdateColorByCost();
        UpdateLabel();
    }

    public void UpdateColorByCost()
    {
        spriteRenderer.color = Color.Lerp(Color.white, Color.black, cost / 10f);
    }

    public void UpdateLabel()
    {
        label.text = $"G:{gCost:0.0}\nH:{hCost:0.0}\nF:{FCost:0.0}\nC:{cost:0}";
    }
}