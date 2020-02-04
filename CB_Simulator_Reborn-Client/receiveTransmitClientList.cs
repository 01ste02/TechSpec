using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CB_Simulator_Reborn_Client
{
    [Serializable]
    class receiveTransmitClientList : ISerializable
    {
        public Int32 x;
        public List<CB_Simulator_clientInfoLight> transmitList;
        public String name;

        // The security attribute demands that code that calls
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x);
            info.AddValue("name", name);
        }

        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        private receiveTransmitClientList(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("x");
            try
            {
                name = info.GetString("name");
            }
            catch (SerializationException)
            {
                // The "name" field was not serialized because Version1Type 
                // did not contain this field.
                // Set this field to a reasonable default value.
                name = "Reasonable default value";
            }
        }
    }


    sealed class Version1ToVersion2DeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String assemVer1 = Assembly.GetExecutingAssembly().FullName;
            String typeVer1 = "Version1Type";

            if (assemblyName == assemVer1 && typeName == typeVer1)
            {
                // To use a type from a different assembly version, 
                // change the version number.
                // To do this, uncomment the following line of code.
                // assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");

                // To use a different type from the same assembly, 
                // change the type name.
                typeName = "Version2Type";
            }

            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, assemblyName));

            return typeToDeserialize;
        }
    }
}
