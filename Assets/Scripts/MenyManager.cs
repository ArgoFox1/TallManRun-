using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenyManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void Support()
        {
            string url = "https://www.youtube.com/channel/UCGPS3t2FfqaljxiaNs9YaSA";
            Application.OpenURL(url);
        }
        public void SupportTwitter()
        {
            string url = "https://twitter.com/CharlesTColley1";
            Application.OpenURL(url);
        }
    }
}
