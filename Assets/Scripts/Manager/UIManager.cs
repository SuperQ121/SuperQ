using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // UIԪ������
    public GameObject pausePanel;       // ��ͣ�������
    public Button pauseButton;         // ��ͣ��ť
    public Button continueButton;      // ������ť
    public Button exitButton;          // �˳���ť

    private bool isPaused = false;     // ��Ϸ��ͣ״̬

    void Start()
    {
        // ��ʼ����ť�¼���
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // ��ʼ������ͣ���
        pausePanel.SetActive(false);
    }

    void Update()
    {
        // ���������루���� "P" ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        // ԭ����ͣ/�����߼�
        if (isPaused)
        {
            Time.timeScale = 0f;         // ��ͣ��Ϸʱ��
        }
        else
        {
            Time.timeScale = 1f;         // �ָ���Ϸʱ��
        }
    }

    // �л���ͣ״̬
    public void TogglePause()
    {
        isPaused = !isPaused;

        // ��ʾ/������ͣ���
        pausePanel.SetActive(isPaused);

        // ���ʹ�ö�����������������������ƶ���
        // animator.SetBool("isPaused", isPaused);
    }

    // ������Ϸ
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;           // �ָ���Ϸʱ��
    }

    // �˳���Ϸ
    public void ExitGame()
    {
        // ����Ǳ༭��ģʽ��ֱ���˳��༭��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ���������ʱ���������˵�������ֱ���˳�����
        SceneManager.LoadScene(0);     // ����0�ų��������˵�
        Application.Quit();            // �˳��������ƶ��˿�����Ч��
#endif
    }
}
