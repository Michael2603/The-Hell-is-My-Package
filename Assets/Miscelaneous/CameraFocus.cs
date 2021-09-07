using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] Transform Greg;

    public List<Transform> Enemy = new List<Transform>();

    CinemachineTargetGroup targetGroup;

    void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Update()
    {
        List<Transform> Characters = new List<Transform> { Greg };

        for (int i = 0; i < Characters.Count; i++)
        {
            for (int j = 0; j < Enemy.Count; j++)
            {
                if ( Characters[i] == Enemy[j] )
                    break;
                else if ( j == Enemy.Count - 1 )
                    Characters.Add(Enemy[Enemy.Count - 1]);
            }
            
        }

        for (int i = 0;i < Characters.Count; i++)
        {
            if (Characters[i].gameObject.activeSelf == true)
                targetGroup.m_Targets[i].target = Characters[i];
            else
                targetGroup.m_Targets[i].target = null;
            
        }

    }

    public void AddCharacter(Transform transf)
    {
        this.Enemy.Add(transf);
    }

    public void RemoveCharacter()
    {
        this.Enemy = null;
    }
}
