using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TMP_Text m_scoreText;
    [SerializeField] TMP_Text m_buttonText;
    [SerializeField] TMP_Text m_SuccessText;
    [SerializeField] Button m_button;
    List<TMP_Text> m_textList;

    [Header("Material")]
    [SerializeField] Material m_cubeMaterial;

    [Header("Angle")]
    [SerializeField] TMP_Text m_angleText;
    [SerializeField] TMP_Text m_angleListText;
    [SerializeField] bool m_isAngleUpdate;
    [SerializeField] [Range(0,1)] float m_angleUpdateTimer;

    [Header("Game var")]
    [SerializeField] bool m_isHit;
    [SerializeField] int m_score;

    private void Awake()
    {
        m_textList = new List<TMP_Text>()
        {
            m_scoreText,
            m_angleText,
            m_angleListText,
            m_SuccessText
        };
    }

    private void Start()
    {
        m_SuccessText.text = "Hit before, angle between 0 - " + Cube.m_instance.m_rotationSuccess.ToString() + " score it";
    }

    void Update() => AngleCorutineStart();

    #region  Hit Button

    public void ButtonClick()
    {
        if (!m_isHit && Cube.m_instance.RotationSuccess()) 
        {
            StartCoroutine(ScoreUpdate());
        }
    }

    IEnumerator ScoreUpdate()
    {
        m_isHit = true;

        ScoreTextUpdate();
        AngleListUpdate();
        ColorChange();
        yield return new WaitForSeconds(0.5f);
        
        m_isHit = false;
    }

    void ScoreTextUpdate()
    {
        m_score += 1;
        m_scoreText.text = "Score : " + m_score.ToString();
    }
    void AngleListUpdate()
    {
        string angleList = "";

        foreach (string list in Cube.m_instance.m_angleList)
        {
            angleList += list + "\n";
        }

        m_angleListText.text = angleList;
    }

    void ColorChange()
    {
        Vector3 newMaterial = new Vector3(RandomGet(), RandomGet(), RandomGet());
        m_cubeMaterial.color = new Color(newMaterial.x, newMaterial.y, newMaterial.z);

        foreach (var colorList in m_textList)
            colorList.color = m_cubeMaterial.color;

        m_button.image.color = m_cubeMaterial.color;

        Camera.main.backgroundColor = new Color(1f - newMaterial.x, 1f - newMaterial.y, 1f - newMaterial.z);
        m_buttonText.color = Camera.main.backgroundColor;
    }

    #endregion

    #region Angle Update
    void AngleCorutineStart()
    {
        if (!m_isAngleUpdate)
        {
            StartCoroutine(AngleUpdate());
        }
    }

    IEnumerator AngleUpdate()
    {
        m_isAngleUpdate = true;
        print("Angle update");
        m_angleText.text = "Angle : " + Cube.m_instance.GetEularAnglesX().ToString();
        yield return new WaitForSeconds(m_angleUpdateTimer);

        m_isAngleUpdate = false;
    }

    #endregion

    #region  Quit Button

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    #endregion

    private float RandomGet() => Random.Range(0, 1f);
}
