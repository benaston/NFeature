namespace NFeature
{
    using System.Configuration;

    public class FeatureConfigurationElementCollection<TFeatureEnumeration> : ConfigurationElementCollection
        where TFeatureEnumeration : struct
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public FeatureConfigurationElement<TFeatureEnumeration> this[int index]
        {
            get { return (FeatureConfigurationElement<TFeatureEnumeration>) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(FeatureConfigurationElement<TFeatureEnumeration> element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureConfigurationElement<TFeatureEnumeration>();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureConfigurationElement<TFeatureEnumeration>) element).Name;
        }

        public void Remove(FeatureConfigurationElement<TFeatureEnumeration> element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}