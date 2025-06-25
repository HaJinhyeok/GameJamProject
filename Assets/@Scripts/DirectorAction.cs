using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorAction : MonoBehaviour
{
    PlayableDirector pd; // ���� ������Ʈ

    public Camera targetCam;

    void Start()
    {
        pd= GetComponent<PlayableDirector>();
        pd.Play();
    }

    void Update()
    {
        // ���� �������� �ð��� ��ü �ð��� ũ�ų� ������ (��� �ð��� �� �Ǹ�)
        if(pd.time>=pd.duration)
        {
            // ���࿡ ����ī�޶� Ÿ��ī�޶�(�ó׸ӽſ� Ȱ���ϴ� ī�޶�)���
            // ��� ���ؼ� �ó׸ӽ� �극���� ��Ȱ��ȭ�ض�
            if(Camera.main==targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            // �ó׸ӽſ� ����� ī�޶� ��Ȱ��ȭ �ض�
            targetCam.gameObject.SetActive(false);

            // Director �ڽ��� ��Ȱ��ȭ �ض�
            gameObject.SetActive(false);
        }
    }
}
