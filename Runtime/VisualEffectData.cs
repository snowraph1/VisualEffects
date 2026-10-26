using UnityEngine;

namespace Snowraph.VisualEffects
{
    [CreateAssetMenu(fileName = "VisualEffectData", menuName = "Snowraph/VisualEffects/Create Visual Effect Data", order = 1)]
    public class VisualEffectData : ScriptableObject
    {
        #region Private Fields

        [SerializeField] private string _id;

        [SerializeField] private int _basePoolCount;

        [SerializeField] private GameObject _prefab;

        #endregion

        #region Properties

        public string Id
        {
            get {return _id;}
        }

        public int BasePoolCount
        {
            get {return _basePoolCount;}
        }

        #endregion

        #region Public Methods

        public IVisualEffect CreateVisualEffect()
        {
            GameObject visualEffectObject = Instantiate(_prefab);
            if (visualEffectObject.TryGetComponent(out IVisualEffect visualEffect))
            {
                return visualEffect;
            }

            Debug.LogError($"Visual effect {_prefab.name} doesn't have a IVisualEffect component");
            Destroy(visualEffectObject);
            return null;
        }

        #endregion
    }
}
