using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHidingSpotsData : MonoBehaviour
{
    [SerializeField]private List<HidingSpot> closerSpots = new List<HidingSpot>();

    private HidingSpot targetHidingSpot;

    //private Vector3 currentTargetSpotPosition;

    public void SetCloserSpots(HidingSpot newCloserSpot)
    {
        closerSpots.Add(newCloserSpot);
    }

    public List<HidingSpot> GetSpots()
    {
        return closerSpots;
    }

    public void SetTargetSopt(HidingSpot newTarget)
    {
        targetHidingSpot = newTarget;
    }

    public HidingSpot GetTargetSpot()
    {
        return targetHidingSpot;
    }

    public void ClearData()
    {
        closerSpots.Clear();
    }
}
