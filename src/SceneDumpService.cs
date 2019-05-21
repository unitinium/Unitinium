using System.Linq;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class SceneDumpService
    {
        public readonly IRuntimeObjectWrapperService WrapperService;

        public SceneDumpService(IRuntimeObjectWrapperService wrapperService)
        {
            WrapperService = wrapperService;
        }

        public SceneNode DumpScenes()
        {
            var result = new SceneNode()
            {
                name = "world",
                childs = Enumerable.Range(0, SceneManager.sceneCount)
                    .Select(i => SceneManager.GetSceneAt(i))
                    .Select(s => new SceneNode()
                    {
                        name = s.name,
                        childs = s.GetRootGameObjects()
                            .Select(o => (SceneNode)WrapperService.Wrap(o.transform))
                            .ToList()
                    }).ToList()
            };

            return result;
        }
    }
}