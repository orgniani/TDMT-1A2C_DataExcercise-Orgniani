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
        }

        private void DisableLoadingScreen()
        {
            Invoke(nameof(TurnOffLoadingScreen), fillDuration);
        }

        private void TurnOffLoadingScreen()
        {
            loadingScreen.enabled = false;
        }

        private void UpdateLoadBarFill(float percentage)
        {
            StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
        }

        private IEnumerator LerpFill(float from, float to)
        {
            var start = Time.time;
            var now = start;

            while (start + fillDuration > now)
            {
                loadingBarFill.fillAmount = Mathf.Lerp(from, to, (now - start) / fillDuration);
                yield return null;
                now = Time.time;
            }

            loadingBarFill.fillAmount = to;
        }
    }

}