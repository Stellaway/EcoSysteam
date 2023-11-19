using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnDestroy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioClip _clip;

    
 
    private void OnDestroy()
    {
        SoundManager.Instance.PlaySound(_clip);
    }
}
