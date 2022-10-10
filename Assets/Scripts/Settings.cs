using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private int gridSize = 9;

    public static bool isColorRandom;

    public static bool isCameraPerspective;

    private float delay = 0f;

    [SerializeField]
    private WaveFunctionCollapse function;

    [Space, SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private Toggle isColorRandomToggle;

    [SerializeField]
    private Toggle cameraPerspectiveToggle;

    [SerializeField]
    private Slider gridSizeSlider;

    [SerializeField]
    private TextMeshProUGUI sliderText;

    private bool isMenuVisible = true;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
    }

    private void Update()
    {
        sliderText.text = gridSizeSlider.value.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            isMenuVisible = !isMenuVisible;

        menuPanel.SetActive(isMenuVisible);
    }

    public void OnGenerateButtonClick()
    {
        gridSize = (int)gridSizeSlider.value;
        isColorRandom = isColorRandomToggle.isOn;
        isCameraPerspective = cameraPerspectiveToggle.isOn;

        function.Generate(gridSize, delay);

        isMenuVisible = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
}