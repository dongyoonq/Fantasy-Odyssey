using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadManager
{
    private static string playerCSVPath = "/Editors/CSVs/PlayerCSV.csv";
    private static string npcTalkCSVPath = "/Editors/CSVs/NpcTalkCSV.csv";
    private static string npcQuestCSVPath = "/Editors/CSVs/NpcQuestCSV.csv";
    private static string monsterAgrCSVPath = "/Editors/CSVs/AgreesiveMonsterCSV.csv";
    private static string monsterMelCSVPath = "/Editors/CSVs/MeleeMonsterCSV.csv";
    private static string monsterRanCSVPath = "/Editors/CSVs/RangeMonsterCSV.csv";
    private static string monsterCSVPath = "/Editors/CSVs/MonsterCSV.csv";

    [MenuItem("Utilities/Generate Player Data")]
    public static void CSVtoSO_GeneratePlayerData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + playerCSVPath);

        foreach (string s in allLines)
        {
            if (s == allLines[0])
                continue;

            string[] splitData = s.Split(',');

            if (splitData.Length > 8)
                Debug.Log("Data Is not Valid");

            PlayerStatusData playerStatusData = ScriptableObject.CreateInstance<PlayerStatusData>();
            playerStatusData.maxHP = int.Parse(splitData[1]);
            playerStatusData.deffense = float.Parse(splitData[2]);
            playerStatusData.attackPower = int.Parse(splitData[3]);
            playerStatusData.attackSpeed = float.Parse(splitData[4]);
            playerStatusData.walkSpeed = float.Parse(splitData[5]);
            playerStatusData.runSpeed = float.Parse(splitData[6]);
            playerStatusData.jumpPower = float.Parse(splitData[7]);

            AssetDatabase.CreateAsset(playerStatusData, $"Assets/Imports/Resources/Data/PlayerData/{splitData[0]}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate NPC Data")]
    public static void CSVtoSO_NpcData()
    {
        List<TalkData> npcTalkDataList = new List<TalkData>();
        List<QuestData> npcQuestDataList = new List<QuestData>();

        string[] talkDataLines = File.ReadAllLines(Application.dataPath + npcTalkCSVPath);
        TalkData npcTalkData;

        foreach (string s in talkDataLines)
        {
            if (s == talkDataLines[0])
                continue;

            npcTalkData = ScriptableObject.CreateInstance<TalkData>();
            string[] splitData = s.Split(',');
            npcTalkData.talkContents = new List<string>();

            npcTalkData.id = splitData[0];
            for (int i = 1; i < splitData.Length; i++)
                npcTalkData.talkContents.Add(splitData[i]);

            AssetDatabase.CreateAsset(npcTalkData, $"Assets/Imports/Resources/Data/NpcData/TalkData/{splitData[0]}.asset");

            npcTalkDataList.Add(npcTalkData);
        }

        AssetDatabase.SaveAssets();

        string[] questDataLines = File.ReadAllLines(Application.dataPath + npcQuestCSVPath);
        QuestData npcQuestData;

        foreach (string s in questDataLines)
        {
            if (s == questDataLines[0])
                continue;

            npcQuestData = ScriptableObject.CreateInstance<QuestData>();
            string[] splitData = s.Split(',');

            npcQuestData.id = splitData[0];
            npcQuestData.questName = splitData[1];
            npcQuestData.title = splitData[2];
            npcQuestData.target = splitData[3];
            npcQuestData.description = splitData[4];
            npcQuestData.expReward = int.Parse(splitData[5]);
            npcQuestData.goal = new QuestGoal();
            npcQuestData.goal.goalType = (GoalType)(int.Parse(splitData[6]));
            npcQuestData.goal.requiredAmount = int.Parse(splitData[7]);

            if (!string.IsNullOrEmpty(splitData[8]))
                npcQuestData.goal.item = Resources.Load<ItemData>(splitData[8]);

            if (!string.IsNullOrEmpty(splitData[9]))
                npcQuestData.goal.targetNpc = Resources.Load<NpcData>(splitData[9]);

            AssetDatabase.CreateAsset(npcQuestData, $"Assets/Imports/Resources/Data/NpcData/QuestData/{splitData[0]}.asset");

            npcQuestDataList.Add(npcQuestData);
        }

        AssetDatabase.SaveAssets();

        for (int i = 0; i < npcTalkDataList.Count; i++)
        {
            NpcData npcData = ScriptableObject.CreateInstance<NpcData>();
            npcData.talkData = npcTalkDataList[i];
            npcData.questNpc = null;

            for (int j = 0; j < npcQuestDataList.Count; j++)
            {
                if (npcTalkDataList[i].id == npcQuestDataList[j].id)
                {
                    npcData.isQuestNPC = true;
                    npcData.quest = npcQuestDataList[j];
                }
            }

            if (npcData.talkData != null || npcData.quest != null)
                AssetDatabase.CreateAsset(npcData, $"Assets/Imports/Resources/Data/NpcData/{npcData.talkData.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }


    [MenuItem("Utilities/Generate Monster Data")]
    public static void CSVtoSO_MonsterData()
    {
        List<AgressiveMonsterData> agrDataList = new List<AgressiveMonsterData>();
        List<MeleeMonsterData> meleeDataList = new List<MeleeMonsterData>();
        List<RangeMonsterData> rangeDataList = new List<RangeMonsterData>();

        string[] agrDataLines = File.ReadAllLines(Application.dataPath + monsterAgrCSVPath);

        foreach (string s in agrDataLines)
        {
            int i = 0;

            if (s == agrDataLines[0])
                continue;

            AgressiveMonsterData agrData = ScriptableObject.CreateInstance<AgressiveMonsterData>();
            string[] splitData = s.Split(',');

            agrData.id = splitData[0];
            agrData.detectRange = float.Parse(splitData[1]);

            for (int j = 0; j < agrDataList.Count; j++)
                if (agrDataList[j].id == agrData.id)
                    i++;
            AssetDatabase.CreateAsset(agrData, $"Assets/Imports/Resources/Data/MonsterData/{splitData[0]}/Trace Data{i}.asset");

            agrDataList.Add(agrData);
        }

        AssetDatabase.SaveAssets();

        string[] melDataLines = File.ReadAllLines(Application.dataPath + monsterMelCSVPath);

        foreach (string s in melDataLines)
        {
            int i = 0;

            if (s == melDataLines[0])
                continue;

            MeleeMonsterData melData = ScriptableObject.CreateInstance<MeleeMonsterData>();
            string[] splitData = s.Split(',');

            melData.id = splitData[0];
            melData.detectRange = float.Parse(splitData[1]);
            melData.attackDistance = float.Parse(splitData[2]);
            melData.angle = float.Parse(splitData[3]);
            melData.attackDamage = int.Parse(splitData[4]);

            for (int j = 0; j < meleeDataList.Count; j++)
                if (meleeDataList[j].id == melData.id)
                    i++;

            AssetDatabase.CreateAsset(melData, $"Assets/Imports/Resources/Data/MonsterData/{splitData[0]}/Melee Data{i}.asset");

            meleeDataList.Add(melData);
        }

        AssetDatabase.SaveAssets();

        string[] ranDataLines = File.ReadAllLines(Application.dataPath + monsterRanCSVPath);

        foreach (string s in ranDataLines)
        {
            int i = 0;

            if (s == ranDataLines[0])
                continue;

            RangeMonsterData ranData = ScriptableObject.CreateInstance<RangeMonsterData>();
            string[] splitData = s.Split(',');

            ranData.id = splitData[0];
            ranData.detectRange = float.Parse(splitData[1]);
            ranData.attackDistance = float.Parse(splitData[2]);
            ranData.angle = float.Parse(splitData[3]);
            ranData.attackDamage = int.Parse(splitData[4]);

            for (int j = 0; j < rangeDataList.Count; j++)
                if (rangeDataList[j].id == ranData.id)
                    i++;

            AssetDatabase.CreateAsset(ranData, $"Assets/Imports/Resources/Data/MonsterData/{splitData[0]}/Range Data{i}.asset");

            rangeDataList.Add(ranData);
        }

        AssetDatabase.SaveAssets();

        string[] monsterDataLines = File.ReadAllLines(Application.dataPath + monsterCSVPath);

        foreach (string s in monsterDataLines)
        {
            if (s == monsterDataLines[0])
                continue;

            BaseMonsterData monsterData = ScriptableObject.CreateInstance<BaseMonsterData>();
            monsterData.agressiveMonsterData = new List<AgressiveMonsterData>();
            monsterData.meleeMonsterData = new List<MeleeMonsterData>();
            monsterData.rangeMonsterData = new List<RangeMonsterData>();
            monsterData.dropTable = new List<ItemData>();

            string[] splitData = s.Split(',');

            monsterData.id = splitData[0];

            for (int j = 0; j < agrDataList.Count; j++)
                if (monsterData.id == agrDataList[j].id)
                    monsterData.agressiveMonsterData.Add(agrDataList[j]);

            for (int j = 0; j < meleeDataList.Count; j++)
                if (monsterData.id == meleeDataList[j].id)
                    monsterData.meleeMonsterData.Add(meleeDataList[j]);

            for (int j = 0; j < rangeDataList.Count; j++)
                if (monsterData.id == rangeDataList[j].id)
                    monsterData.rangeMonsterData.Add(rangeDataList[j]);

            monsterData.dropExp = int.Parse(splitData[1]);
            monsterData.maxHp = int.Parse(splitData[2]);
            monsterData.moveSpeed = float.Parse(splitData[3]);
            monsterData.rotSpeed = float.Parse(splitData[4]);

            for (int j = 5; j < splitData.Length; j++)
                monsterData.dropTable.Add(Resources.Load<ItemData>(splitData[j]));

            AssetDatabase.CreateAsset(monsterData, $"Assets/Imports/Resources/Data/MonsterData/{splitData[0]}/{monsterData.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    private static string etcItemCSVPath = "/Editors/CSVs/EtcItemCSV.csv";

    [MenuItem("Utilities/Generate Etc Item Data")]
    public static void CSVtoSO_EtcItemData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + etcItemCSVPath);

        foreach (string s in allLines)
        {
            if (s == allLines[0])
                continue;

            string[] splitData = s.Split(',');

            if (splitData.Length > 5)
                Debug.Log("Data Is not Valid");

            EtcItemData etcData = ScriptableObject.CreateInstance<EtcItemData>();

            etcData.itemName = splitData[0];
            etcData.Tooltip = splitData[1];
            etcData.MaxAmount = int.Parse(splitData[2]);
            etcData.sprite = Resources.Load<Sprite>(splitData[3]);
            etcData.prefab = Resources.Load<Item>(splitData[4]);

            AssetDatabase.CreateAsset(etcData, $"Assets/Imports/Resources/Data/ItemData/EtcData/{etcData.itemName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    private static string potionItemCSVPath = "/Editors/CSVs/PotionItemCSV.csv";

    [MenuItem("Utilities/Generate Potion Item Data")]
    public static void CSVtoSO_PotionItemData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + potionItemCSVPath);

        foreach (string s in allLines)
        {
            if (s == allLines[0])
                continue;

            string[] splitData = s.Split(',');

            if (splitData.Length > 6)
                Debug.Log("Data Is not Valid");

            PotionData potionData = ScriptableObject.CreateInstance<PotionData>();

            potionData.itemName = splitData[0];
            potionData.Tooltip = splitData[1];
            potionData.MaxAmount = int.Parse(splitData[2]);
            potionData.Value = int.Parse(splitData[3]);
            potionData.sprite = Resources.Load<Sprite>(splitData[4]);
            potionData.prefab = Resources.Load<Item>(splitData[5]);

            AssetDatabase.CreateAsset(potionData, $"Assets/Imports/Resources/Data/ItemData/PotionData/{potionData.itemName}.asset");
        }

        AssetDatabase.SaveAssets();

    }

    private static string weaponItemPath = "/Editors/CSVs/WeaponItemCSV.csv";

    [MenuItem("Utilities/Generate Weapon Item Data")]
    public static void CSVtoSO_WeaponItemData()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + weaponItemPath);

        foreach (string s in allLines)
        {
            if (s == allLines[0])
                continue;

            string[] splitData = s.Split(',');

            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
            weaponData.Effects = new List<ParticleSystem>();

            weaponData.itemName = splitData[0];
            weaponData.Tooltip = splitData[1];
            weaponData.ReqLvl = int.Parse(splitData[2]);
            weaponData.ReqJob = splitData[3];
            weaponData.equipType = (EquipmentData.EquipType)(int.Parse(splitData[4]));
            weaponData.localPosition = new Vector3(float.Parse(splitData[5]), float.Parse(splitData[6]), float.Parse(splitData[7]));
            weaponData.localRotation = new Vector3(float.Parse(splitData[8]), float.Parse(splitData[9]), float.Parse(splitData[10]));
            weaponData.localScale = new Vector3(float.Parse(splitData[11]), float.Parse(splitData[12]), float.Parse(splitData[13]));
            weaponData.AttackPower = int.Parse(splitData[14]);
            weaponData.AttackSpeed = float.Parse(splitData[15]);
            weaponData.MaxCombo = int.Parse(splitData[16]);
            weaponData.CoolTimeSkill = int.Parse(splitData[17]);
            weaponData.sprite = Resources.Load<Sprite>(splitData[18]);
            weaponData.prefab = Resources.Load<Item>(splitData[19]);

            for (int i = 20; i < splitData.Length - 1; i++)
                weaponData.Effects.Add(Resources.Load<ParticleSystem>(splitData[i]));

            weaponData.WeaponAnimator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(splitData[splitData.Length - 1]);

            AssetDatabase.CreateAsset(weaponData, $"Assets/Imports/Resources/Data/ItemData/WeaponData/{weaponData.itemName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}