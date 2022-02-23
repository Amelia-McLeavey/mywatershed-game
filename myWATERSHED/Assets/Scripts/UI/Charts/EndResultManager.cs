using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndResultManager : MonoBehaviour
{
    [SerializeField] private UIGridRenderer grid; 
    [SerializeField] private UILineRenderer redDaceLine;
    [SerializeField] private RectTransform dot;

    public void CallStart()
    {
        grid.gridSize = new Vector2Int(1, 25);
        redDaceLine.gridSize = new Vector2Int(1, 25);

        redDaceLine.points.Clear();
        redDaceLine.points.Add(new Vector2(0f, 0f));
    }

    public void AddDataPoint(int year, float data)
    {
        grid.gridSize = new Vector2Int(year, 25);
        redDaceLine.gridSize = new Vector2Int(year, 25);   

        if (year == 1)
        {
            redDaceLine.points.Add(new Vector2(0, data / 1000f));
        }
        redDaceLine.points.Add(new Vector2(year, data / 1000f));

    }

    private void Update()
    {
        Vector2 mousePos = new Vector2();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.rectTransform, Input.mousePosition, null, out mousePos);
        
        if(mousePos.x<0 || mousePos.y < 0 || mousePos.x>grid.rectTransform.rect.width || mousePos.y > grid.rectTransform.rect.height)
        {
            return;
        }

        float gapBetweenLines = grid.rectTransform.rect.width / grid.gridSize.x;

        float closestLine = Mathf.Round(mousePos.x / gapBetweenLines) * gapBetweenLines;

        

        dot.anchoredPosition = new Vector2(closestLine, 0f);
    }
}
