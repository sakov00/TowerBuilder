using Unity.VisualScripting;

namespace _Project.Scripts.Interfaces
{
    public interface ISavableController : IInitializable
    {
        public ISavableModel GetSavableModel();
        public void SetSavableModel(ISavableModel savableModel);
    }
}