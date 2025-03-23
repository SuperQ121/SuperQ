using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card1_10", menuName = "Data/CardInfo1_/Card1_10",order = 10)]
public class Card1_10 : CraftCardInfo
{
    public int reduceLayers;
    public override bool CardFuction()
    {
      
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            foreach (var buffState in PlayerManager.instance.player.stat.buffState)
            {
                if (GameManager.instance.debuffList.Contains(buffState.Key))
                {
                    PlayerManager.instance.player.stat.ReduceBuffLayers(buffState.Key,reduceLayers);
                }
            }
            return true;
        }
        else
        { 
            return false;
        }
    }
}

