using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public Button restartButton;

    private void Start()
    {
        SpawnCircles(Random.Range(5, 11));
        restartButton.onClick.AddListener(RestartGame);
    }

    public void SpawnCircles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0);
            Instantiate(circlePrefab, randomPos, Quaternion.identity);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
