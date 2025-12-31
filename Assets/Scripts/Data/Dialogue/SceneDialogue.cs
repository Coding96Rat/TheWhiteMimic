using System.Collections.Generic;
using UnityEngine;



public class SceneDialogue
{
    public string sceneName;            // 씬 이름 (Key 역할)
    public int sceneId;                 // 씬 ID (정수로 관리할 경우)
    public List<DialogueLine> dialogues; // 대화 목록 (순서대로 저장됨)

    public SceneDialogue()
    {
        dialogues = new List<DialogueLine>();
    }

    public class DialogueLine
    {
        public string speakerName;  // 화자 이름 (예: "주인공", "상점주인")
        public string sentence;     // 대사 내용 (예: "어서오세요!")
        public string portraitID;   // 초상화 이미지 ID (예: "hero_smile", "npc_angry")
        public float duration;      // 대사 유지 시간 (선택사항)

        // 생성자 (편의용)
        public DialogueLine(string name, string text, string portrait = "default")
        {
            speakerName = name;
            sentence = text;
            portraitID = portrait;
        }
    }
}


