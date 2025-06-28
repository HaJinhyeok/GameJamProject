using UnityEngine;

// 화살표 애니메이션 끄는 함수
public class VanishArrow : MonoBehaviour
{
    public void DisappearArrow()
    {
        gameObject.SetActive(false);
    }
}
