using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorAction : MonoBehaviour
{
    PlayableDirector pd; // 감독 오브젝트

    public Camera targetCam;

    void Start()
    {
        pd= GetComponent<PlayableDirector>();
        pd.Play();
    }

    void Update()
    {
        // 현재 진행중인 시간이 전체 시간과 크거나 같으면 (재생 시간이 다 되면)
        if(pd.time>=pd.duration)
        {
            // 만약에 메인카메라가 타겟카메라(시네머신에 활용하는 카메라)라면
            // 제어를 위해서 시네머신 브레인을 비활성화해라
            if(Camera.main==targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            // 시네머신에 사용한 카메라도 비활성화 해라
            targetCam.gameObject.SetActive(false);

            // Director 자신을 비활성화 해라
            gameObject.SetActive(false);
        }
    }
}
