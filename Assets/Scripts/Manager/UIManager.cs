using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // UI元素引用
    public GameObject pausePanel;       // 暂停界面面板
    public Button pauseButton;         // 暂停按钮
    public Button continueButton;      // 继续按钮
    public Button exitButton;          // 退出按钮

    private bool isPaused = false;     // 游戏暂停状态

    void Start()
    {
        // 初始化按钮事件绑定
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // 初始隐藏暂停面板
        pausePanel.SetActive(false);
    }

    void Update()
    {
        // 检测键盘输入（按下 "P" 键）
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        // 原有暂停/继续逻辑
        if (isPaused)
        {
            Time.timeScale = 0f;         // 暂停游戏时间
        }
        else
        {
            Time.timeScale = 1f;         // 恢复游戏时间
        }
    }

    // 切换暂停状态
    public void TogglePause()
    {
        isPaused = !isPaused;

        // 显示/隐藏暂停面板
        pausePanel.SetActive(isPaused);

        // 如果使用动画控制器，可以在这里控制动画
        // animator.SetBool("isPaused", isPaused);
    }

    // 继续游戏
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;           // 恢复游戏时间
    }

    // 退出游戏
    public void ExitGame()
    {
        // 如果是编辑器模式，直接退出编辑器
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果是运行时，加载主菜单场景或直接退出程序
        SceneManager.LoadScene(0);     // 假设0号场景是主菜单
        Application.Quit();            // 退出程序（在移动端可能无效）
#endif
    }
}
