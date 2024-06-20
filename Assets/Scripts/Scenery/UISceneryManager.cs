using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO: Change this class to the UI folder and assembly definition - SF
namespace Scenery
{
    [RequireComponent(typeof(SceneryManager))]
    public class UISceneryManager : MonoBehaviour
    {
        [SerializeField] private Canvas loadingScreen;
        [SerializeField] private Image loadingBarFill;
        [SerializeField] private float fillDuration = .25f;

        private Coroutine currentFillCoroutine;

        private void Awake()
        {
            var sceneryManager = GetComponent<SceneryManager>();
            sceneryManager.OnLoadStart += EnableLoadingScreen;
            sceneryManager.OnLoadEnd += DisableLoadingScreen;
            sceneryManager.OnLoadPercentage += UpdateLoadBarFill;
        }

        private void EnableLoadingScreen()
        {
            loadingScreen.enabled = true;
            loadingBarFill.fillAmount = 0;
        }

        private void DisableLoadingScreen()
        {
            if (currentFillCoroutine != null)
                StopCoroutine(currentFillCoroutine);

            loadingScreen.enabled = false;
        }

        private void UpdateLoadBarFill(float percentage)
        {
            if (currentFillCoroutine != null)
                StopCoroutine(currentFillCoroutine);

            currentFillCoroutine = StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
        }

        private IEnumerator LerpFill(float from, float to)
        {
            float startTime = Time.time;
            float endTime = startTime + fillDuration;
            float startFillAmount = loadingBarFill.fillAmount;

            while (Time.time < endTime)
            {
                float timeProgress = (Time.time - startTime) / fillDuration;
                loadingBarFill.fillAmount = Mathf.Lerp(startFillAmount, to, timeProgress);
                yield return null;
            }

            loadingBarFill.fillAmount = to;

            yield return new WaitForSeconds(0.5f);
        }
    }

}