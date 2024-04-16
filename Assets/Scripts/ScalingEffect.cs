using System.Collections;
using UnityEngine;

enum PlayOn
{
    None,
    Awake,
    Start,
    OnEnable,
}

enum PlayCondition
{
    To,
    From,

}

public class ScalingEffect : MonoBehaviour
{

    #region PUBLIC_VARS

    [SerializeField] bool useUnscaledTime;
    [Space]

    [SerializeField] PlayOn playState = PlayOn.None;
    [HeaderAttribute("Animation Curve")]
    public AnimationCurve scalingCurve;
    [Space]
    [SerializeField] PlayCondition playCondition = PlayCondition.To;
    [Space]

    public float time = 0.25f;
    [Space]
    public float scaleMultiplier;
    [Space]

    public float startDelay;


    #endregion

    #region PRIVATE_VARS



    private float endDelay;

    private Vector3 initialScale;
    private Vector3 finalScale;

    private IEnumerator scaleCoroutine;
    private WaitForSeconds startingDelay;
    private WaitForSeconds endingDelay;

    //
    private WaitForSecondsRealtime startingDelayRealTime;
    private WaitForSecondsRealtime endingDelayRealTime;
    #endregion

    #region UNITY_CALLBACKS

    private void Awake()
    {
        initialScale = transform.localScale;
        finalScale = Vector3.one * scaleMultiplier;

        transform.localScale = initialScale;

        startingDelay = new WaitForSeconds(startDelay);
        endingDelay = new WaitForSeconds(endDelay);

        startingDelayRealTime = new WaitForSecondsRealtime(startDelay);
        endingDelayRealTime = new WaitForSecondsRealtime(endDelay);


        if (playState == PlayOn.Awake)
            PlayEffect();


    }
    private void Start()
    {
        if (playState == PlayOn.Start)
            PlayEffect();

    }


    private void OnEnable()
    {
        if (playState == PlayOn.OnEnable)
            PlayEffect();
    }

    #endregion

    #region PUBLIC_FUNCTIONS
    public void PlayEffect()
    {

        scaleCoroutine = ScaleEffect();
        StartCoroutine(scaleCoroutine);
    }
    #endregion

    #region PRIVATE_FUNCTIONS


    #endregion

    #region CO-ROUTINES

    IEnumerator ScaleEffect()
    {
        float i = 0;
        float rate = 1 / time;
        if (playCondition == PlayCondition.From)
            transform.localScale = finalScale;

        if (useUnscaledTime)
        {
            yield return startingDelayRealTime;
        }
        else
        {
            yield return startingDelay;

        }

        while (i < 1)
        {
            if (useUnscaledTime)
            {
                i += rate * Time.unscaledDeltaTime;
            }
            else { i += rate * Time.deltaTime; }


            if (playCondition == PlayCondition.To)
            {
                transform.localScale = Vector3.Lerp(initialScale, finalScale, scalingCurve.Evaluate(i));

            }
            else
            {
                transform.localScale = Vector3.Lerp(finalScale, initialScale, scalingCurve.Evaluate(i));
            }

            yield return 0;
        }

    }

    IEnumerator ExitEffect()
    {
        float i = 0;
        float rate = 1 / time;

        if (useUnscaledTime)
        {
            yield return endingDelay;
        }
        else
        {
            yield return endingDelayRealTime;
        }

        while (i < 1)
        {
            i += rate * Time.deltaTime;


            transform.localScale = Vector3.Lerp(finalScale, initialScale, i);
            yield return 0;
        }
    }

    #endregion

    #region EVENT_HANDLERS

    #endregion

    #region UI_CALLBACKS

    #endregion
}