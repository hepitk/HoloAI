using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AISceneManager : MonoBehaviour
{
    private Dictionary<GameObject, AIStateComponent> managedAIComponents = new Dictionary<GameObject, AIStateComponent>();

    public void AddState(GameObject gameObject, IAIState newState)
    {
        AIStateComponent aiStateComponent;

        if (!managedAIComponents.ContainsKey(gameObject))
        {
            aiStateComponent = gameObject.GetComponent<AIStateComponent>();
            if (aiStateComponent == null)
            {
                aiStateComponent = gameObject.AddComponent<AIStateComponent>();
            }
            managedAIComponents.Add(gameObject, aiStateComponent);

            // Initialize the AIStateComponent with the first state
            aiStateComponent.ChangeState(newState);
        }
        else
        {
            aiStateComponent = managedAIComponents[gameObject];
            aiStateComponent.ChangeState(newState);
        }
    }
}

public class AIStateComponent : MonoBehaviour
{
    private IAIState currentState;

    public void ChangeState(IAIState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter(GetComponent<NavMeshAgent>());
    }

    void Update()
    {
        currentState?.OnUpdate();
    }
}

public interface IAIState
{
    void OnEnter(NavMeshAgent agent);
    void OnUpdate();
    void OnExit();
}
