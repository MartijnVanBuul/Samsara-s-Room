using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public static FadeManager instance;

    [SerializeField]
    private Image BackgroundFader;
    [SerializeField]
    private Image GameplayFader;
    [SerializeField]
    private Image TotalFader;

    [SerializeField]
    private float duration = 0.3f;

    [SerializeField]
    private float startBackgroundFade = 0f;
    [SerializeField]
    private float endBackgroundFade = 0.4f;
    [SerializeField]
    private float startGameplayFade = 0.3f;
    [SerializeField]
    private float endGameplayFade = 0.7f;
    [SerializeField]
    private float startTotalFade = 0.6f;
    [SerializeField]
    private float endTotalFade = 1f;

    private float progression;
    private float fadeDirection;

    private bool isFading;
    private bool isManualFade;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BackgroundFader.color = Color.black;
        GameplayFader.color = Color.black;
        TotalFader.color = Color.black;

        BackgroundFader.canvasRenderer.SetAlpha(0);
        GameplayFader.canvasRenderer.SetAlpha(0);
        TotalFader.canvasRenderer.SetAlpha(0);
    }

    void Update () {
        if (!isFading)
            return;

        if (!isManualFade)
            progression = Mathf.Clamp(progression + fadeDirection * (Time.deltaTime / duration), 0, 1);

        SetImageAlpha(BackgroundFader, Map(progression, startBackgroundFade, endBackgroundFade, 0, 1));
        SetImageAlpha(GameplayFader, Map(progression, startGameplayFade, endGameplayFade, 0, 1));
        SetImageAlpha(TotalFader, Map(progression, startTotalFade, endTotalFade, 0, 1));
    }

    private float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    private void SetImageAlpha(Image fadeImage, float alpha)
    {
        fadeImage.canvasRenderer.SetAlpha(alpha);
    }

    public void StartFade(float fadeDirection, float duration, bool isManualFade = false)
    {
        isFading = true;
        this.isManualFade = isManualFade;
        this.fadeDirection = fadeDirection;
        this.duration = duration;
    }

    public void StopFade()
    {
        isFading = false;
        isManualFade = false;
    }

    public void SetProgression(float progression)
    {
        isFading = true;
        isManualFade = true;
        this.progression = progression;
    }
}
