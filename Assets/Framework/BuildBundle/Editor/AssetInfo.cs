﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AssetInfo:Editor
{

    //是不是被打包文件夹下的直接资源
    private bool isRootAsset = false;

    public string assetPath { get; private set; }

    private HashSet<AssetInfo> childSet = new HashSet<AssetInfo>();
    private HashSet<AssetInfo> parentSet = new HashSet<AssetInfo>();

    public AssetInfo(string assetPath, bool isRootAsset = false)
    {
        this.assetPath = assetPath;
    }
    public Object GetAsset()
    {
        Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        return asset;
    }
    /// <summary>
    /// 从这里开始分析构建资源依赖树
    /// </summary>
    /// <param name="parent"></param>
    public void AddParent(AssetInfo parent)
    {
        if (parent == this || IsParentEarlyDep(parent) || parent == null)
            return;

        parentSet.Add(parent);
        parent.AddChild(this);

        parent.RemoveRepeatChildDep(this);
        RemoveRepeatParentDep(parent);
    }
    /// <summary>
    /// 清除我父节点对我子节点的重复引用，保证树形结构
    /// </summary>
    /// <param name="targetParent"></param>
    private void RemoveRepeatChildDep(AssetInfo targetChild)
    {

        List<AssetInfo> infolist = new List<AssetInfo>(parentSet);
        for (int i = 0; i < infolist.Count; i++)
        {
            AssetInfo pinfo = infolist[i];
            pinfo.RemoveChild(targetChild);
            pinfo.RemoveRepeatChildDep(targetChild);
        }
    }
    /// <summary>
    /// 清除我子节点被我父节点的重复引用，保证树形结构
    /// </summary>
    /// <param name="targetChild"></param>
    private void RemoveRepeatParentDep(AssetInfo targetParent)
    {

        List<AssetInfo> infolist = new List<AssetInfo>(childSet);
        for (int i = 0; i < infolist.Count; i++)
        {
            AssetInfo cinfo = infolist[i];
            cinfo.RemoveParent(targetParent);
            cinfo.RemoveRepeatParentDep(targetParent);
        }
    }

    private void RemoveChild(AssetInfo targetChild)
    {
        childSet.Remove(targetChild);
        targetChild.parentSet.Remove(this);
    }
    private void RemoveParent(AssetInfo parent)
    {
        parent.childSet.Remove(this);
        parentSet.Remove(parent);
    }


    private void AddChild(AssetInfo child)
    {
        childSet.Add(child);
    }

    /// <summary>
    /// 如果父节点早已当此父节点为父节点
    /// </summary>
    /// <param name="targetParent"></param>
    /// <returns></returns>
    private bool IsParentEarlyDep(AssetInfo targetParent)
    {
        if (parentSet.Contains(targetParent))
        {
            return true;
        }
        var e = parentSet.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.IsParentEarlyDep(targetParent))
            {
                return true;
            }
        }
        return false;
    }
    public bool HasParent(AssetInfo p)
    {
        if (parentSet.Contains(p))
            return true;
        return false;
    }
    /// <summary>
    /// 打包碎片粒度
    /// </summary>
    /// <param name="pieceThreshold"></param>
    public void SetAssetBundleName(int pieceThreshold)
    {
        AssetImporter ai = AssetImporter.GetAtPath(this.assetPath);
        //针对UGUI图集的处理
        if (ai is TextureImporter)
        {
            TextureImporter tai = ai as TextureImporter;
            if (!string.IsNullOrEmpty(tai.spritePackingTag))
            {
                //AssetBundleName和spritePackingTag保持一致
                tai.SetAssetBundleNameAndVariant(tai.spritePackingTag + ".ab", null);
            }
        }
        else
        {
            string[] ps = this.assetPath.Split('/');
            string abname = this.assetPath.Substring(this.assetPath.IndexOf(ps[ps.Length -2]));
            abname = abname.Remove(abname.IndexOf(Path.GetExtension(abname)));
            if (ps[ps.Length - 2] == "uiprefab")
                abname = "uiprefab/uiprefab";
            //不是图集，而且大于阀值
            if (this.parentSet.Count >= pieceThreshold)
            {
                ai.SetAssetBundleNameAndVariant(abname, "ab");
            }
            //根节点
            else if (this.parentSet.Count == 0)
            {
                ai.SetAssetBundleNameAndVariant(abname, "ab");
            }
            else if (this.isRootAsset)
            {
                ai.SetAssetBundleNameAndVariant(abname, "ab");
            }
            else
            {
                //其余的子资源
                ai.SetAssetBundleNameAndVariant(abname, "ab");
            }
        }
    }
}