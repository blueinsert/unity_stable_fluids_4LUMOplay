using SimpleJson;

public partial class JsonUtility
{
    public static string Serialize(object j)
    {
        if (j == null)
            return string.Empty;

        return SimpleJson.SimpleJson.SerializeObject(j);
    }

    public static object Deserialize(string txt)
    {
        if (string.IsNullOrEmpty(txt))
            return null;

        try
        {
            return SimpleJson.SimpleJson.DeserializeObject(txt);
        }
        //catch (System.Runtime.Serialization.SerializationException)
        catch (System.Exception)
        {
            return null;
        }
    }

    public static T Deserialize<T>(string txt) where T : class
    {
        if (string.IsNullOrEmpty(txt))
            return null;

        try
        {
            return SimpleJson.SimpleJson.DeserializeObject<T>(txt);
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("SimpleJson.DeserializeObject from {0} failed, error: {1}",
                txt, e.Message));
            return null;
        }
    }

    public static string SerializeObject(JsonObject j)
    {
        if (j == null)
            return string.Empty;

        return SimpleJson.SimpleJson.SerializeObject(j);
    }

    public static string SerializeArray(JsonArray j)
    {
        if (j == null)
            return string.Empty;

        return SimpleJson.SimpleJson.SerializeObject(j);
    }

    public static JsonObject DeserializeObject(string txt)
    {
        return Deserialize(txt) as JsonObject;
    }

    public static JsonArray DeserializeArray(string txt)
    {
        return Deserialize(txt) as JsonArray;
    }

    public static JsonArray AddArray(JsonObject j, string key)
    {
        JsonArray a = new JsonArray();
        j[key] = a;
        return a;
    }

    public static JsonObject AddObject(JsonObject j, string key)
    {
        JsonObject j2 = new JsonObject();
        j[key] = j2;
        return j2;
    }

    public static JsonObject AddObject(JsonArray a)
    {
        JsonObject j = new JsonObject();
        a.Add(j);
        return j;
    }

    public static JsonObject GetObject(JsonObject j, string key)
    {
        return GetObj(j, key) as JsonObject;
    }

    public static JsonArray GetArray(JsonObject j, string key)
    {
        return GetObj(j, key) as JsonArray;
    }

    static object GetObj(JsonObject j, string key)
    {
        if (j == null)
            return false;
        if (string.IsNullOrEmpty(key))
            return false;

        object o;
        if (!j.TryGetValue(key, out o))
            return null;
        return o;
    }

    public static void Set(JsonObject j, string key, long value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref long value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, int value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref int value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (int)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, uint value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref uint value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (uint)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, short value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref short value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (short)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, ushort value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref ushort value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (ushort)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, sbyte value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref sbyte value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (sbyte)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, byte value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref byte value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (byte)(long)o;
        return true;
    }

    public static void Set(JsonObject j, string key, double value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref double value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(double))
            return false;
        value = (double)o;
        return true;
    }

    public static void Set(JsonObject j, string key, float value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref float value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(double))
            return false;
        value = (float)(double)o;
        return true;
    }

    public static void Set(JsonObject j, string key, bool value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref bool value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(bool))
            return false;
        value = (bool)o;
        return true;
    }

    public static void Set(JsonObject j, string key, string value)
    {
        j[key] = value;
    }

    public static bool Get(JsonObject j, string key, ref string value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(string))
            return false;
        value = (string)o;
        return true;
    }

    public static void Set(JsonObject j, string key, System.DateTime value)
    {
        j[key] = value.ToBinary();
    }

    public static bool Get(JsonObject j, string key, ref System.DateTime value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = System.DateTime.FromBinary((long)o);
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PLong value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PInt value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (int)(long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PUInt value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (uint)(long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PShort value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (short)(long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PUShort value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (ushort)(long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PSByte value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (sbyte)(long)o;
        return true;
    }

    public static bool Get(JsonObject j, string key, ref PByte value)
    {
        object o = GetObj(j, key);
        if (o == null)
            return false;
        if (o.GetType() != typeof(long))
            return false;
        value = (byte)(long)o;
        return true;
    }
}

