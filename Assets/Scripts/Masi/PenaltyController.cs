using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Masi
{
   public class PenaltyController : MonoBehaviour
   {
      [SerializeField] private GameObject _masi;
      [SerializeField] private GameObject _penaltyPrefab;

      [SerializeField] List<PenaltyHit> _penaltyHits;
      [SerializeField] List<Sprite> _fiveSecondSprites;
      [SerializeField] List<Sprite> _tenSecondSprites;
      [SerializeField] List<Sprite> powerUpSprites;

      private void PreparePenalty(PenaltyHit penalty)
      {
         int i = Random.Range(0, 5);
         int j = Random.Range(0, 2);

         Penalty penaltyType;
         Sprite penaltySprite;

         switch (i)
         {
            case 1:
               penaltyType = Penalty.TenSeconds;
               penaltySprite = _tenSecondSprites[j];
               break;
            case 2:
               penaltyType = Penalty.PowerUp;
               penaltySprite = powerUpSprites[j];
               break;
            default:
               penaltyType = Penalty.FiveSeconds;
               penaltySprite = _fiveSecondSprites[j];
               break;
         }

         penalty.ChangePenalty(penaltyType, penaltySprite);
      }
      public PenaltyHit GetPooledObject()
      {
         for (int i = 0; i < _penaltyHits.Count; i++)
         {
            if (!((PenaltyHit)_penaltyHits[i]).gameObject.activeInHierarchy)
            {
               PreparePenalty(_penaltyHits[i]);
               return (PenaltyHit)_penaltyHits[i];
            }
         }

         GameObject obj = (GameObject)Instantiate(_penaltyPrefab);
         PenaltyHit objPenaltyHit = obj.GetComponent<PenaltyHit>();
         _penaltyHits.Add(objPenaltyHit);
         return objPenaltyHit;
      }
   }
}
