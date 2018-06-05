using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CoreClass;
using System.Configuration;

/// <summary>
/// 公共方法库
/// </summary>
public class Factory
{
	/// <summary>
	/// 字符串操作
	/// </summary>
	public OperateStringClass OpString
	{
		get { return new OperateStringClass(); }
	}
	/// <summary>
	/// 数据集操作
	/// </summary>
	public OperateDataClass OpData
	{
		get { return new OperateDataClass(); }
	}
	/// <summary>
	/// 功能操作
	/// </summary>
	public OperateMemoryClass OpMemory
	{
		get { return new OperateMemoryClass(); }
	}
	/// <summary>
	/// 文件操作
	/// </summary>
	public OperateFileClass OpFile
	{
		get { return new OperateFileClass(); }
	}
	/// <summary>
	/// 数组操作
	/// </summary>
	public OperateArrayClass OpArray
	{
		get { return new OperateArrayClass(); }
	}
	/// <summary>
	/// 数据库操作
	/// </summary>
	protected OperateSqlClass OpSql = new OperateSqlClass();

	/// <summary>
	/// 初始化Factory类型的新实例
	/// </summary>
	public Factory()
	{

	}
}