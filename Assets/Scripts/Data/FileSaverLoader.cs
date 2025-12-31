using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileSaverLoader
{
    private JsonSerializer serializer;
    private string filePath;

    public FileSaverLoader(string path)
    {
        serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented, // 가독성을 위해 들여쓰기 (배포 시 None으로 변경 추천)
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 순환 참조 에러 방지
        };

        filePath = path;
    }


    // 데이터 저장 (직렬화)
    public void Save<T>(T data, string fileName)
    {
        string path = Path.Combine(filePath, fileName);

        try
        {
            using (var sw = new StreamWriter(path))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, data);
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error saving file at {path}: {e.Message}");
        }
    }

    public T Load<T>(string fileName)
    {
        string path = Path.Combine(filePath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"파일을 찾을 수 없습니다: {path}");
            return default(T); // 파일이 없으면 null 또는 기본값 반환
        }

        try
        {
            using (var sr = new StreamReader(path))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(reader);
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error loading file at {path}: {e.Message}");
            return default;
        }
    }


    public List<T> LoadAll<T>(string folderName)
    {
        List<T> resultList = new List<T>();

        string path = Path.Combine(filePath, folderName);

        // 1. 폴더 존재 여부 확인
        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"폴더가 존재하지 않습니다: {path}");
            return resultList;
        }

        // 2. 해당 경로의 모든 .json 파일 경로 가져오기
        string[] files = Directory.GetFiles(path, "*.json");

        // 3. 반복문으로 하나씩 로드해서 리스트에 추가
        foreach (string fullPath in files)
        {
            try
            {
                using (var sr = new StreamReader(fullPath))
                using (var reader = new JsonTextReader(sr))
                {
                    T data = serializer.Deserialize<T>(reader);
                    if (data != null)
                    {
                        resultList.Add(data);
                    }
                }
            }
            catch (System.Exception e)
            {
                // 하나가 실패해도 나머지는 계속 로드하도록 에러 로그만 찍음
                Debug.LogError($"파일 로드 건너뜀 ({Path.GetFileName(fullPath)}): {e.Message}");
            }
        }

        return resultList;
    }
}
