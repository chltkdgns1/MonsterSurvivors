using System.Collections;
using System.Collections.Generic;


public interface SerializableInterface
{
    string GetJsonString();
    void SetJsonToObject(string jsonData);

    byte[] GetByteObject();

    void SetByteObject(byte[] value);

    void SetData(IDictionary idict);

    Dictionary<string, object> ToDictionary();
}
