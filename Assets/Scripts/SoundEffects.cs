using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip promoteNode;
    [SerializeField] private AudioClip nodeGoBad;
    [SerializeField] private AudioClip nodeGoNeutral;
    [SerializeField] private AudioClip nodeGoGood;
    [SerializeField] private AudioClip cutSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameController.StartGame += RegisterForNodeChanges;
        Unfollow.CutNodes += PlayCutAudio;
        PlayerCursor.NewToolSelected += ButtonPress;
        Promote.NodePromoted += PlayPromoteSound;
    }

    private void PlayPromoteSound()
    {
        _audioSource.PlayOneShot(promoteNode);
    }

    private void ButtonPress()
    {
        _audioSource.PlayOneShot(buttonClick);
    }

    private void PlayCutAudio()
    {
        _audioSource.PlayOneShot(cutSound);
    }

    private void RegisterForNodeChanges(GameVars gv)
    {
        Node.NodeTypeChanged += PlayNodeChangedAudio;
    }

    private void PlayNodeChangedAudio(NodeType type, Node node)
    {
        _audioSource.PlayOneShot(type switch
        {
            NodeType.Misinformed => nodeGoBad,
            NodeType.Neutral => nodeGoNeutral,
            NodeType.Reliable => nodeGoGood,
            _ => null
        });
    }

    private void OnDisable()
    {
        GameController.StartGame -= RegisterForNodeChanges;
        Unfollow.CutNodes -= PlayCutAudio;
        Node.NodeTypeChanged -= PlayNodeChangedAudio;
        PlayerCursor.NewToolSelected -= ButtonPress;
        Promote.NodePromoted -= PlayPromoteSound;
    }
}