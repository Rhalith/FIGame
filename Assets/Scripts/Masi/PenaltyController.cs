using Scripts.EventBus;
using Scripts.Events;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Masi
{
   public class PenaltyController : MonoBehaviour
   {
      [SerializeField] private GameObject _masi;
      [SerializeField] private GameObject _penaltyPrefab;

      [SerializeField] private List<PenaltyHit> _penaltyHits;
      [SerializeField] private Sprite _fiveSecondSprite;
      [SerializeField] private Sprite _tenSecondSprite;
      [SerializeField] private Sprite powerUpSprite;

      private void OnEnable()
      {
         EventBus<DisablePenaltiesEvent>.AddListener(DisablePenalties);
      }

      private void OnDisable()
      {
         EventBus<DisablePenaltiesEvent>.RemoveListener(DisablePenalties);
      }

      private void DisablePenalties(object sender, DisablePenaltiesEvent @event)
      {
         foreach (var penalty in _penaltyHits)
         {
            penalty.gameObject.SetActive(false);
         }
      }

      private void PreparePenalty(PenaltyHit penalty)
      {
         int i = Random.Range(0, 5);
         
         switch (i)
         {
            case 1:
               penalty.ChangePenalty(Penalty.TenSeconds, _tenSecondSprite);
               break;
            case 2:
               penalty.ChangePenalty(Penalty.PowerUp, powerUpSprite);
               break;
            default:
               penalty.ChangePenalty(Penalty.FiveSeconds, _fiveSecondSprite);
               break;
         }

      }
      public PenaltyHit GetPooledObject()
      {
         for (int i = 0; i < _penaltyHits.Count; i++)
         {
            if (!_penaltyHits[i].gameObject.activeInHierarchy)
            {
               PreparePenalty(_penaltyHits[i]);
               return _penaltyHits[i];
            }
         }

         GameObject obj = Instantiate(_penaltyPrefab);
         PenaltyHit objPenaltyHit = obj.GetComponent<PenaltyHit>();
         _penaltyHits.Add(objPenaltyHit);
         return objPenaltyHit;
      }
   }
}
