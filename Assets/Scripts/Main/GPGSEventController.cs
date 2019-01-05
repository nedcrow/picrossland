using UnityEngine;

public class GPGSEventController : MonoBehaviour {

	public void ClearAchievements()
    {
        Social.ReportProgress(GPGSIds.achievement_testevent,100f,(bool success) => // Social.ReportProgress(업적ID, 달성도 0~100, callBack)
        {
            //업적 달성 효과 있으면 적용.
        });
    }//콘솔에서 업적 바뀌면 반드시 Firebase나 콘솔에서 google-services.json 다운 받아 업데이트.
}
