using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Christophe.Fanchamps
{
public class Projection : MonoBehaviour
{
        Scene _simulationScene;
        PhysicsScene m_physicsScene;
        Vector3 destination;


        private void Start()
        {
            CreatePhysicsScene();
        }


        void CreatePhysicsScene()
        {
            _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            m_physicsScene = _simulationScene.GetPhysicsScene();
        }

        public Vector3 SimulateTrajectory(Throw _throwPrefab, Vector3 _pos, Vector3 _velocity)
        {
            var ghostObj= Instantiate(_throwPrefab, _pos, Quaternion.identity);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

            ghostObj.ThrowObject(_velocity);

            while (ghostObj.transform.position.y > 0.5)
            {
                m_physicsScene.Simulate(Time.fixedDeltaTime);
                destination = ghostObj.transform.position;
                Debug.Log("position predicted : " + destination);
            }


            Destroy(ghostObj.gameObject);
            return destination;
        }

}

}

