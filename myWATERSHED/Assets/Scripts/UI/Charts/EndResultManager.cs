using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndResultManager : MonoBehaviour
{
    [SerializeField] private UIGridRenderer grid; 
    [SerializeField] private UILineRenderer redDaceLine;
    [SerializeField] private UILineRenderer chubLine;
    [SerializeField] private UILineRenderer troutLine;
    [SerializeField] private RectTransform playerLine;
    [SerializeField] private RectTransform daceDot;
    [SerializeField] private RectTransform chubDot;
    [SerializeField] private RectTransform troutDot;

    [SerializeField] private TMP_Text[] xAxisNumbers;
    [SerializeField] private TMP_Text[] yAxisNumbers;

    private List<float> redDaceValues = new List<float>();
    private float maxRedDaceValue = 0f;
    private float minRedDaceValue = 999999f;

    private List<float> chubValues = new List<float>();
    private float maxChubValue = 0f;
    private float minChubValue = 999999f;

    private List<float> troutValues = new List<float>();
    private float maxTroutValue = 0f;
    private float minTroutValue = 999999f;

    private int graphSize;

    [SerializeField] private RectTransform rect;

    public List<CardInstance> cardInstances = new List<CardInstance>();

    public bool showChub = true;
    public bool showTrout = true;

    public void CallStart()
    {
        grid.gridSize = new Vector2Int(10, 50);
        redDaceLine.gridSize = new Vector2Int(10, 50);
        chubLine.gridSize = new Vector2Int(10, 50);
        troutLine.gridSize = new Vector2Int(10, 50);

        redDaceLine.points.Clear();
        chubLine.points.Clear();
        troutLine.points.Clear();
        //redDaceLine.points.Add(new Vector2(0f, 0f));
    }

    public void AddDataPoint(int year, float redDacePop, float chubPop, float troutPop)
    {    
        if (redDacePop > maxRedDaceValue)
        {
            maxRedDaceValue = redDacePop;
        }
        if (redDacePop < minRedDaceValue)
        {
            minRedDaceValue = redDacePop;
        }
        if (chubPop > maxChubValue)
        {
            maxChubValue = chubPop;
        }
        if (chubPop < minChubValue)
        {
            minChubValue = chubPop;
        }
        if (troutPop > maxTroutValue)
        {
            maxTroutValue = troutPop;
        }
        if (troutPop < minTroutValue)
        {
            minTroutValue = troutPop;
        }


        graphSize = (Mathf.FloorToInt(year / 10) + 1) * 10;
        grid.gridSize = new Vector2Int(graphSize, 50);
        redDaceLine.gridSize = new Vector2Int(graphSize, 50);
        chubLine.gridSize = new Vector2Int(graphSize, 50);
        troutLine.gridSize = new Vector2Int(graphSize, 50);

        if (year == 1)
        {
            redDaceValues.Add(redDacePop);
            redDaceLine.points.Add(new Vector2(0, 0f));

            chubValues.Add(chubPop);
            chubLine.points.Add(new Vector2(0, 0f)); 
            
            troutValues.Add(troutPop);
            troutLine.points.Add(new Vector2(0, 0f));
        }
        redDaceValues.Add(redDacePop);
        redDaceLine.points.Add(new Vector2(year, 0f));

        chubValues.Add(chubPop);
        chubLine.points.Add(new Vector2(year, 0f));

        troutValues.Add(troutPop);
        troutLine.points.Add(new Vector2(year, 0f));


        for (int i=0; i< xAxisNumbers.Length; i++)
        {
            xAxisNumbers[i].text = (i * (graphSize / 10)).ToString();
        }

        LayoutPoints();
    }

    private void LayoutPoints()
    {
        float minGraphValue = Mathf.Max(Mathf.FloorToInt(minRedDaceValue / 500) * 500, 0f);
        float maxGraphValue = Mathf.CeilToInt(maxRedDaceValue / 500) * 500;

        if (showChub)
        {
            minGraphValue = Mathf.Min(minGraphValue, (Mathf.Max(Mathf.FloorToInt(minChubValue / 500) * 500, 0f)));
            maxGraphValue = Mathf.Max(maxGraphValue, Mathf.CeilToInt(maxChubValue / 500) * 500);
        }

        if (showTrout)
        {
            minGraphValue = Mathf.Min(minGraphValue, (Mathf.Max(Mathf.FloorToInt(minTroutValue / 500) * 500, 0f)));
            maxGraphValue = Mathf.Max(maxGraphValue, Mathf.CeilToInt(maxTroutValue / 500) * 500);
        }

        yAxisNumbers[0].text = minGraphValue.ToString();
        yAxisNumbers[1].text = ((minGraphValue + maxGraphValue) / 2).ToString();
        yAxisNumbers[2].text = maxGraphValue.ToString();

        redDaceLine.gameObject.SetActive(false);
        chubLine.gameObject.SetActive(false);
        troutLine.gameObject.SetActive(false);


        for (int i = 0; i < redDaceLine.points.Count; i++)
        {
            redDaceLine.points[i] = new Vector2(i, (redDaceValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * 50);//new Vector2((i*10f)/(float)graphSize, (redDaceValues[i]-minGraphValue)/(maxGraphValue-minGraphValue) *50);//redDaceValues[i]/

            if (showChub)
            {
                chubLine.points[i] = new Vector2(i, (chubValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * 50);
            }

            if (showTrout)
            {
                troutLine.points[i] = new Vector2(i, (troutValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * 50);
            }

        }

        redDaceLine.gameObject.SetActive(true);
        if (showChub)
        {
            chubLine.gameObject.SetActive(true);
        }
        if (showTrout)
        {
            troutLine.gameObject.SetActive(true);
        }
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

        playerLine.anchoredPosition = new Vector2(closestLine, 0f);

        if (redDaceLine.points.Count > Mathf.RoundToInt(closestLine / gapBetweenLines))
        {
            daceDot.gameObject.SetActive(true);
            daceDot.anchoredPosition = new Vector2(0f, redDaceLine.points[Mathf.RoundToInt(closestLine / gapBetweenLines)].y * rect.rect.height / 50f);
        }
        else
        {
            daceDot.gameObject.SetActive(false);
        }

        if (chubLine.points.Count > Mathf.RoundToInt(closestLine / gapBetweenLines) && showChub)
        {
            chubDot.gameObject.SetActive(true);
            chubDot.anchoredPosition = new Vector2(0f, chubLine.points[Mathf.RoundToInt(closestLine / gapBetweenLines)].y * rect.rect.height / 50f);
        }
        else
        {
            chubDot.gameObject.SetActive(false);
        }

        if (troutLine.points.Count > Mathf.RoundToInt(closestLine / gapBetweenLines) && showTrout)
        {
            troutDot.gameObject.SetActive(true);
            troutDot.anchoredPosition = new Vector2(0f, troutLine.points[Mathf.RoundToInt(closestLine / gapBetweenLines)].y * rect.rect.height / 50f);
        }
        else
        {
            troutDot.gameObject.SetActive(false);
        }
    }


    public void toggleChub()
    {
        showChub = !showChub;
        LayoutPoints();
    }
    public void toggleTrout()
    {
        showTrout = !showTrout;
        LayoutPoints();
    }
}
