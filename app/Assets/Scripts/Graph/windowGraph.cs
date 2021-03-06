﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using CodeMonkey.Utils; 

public class windowGraph : MonoBehaviour {
	[SerializeField] private Sprite circleSprite; 
	private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
	// Use this for initialization
	private void Awake() {
		graphContainer = transform.Find ("graphContainer").GetComponent<RectTransform> ();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>(); 
        CreateCircle(new Vector2 (200, 200)); 

        // get data from list 
		List<int> valueList = new List<int>() {1000, 2405, 2303, 2205, 3058, 2459, 3104, 3294, 3194, 2134, 2543, 2489, 2483, 2849};
		ShowGraph(valueList);
	}
	private GameObject CreateCircle(Vector2 anchoredPosition) {
		GameObject gameObject = new GameObject("circle", typeof(Image)); 
		gameObject.transform.SetParent (graphContainer, false);
		gameObject.GetComponent<Image>().sprite = circleSprite; 
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2(11, 11);
		rectTransform.anchorMin = new Vector2 (0, 0);
		rectTransform.anchorMax = new Vector2 (0, 0);
		return gameObject; 
	}

	private void ShowGraph(List<int> valueList) {
		float graphHeight = graphContainer.sizeDelta.y;
		float yMaximum = 4100f; 

		float xSize = 50f;

		GameObject lastCircleGameObject = null;
		for (int i = 0; i < valueList.Count; i++) {
			float xPosition = xSize + i * xSize;
			float yPosition = (valueList [i] / yMaximum) * graphHeight;
			GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
			if (lastCircleGameObject != null) {
				CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition); 
			}
			lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = i.ToString(); 
            

		}

        int separatorCount = 10; 
        for (int i =0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i *1f/ separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue*graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue*yMaximum).ToString();
        }
	}

	private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
		GameObject gameObject = new GameObject("dotConnection", typeof(Image));
		gameObject.transform.SetParent (graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(155, 255, 23, .5f); 
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
		rectTransform.sizeDelta = new Vector2(distance, 3f);
		rectTransform.anchoredPosition = dotPositionA + dir*distance*.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir) );
    }
}
