namespace Unitinium
{
    public class SceneQueryService
    {
        /// Selectors:
        ///   #TypeName - select components with concrete type
        ///   &TypeName - select components with assingable type
        /// 
        /// Operations:
        ///   (selector)> - get first from sequence
        ///   (selector)< - get last from sequence
        ///   (object). - return sequence of components from GameObject or sequence of fieds/properties from component
        ///   [index] - select concrete object with index
        /// 
        /// Global objects:
        ///   S: - returns sequence of loaded scenes (Default)
        ///   C: - returns sequence of all components from scenes
        /// 
        /// Sample:
        ///   GameManager#Camera.fieldOfView - return fieldOfView field from first Camera component of first GameMager gameObject
        ///   GameManager#Camera.fieldOfView - return fieldOfView field from first Camera component of first GameMager gameObject
        //public object GetObject(string query)
        //{
        //}
    }
}