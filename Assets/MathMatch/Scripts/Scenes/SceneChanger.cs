using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace MathMatch.Scenes
{
    public static class SceneChanger
    {
        public static bool SceneChanging { get; private set; }
        
        public static async void LoadScene(string name)
        {
            if (SceneChanging)
            {
                throw new System.Exception("Cannot load scene at another scene load process");
            }

            SceneChanging = true;
            await SceneManager.LoadSceneAsync(name);
            SceneChanging = false;
        }
    }
}