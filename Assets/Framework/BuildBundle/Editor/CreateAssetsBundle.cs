using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class CreateAssetsBundle : Editor
{
    public static string sourcePath = Application.dataPath + "/GameAssets";
    const string AssetBundlesOutputPath = "Assets/StreamingAssets";

    [MenuItem("工具/CreateAssetBundle")]
   private static void Create()
    {
        string outPath = Path.Combine(AssetBundlesOutputPath, EditorUserBuildSettings.activeBuildTarget.ToString());
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
        LoaderManager.CleaarAllABNames();
        LoaderManager.SetABNames(sourcePath);

        //根据BuildSetting里面所激活的平台进行打包
       AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("","打包"+( manifest == null ? "失败" : "成功"), "确定");
    }
    [MenuItem("工具/DeleteAssetBundle")]
    private static void Delete()
    {
        if (Directory.Exists(AssetBundlesOutputPath))
            Directory.Delete(AssetBundlesOutputPath, true);
        Directory.CreateDirectory(AssetBundlesOutputPath);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("","Bundle包成功删除", "关闭");
    }
  
}
