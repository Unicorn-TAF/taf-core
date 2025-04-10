using System.Collections.Generic;

namespace Unicorn.UnitTests.BO
{
    public interface IObject
    {

    }

    public class ComplexObject
    {
        private int privateIntField;

        public ComplexObject()
        {
            PublicStringsListProperty = new List<string>();
            PublicObjectsListProperty = new List<InnerObject>();
        }

        public InnerObject PublicNullField = null;
        public InnerObject PublicInnerObject { get; set; } = new InnerObject();

        public IObject PublicObjectWithInterface { get; set; }

        public List<string> PublicStringsListProperty { get; set; }
        public List<InnerObject> PublicObjectsListProperty { get; set; }

        protected string ProtectedStringProperty { get; set; }
        protected string ProtectedStringField;

        public void SetPrivateInt(int value)
        {
            privateIntField = value;
        }

        public void SetProtectedStrings(string value)
        {
            ProtectedStringField = value;
            ProtectedStringProperty = value;
        }
    }

    public class Object1 : IObject
    {
    }

    public class Object2 : IObject
    {
    }

    public class InnerObject
    {
        public double PublicDouble { get; set; } = 0.3;

        public string PublicString { get; set; } = "someValue";
    }
}
