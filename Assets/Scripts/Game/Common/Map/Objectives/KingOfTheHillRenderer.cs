using UnityEngine;

namespace Game.Common.Map.Objectives
{
    public class KingOfTheHillRenderer : MonoBehaviour
    {
        public KingOfTheHillObjectiveV2 objective;

        // TODO: Temporary graphics that should be redesigned later
        public GameObject[] TeamCircles;
        private int currentActivatedCircle = -1;

        public void Update()
        {
            Render();
        }
        
        // TODO: Only reners the team circle, does not render with contested circle graphics nor losing circle graphics (tbd)
        // TODO: Events when a circle is capture and graphics that go along with that will require unity events on the objective first
        public void Render()
        {
            if (objective.IsTaken())
            {
                var focalTeam = objective.GetFocalTeam();
                if (currentActivatedCircle != focalTeam)
                {
                    EnableCircle(focalTeam);
                }
                else
                {
                    UpdateCurrentCircleScale();
                }
            }
        }


        public void EnableCircle(int teamID)
        {
            for (int i = 0; i < TeamCircles.Length; i++)
            {
                if (i == teamID)
                {
                    currentActivatedCircle = teamID;
                    TeamCircles[i].gameObject.SetActive(true);
                    UpdateCurrentCircleScale();
                }
                else
                {
                    TeamCircles[i].gameObject.SetActive(false);
                }
            }
            
        }

        public void UpdateCurrentCircleScale()
        {
            if (currentActivatedCircle == -1) return;

            var scale = objective.GetRadiusScale();
            TeamCircles[currentActivatedCircle].transform.localScale = new Vector3(scale, 1, scale);
        }
    }
}