/*
JsonHelper.cs

개요:
Unity의 JsonUtility를 확장하여 JSON 배열을 직접 처리할 수 있게 해줌

주요 기능:
1. JSON 배열 문자열을 C# 객체 배열로 변환
2. Unity의 기본 JSON 파싱 제한을 우회하는 래퍼 클래스 제공

사용 예시:
string jsonArray = "[{\"id\":1},{\"id\":2}]";
MyClass[] objects = JsonHelper.FromJson<MyClass>(jsonArray);
*/

using System;
using UnityEngine;

public static class JsonHelper
{
    // JSON 배열 문자열을 지정된 타입의 배열로 변환
    public static T[] FromJson<T>(string json)
    {
        // JSON 배열을 객체의 프로퍼티로 감싸서 Unity가 처리할 수 있는 형태로 변환
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    // JSON 배열을 감싸기 위한 내부 래퍼 클래스
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array; // JSON 배열을 담을 프로퍼티
    }
}
