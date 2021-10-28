using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation Data")]
public class ConversationData : ScriptableObject
{
    public List<SpeakerData> list;
}
