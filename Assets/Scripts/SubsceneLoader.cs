using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Scenes;

public class SubsceneLoader : ComponentSystem
{
    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadSubscene(SubSceneReferences.Instance.room1);
        }
    }

    private void LoadSubscene(SubScene subscene)
    {
        sceneSystem.LoadSceneAsync(subscene.SceneGUID);
    }
}
