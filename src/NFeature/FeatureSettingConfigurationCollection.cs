namespace NFeature
{
    using System.Configuration;

    public class FeatureConfigurationElementCollection<TFeatureEnum, TTenant> : ConfigurationElementCollection
        where TFeatureEnum : struct
        where TTenant : struct
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public FeatureConfigurationElement<TFeatureEnum, TTenant> this[int index]
        {
            get { return (FeatureConfigurationElement<TFeatureEnum, TTenant>) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(FeatureConfigurationElement<TFeatureEnum, TTenant> element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureConfigurationElement<TFeatureEnum, TTenant>();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureConfigurationElement<TFeatureEnum, TTenant>) element).Name;
        }

        public void Remove(FeatureConfigurationElement<TFeatureEnum, TTenant> element)
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