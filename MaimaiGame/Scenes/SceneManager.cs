﻿using System;
using System.Collections.Generic;

namespace MaimaiGame.Scenes;

public static class SceneManager
{
	private static readonly Stack<Scene> Scenes = new Stack<Scene>();

	public static Scene? Current => Scenes.Count > 0 ? Scenes.Peek() : null;

	public static int Count => Scenes.Count;


	public static void Push(Scene scene)
	{
		Current?.OnLeave();
		Scenes.Push(scene);

		Current!.OnCreate();
		Current!.OnEnter();
	}

	public static void Pop()
	{
		if (Count == 0)
			throw new Exception();

		Current!.OnLeave();
		Current!.OnDestroy();

		Scenes.Pop();
		Current?.OnEnter();
	}

	public static void Clear()
	{
		Current?.OnLeave();

		while (Count > 0)
		{
			Current!.OnDestroy();
			Scenes.Pop();
		}
	}
}