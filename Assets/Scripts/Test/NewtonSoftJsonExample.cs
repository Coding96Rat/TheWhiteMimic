using UnityEngine;
using Newtonsoft.Json;


public class NewtonSoftJsonExample : MonoBehaviour
{

    private void Start()
    {
        JsonTestClass testJson = new JsonTestClass();
        // 직렬화
        string jsonString = JsonConvert.SerializeObject(testJson);
        
        // 역직렬화
        JsonTestClass testJson2 = JsonConvert.DeserializeObject<JsonTestClass>(jsonString);

    }

    public class JsonTestClass
    {
        public int age;
        public float height;
        public bool isMale;
        public string name;

        public JsonTestClass()
        {
            age = 25;
            height = 175.5f;
            isMale = true;
            name = "John Doe";
        }
    }
}
