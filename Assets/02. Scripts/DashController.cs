using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR;
using DG.Tweening;
public class DashController : MonoBehaviour
{
    ArduinoManager arduinoManager;
    TaskManager taskManager;
    [SerializeField]
    private float minDashRange = 0.5f;
    [SerializeField]
    private float maxDashRange = 40f;
    [SerializeField]
    private float dashTime = 0.2f;

    [SerializeField]
    private Animator maskAnimator;

    public SteamVR_Action_Boolean teleportAction = null;
    public SteamVR_Behaviour_Pose pose = null;

    private Transform cameraRigRoot;
    public PostProcessVolume volume;
    private Vignette vignette;
    private void Start()
    {
        taskManager=GameObject.Find("TaskManager").GetComponent<TaskManager>();
        arduinoManager=GameObject.Find("ArduinoManager").GetComponent<ArduinoManager>();
        cameraRigRoot = transform.parent;
        if (teleportAction == null || pose == null)
        {
            Debug.LogError("A required SteamVR action or pose has not been assigned.");
            return;
        }

        teleportAction.AddOnStateDownListener(TryDash, pose.inputSource);
    }

    
    private void TryDash(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > minDashRange && hit.distance < maxDashRange)
            {
                if(hit.transform.CompareTag("Teleport"))
                    StartCoroutine(DoDash(hit.point));
            }
        }
    }

    private IEnumerator DoDash(Vector3 endPoint)
    {
        float currentIntensity = 0f;

        if (volume.profile.TryGetSettings(out vignette))
        {
            // 0.1초 동안 0에서 0.5로 보간
            DOTween.To(() => currentIntensity, x => currentIntensity = x, 1f, 0.4f).OnUpdate(() => {
                vignette.intensity.Override(currentIntensity);
            });
            yield return new WaitForSeconds(0.1f);
        }

        float elapsed = 0f;
        Vector3 startPoint = cameraRigRoot.position;
        if(taskManager.istutorial==true){
                Invoke("tutorialsmell",dashTime/3);
            }
            else{
                Invoke("smellengine",dashTime/3);}
        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            float elapsedPct = elapsed / dashTime;
            
            cameraRigRoot.position = Vector3.Lerp(startPoint, endPoint, elapsedPct);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // 0.1초 동안 0.5에서 0으로 보간
        DOTween.To(() => currentIntensity, x => currentIntensity = x, 0f, 0.4f).OnUpdate(() => {
            vignette.intensity.Override(currentIntensity);
        });

        yield return new WaitForSeconds(0.4f);
        taskManager.MovementFinishEvent();
    }
        private void smellengine(){
			taskManager.Selectfragrance();
			Invoke("smellstop",dashTime);
		}
		private void smellstop(){
			if(taskManager.isvent){
				arduinoManager.click3();
				//Invoke("UIshow",0);
			}
			else if(taskManager.isvent==false){
				taskManager.Selectfragrance();
				//Invoke("UIshow",0);
			}
		}
        private void tutorialsmell(){
            taskManager.Selectfragrance();

        }

}