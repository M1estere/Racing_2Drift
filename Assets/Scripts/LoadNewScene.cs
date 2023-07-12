using UnityEngine;

public class LoadNewScene : MonoBehaviour
{
   [SerializeField] private string _newSceneName = "StartScreen";

   [SerializeField] private SceneFader _fader;
   
   private void OnTriggerEnter2D(Collider2D collider)
   {
      if (collider.gameObject.CompareTag("Player"))
      {
         _fader.FadeTo(_newSceneName);
      }
   }
}
