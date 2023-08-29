using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] checkPoints;
    [SerializeField]
    private int currentIndex = 0;

    private List<CPInterface> cpInts;

    private void Start()
    {
        cpInts = new List<CPInterface>();
        foreach (var cp in checkPoints)
        {
            CPInterface temp = cp.gameObject.GetComponent<CPInterface>();
            cpInts.Add(temp);
            temp.IsHidden = true;
        }
        cpInts[0].IsHidden = false;
    }

    public Transform CurrentCP
    {
         get { return checkPoints[currentIndex]; }
    }

    public Transform NextCP
    {
        get 
        {
            if (currentIndex >= cpInts.Count - 1)
            {
                cpInts[cpInts.Count - 1].IsHidden = true;
                currentIndex = 0;
                cpInts[0].IsHidden = false;
                return checkPoints[currentIndex];
            } else
            {
                cpInts[currentIndex].IsHidden = true;
                currentIndex++;
                cpInts[currentIndex].IsHidden = false;
                return checkPoints[currentIndex];
            }
            
        }
    }
}
