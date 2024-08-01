using UnityEngine;
using UnityEngine.UI;

public class VictoryCanvas : MonoBehaviour
{
    [SerializeField] private Image starImagePrefab;
    [SerializeField] private Transform starContainer;

    [SerializeField] private GameObject victoryText;

    private Image[] starObjects = new Image[5];

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            starObjects[i] = Instantiate(starImagePrefab, starContainer);
            starObjects[i].gameObject.SetActive(false);
        }
    }

    public void TriggerCanvas()
    {
        victoryText.SetActive(true);

        int starCount = ScoreCalculator.GetScore();

        for (int i = 0; i < starCount; i++)
        {
            starObjects[i].gameObject.SetActive(true);
        }
    }
}
