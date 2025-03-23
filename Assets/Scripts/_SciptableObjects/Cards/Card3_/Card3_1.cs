
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card3_1", menuName = "Data/CardInfo3_/Card3_1",order = 1)]
public class Card3_1 : CardInfo
{
    public int reduceLayers;
    public int addLayers;
    public override bool CardFuction()
    {
        bool baseResult= base.CardFuction();

        if (baseResult)
        {
            foreach (var buffType in GameManager.instance.debuffList)
            {
                PlayerManager.instance.player.stat.ReduceBuffLayers(buffType,reduceLayers);
            }
            PlayerManager.instance.player.stat.AddBufflayers(BuffType._紊乱,addLayers);
            return true;
        }
        else
        { 
            return false;
        }
    }
}

