using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public static ResultManager instance;
    float curTime;
    public float limitTime = 3;
    public GameObject resultUI;
    public TextMeshProUGUI textTime;

    private void Awake()
    {
        instance = this;
        resultUI.SetActive(false);
    }


    // ���� �ð� ���� �� ���UI ǥ�� �� TimeScale = 0
    void Start()
    {
        
    }

    void Update()
    {
        // ���� �ð� ���� �� UI ǥ�� �� �ð� ����
        if (!resultUI.activeSelf)
        {
            curTime += Time.deltaTime;

            int time = (int)(limitTime  - curTime);
            textTime.SetText(time.ToString() + "s");

            if (curTime >= limitTime) {
                textTime.SetText("0s");
                ShowResultUI();
            }
        }
    }

    public void ShowResultUI()
    {
        resultUI.SetActive (true);
        Time.timeScale = 0;
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
