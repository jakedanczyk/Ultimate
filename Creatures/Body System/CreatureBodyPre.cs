using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class CreatureBodyPre : MonoBehaviour
    {

        public List<int> bodyPartIds;
        public Dictionary<int, BodyPart> bodyParts;

        public Thermometer thermometer;
        public float localTemperature = 0;
        public float precipRate = 0;
        public float windSpeed = 0;
        public float humidity = 0;
        //public ShelterCheck shelterCheck;
        public float coreTemp = 37f;
        float coreTempBase = 37f;
        float averageSkinTemp = 33.2f, headSkinTemp, neckSkinTemp, chestSkinTemp, stomachSkinTemp, pelvisSkinTemp, upperBack, lowerBackSkinTemp,
        leftShoulderSkinTemp, rightShoulderSkinTemp, leftUpperArmSkinTemp, rightUpperArmSkinTemp, leftForearmSkinTemp, rightForearmSkinTemp,
        leftHandSkinTemp, rightHandSkinTemp, leftThighSkinTemp, rightThighSkinTemp, leftCalfSkinTemp, rightCalfSkinTemp, leftFootSkinTemp,
        rightFootSkinTemp;

        public float heartRate;

        public WorldTime worldTime;

        public StatCollection stats;

        //public CreatureAnimator animator;

        // Start is called before the first frame update
        void Start()
        {
            //animator = GetComponent<CreatureAnimator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float lastTempCheckTime = 28800;
        IEnumerator weaponSwing, teethChatter;

        public float totalInsulation = 0;
        public float totalWaterCover = 0;
        public float totalWindCover = 0;
        void UpdateCoreTemp()
        {
            ReadWeather();
            float wetness = 0;
            float totalSize = 0;
            foreach (int bodyPartId in bodyPartIds)
            {
                BodyPart bodyPart = bodyParts[bodyPartId];
                wetness += bodyPart.wetness * bodyPart.size;
            }
            float avgWetness = wetness / totalSize;
            float heatProduction = 0.001f * stats.GetStat(StatType.Weight).StatValue * Mathf.Pow(heartRate / (stats.GetStat(StatType.RestingHeartRate).StatValue), 1.285f); // 1w/kg (.001w/g) at rest, 
            float heatLoss = stats.GetStat(StatType.SurfaceArea).StatValue * (averageSkinTemp - localTemperature) * .000075f * (.1f + avgWetness + windSpeed / 100) / totalInsulation; // coeff * area * temperature difference

            //(Mathf.Pow(coreTemp / coreTempBase,3) * (coreTemp - localTemperature) * (.1f+humidity) + localTemperature * localTemperature * humidity) * stats.GetStat(StatType.Height).StatValue / totalInsulation;
            if (coreTemp > coreTempBase + .5f)
                averageSkinTemp = coreTemp;
            else
                averageSkinTemp = coreTemp - 4;

            //print(coreTemp + ", " + localTemperature + ", " + heartRate);
            //float coolingFactor = coreTemp * (0.000001f * (coreTemp - localTemperature + 10 - (5*heartRate/60)));
            //float heatingFactor = coreTemp * (0.000001f * (coreTemp - localTemperature + 5 - (5 * heartRate / 60)));
            //if (localTemperature < 15)
            //{
            //    coreTemp = coreTemp - (coolingFactor / (1 + totalInsulation));
            //}

            //if (localTemperature > 32)
            //{
            //    coreTemp = (coreTemp - coreTempBase) + (heatingFactor * (1 + totalInsulation));
            //}
            float now = worldTime.totalGameSeconds;
            coreTemp += (now - lastTempCheckTime) * (heatProduction - heatLoss) / (4.184f * stats.GetStat(StatType.Weight).StatValue);
            if (coreTemp < (coreTempBase - 1))
            {
                if (!shivering)
                {
                    StartCoroutine(teethChatter);
                    shivering = true;
                }
                //if (coreTemp < (coreTempBase - 5))
                //{
                //    FrostEffect frost = camTransform.GetComponent<FrostEffect>();
                //    frost.enabled = true;
                //    frost.FrostAmount = 0.1f + (Mathf.Pow((coreTempBase - 5f) / coreTemp, 2) - 1);
                //    if (coreTemp < (coreTempBase - 7))
                //    {
                //        //die
                //    }
                //}
            }
            else
            {
                if (shivering)
                {
                    StopCoroutine(teethChatter);
                    shivering = false;
                    //camTransform.GetComponent<FrostEffect>().enabled = false;
                }
                if (coreTemp > (coreTempBase))
                    Sweat(now - lastTempCheckTime);
            }
            lastTempCheckTime = now;
        }

        void UpdateWeatherProtection()
        {
            totalInsulation = 0;
            totalWaterCover = 0;
            totalWindCover = 0;

            ReadWeather();
            humidity = Mathf.Clamp(humidity + precipRate / totalWaterCover, 0, 1);

            foreach (int bodyPartId in bodyPartIds)
            {
                BodyPart bodyPart = bodyParts[bodyPartId];
                bodyPart.wetness = Mathf.Clamp01(bodyPart.wetness + (precipRate / totalWaterCover));
            }
        }

        public void ReadWeather()
        {
            //float altitude = this.transform.position.y;
            //localTemperature = thermometer.temperature;
            //if (shelterCheck.roof) { precipRate = 0; }
            //else { precipRate = skyControl.weathers[skyControl.schedule[0]].precipRate; }
            //if (shelterCheck.walls) { windSpeed = 0; }
            //else { windSpeed = skyControl.weathers[skyControl.schedule[0]].windSpeed; }
        }

        public float calories = 2000f;
        public float calorieBurn = 0;
        [SerializeField]
        float calorieTime, hydrationTime, sleepTime, staminaTime;
        void CalorieBurn()
        {
            float now = worldTime.totalGameSeconds;
            calorieBurn = stats.GetStat(StatType.Weight).StatValue * (Mathf.Max(heartRate - 50, 0) * .00000068481f + 0.000001175f) * (now - calorieTime) / 6; // TODO
            stomachSolidsVolume = Mathf.Max(stomachLiquidsVolume - .0000086806f * stomachMaxVolume * (now - calorieTime), 0);
            stomachLiquidsVolume = Mathf.Max(stomachLiquidsVolume - .000086806f * stomachMaxVolume * (now - calorieTime), 0);
            calorieTime = now;
            calories -= calorieBurn;
        }

        float stomachMaxVolume, stomachSolidsVolume, stomachLiquidsVolume;

        //
        public float StomachFillLevel()
        {
            return (stomachLiquidsVolume + stomachSolidsVolume) / stomachMaxVolume;
        }
        public bool shivering, sweating;
        [SerializeField]
        float waterContent; //mL of STP water available in body
        public float hydration = 60f; // body water %
        void UpdateWaterContent()
        {
            waterContent -= calorieBurn * 1f; //livestrong says 1 to 1.5 mL of water needed per calorie, we'll go with low end since sweat is done separately
            hydration = waterContent / stats.GetStat<StatAttribute>(StatType.Weight).StatValue;
        }

        void Sweat(float amount)
        {
            sweating = true;
            waterContent -= Mathf.Clamp01(coreTemp - coreTempBase) * amount;
            hydration = waterContent / stats.GetStat<StatAttribute>(StatType.Weight).StatValue;

            foreach (int bodyPartId in bodyPartIds)
            {
                BodyPart bodyPart = bodyParts[bodyPartId];
                bodyPart.wetness = Mathf.Clamp01(bodyPart.wetness + Mathf.Clamp01(coreTemp - coreTempBase) * amount / 100);
            }
        }
    }
}
