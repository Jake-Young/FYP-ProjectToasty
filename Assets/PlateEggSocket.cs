﻿using UnityEngine;

public class PlateEggSocket : Socket
{
    private void OnTriggerEnter(Collider other)
    {
        FriedEgg friedEgg = other.gameObject.GetComponent<FriedEgg>();

        if (friedEgg != null)
        {
            if (friedEgg.m_CurrentEggState == FriedEgg.EggState.FriedEgg)
            {
                Debug.Log(m_PositiveCollisionTag);
                m_MLManager.IsEggOnToast();
                m_MLManager.EggToastOnPlate();
            }
            else if (friedEgg.m_CurrentEggState == FriedEgg.EggState.Burnt || 
                     friedEgg.m_CurrentEggState == FriedEgg.EggState.RawEgg)
            {
                m_CookingAgent.AddReward(m_MLManager.m_IsBurntPoints);
                Debug.Log("End");
                m_CookingAgent.EndEpisode();
            }
        }
    }
}
