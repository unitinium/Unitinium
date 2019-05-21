using System.Reflection;

namespace Unitinium
{
    public class TypeMemberInstance
    {
        public MemberInfo Member { get; set; }
        public object Instance { get; set; }
        public string Name => Member.Name;

        public TypeMemberInstance(MemberInfo member, object instance)
        {
            this.Member = member;
            this.Instance = instance;
        }

        public object GetValue()
        {
            if(Member is FieldInfo field)
            {
                return field.GetValue(Instance);
            }

            if(Member is PropertyInfo property)
            {
                return property.GetValue(Instance);
            }

            return null;
        }

        public void SetValue(object value)
        {
            if(Member is FieldInfo field)
            {
                field.SetValue(Instance, value);
                return;
            }

            if(Member is PropertyInfo property)
            {
                property.SetValue(Instance, value);
                return;
            }
        }
    }
}