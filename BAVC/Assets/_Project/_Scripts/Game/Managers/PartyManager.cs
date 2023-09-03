using System;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game
{
    public class PartyManager : Singleton<PartyManager>
    {
        private Dictionary<BallClass, float> _multipliers;

        public float GetMultiplier(BallClass ballClass)
        {
            return _multipliers.TryGetValue(ballClass, out float multiplier) ? multiplier : 1f;
        }
        
        [SerializeField] private Transform[] activeBallsParents;
        [SerializeField] private Transform[] passiveBallsParents;

        private List<GameObject> _currentActiveBallsParty = new ();
        private List<GameObject> _currentPassiveBallsParty = new ();

        private int _maxPartyLimit;
        
        public List<GameObject> CurrentActiveParty => _currentActiveBallsParty;
        public List<GameObject> CurrentPassiveParty => _currentPassiveBallsParty;

        public int MaxPartyLimit => _maxPartyLimit;
        public int CurrentActivePartyCount => _currentActiveBallsParty.Count;
        public int CurrentPassivePartyCount => _currentPassiveBallsParty.Count;

        public void ConfigureCurrentParty()
        {
            _multipliers = new Dictionary<BallClass, float>()
            {
                { BallClass.Explosive, 1f },
                { BallClass.Magical, 1f },
                { BallClass.Physical, 1f },
                { BallClass.Shooter, 1f }
            };
            
            _maxPartyLimit = 4;
            
            foreach (Transform t in activeBallsParents)
            {
                if (t.childCount == 0)
                    continue;
                
                Destroy(t.GetChild(0).gameObject);
            }
            
            if (_currentActiveBallsParty.Count > 0)
                _currentActiveBallsParty.Clear();
            
            foreach (Transform t in passiveBallsParents)
            {
                if (t.childCount == 0)
                    continue;
                
                Destroy(t.GetChild(0).gameObject);
            }
            
            if (_currentPassiveBallsParty.Count > 0)
                _currentPassiveBallsParty.Clear();
        }
        
        public void UpgradeMaxPartyLimit()
        {
            _maxPartyLimit = Math.Clamp(_maxPartyLimit + 1, 4, 8);
        }

        public bool CanCreateBall(bool active)
        {
            if (PartyMaxLimitReached(active ? _currentActiveBallsParty : _currentPassiveBallsParty))
                return false;
            else
                return CanCreateBall(active ? activeBallsParents : passiveBallsParents);
        }
        private bool CanCreateBall(Transform[] ballsParents)
        {
            foreach (Transform t in ballsParents)
            {
                if (t.childCount == 0)
                    return true;
            }

            return false;
        }
        
        private bool PartyMaxLimitReached(List<GameObject> currentParty)
        {
            return currentParty.Count == _maxPartyLimit;
        }
        
        public bool CheckBallExists(bool active, BallType ballType)
        {
            return CheckBallExists(ballType, active ? activeBallsParents : passiveBallsParents);
        }
        private bool CheckBallExists(BallType ballType, Transform[] ballsParents)
        {
            foreach (Transform t in ballsParents)
            {
                if (t.childCount == 0)
                    continue;

                if (t.GetChild(0).GetComponent<Ball>().CheckBallType(ballType))
                    return true;
            }

            return false;
        }

        public void CreateBall(bool active, GameObject ballPrefab)
        {
            CreateBall(ballPrefab, active ? activeBallsParents : passiveBallsParents, active ? _currentActiveBallsParty : _currentPassiveBallsParty);
        }
        private void CreateBall(GameObject ballPrefab, Transform[] ballsParents, List<GameObject> currentParty)
        {
            foreach (Transform t in ballsParents)
            {
                if (t.childCount != 0)
                    continue;

                currentParty.Add(Instantiate(ballPrefab, t));
                return;
            }
        }

        public bool CanUpgradeBall(bool active, BallType ballType)
        {
            return CanUpgradeBall(ballType, active ? activeBallsParents : passiveBallsParents);
        }
        private bool CanUpgradeBall(BallType ballType, Transform[] ballsParents)
        {
            foreach (Transform t in ballsParents)
            {
                if (t.childCount == 0)
                    continue;

                if (!t.GetChild(0).GetComponent<Ball>().CheckBallType(ballType))
                    continue;
                    
                return t.GetChild(0).GetComponent<Ball>().GetLevel() < 9;
            }

            return false;
        }
        
        public void UpgradeBall(bool active, BallType ballType)
        {
            UpgradeBall(ballType, active ? activeBallsParents : passiveBallsParents);
        }
        private void UpgradeBall(BallType ballType, Transform[] ballsParents)
        {
            foreach (Transform t in ballsParents)
            {
                if (t.childCount == 0)
                    continue;

                if (!t.GetChild(0).GetComponent<Ball>().CheckBallType(ballType))
                    continue;
                    
                t.GetChild(0).GetComponent<Ball>().Upgrade();
                return;
            }
        }

        public void UpgradeBalls(float multiplier, BallClass ballClass)
        {
            if (_multipliers.ContainsKey(ballClass))
            {
                _multipliers[ballClass] += multiplier;
                print("Ball class: " + ballClass + ", multiplier: " + _multipliers[ballClass]);
            }
            
            UpgradeBalls(multiplier, ballClass, activeBallsParents);
            UpgradeBalls(multiplier, ballClass, passiveBallsParents);
        }
        
        private void UpgradeBalls(float multiplier, BallClass ballClass, Transform[] ballsParent)
        {
            foreach (Transform t in ballsParent)
            {
                if (t.childCount == 0)
                    continue;

                if (!t.GetChild(0).GetComponent<Ball>().CheckBallClass(ballClass))
                    continue;
                
                if (t.GetChild(0).TryGetComponent(out IUpgradable iu))
                    iu.UpgradeObject(multiplier);
            }
        }
    }
}