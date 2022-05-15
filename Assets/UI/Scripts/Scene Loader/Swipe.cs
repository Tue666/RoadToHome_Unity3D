using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour
{
    public Transform content;
    public Scrollbar scrollbar;
    public int skipIndex = 0;

    private int currentIndex = -1;
    private int mutableIndex = 0;
    private bool buttonPressed = false;
    private float scrollPosition = 0;
    private float[] positions;
    private float distance;

    private IEnumerator moveToIndexCoroutine;

    void InitializeIfNecessary()
    {
        if (content == null) content = transform.GetChild(0).GetChild(0);
        if (scrollbar == null) scrollbar = transform.GetChild(1).GetComponent<Scrollbar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeIfNecessary();

        // Position each item and distance between of them
        positions = new float[content.childCount];
        distance = 1f / (positions.Length - 1f);
        for (int i = 0; i < positions.Length; i++)
            positions[i] = distance * i;
    }

    void OnEnable()
    {
        mutableIndex = SceneLoader.currentScene - skipIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            scrollPosition = scrollbar.value;

        // Mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            SwipeNext();
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            SwipePrevious();

        // Mouse click
        if (Input.GetMouseButtonUp(0) && !buttonPressed)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (scrollPosition < positions[i] + (distance / 2) && scrollPosition > positions[i] - (distance / 2))
                    mutableIndex = i;
            }
        }
        buttonPressed = false;

        if (mutableIndex != currentIndex)
        {
            StartMoveToIndex(mutableIndex, 0.6f);
        }
    }

    void StartMoveToIndex(int index, float time)
    {
        if (moveToIndexCoroutine != null)
            StopCoroutine(moveToIndexCoroutine);
        moveToIndexCoroutine = MoveToIndex(index, time);
        StartCoroutine(moveToIndexCoroutine);
    }

    IEnumerator MoveToIndex(int index, float time)
    {
        currentIndex = index;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            scrollbar.value = Mathf.Lerp(scrollbar.value, positions[index], 0.1f);
            for (int i = 0; i < positions.Length; i++)
            {
                if (i == index)
                {
                    content.GetChild(i).localScale = Vector2.Lerp(content.GetChild(i).localScale, new Vector2(1.1f, 1.1f), 0.1f);
                    content.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    content.GetChild(i).localScale = Vector2.Lerp(content.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    content.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
            yield return null;
        }
    }

    public void SwipePrevious()
    {
        buttonPressed = true;
        AudioManager.Instance.PlayEffect("PLAYER", "Collect Item");
        int newIndex = currentIndex - 1;
        if (newIndex < 0)
            newIndex = positions.Length - 1;
        mutableIndex = newIndex;
    }

    public void SwipeNext()
    {
        buttonPressed = true;
        AudioManager.Instance.PlayEffect("PLAYER", "Collect Item");
        int newIndex = currentIndex + 1;
        if (newIndex > positions.Length - 1)
            newIndex = 0;
        mutableIndex = newIndex;
    }

    public void Teleport()
    {
        if (SceneLoader.currentScene == (currentIndex + skipIndex)) return;

        AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
        SceneLoader.Instance.LoadMap(currentIndex + skipIndex);
        UIManager.Instance.HideView("Select Map UI");
    }
}