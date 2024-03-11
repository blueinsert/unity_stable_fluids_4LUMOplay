
public partial class FileUtility
{
    public static string ReadText(string fname)
    {
        if (string.IsNullOrEmpty(fname))
            return string.Empty;

        try
        {
            string txt = System.IO.File.ReadAllText(fname, System.Text.Encoding.UTF8);
            if (!string.IsNullOrEmpty(txt))
                return txt;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.ReadText Error: " + e);
        }

        return string.Empty;
    }

    public static bool WriteText(string fname, string txt)
    {
        if (string.IsNullOrEmpty(fname))
            return false;

        try
        {
            System.IO.File.WriteAllText(fname, txt, System.Text.Encoding.UTF8);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.WriteText Error: " + e);
        }
        return false;
    }

    public static byte[] ReadBytes(string fname)
    {
        if (string.IsNullOrEmpty(fname))
            return null;

        try
        {
            return System.IO.File.ReadAllBytes(fname);
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.ReadBytes Error: " + e);
        }

        return null;
    }

    public static bool WriteBytes(string fname, byte[] bytes)
    {
        if (string.IsNullOrEmpty(fname))
            return false;
        if (bytes == null || bytes.Length <= 0)
            return false;

        try
        {
            System.IO.File.WriteAllBytes(fname, bytes);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.WriteBytes Error: " + e);
        }
        return false;
    }

    public static bool DeleteFile(string fname)
    {
        if (string.IsNullOrEmpty(fname))
            return false;

        try
        {
            System.IO.File.Delete(fname);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.DeleteFile Error: " + e);
        }
        return false;
    }

    public static bool IsFileExist(string fname)
    {
        if (string.IsNullOrEmpty(fname))
            return false;
        try
        {
            var fileInfo = new System.IO.FileInfo(fname);
            return fileInfo.Exists;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileUtility.IsFileExist Error: " + e);
        }
        return false;
    }
}

