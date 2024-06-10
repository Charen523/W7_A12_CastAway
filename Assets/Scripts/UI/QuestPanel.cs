using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI questname;
    public TextMeshProUGUI questdescription;
    public TextMeshProUGUI count;
    // Start is called before the first frame update
    void Start()
    {
        LoadQuest();
    }

    void LoadQuest()
    {
        DataManager dataManager = DataManager.Instance;
        QuestData unclearedQuest = dataManager.GetQuest();

        if (unclearedQuest != null)
        {
            questname.text = unclearedQuest.QuestName;
            questdescription.text = unclearedQuest.QuestDescription;
            count.text = $"({unclearedQuest.CurrentCount} / {unclearedQuest.MaxCount})";

        }
        else
        {
            questname.text = null;
            questdescription.text = "남은 퀘스트가 없습니다";
            count.text = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    void PlusCount()
    {

    }
    // 퀘스트 클리어시 (카운트가 풀로 찼을 경우) Bool값 True로 변환 후 다음 퀘스트 로드해옴
    //각 조건 (사냥/ 파밍/ 제작) 에 맞는 행동을 했을 시 그 스크립트에서 카운트를 올리기
}
